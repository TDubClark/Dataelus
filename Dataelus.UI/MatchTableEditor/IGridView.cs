using System;
using System.Collections.Generic;

namespace Dataelus.UI.MatchTableEditor
{
	/// <summary>
	/// Interface for a grid view.
	/// </summary>
	public interface IGridView
	{
		/// <summary>
		/// Gets or sets the controller.
		/// </summary>
		/// <value>The controller.</value>
		IController Controller { get; set; }

		/// <summary>
		/// Loads the View values from the specified table.
		/// </summary>
		/// <param name="table">Table.</param>
		/// <param name="rowRetriever">Row retriever.</param>
		/// <param name="columnRetriever">Column retriever.</param>
		void LoadColumns (Table.ObjectTable table, IRowItem rowRetriever, IColumnItem columnRetriever);

		void AddColumns (Table.ObjectTable table, IRowItem rowRetriever, IColumnItem columnRetriever);

		void AddRows (Table.ObjectTable table, IRowItem rowRetriever, IColumnItem columnRetriever);

		/// <summary>
		/// Unload the View values to the specified table.
		/// </summary>
		/// <param name="table">Table.</param>
		/// <param name="rowRetriever">Row retriever.</param>
		/// <param name="columnRetriever">Column retriever.</param>
		void Unload (Table.ObjectTable table, IRowItem rowRetriever, IColumnItem columnRetriever);
	}
}
