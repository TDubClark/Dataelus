using System;
using System.Collections;
using System.Collections.Generic;

using Dataelus.Extensions;
using System.Linq;

namespace Dataelus.Mono.Extensions
{
	/// <summary>
	/// Table extensions.
	/// </summary>
	public static class TableExtensions
	{
		/// <summary>
		/// Gets the Mono/.NET DataTable from the this ObjectTable.
		/// </summary>
		/// <returns>The mono data table.</returns>
		/// <param name="table">Table.</param>
		public static System.Data.DataTable ToMonoDataTable (this Dataelus.Table.ObjectTable table)
		{
			return DataServices.GetMonoDataTable (table);
		}

		/// <summary>
		/// Gets the ObjectTable from the this Mono/.NET DataTable.
		/// </summary>
		/// <returns>The mono data table.</returns>
		/// <param name="table">Table.</param>
		public static Dataelus.Table.ObjectTable ToObjectTable (this System.Data.DataTable table)
		{
			return DataServices.GetObjectTable (table);
		}


		#region Mono/.NET DataTable Extensions

		/// <summary>
		/// Gets this DataColumnCollection as a list.
		/// </summary>
		/// <returns>The list.</returns>
		/// <param name="columns">Columns.</param>
		public static List<System.Data.DataColumn> ToList (this System.Data.DataColumnCollection columns)
		{
			return columns.ToList<System.Data.DataColumn> ();
		}

		/// <summary>
		/// Gets the column names.
		/// </summary>
		/// <returns>The array of column names.</returns>
		/// <param name="columns">Columns.</param>
		public static string[] GetColumnNames (this System.Data.DataColumnCollection columns)
		{
			return columns.ToList ().Select (x => x.ColumnName).ToArray ();
		}

		#endregion
	}
}

