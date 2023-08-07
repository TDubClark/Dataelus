using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Dataelus.Extensions;

namespace Dataelus.Mono.Misc
{
	/// <summary>
	/// Class storing Bean code components.
	/// </summary>
	public class BeanCodeComponents
	{
		public string BeanClassName { get; set; }

		public string BeanCollectionClassName { get; set; }

		/// <summary>
		/// Gets or sets the bean class code string.
		/// </summary>
		/// <value>The bean class.</value>
		public string BeanClass { get; set; }

		/// <summary>
		/// Gets or sets the bean collection class code string.
		/// </summary>
		/// <value>The bean collection class.</value>
		public string BeanCollectionClass { get; set; }

		public string Includes { get; set; }

		public string Namespace { get; set; }

		public BeanCodeComponents ()
		{
			this.Namespace = null;
		}

		/// <summary>
		/// Gets the string for output to a file.
		/// </summary>
		/// <returns>The string for output to a file.</returns>
		public string GetFileString ()
		{
			if (String.IsNullOrEmpty (this.Namespace))
				return String.Format ("{1}{0}{2}{0}{3}", Environment.NewLine, this.Includes, this.BeanClass, this.BeanCollectionClass);
			return String.Format ("{1}{0}namespace {4}{0}{{{0}{2}{0}{3}{0}}}", Environment.NewLine, this.Includes, this.BeanClass, this.BeanCollectionClass, this.Namespace);
		}
	}

	/// <summary>
	/// Code creator class.
	/// </summary>
	public class CodeCreator
	{
		public Database.DBFieldCollection DatabaseFields { get; set; }

		/// <summary>
		/// Gets or sets the number of space characters for each indent level.
		/// </summary>
		/// <value>The indent level space count.</value>
		public int IndentLevelSpaceCount { get; set; }

		public string RootNamespace { get; set; }

		/// <summary>
		/// Gets the indent count for the specified level.
		/// </summary>
		/// <param name="level">Level.</param>
		private int Indent (int level)
		{
			return level * this.IndentLevelSpaceCount;
		}

		DataTypeConverter _converter;

		public CodeCreator ()
			: this (4)
		{
		}

		public CodeCreator (int indentLevelSpaceCount)
		{
			this.IndentLevelSpaceCount = indentLevelSpaceCount;
		}

		public CodeCreator (Dataelus.Database.DBFieldCollection databaseFields, DataTypeConverter converter, int indentLevelSpaceCount)
		{
			if (databaseFields == null)
				throw new ArgumentNullException ("databaseFields");
			if (converter == null)
				throw new ArgumentNullException ("converter");
			
			this.DatabaseFields = databaseFields;
			this.IndentLevelSpaceCount = indentLevelSpaceCount;

			_converter = converter;
		}


		public BeanCodeComponents CreateBeanClass (IEnumerable<Database.DBFieldSimple> fields, string tableName, string className)
		{
			string classModifier = "public";
			string baseListClassName = "Dataelus.ListBase";
			bool isSerializable = true;

			return CreateBeanClass (fields, tableName, className, classModifier, baseListClassName, isSerializable, TypeParser);
		}

		/// <summary>
		/// Parses the given type (can be a .NET or a Database Type)
		/// </summary>
		/// <returns>The parser.</returns>
		/// <param name="typeName">Type name.</param>
		public Type TypeParser (string typeName)
		{
			try {
				return _converter.GetDataType (typeName);
			} catch {
				
			}

			Type retType = System.Type.GetType (typeName);

			if (retType == null)
				throw new ArgumentException (String.Format ("Unrecognized Database or .NET Type '{0}'", typeName), "typeName");
			
			return retType;
		}

		public BeanCodeComponents CreateBeanClass (IEnumerable<Database.DBFieldSimple> fields, string tableName, string className, string classModifier, string baseListClassName, bool isSerializable, Func<string, Type> typeGetter)
		{
			if (DatabaseFields == null)
				throw new NullReferenceException (String.Format ("Null reference: Local property '{0}' is null.", "DatabaseFields"));
			
			var fullFields = fields.Select (x => DatabaseFields.Find (x)).ToList ();
			var columns = fullFields.Select (x => new System.Data.DataColumn (x.FieldName, typeGetter (x.DataType))).ToList ();

			// Add the Nullability of each field
			Dictionary<string, bool> fieldNullable = new Dictionary<string, bool> ();
			fullFields.ForEach (x => fieldNullable.Add (x.FieldName, x.Nullable));

			return CreateBeanClass (columns, fieldNullable, tableName, className, classModifier, baseListClassName, isSerializable, PropertyNameConverter);
		}

