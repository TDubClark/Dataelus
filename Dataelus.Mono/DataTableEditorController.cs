using System;

namespace Dataelus.Mono
{
	/// <summary>
	/// Data table editor controller. (NOT IMPLEMENTED)
	/// </summary>
	public class DataTableEditorController : Dataelus.UI.GridEditor.Controller<System.Data.DataTable>
	{
		// NOT IMPLEMENTED

		public DataTableEditorController ()
			: base ()
		{
		}

		#region implemented abstract members of Controller

		///<summary>
		///Moves the row.
		///</summary>
		///<param name="rowIndex">The index of the row to be moved.</param>
		///<param name="direction">Direction.</param>
		///<param name="magnitude">Magnitude.</param>
		public override void MoveRow (int rowIndex, Dataelus.UI.VerticalDirection direction, int magnitude)
		{
			throw new NotImplementedException ();
		}

		///<summary>
		///Moves the column.
		///</summary>
		///<param name="columnName">The name of the column to be moved.</param>
		///<param name="direction">Direction.</param>
		///<param name="magnitude">Magnitude.</param>
		public override void MoveColumn (string columnName, Dataelus.UI.HorizontalDirection direction, int magnitude)
		{
			throw new NotImplementedException ();
		}

		///<summary>
		///Moves the column.
		///</summary>
		///<param name="columnIndex">The index of the column to be moved.</param>
		///<param name="direction">Direction.</param>
		///<param name="magnitude">Magnitude.</param>
		public override void MoveColumn (int columnIndex, Dataelus.UI.HorizontalDirection direction, int magnitude)
		{
			throw new NotImplementedException ();
		}

		///<summary>
		///Called when the value of the given row index is changed, for the given column name.
		///</summary>
		///<param name="rowIndex">Row index.</param>
		///<param name="columnName">Column name.</param>
		///<param name="newValue">New value of the given cell.</param>
		public override void ValueChanged (int rowIndex, string columnName, string newValue)
		{
			throw new NotImplementedException ();
		}

		///<summary>
		///Called when the value of the given row index is changed, for the given column name.
		///</summary>
		///<param name="rowIndex">Row index.</param>
		///<param name="columnName">Column name.</param>
		///<param name="newValue">New value of the given cell.</param>
		public override void ValueChanged (int rowIndex, string columnName, object newValue)
		{
			throw new NotImplementedException ();
		}

		///<summary>
		///Saves the data.
		///</summary>
		public override void SaveData ()
		{
			throw new NotImplementedException ();
		}

		///<summary>
		///Append a row to the end.
		///</summary>
		public override void AppendRow ()
		{
			throw new NotImplementedException ();
		}

		///<summary>
		///Insert a row at the given index.
		///</summary>
		///<param name="rowIndex">Row index.</param>
		public override void InsertRow (int rowIndex)
		{
			throw new NotImplementedException ();
		}

		///<summary>
		///Delete the row at the given index.
		///</summary>
		///<param name="rowIndex">Row index.</param>
		public override void DeleteRow (int rowIndex)
		{
			throw new NotImplementedException ();
		}

		///<summary>
		///Validates the row at the given index.
		///</summary>
		///<param name="rowIndex">Row index.</param>
		public override void ValidateRow (int rowIndex)
		{
			throw new NotImplementedException ();
		}

		///<summary>
		///Validates the grid.
		///</summary>
		public override void ValidateGrid ()
		{
			throw new NotImplementedException ();
		}

		///<summary>
		///Validates the grid, loading the issues into the given Issue view
		///</summary>
		public override void ValidateGrid (Dataelus.UI.GridItemViewer.Generic.IView<System.Data.DataTable> gridItemViewer)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

