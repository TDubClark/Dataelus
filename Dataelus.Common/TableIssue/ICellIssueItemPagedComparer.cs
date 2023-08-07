using System;
using System.Collections.Generic;

namespace Dataelus.TableIssue
{
	public class ICellIssueItemPagedComparer : IComparer<ICellIssueItemPaged>
	{
		public Grid.IGridCellComparer CellComparer { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.TableIssue.ICellIssueItemPagedComparer"/> class.
		/// Sorts by Page, then row, then column
		/// </summary>
		public ICellIssueItemPagedComparer ()
		{
			this.CellComparer = new Dataelus.Grid.IGridCellComparer ();
		}

		#region IComparer implementation

		public int Compare (ICellIssueItemPaged x, ICellIssueItemPaged y)
		{
			var result = x.PageID.CompareTo (y.PageID);
			if (result == 0)
				result = this.CellComparer.Compare (x.GridCell, y.GridCell);

			return result;
		}

		#endregion
	}
}