		public BeanCodeComponents CreateBeanClass (System.Data.DataTable table)
		{
			return CreateBeanClass (table, new Dictionary<string, bool> ());
		}

		public BeanCodeComponents CreateBeanClass (System.Data.DataTable table, Dictionary<string, bool> fieldNullable)
		{
			string className = table.TableName;
			return CreateBeanClass (table, fieldNullable, className, className);
		}

		/// <summary>
		/// Creates the bean class.
		/// </summary>
		/// <returns>The bean class.</returns>
		/// <param name="table">Table.</param>
		/// <param name="nonNullableFields">List of Non-nullable fields.</param>
		/// <param name="className">Class name.</param>
		public BeanCodeComponents CreateBeanClass (System.Data.DataTable table, IEnumerable<string> nonNullableFields, string tableName, string className)
		{
			var fieldNullable = new Dictionary<string, bool> (new StringEqualityComparer ());
			nonNullableFields.ToList ().ForEach (line => fieldNullable.Add (line, false));

			return CreateBeanClass (table, fieldNullable, tableName, className);
		}

		public BeanCodeComponents CreateBeanClass (System.Data.DataTable table, Dictionary<string, bool> fieldNullable, string tableName, string className)
		{
			string classModifier = "public";
			string baseListClassName = "Dataelus.ListBase";
			bool isSerializable = true;

			return CreateBeanClass (table, fieldNullable, tableName, className, classModifier, baseListClassName, isSerializable);
		}

		public BeanCodeComponents CreateBeanClass (System.Data.DataTable table, Dictionary<string, bool> fieldNullable, string tableName, string className, string classModifier, string baseListClassName, bool isSerializable)
		{
			var columns = new List<System.Data.DataColumn> ();
			foreach (System.Data.DataColumn column in table.Columns) {
				columns.Add (column);
			}
			return CreateBeanClass (columns, fieldNullable, tableName, className, classModifier, baseListClassName, isSerializable, PropertyNameConverter);
		}

