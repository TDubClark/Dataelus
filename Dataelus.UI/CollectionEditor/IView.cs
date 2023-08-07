using System;

namespace Dataelus.UI.CollectionEditor
{
	public interface IView<T>
	{
		/// <summary>
		/// Loads the form.
		/// </summary>
		/// <param name="table">Table.</param>
		void LoadForm (Dataelus.Table.ObjectTable table);

		/// <summary>
		/// Unloads the form.
		/// </summary>
		/// <param name="table">Table.</param>
		void UnloadForm (Dataelus.Table.ObjectTable table);

		/// <summary>
		/// Append a row to the end.
		/// </summary>
		void AppendRow ();

		/// <summary>
		/// Inserts a row at the given index.
		/// </summary>
		/// <param name="rowIndex">Row Index.</param>
		void InsertRow (int rowIndex);

		/// <summary>
		/// Deletes the row at the given index.
		/// </summary>
		/// <param name="rowIndex">Row Index.</param>
		void DeleteRow (int rowIndex);
	}
}

