using System;
using System.Collections.Generic;

namespace Dataelus.UI.MatchTableEditor
{
	/// <summary>
	/// Interface for a row item translator.
	/// </summary>
	public interface IRowItem
	{
		object GetItem (Table.ObjectRow row);

		object GetItem (long uniqueID);

		long GetUniqueID (Table.ObjectRow row);

		int GetRowIndex (Table.ObjectTable table, long uniqueID);
	}
	
	/// <summary>
	/// A row item translator.
	/// </summary>
	public class RowItem : IRowItem
	{
		protected IdentifiedObjectCollection _items;

		public IdentifiedObjectCollection Items {
			get { return _items; }
			set { _items = value; }
		}

		public RowItem ()
		{
			_items = new IdentifiedObjectCollection ();
		}

		public RowItem (IEnumerable<IdentifiedObject> collection)
			: this ()
		{
			_items.AddItems (collection);
		}

		#region IRowItem implementation

		public object GetItem (Dataelus.Table.ObjectRow row)
		{
			return GetItem (GetUniqueID (row));
		}

		public object GetItem (long uniqueID)
		{
			return _items.FindItem (uniqueID);
		}

		public long GetUniqueID (Dataelus.Table.ObjectRow row)
		{
			return (long)row.CustomTag;
		}

		public int GetRowIndex (Dataelus.Table.ObjectTable table, long uniqueID)
		{
			return table.Rows.FindIndexByTag (uniqueID, new IDTagComparer ());
		}

		#endregion
	}
}
