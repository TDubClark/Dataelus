using System;

namespace Dataelus.Table
{
	public class ColumnCollection : CollectionBase<Column>
	{
		public UniqueIdentifierManager ColumnIDManager { get; protected set; }

		public ColumnCollection ()
			: base ()
		{
			this.ColumnIDManager = new UniqueIdentifierManager ();
		}

		public ColumnCollection (System.Collections.Generic.IEnumerable<Column> other)
			: this ()
		{
			foreach (var item in other) {
				AddColumn (new Column (item));
			}
		}

		/// <summary>
		/// Finds the index of the column in the collection, according to the given column name.
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="columnName">Column name.</param>
		public int FindIndex (string columnName)
		{
			return _items.FindIndex (x => x.ColumnName.Equals (columnName, StringComparison.OrdinalIgnoreCase));
		}

		public int FindIndex (string columnName, System.Collections.Generic.IEqualityComparer<string> comparer)
		{
			return _items.FindIndex (x => comparer.Equals (x.ColumnName, columnName));
		}

		public Column AddColumn (Column item)
		{
			base.Add (item);
			item.ColumnIndex = base.Count - 1;
			item.ColumnID = (int)this.ColumnIDManager.GetUniqueID ();
			return item;
		}
	}
}

