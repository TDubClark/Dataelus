using System;
using System.Collections.Generic;

namespace Dataelus.Mono.DynamiQuery
{
	public interface IEditorView
	{
		IEditorController Controller { get; set; }

		void Load (IEnumerable<TableSelectionDef> items);

		void Edit (TableSelectionDef item);

		/// <summary>
		/// Update the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		void Update (TableSelectionDef item);
	}
}