		/// <summary>
		/// Creates the bean class.
		/// </summary>
		/// <returns>The bean class.</returns>
		/// <param name="columns">Columns.</param>
		/// <param name="fieldNullable">The dictionary of fields and whether they are nullable (default is DataColumn.AllowDBNull).</param>
		/// <param name="className">Class name.</param>
		/// <param name="classModifier">Class modifier.</param>
		/// <param name="baseListClassName">Base list class name.</param>
		/// <param name="isSerializable">If set to <c>true</c> is serializable.</param>
		/// <param name="propertyNameConverter">The converter from a database field name to a class property name</param>
		public BeanCodeComponents CreateBeanClass (IEnumerable<System.Data.DataColumn> columns, Dictionary<string, bool> fieldNullable, string tableName, string className, string classModifier, string baseListClassName, bool isSerializable, Func<string, string> propertyNameConverter)
		{
			if (!String.IsNullOrWhiteSpace (classModifier))
				classModifier += " ";

			// Allow the first part of the class name to be a sub-namespace
			string subNamespace = "";
			if (className.IndexOf (".") >= 0) {
				int indexDot = className.LastIndexOf (".");
				subNamespace = className.Substring (0, indexDot);
				className = className.Substring (indexDot + 1, className.Length - (indexDot + 1));
			}

			string[] namespaceList = { this.RootNamespace, subNamespace };
			string classNamespace = String.Join (".", namespaceList.Where (x => !String.IsNullOrWhiteSpace (x)).ToArray ());

			int level = 0;

			string varnameDataRow = "dr";
			string varnameNewItem = "item";

			var codeBuilderBean = new StringBuilder ();
			if (isSerializable)
				codeBuilderBean.AppendLine ("[Serializable]");
			codeBuilderBean.AppendLineFormat ("{1}class {0} {{", className, classModifier);
			level++;

			int levelAsgn = 1;
			var codeBuilderBeanAssignFromDataRow = new StringBuilder ();
			codeBuilderBeanAssignFromDataRow.AppendLineFormat (Indent (levelAsgn), "public {0} GetItem (System.Data.DataRow {1}) {{", className, varnameDataRow);
			levelAsgn++;
			codeBuilderBeanAssignFromDataRow.AppendLineFormat (Indent (levelAsgn), "var {1} = new {0} ();", className, varnameNewItem);

			var dictColNamesPropNames = new Dictionary<string, string> ();

			foreach (System.Data.DataColumn column in columns) {
				string typeName = column.DataType.Name;
				string columnName = column.ColumnName;
				string beanPropName = propertyNameConverter (columnName);

				dictColNamesPropNames.Add (columnName, beanPropName);

				bool isStringType = column.DataType.Equals (typeof(String));
				bool nullable;
				if (!fieldNullable.TryGetValue (columnName, out nullable)) {
					nullable = column.AllowDBNull;
				}

				bool isNullableNotation = (nullable && !isStringType);

				codeBuilderBean.AppendLineFormat (Indent (level), "public {0}{2} {1} {{ get; set; }}", typeName, beanPropName, isNullableNotation ? "?" : "");

				string nullableFunction = "";
				if (nullable) {
					if (isStringType) {
						nullableFunction = ".ToNullableString()";
					} else {
						nullableFunction = String.Format (".ToNullable<{0}?>(null)", typeName);
					}
				}

				string castString = nullable ? "" : String.Format ("({0})", typeName);

				codeBuilderBeanAssignFromDataRow.AppendLineFormat (Indent (levelAsgn), "{3}.{0} = {5}{2}[\"{1}\"]{4};", beanPropName, columnName, varnameDataRow, varnameNewItem, nullableFunction, castString);
			}
			codeBuilderBeanAssignFromDataRow.AppendLine ();
			codeBuilderBeanAssignFromDataRow.AppendLineFormat (Indent (levelAsgn), "return {0};", varnameNewItem);
			levelAsgn--;
			codeBuilderBeanAssignFromDataRow.AppendLine (Indent (levelAsgn), "}");

			// Add constructor
			codeBuilderBean.AppendLine ();
			codeBuilderBean.AppendLineFormat (Indent (level), "public {0} () {{", className);
			codeBuilderBean.AppendLine (Indent (level), "}");

			level--;
			codeBuilderBean.AppendLine (Indent (level), "}");





			level = 0;

			var codeBuilderIncludes = new StringBuilder ();
			codeBuilderIncludes.AppendLine ("using System;");
			codeBuilderIncludes.AppendLine ("using System.Collections.Generic;");
			codeBuilderIncludes.AppendLine ("using System.Linq;");
			codeBuilderIncludes.AppendLine ();
			codeBuilderIncludes.AppendLine ("using Dataelus.Mono.Extensions;");

			var codeBuilderBeanCollection = new StringBuilder ();
			//codeBuilderBeanCollection.AppendLine ();
			//if (!String.IsNullOrEmpty (classNamespace)) {
			//	codeBuilderBeanCollection.AppendLineFormat ("namespace {0} {{", classNamespace);
			//	level++;
			//}
			if (isSerializable)
				codeBuilderBeanCollection.AppendLine ("[Serializable]");
			string collectionClassName = className + "Collection";
			codeBuilderBeanCollection.AppendLineFormat ("{1}class {0} : {2}<{3}>, IDbTableCollection, System.Collections.IEnumerable {{", collectionClassName, classModifier, baseListClassName, className);
			level++;

			// Add constructor
			codeBuilderBeanCollection.AppendLineFormat (Indent (level), "public {0} () : base () {{", collectionClassName);
			codeBuilderBeanCollection.AppendLine (Indent (level), "}");

			// Add constructor - load from DataTable
			codeBuilderBeanCollection.AppendLine ();
			codeBuilderBeanCollection.AppendLineFormat (Indent (level), "public {0} (System.Data.DataTable table) : this () {{", collectionClassName);
			codeBuilderBeanCollection.AppendLine (Indent (level + 1), "AddFromTable(table);");
			codeBuilderBeanCollection.AppendLine (Indent (level), "}");

			// Add function: Add items from DataTable
			codeBuilderBeanCollection.AppendLine ();
			codeBuilderBeanCollection.AppendLine (Indent (level), "public void AddFromTable (System.Data.DataTable table) {");
			level++;
			codeBuilderBeanCollection.AppendLine (Indent (level), "foreach (System.Data.DataRow row in table.Rows) {");
			codeBuilderBeanCollection.AppendLine (Indent (level + 1), "Add(GetItem(row));");
			codeBuilderBeanCollection.AppendLine (Indent (level), "}");
			level--;
			codeBuilderBeanCollection.AppendLine (Indent (level), "}");

			// Add function: Get new item from DataRow
			codeBuilderBeanCollection.AppendLine ();
			codeBuilderBeanCollection.AppendLine (codeBuilderBeanAssignFromDataRow.ToString ());

			// Add function: explicit implementation of System.Collections.IEnumerable
			codeBuilderBeanCollection.AppendLine ();
			codeBuilderBeanCollection.AppendLine (Indent (level), "#region IEnumerable implementation");
			codeBuilderBeanCollection.AppendLine ();
			codeBuilderBeanCollection.AppendLine (Indent (level), "System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {");
			codeBuilderBeanCollection.AppendLine (Indent (level + 1), "return base.GetEnumerator ();");
			codeBuilderBeanCollection.AppendLine (Indent (level), "}");
			codeBuilderBeanCollection.AppendLine ();
			codeBuilderBeanCollection.AppendLine (Indent (level), "#endregion");

			// Add functions: Implementation of the IDbTableCollection interface
			codeBuilderBeanCollection.AppendLine ();
			codeBuilderBeanCollection.AppendLine (Indent (level), "#region IDbTableCollection implementation");
			codeBuilderBeanCollection.AppendLine ();
			codeBuilderBeanCollection.AppendLineFormat (Indent (level), "string _dbTableName = \"{0}\";", tableName);
			codeBuilderBeanCollection.AppendLine (Indent (level), "public string GetDbTableName () {");
			codeBuilderBeanCollection.AppendLine (Indent (level + 1), "return _dbTableName;");
			codeBuilderBeanCollection.AppendLine (Indent (level), "}");
			codeBuilderBeanCollection.AppendLine ();
			codeBuilderBeanCollection.AppendLine (Indent (level), "public virtual string GetSql () {");
			codeBuilderBeanCollection.AppendLine (Indent (level + 1), "return String.Format(\"SELECT * FROM {0}\", _dbTableName);");
			codeBuilderBeanCollection.AppendLine (Indent (level), "}");
			codeBuilderBeanCollection.AppendLine ();
			codeBuilderBeanCollection.AppendLine (Indent (level), "public virtual void LoadFromTable (System.Data.DataTable table) {");
			codeBuilderBeanCollection.AppendLine (Indent (level + 1), "AddFromTable(table);");
			codeBuilderBeanCollection.AppendLine (Indent (level), "}");
			codeBuilderBeanCollection.AppendLine ();
			codeBuilderBeanCollection.AppendLine (Indent (level), "public virtual Dictionary<string, string> GetDBFieldToPropNames () {");
			codeBuilderBeanCollection.AppendLine (Indent (level + 1), "var dict = new Dictionary<string, string> ();");

			codeBuilderBeanCollection.AppendLine ();
			foreach (var item1 in dictColNamesPropNames) {
				codeBuilderBeanCollection.AppendLineFormat (Indent (level + 1), "dict.Add (\"{0}\", \"{1}\");", item1.Key, item1.Value);
			}
			codeBuilderBeanCollection.AppendLine ();

			codeBuilderBeanCollection.AppendLine (Indent (level + 1), "return dict;");
			codeBuilderBeanCollection.AppendLine (Indent (level), "}");
			codeBuilderBeanCollection.AppendLine ();
			codeBuilderBeanCollection.AppendLine (Indent (level), "#endregion");

			level--;
			codeBuilderBeanCollection.AppendLine (Indent (level), "}");

			//if (!String.IsNullOrEmpty (classNamespace)) {
			//	codeBuilderBeanCollection.AppendLine ("}");
			//	level--;
			//}


			var component = new BeanCodeComponents ();
			component.BeanClassName = className;
			component.BeanCollectionClassName = collectionClassName;
			component.BeanClass = codeBuilderBean.ToString ();
			component.BeanCollectionClass = codeBuilderBeanCollection.ToString ();
			component.Includes = codeBuilderIncludes.ToString ();

			if (!String.IsNullOrEmpty (classNamespace))
				component.Namespace = classNamespace;

			return component;
		}

