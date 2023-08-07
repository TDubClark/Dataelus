using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Database
{
	/// <summary>
	/// Represents a Database Constraint between one or more columns.
	/// </summary>
	public class DBConstraint : IEquatable<DBConstraint>
	{
		protected string _constraintName;

		/// <summary>
		/// The name of the constraint.
		/// </summary>
		public string ConstraintName {
			get { return _constraintName; }
			set { _constraintName = value; }
		}

		protected DBConstraintColumnCollection _constraintColumns;

		/// <summary>
		/// The constraint columns; the Source Table is the Key, and the table which refers to the source is the value.
		/// Ex: if table_data.location_id is a FK to table_location.location_id, then key=table_location, value=table_data.
		/// </summary>
		public DBConstraintColumnCollection ConstraintColumns {
			get { return _constraintColumns; }
			set { _constraintColumns = value; }
		}

		/// <summary>
		/// Firsts the column source.
		/// </summary>
		/// <returns>The column source.</returns>
		public DBFieldSimple FirstColumnSource ()
		{
			return _constraintColumns [0].ReferencedColumn;
		}

		/// <summary>
		/// Firsts the column source.
		/// </summary>
		/// <returns>The column source.</returns>
		public DBFieldSimple FirstColumnSource (string fieldName, IEqualityComparer<string> fieldNameComparer)
		{
			return _constraintColumns.First (x => fieldNameComparer.Equals (x.Column.FieldName, fieldName)).ReferencedColumn;
		}

		/// <summary>
		/// The comparer between fields.
		/// </summary>
		protected DBFieldSimpleComparer _comparer;

		/// <summary>
		/// Gets or sets the comparer between fields.
		/// </summary>
		/// <value>The comparer.</value>
		public DBFieldSimpleComparer Comparer {
			get { return _comparer; }
			set { _comparer = value; }
		}

		/// <summary>
		/// Determines whether the table [tableName] is constrained by the values in table [referenceTablename].
		/// </summary>
		/// <returns><c>true</c> if [tableName] is constrained by [referenceTableName]; otherwise, <c>false</c>.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="referenceTableName">Reference Table name.</param>
		public bool IsConstrainedBy (string tableName, string referenceTableName)
		{
			return _constraintColumns.Any (x => 
				_comparer.FieldComparer.Equals (x.Column.TableName, tableName) &&
				_comparer.FieldComparer.Equals (x.ReferencedColumn.TableName, referenceTableName)
			);
		}

		/// <summary>
		/// Determines whether there is a match with a target table for the specified tableName and fieldname.
		/// </summary>
		/// <returns><c>true</c> if there is a target-field match for the specified tableName fieldname; otherwise, <c>false</c>.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldname">Fieldname.</param>
		public bool IsMatchTarget (string tableName, string fieldname)
		{
			return _constraintColumns.Any (x => _comparer.Equals (x.Column, new DBFieldSimple (x.Column.SchemaName, tableName, fieldname)));
		}

		/// <summary>
		/// Determines whether there is a match with a target table for the specified tableName.
		/// </summary>
		/// <returns><c>true</c> if there is a target-field match for the specified tableName fieldname; otherwise, <c>false</c>.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="tableNameComparer">The equality comparer for table names</param>
		public bool IsMatchTarget (string tableName, IEqualityComparer<string> tableNameComparer)
		{
			return _constraintColumns.Any (x => tableNameComparer.Equals (x.Column.TableName, tableName));
		}

		public bool IsMatchTarget (IDBFieldSimple field)
		{
			return _constraintColumns.Any (x => x.Column.Equals (field, _comparer.FieldComparer));
		}

		/// <summary>
		/// Adds the column to the constraint list.
		/// </summary>
		/// <param name="sourceTableName">Source table name.</param>
		/// <param name="sourceFieldName">Source field name.</param>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldName">Field name.</param>
		public void AddColumn (string sourceTableName, string sourceFieldName, string tableName, string fieldName)
		{
			_constraintColumns.Add (new DBFieldSimple (tableName, fieldName), new DBFieldSimple (sourceTableName, sourceFieldName));
		}

		public DBConstraint (string constraintName, IEqualityComparer<string> fieldComparer)
		{
			_constraintName = constraintName;
			_comparer = new DBFieldSimpleComparer (fieldComparer);
			_constraintColumns = new DBConstraintColumnCollection ();
		}

		#region IEquatable implementation

		public bool Equals (DBConstraint other)
		{
			return Equals (other, new StringEqualityComparer ());
		}

		#endregion

		public bool Equals (DBConstraint other, IEqualityComparer<string> nameComparer)
		{
			return nameComparer.Equals (this.ConstraintName, other.ConstraintName);
		}

		public override string ToString ()
		{
			return string.Format ("[DBConstraint: ConstraintName={0}, ConstraintColumns={1}, Comparer={2}]", ConstraintName, ConstraintColumns, Comparer);
		}
	}

	public class DBConstraintEqualityComparer : IEqualityComparer<DBConstraint>
	{
		IEqualityComparer<string> _nameComparer;

		public DBConstraintEqualityComparer (IEqualityComparer<string> nameComparer)
		{
			_nameComparer = nameComparer;
		}

		#region IEqualityComparer implementation

		public bool Equals (DBConstraint x, DBConstraint y)
		{
			return _nameComparer.Equals (x.ConstraintName, y.ConstraintName);
		}

		public int GetHashCode (DBConstraint obj)
		{
			return obj.ConstraintName.GetHashCode ();
		}

		#endregion
	}

	public class DBConstraintPrioritizerComparer : IComparer<Database.DBConstraint>
	{
		protected IComparer<string> _tableDepenencyComparer;

		/// <summary>
		/// Gets or sets the table dependency comparer.
		/// </summary>
		/// <value>The table dependency comparer.</value>
		public IComparer<string> TableDependencyComparer {
			get { return _tableDepenencyComparer; }
			set { _tableDepenencyComparer = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBConstraintPrioritizerComparer"/> class.
		/// </summary>
		/// <param name="tableDepenencyComparer">Table depenency comparer (most dependent table first).</param>
		public DBConstraintPrioritizerComparer (IComparer<string> tableDepenencyComparer)
		{
			if (tableDepenencyComparer == null)
				throw new ArgumentNullException ("tableDepenencyComparer");
			_tableDepenencyComparer = tableDepenencyComparer;
		}

		#region IComparer implementation

		public int Compare (DBConstraint x, DBConstraint y)
		{
			return _tableDepenencyComparer.Compare (x.ConstraintColumns.First ().Column.TableName, y.ConstraintColumns.First ().Column.TableName);
		}

		#endregion
	}
}

