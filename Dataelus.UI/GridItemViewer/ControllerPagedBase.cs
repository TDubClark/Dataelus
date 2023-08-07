using System;

using Dataelus.TableIssue;
using Dataelus.UI.GridItemViewer.Generic;

namespace Dataelus.UI.GridItemViewer
{
	/// <summary>
	/// Controller paged base.
	/// </summary>
	public class ControllerPagedBase<D> : IControllerGridPaged<D, CellIssueItemPagedCollectionBase>, IControllerIssueBrowser<CellIssueItemPagedCollectionBase>
	{
		#region IControllerIssueBrowser implementation

		public void UserItemSelected (int objectId)
		{
			int index = _itemDataObject.FindIndexByItemID (objectId);
			if (index < 0) {
				_gridView.ClearHilighting ();
				return;
			}
			var gridCell = _itemDataObject [index].GridCell;
			_gridView.HilightGridItem (gridCell.RowIndex, gridCell.ColumnIndex);
		}

		protected IViewIssueBrowser<CellIssueItemPagedCollectionBase> _issueView;

		public IViewIssueBrowser<CellIssueItemPagedCollectionBase> IssueView {
			get { return _issueView; }
			set { _issueView = value; }
		}

		#endregion

		#region IControllerGrid implementation

		public void LoadViewData ()
		{
			_issueView.LoadItems (_itemDataObject);
			_gridView.LoadGridData (_gridDataObject);
		}

		protected IViewGridPaged<D, CellIssueItemPagedCollectionBase> _gridView;

		public IViewGridPaged<D, CellIssueItemPagedCollectionBase> GridView {
			get { return _gridView; }
			set { _gridView = value; }
		}

		protected CellIssueItemPagedCollectionBase _itemDataObject;

		public CellIssueItemPagedCollectionBase ItemDataObject {
			get { return _itemDataObject; }
			set { _itemDataObject = value; }
		}

		protected D _gridDataObject;

		public D GridDataObject {
			get { return _gridDataObject; }
			set { _gridDataObject = value; }
		}

		#endregion
	}
}