		public BeanCodeComponents BuildDbMasterClass (IEnumerable<BeanCodeComponents> beanClasses)
		{
			var builder1 = new StringBuilder ();

			int levelAsgn = 1;

			var dictObjects = new Dictionary<string, string> ();
			foreach (var bean in beanClasses) {
				if (String.IsNullOrWhiteSpace (bean.BeanCollectionClassName))
					continue;
				
				string pluralName = bean.BeanClassName + (bean.BeanClassName.EndsWith ("s") ? "es" : "s");

				dictObjects.Add (pluralName, bean.BeanCollectionClassName);
			}

			string masterClassName = "DatabaseTableObjects";

			builder1.AppendLine ("// Database of table collections");
			builder1.AppendLineFormat ("public class {0} {{", masterClassName);


			foreach (var item in dictObjects) {
				builder1.AppendLineFormat (Indent (levelAsgn), "public {1} {0} {{ get; set; }}", item.Key, item.Value);
			}

			builder1.AppendLine ();
			builder1.AppendLineFormat (Indent (levelAsgn), "public {0} () {{", masterClassName);
			foreach (var item in dictObjects) {
				builder1.AppendLineFormat (Indent (levelAsgn + 1), "this.{0} = new {1} ();", item.Key, item.Value);
			}
			builder1.AppendLine (Indent (levelAsgn), "}");

			builder1.AppendLine (Indent (levelAsgn), "public object[] GetCollections () {");

			builder1.AppendLine (Indent (levelAsgn + 1), "object[] collections = new object[] {");

			bool isFirst = true;
			foreach (var collName in dictObjects.Keys) {
				builder1.AppendLineFormat (Indent (levelAsgn + 2), "{1}this.{0}", collName, isFirst ? " " : ",");
				if (isFirst)
					isFirst = false;
			}

			builder1.AppendLine (Indent (levelAsgn + 1), "};");
			builder1.AppendLine (Indent (levelAsgn + 1), "return collections;");
			builder1.AppendLine (Indent (levelAsgn), "}");

			builder1.AppendLine ("}");

			var masterClass = new BeanCodeComponents ();
			masterClass.Includes = String.Join (Environment.NewLine, new string[] {
					"using System;",
					"using System.Collections.Generic;"
				});
			masterClass.Namespace = this.RootNamespace;
			masterClass.BeanClassName = masterClassName;
			masterClass.BeanClass = builder1.ToString ();

			return masterClass;
		}

