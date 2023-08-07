using System;

using Dataelus.TableIssue;
using Dataelus.UI.GridItemViewer.Generic;

namespace Dataelus.UI.GridItemViewer
{

	/// <summary>
	/// Controller paged viewer base.
	/// </summary>
	public class ControllerPagedViewerBase : IControllerGridViewerPaged<CellIssueItemPagedCollectionBase>, IControllerIssueBrowser<CellIssueItemPagedCollectionBase>
	{
		#region IControllerIssueBrowser implementation

		public void UserItemSelected (int objectId)
		{
			int index = _itemDataObject.FindIndexByItemID (objectId);
			if (index < 0) {
				_gridView.ClearHilighting ();
				return;
			}
			var issue = _itemDataObject [index];
			var gridCell = issue.GridCell;
			_gridView.HilightGridItem (issue.PageID, gridCell.RowIndex, gridCell.ColumnIndex);
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
		}

		protected IViewPagedGridViewOnly _gridView;

		public IViewPagedGridViewOnly GridView {
			get { return _gridView; }
			set { _gridView = value; }
		}

		protected CellIssueItemPagedCollectionBase _itemDataObject;

		public CellIssueItemPagedCollectionBase ItemDataObject {
			get { return _itemDataObject; }
			set { _itemDataObject = value; }
		}

		#endregion
	}

	public interface IViewIssueBrowser : IViewIssueBrowser<CellIssueItemPagedCollectionBase>
	{
		
	}
}
