using System;

namespace Dataelus.TableIssue
{
	using Dataelus;
	using Dataelus.Grid;

	/// <summary>
	/// Base class for a Cell Issue Item
	/// </summary>
	public class CellIssueItemBase : IssueBase, ICellIssueItem
	{
		public CellIssueItemBase ()
			: this (-1, null, null, null)
		{
		}

		public CellIssueItemBase (int id, string desc, IGridCell cell)
			: this (id, null, desc, cell)
		{
		}

		public CellIssueItemBase (int id, string desc, int rowIndex, int columnIndex)
			: this (id, null, desc, rowIndex, columnIndex)
		{		
		}

		public CellIssueItemBase (int id, string category, string desc, IGridCell cell)
		{
			_itemID = id;
			_category = category;
			_description = desc;
			_gridCell = cell;
		}

		public CellIssueItemBase (int id, string category, string desc, int rowIndex, int columnIndex)
			: this (id, category, desc, new GridCellBase (rowIndex, columnIndex))
		{
		}

		#region ICellIssueItem implementation

		protected IGridCell _gridCell;

		public IGridCell GridCell {
			get { return _gridCell; }
			set { _gridCell = value; }
		}

		#endregion
	}
}

