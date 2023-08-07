using System;
using System.Collections.Generic;
using System.Linq;

using Dataelus.Mono.Extensions;
using Dataelus.Extensions;

namespace Dataelus.Mono
{
	/// <summary>
	/// Data IO services.
	/// </summary>
	public class DataIOServices
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DataIOServices"/> class.
		/// </summary>
		public DataIOServices ()
		{
		}

		/// <summary>
		/// Gets the given <see cref="System.Data.DataTable"/> object represented as a CSV string.
		/// </summary>
		/// <returns>The CSV string.</returns>
		/// <param name="table">Table.</param>
		public static string ToCSVString (System.Data.DataTable table)
		{
			var sb = new System.Text.StringBuilder ();

			var lstColumns = new List<string> ();
			for (int c = 0; c < table.Columns.Count; c++) {
				lstColumns.Add (table.Columns [c].ColumnName);
			}

			sb.AppendLine (String.Join (",", lstColumns));

			var lstValues = new List<object> ();
			for (int r = 0; r < table.Rows.Count; r++) {
				for (int c = 0; c < table.Columns.Count; c++) {
					lstValues.Add (table.Rows [r] [c]);
				}
				sb.AppendLine (String.Join (",", lstValues.Select (x => x.ToNullableString ().ToSafeCSV ()).ToArray ()));
				lstValues.Clear ();
			}


			return sb.ToString ();
		}

		/// <summary>
		/// Gets the given <see cref="Dataelus.Table.ObjectTable"/> object represented as a CSV string.
		/// </summary>
		/// <returns>The CSV string.</returns>
		/// <param name="table">Table.</param>
		public static string ToCSVString (Dataelus.Table.ObjectTable table)
		{
			var sb = new System.Text.StringBuilder ();

			var lstColumns = new List<string> ();
			for (int c = 0; c < table.ColumnCount; c++) {
				lstColumns.Add (table.Columns [c].ColumnName);
			}

			sb.AppendLine (String.Join (",", lstColumns));

			var lstValues = new List<object> ();
			for (int r = 0; r < table.RowCount; r++) {
				for (int c = 0; c < table.ColumnCount; c++) {
					lstValues.Add (table [r, c]);
				}
				sb.AppendLine (String.Join (",", lstValues.Select (x => x.ToNullableString ().ToSafeCSV ()).ToArray ()));
				lstValues.Clear ();
			}

			return sb.ToString ();
		}
	}
}

