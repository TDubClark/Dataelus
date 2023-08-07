using System;
using Dataelus.Grid;

namespace Dataelus.TableIssue
{
	/// <summary>
	/// Base implementation of a Cell issue item, which applies to a specific page.
	/// </summary>
	public class CellIssueItemPagedBase : CellIssueItemBase, ICellIssueItemPaged
	{
		/// <summary>
		/// The page ID.
		/// </summary>
		protected int _pageID;

		/// <summary>
		/// Gets or sets the page ID to which this issue applies.
		/// </summary>
		/// <value>The page ID.</value>
		public int PageID {
			get { return _pageID; }
			set { _pageID = value; }
		}

		/// <summary>
		/// The object unique ID.
		/// </summary>
		protected long _objectUniqueID;

		/// <summary>
		/// Gets or sets the object unique ID.
		/// </summary>
		/// <value>The object unique ID.</value>
		public long ObjectUniqueID {
			get { return _objectUniqueID; }
			set { _objectUniqueID = value; }
		}

		public CellIssueItemPagedBase ()
			: base ()
		{
		}

		public CellIssueItemPagedBase (int id, string desc, IGridCell cell, int pageID)
			: base (id, desc, cell)
		{
			_pageID = pageID;
		}

		public CellIssueItemPagedBase (int id, string desc, int rowIndex, int columnIndex, int pageID)
			: base (id, desc, rowIndex, columnIndex)
		{
			_pageID = pageID;
		}

		public CellIssueItemPagedBase (int id, string category, string desc, IGridCell cell, int pageID)
			: base (id, category, desc, cell)
		{
			_pageID = pageID;
		}

		public CellIssueItemPagedBase (int id, string category, string desc, int rowIndex, int columnIndex, int pageID)
			: base (id, category, desc, rowIndex, columnIndex)
		{
			_pageID = pageID;
		}
	}
}

