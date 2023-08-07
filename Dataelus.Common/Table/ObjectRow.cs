using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Table
{
	/// <summary>
	/// Row of Object values.
	/// </summary>
	public class ObjectRow : Generic.RowBase<object>, IUniqueIdentifier
	{
		protected ObjectTable _parentTable;

		/// <summary>
		/// Gets or sets the parent table.
		/// </summary>
		/// <value>The parent table.</value>
		[Newtonsoft.Json.JsonIgnore]
		[System.Xml.Serialization.XmlIgnore]
		public ObjectTable ParentTable {
			get { return _parentTable; }
			internal set { _parentTable = value; }
		}

		/// <summary>
		/// Occurs when this row is changed.
		/// </summary>
		public event RowChangedEventHandler RowChanged;

		/// <summary>
		/// Raises the row changed event.
		/// </summary>
		/// <param name="args">Arguments.</param>
		public void OnRowChanged (ObjectRowChangedEventArgs args)
		{
			if (RowChanged != null) {
				RowChanged (this, args);
			}
		}

		protected override void OnUpdated (object priorValue, object newValue, int columnIndex)
		{
			base.OnUpdated (priorValue, newValue, columnIndex);
			OnRowChanged (new ObjectRowChangedEventArgs (this, _parentTable.Columns [columnIndex], priorValue, newValue));
		}

		/// <summary>
		/// Sets the value at the given index.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="columnIndex">Column index.</param>
		protected override void SetValue (object value, int columnIndex)
		{
			if (_parentTable == null)
				throw new NullReferenceException ("Parent Table pointer is Null.");

			var parentColumn = _parentTable.Columns [columnIndex];
			if (parentColumn == null)
				throw new NullReferenceException (String.Format ("Parent Table column at index {0} is Null.", columnIndex));

			var eqComparer = GetEqualityComparer (columnIndex);
			if (eqComparer == null)
				throw new NullReferenceException (String.Format ("Parent Table column at index {0} has a Null value Equality Comparer.", columnIndex));
			
			string textValue = value as string;
			if (textValue != null) {
				if (parentColumn.TextInterpreter == null)
					throw new NullReferenceException (String.Format ("Parent Table column at index {0} has a Null Text Interpreter.", columnIndex));
				if (parentColumn.TextParser == null)
					throw new NullReferenceException (String.Format ("Parent Table column at index {0} has a Null Text Parser.", columnIndex));


				string interprettedValue;
				string message;
				if (parentColumn.TextInterpreter.Interpret (textValue, null, out interprettedValue, out message)) {
					if (parentColumn.TextInterpreter.IsNull (interprettedValue)) {
//						SetEditState (eqComparer, _values [columnIndex], null);
						base.SetValue (null, columnIndex);
					} else {
						object outValue;
						if (parentColumn.TextParser.TryParse (interprettedValue, out outValue)) {
//							SetEditState (eqComparer, _values [columnIndex], outValue);
							base.SetValue (outValue, columnIndex);
						} else {
							throw new FormatException (String.Format ("Could not parse value '{0}'; suggested format: {1}.", textValue, parentColumn.TextParser.GetFormatSuggestion ()));
						}
					}
				} else {
					throw new InvalidValueException (message, textValue, parentColumn.ColumnName);
				}
			} else {
//				SetEditState (eqComparer, _values [columnIndex], value);
				base.SetValue (value, columnIndex);
			}
		}

		/// <summary>
		/// Sets the edit-state of this record.
		/// </summary>
		/// <param name="comparer">Equality Comparer.</param>
		/// <param name="oldValue">Old value.</param>
		/// <param name="newValue">New value.</param>
		protected virtual void SetEditState (System.Collections.Generic.IEqualityComparer<object> comparer, object oldValue, object newValue)
		{
			// Check: are we tracking edits?
			if (_parentTable.IsTrackingEdits ()) {
				switch (_editState) {
					case RowEditState.Inserted:
						// Record is Inserted - do not set to Updated
						break;
					case RowEditState.Deleted:
						// Record is Deleted - do not set to Updated
						break;
					default:
					// Perform comparison
						if (!comparer.Equals (oldValue, newValue)) {
							// New value is different
							_editState = RowEditState.Updated;
						}
						break;
				}
			}
		}

		public ObjectColumn GetColumnByID (int columnID, out int columnIndex)
		{
			columnIndex = _parentTable.FindColumnIndexByID (columnID);
			if (columnIndex < 0)
				throw new ArgumentOutOfRangeException ("columnID", columnID, "Invalid Column ID: not found.");
			return _parentTable.Columns [columnIndex];
		}

		#region implemented abstract members of RowBase

		protected override int FindColumnIndex (string columnName)
		{
			return _parentTable.FindColumnIndex (columnName);
		}

		protected override bool IsEditTracking ()
		{
			return _parentTable.IsTrackingEdits ();
		}

		protected override System.Collections.Generic.IEqualityComparer<object> GetEqualityComparer (int columnIndex)
		{
			return _parentTable.GetEqualityComparer (columnIndex);
		}

		protected override int GetColumnID (int columnIndex)
		{
			return _parentTable.Columns [columnIndex].ColumnID;
		}

		#endregion

		public ObjectRow ()
			: base ()
		{
		}

		public ObjectRow (int length)
			: base (length)
		{
		}

		public ObjectRow (ObjectTable parent, int length)
			: base (length)
		{
			_parentTable = parent;
			while (_values.Count < length) {
				_values.Add (null);
			}
		}

		public ObjectRow (ObjectTable parent, ObjectRow item)
			: base (item)
		{
			_parentTable = parent;
		}

		internal ObjectRow (ObjectTable parent, int length, long uniqueID)
			: this (parent, length)
		{
			_objectUniqueID = uniqueID;
		}

		/// <summary>
		/// Gets this row as a Text row
		/// </summary>
		/// <returns>The text.</returns>
		public RowText AsText ()
		{
			return new RowText2 (_parentTable, _values);
		}

		/// <summary>
		/// Gets the values changed.
		/// </summary>
		/// <returns>The values changed.</returns>
		public Dictionary<string, object> GetValuesChanged (IEnumerable<string> excludeFieldNames)
		{
			var comparer = new StringEqualityComparer ();
			var valueList = new Dictionary<string, object> (comparer);

			var arrExcluded = excludeFieldNames.ToArray ();

			foreach (var colId in _columnIDChangedList) {
				int index;
				var column = GetColumnByID (colId, out index);
				if (arrExcluded.Contains (column.ColumnName, comparer))
					continue;
				valueList.Add (column.ColumnName, _values [index]);
			}

			return valueList;
		}

		public Dictionary<string, object> GetValuesInsertNonNull (IEnumerable<string> keyFieldNames, IEnumerable<string> excludeFieldNames)
		{
			string[] arrExcluded = (excludeFieldNames == null) ? new string[] { } : excludeFieldNames.ToArray ();
			string[] arrKeyField = (keyFieldNames == null) ? new string[] { } : keyFieldNames.ToArray ();

			var comparer = new StringEqualityComparer ();

			var keyFieldsAllowed = arrKeyField.Where (x => !arrExcluded.Contains (x, comparer)).ToArray ();

			// Get Key fields (make sure of this)
			var values = GetValuesByFields (keyFieldsAllowed);

			const bool includeNullValues = false;
			AddValues (arrKeyField, arrExcluded, includeNullValues, comparer, values);

			return values;
		}

		public Dictionary<string, object> GetValuesAll (bool includeNullValues)
		{
			return GetValues (new string[]{ }, new string[]{ }, includeNullValues);
		}

		/// <summary>
		/// Gets the non-null values, except the key fields and excluded fields.
		/// </summary>
		/// <returns>The values non null.</returns>
		/// <param name="arrKeyFields">Array of key fields (excluded).</param>
		/// <param name="arrExcludedFields">Array of excluded fields.</param>
		/// <param name="includeNullValues">Whether to include Null values in the list</param>
		public Dictionary<string, object> GetValues (IEnumerable<string> arrKeyFields, IEnumerable<string> arrExcludedFields, bool includeNullValues)
		{
			Dictionary<string, object> values = new Dictionary<string, object> ();
			var comparer = new StringEqualityComparer ();

			AddValues (arrKeyFields, arrExcludedFields, includeNullValues, comparer, values);

			return values;
		}

		/// <summary>
		/// Adds any non-null values to the list.
		/// </summary>
		/// <param name="arrKeyFields">Array of key fields.</param>
		/// <param name="arrExcludedFields">Array of excluded fields.</param>
		/// <param name="includeNullValues">Whether to include Null values in the list</param>
		/// <param name="comparer">Comparer.</param>
		/// <param name="values">Values.</param>
		protected void AddValues (IEnumerable<string> arrKeyFields, IEnumerable<string> arrExcludedFields, bool includeNullValues, StringEqualityComparer comparer, Dictionary<string, object> values)
		{
			for (int index = 0; index < _values.Count; index++) {
				string colName = _parentTable.Columns [index].ColumnName;
				var objValue = _values [index];

				if (objValue == null && !includeNullValues)
					continue;
				if (arrExcludedFields.Contains (colName, comparer))
					continue;
				if (arrKeyFields.Contains (colName, comparer))
					continue;
				values.Add (colName, objValue);
			}
		}

		/// <summary>
		/// Gets the value for each of the given fields.
		/// </summary>
		/// <returns>The values by fields.</returns>
		/// <param name="fieldNames">Field names.</param>
		public Dictionary<string, object> GetValuesByFields (IEnumerable<string> fieldNames)
		{
			var valueList = new Dictionary<string, object> (new StringEqualityComparer ());

			foreach (var fieldName in fieldNames) {
				int index = _parentTable.FindColumnIndex (fieldName);
				if (index < 0)
					continue;
				var column = _parentTable.Columns [index];
				valueList.Add (column.ColumnName, _values [index]);
			}

			return valueList;
		}

		protected object _CustomTag;

		/// <summary>
		/// Gets or sets the custom tag - a custom value or object associated with this row.
		/// </summary>
		/// <value>The custom tag.</value>
		public object CustomTag {
			get { return _CustomTag; }
			set { _CustomTag = value; }
		}

		#region IUniqueIdentifier implementation

		protected long _objectUniqueID;

		/// <summary>
		/// Gets or sets this row's unique ID.
		/// </summary>
		/// <value>The unique ID.</value>
		public long ObjectUniqueID {
			get { return _objectUniqueID; }
			set { _objectUniqueID = value; }
		}

		#endregion
	}
}

