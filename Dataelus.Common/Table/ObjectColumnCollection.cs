using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Table
{
	/// <summary>
	/// Object column collection.
	/// </summary>
	public class ObjectColumnCollection : CollectionBase<ObjectColumn>, System.Collections.IEnumerable, IEquatable<ObjectColumnCollection>, IEquatable<ObjectColumnCollection, String>
	{
		public UniqueIdentifierManager ColumnIDManager { get; protected set; }

		/// <summary>
		/// The column index dict.
		/// </summary>
		protected Dictionary<string, int> _columnIndexDict = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Table.ObjectColumnCollection"/> class.
		/// </summary>
		public ObjectColumnCollection ()
			: base ()
		{
			_columnIndexDict = new Dictionary<string, int> (new StringEqualityComparer ());
			this.ColumnIDManager = new UniqueIdentifierManager ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Table.ObjectColumnCollection"/> class.
		/// </summary>
		/// <param name="other">Other.</param>
		public ObjectColumnCollection (System.Collections.Generic.IEnumerable<ObjectColumn> other)
			: this ()
		{
			foreach (var item in other) {
				Add (new ObjectColumn (item));
			}
		}

		/// <summary>
		/// Gets the array of column names.
		/// </summary>
		/// <returns>The column names.</returns>
		public string[] GetColumnNames ()
		{
			return _items.Select (x => x.ColumnName).ToArray ();
		}

		/// <summary>
		/// Finds the index by unique column ID.
		/// </summary>
		/// <returns>The index by ID.</returns>
		/// <param name="columnID">Column ID.</param>
		public int FindIndexByID (int columnID)
		{
			return _items.FindIndex (x => x.ColumnID == columnID);
		}

		/// <summary>
		/// Finds the index by tag.
		/// </summary>
		/// <returns>The index by tag.</returns>
		/// <param name="customTag">Custom tag.</param>
		/// <param name="tagComparer">Tag comparer.</param>
		public virtual int FindIndexByTag (object customTag, System.Collections.IEqualityComparer tagComparer)
		{
			return _items.FindIndex (x => tagComparer.Equals (x.CustomValueTag, customTag));
		}

		/// <summary>
		/// Finds the index of the column in the collection, according to the given column name.
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="columnName">Column name.</param>
		public int FindIndex (string columnName)
		{
			return FindIndex (columnName, new StringEqualityComparer ());
			//return _items.FindIndex (x => x.ColumnName.Equals (columnName, StringComparison.OrdinalIgnoreCase));
		}

		/// <summary>
		/// Finds the index of the column in the collection, according to the given column name.
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="columnName">Column name.</param>
		/// <param name="comparer">Comparer.</param>
		public int FindIndex (string columnName, System.Collections.Generic.IEqualityComparer<string> comparer)
		{
			// Check the dictionary first
			int index;
			if (_columnIndexDict.TryGetValue (columnName, out index)) {
				// Verify that the dictionary is accurate (if not, clear it out)
				if (comparer.Equals (_items [index].ColumnName, columnName))
					return index;
				else
					_columnIndexDict.Clear ();
			}

			// Find the index and save it to the dictionary
			index = _items.FindIndex (x => comparer.Equals (x.ColumnName, columnName));
			if (index >= 0)
				_columnIndexDict.Add (columnName, index);
			return index;
		}

		/// <summary>
		/// Adds the column and sets the column index.
		/// </summary>
		/// <returns>The column.</returns>
		/// <param name="item">Item.</param>
		public ObjectColumn AddColumn (ObjectColumn item)
		{
			Add (item);
			return item;
		}

		public override void Add (ObjectColumn item)
		{
			base.Add (item);
			item.ColumnIndex = base.Count - 1;
			item.ColumnID = (int)this.ColumnIDManager.GetUniqueID ();
		}

		/// <summary>
		/// Moves the item from the given index to the given index.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="fromIndex">From index.</param>
		/// <param name="toIndex">To index.</param>
		/// <param name="columnName">Column name.</param>
		public void MoveItem (string columnName, int toIndex)
		{
			throw new NotImplementedException ();
		}

		#region IEquatable implementation

		/// <summary>
		/// Determines whether the specified <see cref="Dataelus.Table.ObjectColumnCollection"/> is equal to the current <see cref="Dataelus.Table.ObjectColumnCollection"/>.
		/// </summary>
		/// <param name="other">The <see cref="Dataelus.Table.ObjectColumnCollection"/> to compare with the current <see cref="Dataelus.Table.ObjectColumnCollection"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="Dataelus.Table.ObjectColumnCollection"/> is equal to the current
		/// <see cref="Dataelus.Table.ObjectColumnCollection"/>; otherwise, <c>false</c>.</returns>
		public bool Equals (ObjectColumnCollection other)
		{
			return Equals (other, true, new StringEqualityComparer ());
		}

		#endregion

		/// <summary>
		/// Determines whether the specified <see cref="Dataelus.Table.ObjectColumnCollection"/> is equal to the current <see cref="Dataelus.Table.ObjectColumnCollection"/>.
		/// </summary>
		/// <param name="other">Other.</param>
		/// <param name="comparer">The column name comparer.</param>
		public bool Equals (ObjectColumnCollection other, IEqualityComparer<string> comparer)
		{
			return Equals (other, true, comparer);
		}

		/// <summary>
		/// Determines whether the specified <see cref="Dataelus.Table.ObjectColumnCollection"/> is equal to the current <see cref="Dataelus.Table.ObjectColumnCollection"/>.
		/// </summary>
		/// <param name="other">The Other item.</param>
		/// <param name="assumeSameOrder">If set to <c>true</c> assume same order.</param>
		/// <param name="nameComparer">Name comparer.</param>
		public bool Equals (ObjectColumnCollection other, bool assumeSameOrder, IEqualityComparer<string> nameComparer)
		{
			foreach (var item in other) {
				int index = -1;
				if (assumeSameOrder) {
					// Assume the same order
					index = item.ColumnIndex;

					// Is this a valid index?
					if (index >= _items.Count) {
						return false;
					}
					// Check the column names
					if (!nameComparer.Equals (_items [index].ColumnName, item.ColumnName)) {
						return false;
					}
				} else {
					// Find the index by name
					index = this.FindIndex (item.ColumnName, nameComparer);
					if (index < 0) {
						return false;
					}
				}

				// Compare Data Type
				if (!_items [index].DataType.Equals (item.DataType)) {
					return false;
				}
			}
			return true;
		}
	}
}

