using System;
using System.Collections.Generic;

namespace Dataelus.Table.Generic
{
	/// <summary>
	/// This is an Abstract Base class for a row of values.
	/// </summary>
	public abstract class RowBase<T>
	{
		/// <summary>
		/// Finds the index of the column (using an un-implemented Parent table).
		/// </summary>
		/// <returns>The column index.</returns>
		/// <param name="columnName">Column name.</param>
		protected abstract int FindColumnIndex (string columnName);

		/// <summary>
		/// Determines whether this instance is edit tracking.
		/// </summary>
		/// <returns><c>true</c> if this instance is edit tracking; otherwise, <c>false</c>.</returns>
		protected abstract bool IsEditTracking ();

		/// <summary>
		/// Gets the equality comparer.
		/// </summary>
		/// <returns>The equality comparer.</returns>
		/// <param name="columnIndex">Column index.</param>
		protected abstract IEqualityComparer<T> GetEqualityComparer (int columnIndex);

		/// <summary>
		/// Gets the column ID at the given index.
		/// </summary>
		/// <returns>The column ID.</returns>
		/// <param name="columnIndex">Column index.</param>
		protected abstract int GetColumnID (int columnIndex);

		/// <summary>
		/// Handles the updated event.
		/// </summary>
		/// <param name="priorValue">The prior value at the given column index</param>
		/// <param name="newValue">The new value at the given column index.</param>
		/// <param name="columnIndex">Column index.</param>
		protected virtual void OnUpdated (T priorValue, T newValue, int columnIndex)
		{
			if (IsEditTracking ()) {
				switch (_editState) {
					case RowEditState.Inserted:
						// Record is Inserted - do not set to Updated
						break;
					case RowEditState.Deleted:
						// Record is Deleted - do not set to Updated
						break;
					default:
						// This is neither an inserted nor a deleted row
						var comparer = GetEqualityComparer (columnIndex);

						// Perform comparison
						if (!comparer.Equals (priorValue, newValue)) {
							// New value is different
							_editState = RowEditState.Updated;
							AddColumnIndexChanged (columnIndex);
						}
						break;
				}
			}
		}

		/// <summary>
		/// Adds a column index to the list of changed column indexes.
		/// </summary>
		/// <param name="columnIndex">Column index.</param>
		protected virtual void AddColumnIndexChanged (int columnIndex)
		{
			if (!_columnIndexChangedList.Contains (columnIndex)) {
				_columnIndexChangedList.Add (columnIndex);
			}

			var colId = GetColumnID (columnIndex);
			if (!_columnIDChangedList.Contains (colId)) {
				_columnIDChangedList.Add (colId);
			}
		}

		protected List<int> _columnIndexChangedList;

		/// <summary>
		/// Gets or sets the list of every column index which has changed.
		/// </summary>
		/// <value>The column index changed.</value>
		public List<int> ColumnIndexChangedList {
			get { return _columnIndexChangedList; }
			set { _columnIndexChangedList = value; }
		}

		protected List<int> _columnIDChangedList;

		/// <summary>
		/// Gets or sets the column identifier changed list.
		/// </summary>
		/// <value>The column identifier changed list.</value>
		public List<int> ColumnIDChangedList {
			get { return _columnIDChangedList; }
			set { _columnIDChangedList = value; }
		}

		/// <summary>
		/// Sets the value at the given index.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="columnIndex">Column index.</param>
		protected virtual void SetValue (T value, int columnIndex)
		{
			_values [columnIndex] = value;
		}

		/// <summary>
		/// Gets or sets the value at the specified column index.
		/// </summary>
		/// <param name="columnIndex">Index.</param>
		public T this [int columnIndex] {
			get {
				if (columnIndex >= _values.Count || columnIndex < 0)
					throw new ArgumentOutOfRangeException ("columnIndex", columnIndex, String.Format ("Requested index={0}; item count={1}", columnIndex, _values.Count));
				return _values [columnIndex];
			}
			set {
				if (columnIndex >= _values.Count || columnIndex < 0)
					throw new ArgumentOutOfRangeException ("columnIndex", columnIndex, String.Format ("Requested index={0}; item count={1}", columnIndex, _values.Count));
				SetValue (value, columnIndex);
			}
		}

		/// <summary>
		/// Gets or sets the value at the specified columnName.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		public T this [string columnName] {
			get {
				var index = FindColumnIndex (columnName);
				if (index < 0)
					throw new ArgumentOutOfRangeException ("columnName", columnName, String.Format ("Could not find column '{0}'.", columnName));
				return this [index];
			}
			set {
				var index = FindColumnIndex (columnName);
				if (index < 0)
					throw new ArgumentOutOfRangeException ("columnName", columnName, String.Format ("Could not find column '{0}'.", columnName));
				this [index] = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Table.Generic.RowBase`1"/> class.
		/// </summary>
		protected RowBase ()
			: this (0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Table.Generic.RowBase`1"/> class.
		/// </summary>
		/// <param name="length">Initial Length of the values list.</param>
		protected RowBase (int length)
		{
			_values = new ListBase<T> (length);
			while (_values.Count < length) {
				_values.Add (default(T));
			}

			Init ();
		}

		/// <summary>
		/// Copy Constructor.
		/// Initializes a new instance of the <see cref="Dataelus.Table.Generic.RowBase`1"/> class.
		/// </summary>
		/// <param name="item">Item.</param>
		protected RowBase (RowBase<T> item)
		{
			_values = new ListBase<T> (item.Values);

			Init ();
		}

		protected virtual void Init ()
		{
			_editState = RowEditState.Undefined;
			_columnIndexChangedList = new List<int> ();
			_columnIDChangedList = new List<int> ();

			_values.ItemSet += Values_ItemSet;
		}

		void Values_ItemSet (object sender, ListItemSetEventArgs<T> args)
		{
			OnUpdated (args.PriorValue, args.NewValue, args.IndexSet);
		}

		/// <summary>
		/// The values of this row.
		/// </summary>
		protected ListBase<T> _values;

		/// <summary>
		/// Gets or sets the values of this row.
		/// </summary>
		/// <value>The values.</value>
		public ListBase<T> Values {
			get { return _values; }
			set { _values = value; }
		}

		/// <summary>
		/// The edit state of this row.
		/// </summary>
		protected RowEditState _editState;

		/// <summary>
		/// Gets or sets the edit state of this row.
		/// </summary>
		/// <value>The edit state of this row.</value>
		public RowEditState EditState {
			get { return _editState; }
			set {
				_editState = value;

				// Set to Unchanged; clear the list of changed indexes and column IDs
				if (value == RowEditState.Unchanged) {
					_columnIndexChangedList.Clear ();
					_columnIDChangedList.Clear ();
				}
			}
		}

		/// <summary>
		/// Determines whether this row is edited (inserted/updated/deleted).
		/// </summary>
		/// <returns><c>true</c> if this row is edited (inserted/updated/deleted); otherwise, <c>false</c>.</returns>
		public bool IsEdited ()
		{
			switch (_editState) {
				case RowEditState.Inserted:
					return true;
				case RowEditState.Updated:
					return true;
				case RowEditState.Deleted:
					return true;
				case RowEditState.Unchanged:
					break;
				case RowEditState.Undefined:
					break;
				default:
					throw new ArgumentOutOfRangeException ();
			}
			return false;
		}
	}
}
