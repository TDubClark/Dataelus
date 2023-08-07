using System;

namespace Dataelus.UI.GridItemViewer.Generic
{
	/// <summary>
	/// Controller for a Grid Item Viewer.
	/// </summary>
	/// <typeparam name="D">The DataGrid type.</typeparam>
	/// <typeparam name="I">The Item Collection type</typeparam>
	public abstract class Controller<D, I> : IController<D, I>
	{
		public Controller ()
		{
		}

		public Controller (IViewGrid<D, I> gridview, IViewIssueBrowser<I> issueView)
		{
			_gridView = gridview;
			_issueView = issueView;
		}

		public Controller (IView<D, I> view)
			: this (view, view)
		{
			_view = view;
		}

		public Controller (IView<D, I> view, D gridData, I itemData)
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

		public abstract void UserItemSelected (int objectId);

		protected IViewIssueBrowser<I> _issueView;

		public IViewIssueBrowser<I> IssueView {
			get { return _issueView; }
			set { _issueView = value; }
		}

		protected IViewGrid<D, I> _gridView;

		public IViewGrid<D, I> GridView {
			get { return _gridView; }
			set { _gridView = value; }
		}

		protected IView<D, I> _view;

		/// <summary>
		/// Gets or sets the combined view.
		/// </summary>
		/// <value>The view.</value>
		public IView<D, I> View {
			get { return _view; }
			set { _view = value; }
		}

		protected D _gridDataObject;

		public D GridDataObject {
			get { return _gridDataObject; }
			set { _gridDataObject = value; }
		}

		protected I _itemDataObject;

		public I ItemDataObject {
			get { return _itemDataObject; }
			set { _itemDataObject = value; }
		}

		#endregion
	}

	// Override with Specific Issue Collection
	public class Controller<D> : Controller<D, Dataelus.TableIssue.ICellIssueItemCollection>
	{
		public Controller (IView<D> view)
			: base (view)
		{
		}

		#region implemented abstract members of Controller

		public override void UserItemSelected (int objectId)
		{
			int index = _itemDataObject.FindIndexByObjectId (objectId);
			if (index < 0)
				_gridView.ClearHilighting ();
			else {
				var gridCell =	_itemDataObject [index].GridCell;
				_gridView.HilightGridItem (gridCell.RowIndex, gridCell.ColumnIndex);
			}
		}

		#endregion
	}

	public class ControllerPaged<D> : Controller<D, Dataelus.TableIssue.ICellIssueItemCollection>
	{
		public ControllerPaged (IView<D> view)
			: base (view)
		{
		}

		#region implemented abstract members of Controller

		public override void UserItemSelected (int objectId)
		{
			int index = _itemDataObject.FindIndexByObjectId (objectId);
			if (index < 0)
				_gridView.ClearHilighting ();
			else {
				var gridCell =	_itemDataObject [index].GridCell;
				_gridView.HilightGridItem (gridCell.RowIndex, gridCell.ColumnIndex);
			}
		}

		#endregion
	}
}

