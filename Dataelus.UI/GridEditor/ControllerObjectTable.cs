using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.UI.GridEditor
{
	/// <summary>
	/// Controller object table.
	/// </summary>
	public class ControllerObjectTable : Controller<Dataelus.Table.ObjectTable>
	{
		public override void RunAfterDeserialization ()
		{
			if (_gridDataObject != null)
				_gridDataObject.RunPostDeserialization ();
			base.RunAfterDeserialization ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.UI.GridEditor.ControllerObjectTable"/> class.
		/// </summary>
		/// <param name="dbFields">Db fields.</param>
		/// <param name="dbConstraints">Db constraints.</param>
		/// <param name="dataObject">Data object.</param>
		/// <param name="viewObject">View object.</param>
		/// <param name="saver">Saver.</param>
		/// <param name="validator">Validator.</param>
		public ControllerObjectTable (EDD.EDDFieldCollection dbFields, Database.DBConstraintCollection dbConstraints
			, Table.ObjectTable dataObject, IView<Table.ObjectTable> viewObject
			, IDataSaver<Table.ObjectTable> saver, IValidator<Table.ObjectTable> validator)
			: base (dbFields, dbConstraints, dataObject, viewObject, saver, validator)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.UI.GridEditor.ControllerObjectTable"/> class.
		/// </summary>
		protected ControllerObjectTable ()
			: base ()
		{
		}

		#region implemented abstract members of Controller

		/// <summary>
		/// Moves the row.
		/// </summary>
		/// <param name="rowIndex">The index of the row to be moved.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="magnitude">Magnitude.</param>
		public override void MoveRow (int rowIndex, VerticalDirection direction, int magnitude)
		{
			_gridDataObject.Rows.MoveItem (rowIndex, GetNewIndex (direction, rowIndex, magnitude, _gridDataObject.RowCount - 1));
			this.ViewObject.MoveRow (rowIndex, direction, magnitude);
		}

		/// <summary>
		/// Moves the column.
		/// </summary>
		/// <param name="columnName">The name of the column to be moved.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="magnitude">Magnitude.</param>
		public override void MoveColumn (string columnName, HorizontalDirection direction, int magnitude)
		{
			MoveColumn (_gridDataObject.Columns.FindIndex (columnName), direction, magnitude);
		}

		/// <summary>
		/// Moves the column.
		/// </summary>
		/// <param name="columnName">The name of the column to be moved.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="magnitude">Magnitude.</param>
		/// <param name="columnIndex">Column index.</param>
		public override void MoveColumn (int columnIndex, HorizontalDirection direction, int magnitude)
		{
			string columnName = _gridDataObject.Columns [columnIndex].ColumnName;
			_gridDataObject.Columns.MoveItem (columnIndex, GetNewIndex (direction, columnIndex, magnitude, _gridDataObject.ColumnCount - 1));
			this.ViewObject.MoveColumn (columnName, direction, magnitude);
		}

		/// <summary>
		/// Called when the value of the given row index is changed, for the given column name.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnName">Column name.</param>
		/// <param name="newValue">New value of the given cell.</param>
		public override void ValueChanged (int rowIndex, string columnName, string newValue)
		{
			_gridDataObject.Rows [rowIndex] [columnName] = newValue;
		}

		public override void ValueChanged (int rowIndex, string columnName, object newValue)
		{
			_gridDataObject.Rows [rowIndex] [columnName] = newValue;
		}

		public override void SaveData ()
		{
			_viewObject.UnloadFormData (_eddFields, _gridDataObject);
			_saver.SaveData (_gridDataObject);
		}

		public override void AppendRow ()
		{
			_gridDataObject.AddRow (_gridDataObject.CreateRow ());
			this.ViewObject.InsertRow (_gridDataObject.RowCount - 1);
		}

		public override void InsertRow (int rowIndex)
		{
			_gridDataObject.InsertRow (rowIndex, _gridDataObject.CreateRow ());
			this.ViewObject.InsertRow (rowIndex);
		}

		public override void DeleteRow (int rowIndex)
		{
			_gridDataObject.DeleteRow (rowIndex);
			this.ViewObject.DeleteRow (rowIndex);
		}

		public override void ValidateRow (int rowIndex)
		{
			var result = _validator.IsValidRecord (_gridDataObject, rowIndex);
			this.ViewObject.ShowValidityRow (rowIndex, result);
		}

		public override void ValidateGrid ()
		{
			var result = _validator.IsValid (_gridDataObject);
			this.ViewObject.ShowValidityGrid (result);
		}

		public override void ValidateGrid (Dataelus.UI.GridItemViewer.Generic.IView<Dataelus.Table.ObjectTable> gridItemViewer)
		{
			if (gridItemViewer == null)
				throw new ArgumentNullException ("gridItemViewer");

			var ctlr = new GridItemViewer.Generic.Controller<Table.ObjectTable> (gridItemViewer);
			ctlr.GridDataObject = _gridDataObject;
			ctlr.ItemDataObject = _validator.GetIssues (_gridDataObject);

			ctlr.LoadViewData ();
		}

		#endregion
	}
}

