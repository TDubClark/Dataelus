using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.EDD
{
	/*
	Database Table Editor - for current clients, and potentially other applications
		Needs to work with subsets of a database table - certain fields only
		View/Controller Interfaces

	Takes:
	- Data:
		The Database Columns Schema/System table
		The Database Constraints table (along with other unassigned constraints)
		The Table Name to be edited
		Any columns which are excluded from editing

	- Logic:
		Column-level type evaluators

	Flexibility Required:
		Other business logic at the row level (such as: does the data exist)
		Other logic at the column-level when validating data (ex: string “yes” => boolean true)
	*/
	/// <summary>
	/// Methods for building Editor data for database tables.
	///  - Create EDD fields for a given database table.
	///  - Create an ObjectTable using EDD fields and a type converter.
	/// </summary>
	public class EditorData
	{
		private const string sClassName = "EditorData";

		public Dataelus.Database.DBFieldCollection DatabaseFields { get; set; }

		public Dataelus.Database.DBConstraintCollection ConstraintsList { get; set; }

		/// <summary>
		/// Gets or sets the user-defined list of simple constraints.
		/// </summary>
		/// <value>The simple constraints list (user-defined).</value>
		public Dataelus.Database.DBConstraintColumnCollection SimpleConstraintsListUserDefined { get; set; }

		public Log.ILogManager ErrorLogger { get; set; }

		public EditorData ()
		{
			this.ConstraintsList = new Dataelus.Database.DBConstraintCollection ();
			this.DatabaseFields = new Dataelus.Database.DBFieldCollection ();
			this.SimpleConstraintsListUserDefined = new Dataelus.Database.DBConstraintColumnCollection ();
			this.ErrorLogger = null;
		}

		public EditorData (
			Dataelus.Database.DBFieldCollection databaseFields
			, Dataelus.Database.DBConstraintCollection constraintsList
		)
		{
			this.DatabaseFields = databaseFields;
			this.ConstraintsList = constraintsList;
			this.SimpleConstraintsListUserDefined = new Dataelus.Database.DBConstraintColumnCollection ();
			this.ErrorLogger = null;
		}

		/// <summary>
		/// Gets the EDD fields for the given table (except for the given list of excluded fields).
		/// </summary>
		/// <returns>The edd fields.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="excludedFields">Excluded fields.</param>
		public Dataelus.EDD.EDDFieldCollection GetEddFields (string tableName, List<string> excludedFields)
		{
			return GetEddFields (tableName, excludedFields, null);
		}

		/// <summary>
		/// Gets the EDD fields for the given table (except for the given list of excluded fields).
		/// </summary>
		/// <returns>The edd fields.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="excludedFields">Excluded fields.</param>
		/// <param name="typeConverter">Type converter for Database Field type names.</param>
		public Dataelus.EDD.EDDFieldCollection GetEddFields (string tableName, List<string> excludedFields, ITypeConverter typeConverter)
		{
			var comparer = new Dataelus.StringEqualityComparer ();
			// Get the database fields for the given table, but which are not in the exclusion list
			var fields = this.DatabaseFields.Where (x => comparer.Equals (x.TableName, tableName) && !excludedFields.Contains (x.FieldName, comparer)).ToList ();

			return GetEDDFields (typeConverter, fields, true);
		}

		/// <summary>
		/// Gets the EDD fields for the given table (including only the given list of included fields).
		/// </summary>
		/// <returns>The edd fields.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="includedFields">Included fields.</param>
		/// <param name="typeConverter">Type converter for Database Field type names.</param>
		public Dataelus.EDD.EDDFieldCollection GetEddFieldsInclude (string tableName, List<string> includedFields, ITypeConverter typeConverter)
		{
			var comparer = new Dataelus.StringEqualityComparer ();
			// Get the database fields for the given table, but which are not in the exclusion list
			var fields = this.DatabaseFields.Where (x => comparer.Equals (x.TableName, tableName) && includedFields.Contains (x.FieldName, comparer)).ToList ();

			return GetEDDFields (typeConverter, fields, true);
		}

		/// <summary>
		/// Gets the EDD field collection.
		/// </summary>
		/// <returns>The EDD fields.</returns>
		/// <param name="typeConverter">Type converter.</param>
		/// <param name="fields">Fields.</param>
		/// <param name="scanMultiFieldFK">If set to <c>true</c> scan multi field F.</param>
		internal Dataelus.EDD.EDDFieldCollection GetEDDFields (ITypeConverter typeConverter, List<Dataelus.Database.DBField> fields, bool scanMultiFieldFK)
		{
			var fieldNameComparer = new StringEqualityComparer ();

			// First, sort the fields
			fields.Sort (new Dataelus.Database.DBFieldComparer ());

			// Create the EDD Field collection using the database fields
			// Apply any constraints found
			var eddFields = new Dataelus.EDD.EDDFieldCollection ();

			foreach (var item in fields) {
				var newField = eddFields.AddByField (item);

				Dataelus.Database.DBFieldSimple dbField = null;

				// Search single-field constraints
				Dataelus.Database.DBConstraint[] constraints;
				if (ConstraintsList.GetConstraintsSingleField (item, out constraints)) {
					dbField = constraints [0].FirstColumnSource ();
				} else {
					// Single-field constraint was not found

					if (scanMultiFieldFK) {
						// Check multi-field constraints
						if (ConstraintsList.GetConstraintsMultiField (item, out constraints)) {
							dbField = constraints [0].FirstColumnSource (item.FieldName, fieldNameComparer);
						}
					}

					if (dbField == null) {
						// Multi-field constraint was not found

						// Scan user-defined list
						Database.DBConstraintColumn simpleConstraint;
						if (SimpleConstraintsListUserDefined.FindReference (item, out simpleConstraint, fieldNameComparer)) {
							dbField = simpleConstraint.ReferencedColumn;
						}
					}
				}

				if (dbField != null)
					newField.DBReferenceField = DatabaseFields.Find (dbField);

				// Apply types
				if (typeConverter != null) {
					try {
						var dataType = typeConverter.GetDataType (item.DataType);
						newField.TextInterpretter = TextInputInterpreter.GetInterpreterByType (dataType, item);
						newField.TextParser = TextInputParser.GetParserByType (dataType);
					} catch (Exception exInner) {
						if (this.ErrorLogger != null) {
							this.ErrorLogger.LogError (DateTime.Now, String.Format ("Error in {0}.{1}() :: {2}", sClassName, "GetEDDFields", exInner));
						}
					}
				}
			}
			return eddFields;
		}

		/// <summary>
		/// Applies the constraints, sorted.
		/// </summary>
		/// <param name="eddFields">Edd fields.</param>
		public void ApplyConstraintsSorted (Dataelus.EDD.EDDFieldCollection eddFields)
		{
			ApplyConstraintsSorted (eddFields, new Database.DBConstraintPrioritizerComparer (this.ConstraintsList.GetDependancyComparer ()), ConstraintPosition.Last);
		}

		/// <summary>
		/// Applies the constraints to any fields where no reference field is specified.
		/// </summary>
		/// <param name="eddFields">Edd fields.</param>
		/// <param name="constraintSorter">Sorter for a list of constraints, where the preferred constraint will appear first</param>
		public void ApplyConstraintsSorted (Dataelus.EDD.EDDFieldCollection eddFields, IComparer<Database.DBConstraint> constraintSorter)
		{
			ApplyConstraintsSorted (eddFields, constraintSorter, ConstraintPosition.First);
		}

		public enum ConstraintPosition
		{
			First,
			Last
		}

		public void ApplyConstraintsSorted (Dataelus.EDD.EDDFieldCollection eddFields, IComparer<Database.DBConstraint> constraintSorter, ConstraintPosition position)
		{
			ApplyConstraints (eddFields, lstConstraints => {
					// Sort the constraints, if a comparer is given
					// This should sort the constraints by table priority
					if (constraintSorter != null)
						lstConstraints.Sort (constraintSorter);
				
					if (lstConstraints.Count > 0) {
						switch (position) {
							case ConstraintPosition.First:
								return lstConstraints.First ();
							case ConstraintPosition.Last:
								return lstConstraints.Last ();
						}
					}
				
					return null;
				});
		}

		/// <summary>
		/// Applies the constraints.
		/// </summary>
		/// <param name="eddFields">Edd fields.</param>
		/// <param name="constraintSelector">Constraint selector, from a list of one or more constraints.</param>
		public void ApplyConstraints (Dataelus.EDD.EDDFieldCollection eddFields, Func<List<Database.DBConstraint>, Database.DBConstraint> constraintSelector)
		{
			for (int i = 0; i < eddFields.Count; i++) {
				if (eddFields [i].DBReferenceField == null) {
					Database.DBConstraint[] fieldConstraints;
					if (this.ConstraintsList.GetConstraints (eddFields [i].DBDataField, out fieldConstraints)) {
						var lst = fieldConstraints.ToList ();

						if (lst.Count > 0) {
							// Get the constraint column which has this DB field as a column
							var constraintColumn1 = constraintSelector (lst).ConstraintColumns.First (x => x.Column.Equals (eddFields [i].DBDataField));
							if (constraintColumn1 != null) {
								var fieldSimple = constraintColumn1.ReferencedColumn;
								eddFields [i].DBReferenceField = this.DatabaseFields.Find (fieldSimple);
							}
						}
					}
				}
			}
		}

		public static Dataelus.EDD.EDDFieldCollection GetEDDFields (ITypeConverter typeConverter, IEnumerable<Dataelus.EDD.EDDField> fields, Func<Dataelus.EDD.EDDField, string> funcGetTypeString)
		{
			var eddFields = new Dataelus.EDD.EDDFieldCollection ();
			foreach (var item in fields) {
				var newField = eddFields.Add (item);
				// Apply types
				if (typeConverter != null) {
					try {
						string typeString = funcGetTypeString (item);
						if (!String.IsNullOrWhiteSpace (typeString)) {
							var dataType = typeConverter.GetDataType (typeString);
							newField.TextInterpretter = TextInputInterpreter.GetInterpreterByType (dataType, item.DBDataField);
							newField.TextParser = TextInputParser.GetParserByType (dataType);
						}
					} catch (Exception exInner) {
//						if (this.ErrorLogger != null) {
//							this.ErrorLogger.LogError (DateTime.Now, String.Format ("Error in {0}.{1}() :: {2}", sClassName, "GetEDDFields", exInner));
//						}
					}
				}
			}
			return eddFields;
		}

		/// <summary>
		/// function which builds an Object table from an EDD Field list
		/// </summary>
		/// <returns>The object table.</returns>
		/// <param name="fields">EDD Fields collection.</param>
		/// <param name="typeConverter">Type converter (between database type names and .NET/Mono Types).</param>
		public static Dataelus.Table.ObjectTable GetObjectTable (Dataelus.EDD.EDDFieldCollection fields, ITypeConverter typeConverter)
		{
			return new Dataelus.Table.ObjectTable (GetObjectColumns (fields, typeConverter), new Dataelus.Table.ObjectRowCollection ());
		}

		/// <summary>
		/// function which builds a Object table columns from an EDD Field list
		/// </summary>
		/// <returns>The object columns.</returns>
		/// <param name="fields">EDD Fields collection.</param>
		/// <param name="typeConverter">Type converter (between database type names and .NET/Mono Types).</param>
		public static Dataelus.Table.ObjectColumnCollection GetObjectColumns (Dataelus.EDD.EDDFieldCollection fields, ITypeConverter typeConverter)
		{
			var cols = new Dataelus.Table.ObjectColumnCollection ();
			for (int i = 0; i < fields.Count; i++) {
				var field = fields [i];
				var dbField = field.DBDataField;
				cols.AddColumn (new Dataelus.Table.ObjectColumn (dbField.FieldName, typeConverter.GetDataType (dbField.DataType), field.TextParser, field.TextInterpretter));
			}
			return cols;
		}
	}
}

