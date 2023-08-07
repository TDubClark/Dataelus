using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Mono.DynamiQuery
{
	/// <summary>
	/// Interface for a table record editor view.
	/// </summary>
	public interface ITableRecordEditorView
	{
		ITableRecordEditorController Controller { get; set; }

		void LoadTableSelector (IEnumerable<DBTable> tables, string selectedTable);

		void LoadFieldList (IEnumerable<FieldSelection> fields);

		void LoadOrderNumber(int order);
	}
}
