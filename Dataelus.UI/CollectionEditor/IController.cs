using System;

namespace Dataelus.UI.CollectionEditor
{
	public interface IController<T> where T : new()
	{
		CollectionEditor<T> ModelObject { get; set; }

		IView<T> ViewObject { get; set; }

		void LoadView ();

		void SaveData ();

		/// <summary>
		/// Append a row to the end.
		/// </summary>
		void AppendRow ();

		void InsertRowAfter (T item);

		/// <summary>
		/// Insert a row at the given index.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		void InsertRow (int rowIndex);

		/// <summary>
		/// Delete the row at the given index.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		void DeleteRow (int rowIndex);
	}

	public class Controller<T> : IController<T> where T : new()
	{
		IView<T> _viewObject;

		CollectionEditor<T> _modelObject;

		#region IController implementation

		public void LoadView ()
		{
			_viewObject.LoadForm (_modelObject.Table);
		}

		public void SaveData ()
		{
			_viewObject.UnloadForm (_modelObject.Table);
		}

		public void AppendRow ()
		{
			var oRow = _modelObject.Table.CreateRow ();
			_modelObject.Table.AddRow (oRow);

			_viewObject.AppendRow ();
		}

		public void InsertRow (int rowIndex)
		{
			var oRow = _modelObject.Table.CreateRow ();
			_modelObject.Table.Rows.Insert (rowIndex, oRow);

			_viewObject.InsertRow (rowIndex);
		}

		public void InsertRowAfter (T item)
		{
			throw new NotImplementedException ();
		}

		public void DeleteRow (int rowIndex)
		{
			_modelObject.Table.Rows.RemoveAt (rowIndex);

			_viewObject.DeleteRow (rowIndex);
		}

		public CollectionEditor<T> ModelObject {
			get { return _modelObject; }
			set { _modelObject = value; }
		}

		public IView<T> ViewObject {
			get { return _viewObject; }
			set { _viewObject = value; }
		}

		#endregion
	}
}