		public BeanCodeComponents[] GetInterfaces ()
		{
			var lst = new List<BeanCodeComponents> ();

			var builder1 = new StringBuilder ();

			int levelAsgn = 1;

			builder1.AppendLine ("// Interface for a Database Table collection");
			builder1.AppendLine ("public interface IDbTableCollection {");
			builder1.AppendLine (Indent (levelAsgn), "string GetDbTableName ();");
			builder1.AppendLine (Indent (levelAsgn), "string GetSql ();");
			builder1.AppendLine (Indent (levelAsgn), "void LoadFromTable (System.Data.DataTable table);");
			builder1.AppendLine (Indent (levelAsgn), "Dictionary<string, string> GetDBFieldToPropNames ();");
			builder1.AppendLine ("}");

			var ifSqlLoader = new BeanCodeComponents ();
			ifSqlLoader.Includes = String.Join (Environment.NewLine, new string[] {
					"using System;",
					"using System.Collections.Generic;"
				});
			ifSqlLoader.Namespace = this.RootNamespace;
			ifSqlLoader.BeanClassName = "IDBTableCollection";
			ifSqlLoader.BeanClass = builder1.ToString ();

			lst.Add (ifSqlLoader);

			builder1.Clear ();

			return lst.ToArray ();
		}

		public static string PropertyNameConverter (string fieldName)
		{
			return Regex.Replace (fieldName, @"\W", "").UnderscoreToSpace ().ToTitleCase ().CapsAcronyms ("ID").RemoveSpace ();
		}
	}


	/// <summary>
	/// String extensions for code creation.
	/// </summary>
	public static class StringExtensions
	{
		public static string UnderscoreToSpace (this string text)
		{
			return Regex.Replace (text, @"[_]", " ");
		}

		public static string ToTitleCase (this string text)
		{
			// Source: http://stackoverflow.com/questions/5820488/vb-net-how-to-camel-case-words-that-are-uppercased
			return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase (text.ToLower ());
		}

		/// <summary>
		/// Puts the given acronymns in Caps.
		/// </summary>
		/// <returns>The acronyms.</returns>
		/// <param name="text">Text.</param>
		/// <param name="acronyms">Acronyms.</param>
		public static string CapsAcronyms (this string text, params string[] acronyms)
		{
			foreach (var acronym in acronyms) {
				text = Regex.Replace (text, @"\b" + acronym + @"\b", acronym.ToUpper (), RegexOptions.IgnoreCase);
			}
			return text;
		}

		public static string RemoveSpace (this string text)
		{
			return Regex.Replace (text, @"\s+", "");
		}
	}
}

