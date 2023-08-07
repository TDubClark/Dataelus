using System;
using System.Linq;
using System.Collections.Generic;

namespace Dataelus.UI.TableCompareViewer
{
	public class Controller<T, R> : IController<T, R>
	{
		protected IView<T, R> _view;
		protected System.Collections.Generic.List<Dataelus.Table.Comparison.IssueItem> _itemDataObject;

		protected Dataelus.Table.Comparison.Generic.TableComparer<T, R> _tableComparer;

		/// <summary>
		/// Gets or sets the table comparer.
		/// </summary>
		/// <value>The table comparer.</value>
		public Dataelus.Table.Comparison.Generic.TableComparer<T, R> TableComparer {
			get { return _tableComparer; }
			set { _tableComparer = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.UI.TableCompareViewer.Controller`2"/> class.
		/// </summary>
		/// <param name="view">View.</param>
		/// <param name="tableComparer">Table comparer.</param>
		public Controller (IView<T, R> view, Dataelus.Table.Comparison.Generic.TableComparer<T, R> tableComparer)
		{
			_view = view;
			_tableComparer = tableComparer;
			_itemDataObject = new System.Collections.Generic.List<Dataelus.Table.Comparison.IssueItem> (tableComparer.GetIssues ());
		}

		protected int[] GetColumnIndex (int gridNumber, string[] columnNames)
		{
			var lst1 = new List<int> ();

			for (int i = 0; i < columnNames.Length; i++) {
				lst1.Add (_view.GetGridColumnIndex (gridNumber, columnNames [i]));
			}

			return lst1.ToArray ();
		}

		protected int[] GetColumnIndex (int gridNumber, Dataelus.Table.CellRange range)
		{
			if (range.IsColumnIndex ())
				return range.ColumnIndex;
			else
				return GetColumnIndex (gridNumber, range.ColumnNames);
		}

		/// <summary>
		/// Calls the hilight grid function.
		/// </summary>
		/// <param name="gridNumber">Grid number.</param>
		/// <param name="range">Range.</param>
		private void CallHilightGrid (int gridNumber, Dataelus.Table.CellRange range)
		{
			_view.HilightGridItems (gridNumber, range.RowIndex, GetColumnIndex (gridNumber, range));
		}

		#region IController implementation

		public void LoadViewData ()
		{
			_view.LoadGridData (1, _tableComparer.Table1);
			_view.LoadGridData (2, _tableComparer.Table2);
			_view.LoadItems (_itemDataObject);
		}

		public void UserItemSelected (int objectId)
		{
			int index = _itemDataObject.FindIndex (x => x.ItemID == objectId);
			if (index < 0)
				_view.ClearHilighting ();
			else {
				CallHilightGrid (1, _itemDataObject [index].Table1Range);
				CallHilightGrid (2, _itemDataObject [index].Table2Range);
			}
		}

		public IView<T, R> View {
			get { return _view; }
			set { _view = value; }
		}

		public System.Collections.Generic.List<Dataelus.Table.Comparison.IssueItem> ItemDataObject {
			get {
				return _itemDataObject; }
			set { _itemDataObject = value;
			}
		}

		#endregion
	}
}

