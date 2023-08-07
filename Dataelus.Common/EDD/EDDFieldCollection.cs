using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.EDD
{
	public class EDDFieldCollection : CollectionBase<EDDField>, System.Collections.IEnumerable
	{
		public EDDFieldCollection ()
			: base ()
		{
		}

		public EDDFieldCollection (IEnumerable<Database.DBField> dbFields)
			: this ()
		{
			AddByField (dbFields);
		}

		public EDDField FindByColumnName (string eddColumnName)
		{
			return FindByColumnName (eddColumnName, new StringEqualityComparer ());
		}

		public EDDField FindByColumnName (string eddColumnName, IEqualityComparer<string> comparer)
		{
			return _items.Find (x => comparer.Equals (x.EDDColumnName, eddColumnName));
		}

		/// <summary>
		/// Adds columns by using the given Database fields.
		/// </summary>
		/// <param name="dbFields">Db fields.</param>
		public void AddByField (IEnumerable<Database.DBField> dbFields)
		{
			foreach (var item in dbFields) {
				AddByField (item);
			}
		}

		/// <summary>
		/// Adds a column by using the given Database field.
		/// </summary>
		/// <param name="dbField">Db field.</param>
		public EDDField AddByField (Database.DBField dbField)
		{
			var newItem = new EDDField ();
			newItem.DBDataField = dbField;
			newItem.EDDColumnName = dbField.FieldName;
			return this.Add (newItem);
		}

		public new EDDField Add (EDDField item)
		{
			base.Add (item);
			return item;
		}

		#region CSV File Methods

		/// <summary>
		/// Gets the list of CSV lines, including a line using the given Constraints.
		/// </summary>
		/// <returns>The list of CSV lines.</returns>
		/// <param name="rowIndexDataTypes">Row index data types.</param>
		/// <param name="rowIndexReference">Row index reference.</param>
		/// <param name="rowIndexColumnNames">Row index column names.</param>
		/// <param name="rowIndexReqd">Row index reqd.</param>
		/// <param name="constraints">Constraints.</param>
		/// <param name="appendMultipleConstraints">If set to <c>true</c>, then appends multiple constraints.</param>
		public virtual List<string[]> GetListCSV (int rowIndexDataTypes, int rowIndexReference, int rowIndexColumnNames, int rowIndexReqd
			, Database.DBConstraintCollection constraints, bool appendMultipleConstraints)
		{
			var lst = GetListCSV (rowIndexDataTypes, rowIndexColumnNames, rowIndexReqd);

			AddLinesCSV (lst, rowIndexReference, GetCellsConstraints (constraints, appendMultipleConstraints), true);

			return lst;
		}

		/// <summary>
		/// Gets the list of cell values, prepared for a CSV file.
		/// </summary>
		/// <returns>The list CS.</returns>
		public virtual List<string[]> GetListCSV (int rowIndexDataTypes, int rowIndexColumnNames, int rowIndexReqd)
		{
			var lst = new List<string[]> ();

			// Column Data Types
			AddLinesCSV (lst, rowIndexDataTypes, GetColumnDataTypes (), true);

			// Column Names
			AddLinesCSV (lst, rowIndexColumnNames, GetColumnNames (), true);

			// Column Nullable
			AddLinesCSV (lst, rowIndexReqd, GetColumnNullableIndicator (), true);

			return lst;
		}

		/// <summary>
		/// Adds the additionalLines to the given list of CSV lines.
		/// </summary>
		/// <param name="lines">Existing list of CSV Lines.</param>
		/// <param name="additionalLines">Additional lines (key=row index, value=row cells).</param>
		public virtual void AddLinesCSV (List<string[]> lines, Dictionary<int, string[]> additionalLines)
		{
			foreach (var item in additionalLines) {
				AddLinesCSV (lines, item.Key, item.Value, true);
			}
		}

		/// <summary>
		/// Adds the [cells] as a row (at the given index) to the given list of CSV lines.
		/// </summary>
		/// <param name="lines">Existing list of CSV Lines.</param>
		/// <param name="index">The row index, at which cells should be inserted.</param>
		/// <param name="cells">Adds this array of cells to the given lines</param>
		/// <param name="isComment">Whether to comment the cells</param>
		public virtual void AddLinesCSV (List<string[]> lines, int index, string[] cells, bool isComment)
		{
			if (isComment)
				CommentRow (cells);
			AddRowAt (lines, cells, index);
		}

		public virtual string[] GetCellsConstraints (Database.DBConstraintCollection constraints, bool isAppendMultiple)
		{
			return GetCellsConstraints (constraints, isAppendMultiple, "; ");
		}

		/// <summary>
		/// Gets the cells which enumerate the constraints on each field.
		/// </summary>
		/// <returns>The cells constraints.</returns>
		/// <param name="constraints">Constraints.</param>
		/// <param name="isAppendMultiple">Whether to concatenate multiple constraints (if false, just lists one)</param>
		/// <param name="separator">The separator between multiple constraints for a given column.</param>
		public virtual string[] GetCellsConstraints (Database.DBConstraintCollection constraints, bool isAppendMultiple, string separator)
		{
			var cells = EmptyRow (false);

			for (int i = 0; i < _items.Count; i++) {
				Database.DBConstraint[] fieldConstraints;
				if (constraints.GetConstraintsSingleField (_items [i].DBDataField, out fieldConstraints)) {

					if (fieldConstraints.Length > 0) {

						var lstConstraints = new List<string> ();
						for (int j = 0; j < fieldConstraints.Length; j++) {
							if (j > 0) {
								// More than one single-field constraint
								if (!isAppendMultiple)
									break;
							}
							// Get the first constraint column
							// Remember - the Key is the reference table
							var refTable = fieldConstraints [j].FirstColumnSource ();

							lstConstraints.Add (String.Format ("{0}.{1}", refTable.TableName, refTable.FieldName));
						}
						cells [i] = String.Join (separator, lstConstraints);
					}
				}
			}

			return cells;
		}

		/// <summary>
		/// Assigns the references for each column.
		/// </summary>
		/// <param name="constraints">Constraints.</param>
		/// <param name="columnSchema">Column schema.</param>
		public virtual void AssignReferences (Database.DBConstraintCollection constraints, Database.DBFieldCollection columnSchema)
		{
			for (int i = 0; i < _items.Count; i++) {
				Database.DBConstraint[] fieldConstraints;
				if (constraints.GetConstraintsSingleField (_items [i].DBDataField, out fieldConstraints)) {

					if (fieldConstraints.Length > 0) {

						for (int j = 0; j < fieldConstraints.Length; j++) {
							if (j > 0) {
								// More than one single-field constraint
								break;
							}
							// Get the first constraint column
							// Remember - the Key is the reference table
							var refTable = fieldConstraints [j].FirstColumnSource ();

							_items [i].DBReferenceField = columnSchema.Find (refTable);
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets the array of column names.
		/// </summary>
		/// <returns>The column names.</returns>
		protected virtual string[] GetColumnNames ()
		{
			int fieldCount = _items.Count;
			string[] cells = new string[fieldCount];
			for (int i = 0; i < fieldCount; i++) {
				cells [i] = GetColumnName (_items [i]);
			}
			return cells;
		}

		/// <summary>
		/// Gets the array of column data types.
		/// </summary>
		/// <returns>The column data types.</returns>
		protected virtual string[] GetColumnDataTypes ()
		{
			int fieldCount = _items.Count;
			string[] cells = new string[fieldCount];
			for (int i = 0; i < fieldCount; i++) {
				cells [i] = GetTypeString (_items [i].DBDataField);
			}
			return cells;
		}

		/// <summary>
		/// Gets the array of column nullable indicators (whether a column is nullable).
		/// </summary>
		/// <returns>The column nullable.</returns>
		protected virtual string[] GetColumnNullableIndicator ()
		{
			int fieldCount = _items.Count;
			string[] cells = new string[fieldCount];
			for (int i = 0; i < fieldCount; i++) {
				cells [i] = GetNullableString (_items [i].DBDataField);
			}
			return cells;
		}

		/// <summary>
		/// Adds the given row of cells to the rows list, at the given index.
		/// Fills in with empty cells, if necessary
		/// </summary>
		/// <param name="rows">Rows.</param>
		/// <param name="cells">Cells.</param>
		/// <param name="index">Index.</param>
		protected virtual void AddRowAt (List<string[]> rows, string[] cells, int index)
		{
			// While the Row Count is less than or equal to the index
			// Keep adding empty rows
			while (index >= rows.Count) {
				rows.Add (EmptyRow ());
			}
			rows [index] = cells;
		}

		/// <summary>
		/// Gets an Empty row (with a comment).
		/// </summary>
		/// <returns>The row.</returns>
		protected virtual string[] EmptyRow ()
		{
			return EmptyRow (true);
		}

		/// <summary>
		/// Gets an Empty row.
		/// </summary>
		/// <returns>The row.</returns>
		/// <param name="isComment">If set to <c>true</c>, then a comment tag is pre-pended to the first cell.</param>
		protected virtual string[] EmptyRow (bool isComment)
		{
			var cells = new string[_items.Count];
			if (isComment)
				CommentRow (cells);
			return cells;
		}

		/// <summary>
		/// Commens the row, by prepending a '#' to the first cell.
		/// </summary>
		/// <param name="cells">Cells.</param>
		protected virtual void CommentRow (string[] cells)
		{
			cells [0] = String.Format ("# {0}", cells [0]);
		}

		/// <summary>
		/// Gets the type string for the given field (just the data type; appends the Max Length if > 0).
		/// </summary>
		/// <returns>The type string.</returns>
		/// <param name="field">Field.</param>
		public virtual string GetTypeString (Database.IDBField field)
		{
			if (field.MaxLength > 0)
				return String.Format ("{0}({1})", field.DataType, field.MaxLength);
			return field.DataType;
		}

		/// <summary>
		/// Gets the nullable string for the given field.
		/// (Default: nullable=>"", non-nullable=>"Required")
		/// </summary>
		/// <returns>The nullable string.</returns>
		/// <param name="field">Field.</param>
		public virtual string GetNullableString (Database.IDBField field)
		{
			return field.Nullable ? "" : "Required";
		}

		/// <summary>
		/// Gets the name of the column.
		/// </summary>
		/// <returns>The column name.</returns>
		/// <param name="field">Field.</param>
		public virtual string GetColumnName (EDDField field)
		{
			return field.EDDColumnName;
		}

		#endregion

		#region System.Collections.IEnumerable implemenation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return base.GetEnumerator ();
		}

		#endregion
	}
}

