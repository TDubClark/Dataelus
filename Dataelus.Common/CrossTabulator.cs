using System;
using System.Collections.Generic;
using System.Linq;

using Dataelus.Table;

namespace Dataelus
{
	public static class CrossTabulator
	{
		public static ObjectTable GetCrossTab<TRow, TColumn, TResult> (IList<TRow> rows, IList<TColumn> columns, Func<TRow, TColumn, object> getIntersection, Func<TColumn, string> getColumnHeader, Func<TRow, object[]> getRowHeader, params ObjectColumn[] rowHeaderColumns)
		{
			var table = new ObjectTable ();

			int rowHeaderCount = rowHeaderColumns.Length;

			// Add the Row Header columns
			table.Columns.AddItems (rowHeaderColumns);

			// Add the value columns
			for (int c = 0; c < columns.Count; c++) {
				table.AddColumn (getColumnHeader (columns [c]), typeof(TResult));
			}

			for (int r = 0; r < rows.Count; r++) {
				var trow = table.CreateRow ();

				// Add the Row Header values
				object[] rowheader = getRowHeader (rows [r]);
				for (int c = 0; c < rowHeaderCount; c++) {
					trow [c] = rowheader [c];
				}

				// Add the values
				for (int c = 0; c < columns.Count; c++) {
					trow [c + rowHeaderCount] = getIntersection (rows [r], columns [c]);
				}

				table.AddRow (trow);
			}

			return table;
		}
	}
}

