using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.TableIssue
{
	/// <summary>
	/// Base implementation of a collection of Cell issue items, according to page.
	/// </summary>
	public class CellIssueItemPagedCollectionBase : UniqueListBase<ICellIssueItemPaged>, System.Collections.IEnumerable
	{
		/// <summary>
		/// The manager for Item IDs.
		/// </summary>
		protected UniqueIdentifierManager _itemIDManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.TableIssue.CellIssueItemPagedCollectionBase"/> class.
		/// </summary>
		public CellIssueItemPagedCollectionBase ()
			: base ()
		{
			_itemIDManager = new UniqueIdentifierManager ();
		}


		/// <summary>
		/// Gets whether the given Item ID is already in use.
		/// </summary>
		/// <returns><c>true</c>, if in use, <c>false</c> otherwise.</returns>
		/// <param name="id">Identifier.</param>
		public bool InUseItemID (int id)
		{
			return _items.Any (x => x.ItemID == id);
		}

		public int GetNewItemID ()
		{
			int id;
			do {
				id = _itemIDManager.GetUniqueIDAsInt ();
			} while (InUseItemID (id));

			return id;
		}

		public override void Add (ICellIssueItemPaged item)
		{
			if (item == null)
				throw new ArgumentNullException ("item");
			if (InUseItemID (item.ItemID))
				throw new Exception (String.Format ("The given item has ItemID {0:d} which is already in use.", item.ItemID));
			
			base.Add (item);
		}


		/// <summary>
		/// Finds the index by item identifier.
		/// </summary>
		/// <returns>The index by item identifier.</returns>
		/// <param name="itemID">Item ID.</param>
		public int FindIndexByItemID (int itemID)
		{
			return _items.FindIndex (x => x.ItemID == itemID);
		}

		/// <summary>
		/// Gets the list of unique Page IDs.
		/// </summary>
		/// <returns>The pages.</returns>
		public List<int> GetPages ()
		{
			return _items.Select (x => x.PageID).Distinct ().OrderBy (x => x).ToList ();
		}

		/// <summary>
		/// Gets the items for the given page.
		/// </summary>
		/// <returns>The items by page.</returns>
		/// <param name="pageId">Page identifier.</param>
		public List<ICellIssueItemPaged> GetItemsByPage (int pageId)
		{
			return _items.Where (x => x.PageID == pageId).ToList ();
		}

		/// <summary>
		/// Sorts the items first by page, then row, then column.
		/// </summary>
		public void SortByPageRowColumn ()
		{
			_items.Sort (new ICellIssueItemPagedComparer ());
		}

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		#endregion
	}

	public class CellIssueItemPagedCollection : UniqueListBase<CellIssueItemPagedBase>, System.Collections.IEnumerable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.TableIssue.CellIssueItemPagedCollection"/> class.
		/// </summary>
		public CellIssueItemPagedCollection ()
			: base ()
		{
		}

		/// <summary>
		/// Gets whether the given item ID is already in use.
		/// </summary>
		/// <returns><c>true</c>, if in use, <c>false</c> otherwise.</returns>
		/// <param name="id">Identifier.</param>
		bool InUseItemID (int id)
		{
			return _items.Any (x => x.ItemID == id);
		}

		protected int GetNewItemID ()
		{
			int id;
			do {
				id = _idManager.GetUniqueIDAsInt ();
			} while (InUseItemID (id));

			return id;
		}

		/// <summary>
		/// Adds a new object, with a new item ID.
		/// </summary>
		/// <returns>The new object.</returns>
		/// <param name="category">Category.</param>
		/// <param name="desc">Desc.</param>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnIndex">Column index.</param>
		/// <param name="pageID">Page I.</param>
		public CellIssueItemPagedBase AddNew (string category, string desc, int rowIndex, int columnIndex, int pageID)
		{
			return AddNew (GetNewItemID (), category, desc, rowIndex, columnIndex, pageID);
		}

		/// <summary>
		/// Adds a new object.
		/// </summary>
		/// <returns>The new object.</returns>
		/// <param name="id">The item Identifier.</param>
		/// <param name="category">Category.</param>
		/// <param name="desc">Desc.</param>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnIndex">Column index.</param>
		/// <param name="pageID">Page I.</param>
		public CellIssueItemPagedBase AddNew (int id, string category, string desc, int rowIndex, int columnIndex, int pageID)
		{
			if (InUseItemID (id)) {
				throw new ArgumentOutOfRangeException ("id", String.Format ("The given item identifier {0} is already in use.", id));
			}

			var item = new CellIssueItemPagedBase (id, category, desc, rowIndex, columnIndex, pageID);
			return AddAssignID (item);
		}

		/// <summary>
		/// Finds the index by item identifier.
		/// </summary>
		/// <returns>The index by item identifier.</returns>
		/// <param name="itemID">item ID.</param>
		public int FindIndexByItemID (int itemID)
		{
			return _items.FindIndex (x => x.ItemID == itemID);
		}

		/// <summary>
		/// Gets the list of unique Page IDs.
		/// </summary>
		/// <returns>The pages.</returns>
		public List<int> GetPages ()
		{
			return _items.Select (x => x.PageID).Distinct ().OrderBy (x => x).ToList ();
		}

		/// <summary>
		/// Gets the items for the given page.
		/// </summary>
		/// <returns>The items by page.</returns>
		/// <param name="pageId">Page identifier.</param>
		public List<CellIssueItemPagedBase> GetItemsByPage (int pageId)
		{
			return _items.Where (x => x.PageID == pageId).ToList ();
		}

		/// <summary>
		/// Sorts the items first by page, then row, then column.
		/// </summary>
		public void SortByPageRowColumn ()
		{
			_items.Sort (new ICellIssueItemPagedComparer ());
		}

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		#endregion
	}
}

