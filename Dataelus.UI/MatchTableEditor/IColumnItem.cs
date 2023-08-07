using System;
using System.Collections.Generic;

namespace Dataelus.UI.MatchTableEditor
{
	/// <summary>
	/// Interface for a column item.
	/// </summary>
	public interface IColumnItem
	{
		object GetItem (Table.ObjectColumn column);

		object GetItem (long uniqueID);

		long GetUniqueID (Table.ObjectColumn column);

		int GetColumnIndex (Table.ObjectTable table, long uniqueID);
	}
	
	public class ColumnItem : IColumnItem
	{
		protected IdentifiedObjectCollection _items;

		public IdentifiedObjectCollection Items {
			get { return _items; }
			set { _items = value; }
		}

		public ColumnItem ()
		{
			_items = new IdentifiedObjectCollection ();
		}

		public ColumnItem (IEnumerable<IdentifiedObject> collection)
			: this ()
		{
			_items.AddItems (collection);
		}

		#region IColumnItem implementation

		public object GetItem (Dataelus.Table.ObjectColumn column)
		{
			return GetItem (GetUniqueID (column));
		}

		public object GetItem (long uniqueID)
		{
			return _items.FindItem (uniqueID);
		}

		public long GetUniqueID (Dataelus.Table.ObjectColumn column)
		{
			return (long)column.CustomValueTag;
		}

		public int GetColumnIndex (Dataelus.Table.ObjectTable table, long uniqueID)
		{
			int index = table.Columns.FindIndexByTag (uniqueID, new IDTagComparer ());
			if (index < 0)
				return -1;
			return table.Columns [index].ColumnIndex;
		}

		#endregion
	}
}
