using System;

namespace Dataelus.UI.GridItemViewer
{
	/// <summary>
	/// Controller base.
	/// </summary>
	public class ControllerBase : IController
	{
		Dataelus.UI.GridItemViewer.Generic.IViewGrid<object[,], Dataelus.TableIssue.ICellIssueItemCollection> _gridView;

		public Dataelus.UI.GridItemViewer.Generic.IViewGrid<object[,], Dataelus.TableIssue.ICellIssueItemCollection> GridView {
			get { return _gridView; }
			set { _gridView = value; }
		}

		Dataelus.UI.GridItemViewer.Generic.IViewIssueBrowser<Dataelus.TableIssue.ICellIssueItemCollection> _issueView;

		/// <summary>
		/// Gets or sets a value indicating whether this instance issue view.
		/// </summary>
		/// <value><c>true</c> if this instance issue view; otherwise, <c>false</c>.</value>
		public Dataelus.UI.GridItemViewer.Generic.IViewIssueBrowser<Dataelus.TableIssue.ICellIssueItemCollection> IssueView {
			get { return _issueView; }
			set { _issueView = value; }
		}

		public ControllerBase (IView view)
		{
			_view = view;
			_gridView = view;
			_issueView = view;
		}

		public ControllerBase (IView view, object[,] gridData, Dataelus.TableIssue.ICellIssueItemCollection itemData)
			: this (view)
		{
			_gridDataObject = gridData;
			_itemDataObject = itemData;
		}

		public void LoadViewData ()
		{
			_gridView.LoadGridData (_gridDataObject);
			_issueView.LoadItems (_itemDataObject);
		}

		#region IController implementation

		public void UserItemSelected (int objectId)
		{
			int index = _itemDataObject.FindIndexByObjectId (objectId);
			if (index < 0)
				_gridView.ClearHilighting ();
			else {
				var gridCell =	_itemDataObject [index].GridCell;
				_gridView.HilightGridItem (gridCell.RowIndex, gridCell.ColumnIndex);
			}
		}

		protected IView _view;

		public IView View {
			get { return _view; }
			set { _view = value; }
		}

		Generic.IView<object[,], Dataelus.TableIssue.ICellIssueItemCollection> Generic.IController<object[,], Dataelus.TableIssue.ICellIssueItemCollection>.View { 
			get { return _view; }
			set { _view = (IView)value; }
		}

		protected object[,] _gridDataObject;

		public object[,] GridDataObject {
			get { return _gridDataObject; }
			set { _gridDataObject = value; }
		}

		protected Dataelus.TableIssue.ICellIssueItemCollection _itemDataObject;

		public Dataelus.TableIssue.ICellIssueItemCollection ItemDataObject {
			get { return _itemDataObject; }
			set { _itemDataObject = value; }
		}

		#endregion
	}
}

