using System;
using System.Collections.Generic;

namespace Dataelus.UI.MatchTableEditor
{
	/// <summary>
	/// Interface for a controller.
	/// </summary>
	public interface IController
	{
		/// <summary>
		/// Gets or sets the view object.
		/// </summary>
		/// <value>The view.</value>
		IGridView ViewObject { get; set; }

		/// <summary>
		/// Gets or sets the model object.
		/// </summary>
		/// <value>The model object.</value>
		Dataelus.MatchTableEditor ModelObject { get; set; }

		/// <summary>
		/// Loads the view.
		/// </summary>
		void LoadView ();

		/// <summary>
		/// Adds the columns to the model and the view.
		/// </summary>
		/// <param name="columns">Columns.</param>
		void AddColumns (IEnumerable<object> columns);

		/// <summary>
		/// Adds the rows to the model and the view.
		/// </summary>
		/// <param name="rows">Rows.</param>
		void AddRows (IEnumerable<object> rows);

		/// <summary>
		/// Saves the data.
		/// </summary>
		void Save ();
	}

	/// <summary>
	/// Controller.
	/// </summary>
	public class Controller : IController
	{
		protected IGridView _viewObject;

		protected Dataelus.MatchTableEditor _modelObject;

		protected IComparer<object> _columnSorter;

		public IComparer<object> ColumnSorter {
			get { return _columnSorter; }
			set { _columnSorter = value; }
		}

		/// <summary>
		/// Gets or sets the save callback.
		/// </summary>
		/// <value>The save callback.</value>
		public EventHandler SaveCallback { get; set; }

		public Controller (IIntersectionValue intersector, IComparer<object> columnSorter, IEnumerable<object> rows, IEnumerable<object> columns)
			: this (intersector, columnSorter)
		{
			_modelObject.RowList.AddRange (rows);
			_modelObject.ColumnList.AddRange (columns);
		}

		public Controller (IIntersectionValue intersector, IEnumerable<object> rows, IEnumerable<object> columns)
		{
			this._modelObject = new Dataelus.MatchTableEditor (intersector, rows, columns);
		}

		public Controller (IIntersectionValue intersector, IComparer<object> columnSorter)
		{
			_modelObject = new Dataelus.MatchTableEditor (intersector);
			this._columnSorter = columnSorter;
		}

		public Controller (IGridView viewObject, Dataelus.MatchTableEditor modelObject)
		{
			if (viewObject == null)
				throw new ArgumentNullException ("viewObject");
			if (modelObject == null)
				throw new ArgumentNullException ("modelObject");
			this._viewObject = viewObject;
			this._modelObject = modelObject;

			viewObject.Controller = this;
		}

		public Controller (IGridView viewObject, Dataelus.MatchTableEditor modelObject, IComparer<object> columnSorter)
		{
			if (viewObject == null)
				throw new ArgumentNullException ("viewObject");
			if (modelObject == null)
				throw new ArgumentNullException ("modelObject");
			this._viewObject = viewObject;
			this._modelObject = modelObject;
			this._columnSorter = columnSorter;

			viewObject.Controller = this;
		}

		#region IController implementation

		public void LoadView ()
		{
			_viewObject.LoadColumns (_modelObject.GetCrossTab (_columnSorter)
				, new RowItem (_modelObject.RowList.ToList ())
				, new ColumnItem (_modelObject.ColumnList.ToList ())
			);
		}

		public void AddColumns (IEnumerable<object> columns)
		{
			throw new NotImplementedException ();
		}

		public void AddRows (IEnumerable<object> rows)
		{
			throw new NotImplementedException ();
		}

		public void Save ()
		{
			var table = _modelObject.GetCrossTab (_columnSorter);
			_viewObject.Unload (table
				, new RowItem (_modelObject.RowList.ToList ())
				, new ColumnItem (_modelObject.ColumnList.ToList ())
			);
			_modelObject.SetCrossTab (table);

			if (SaveCallback != null) {
				SaveCallback (this, new EventArgs ());
			}
		}

		public IGridView ViewObject {
			get { return _viewObject; }
			set { _viewObject = value; }
		}

		public Dataelus.MatchTableEditor ModelObject {
			get { return _modelObject; }
			set { _modelObject = value; }
		}

		#endregion
	}
}

