using System;

namespace Dataelus.Mono.Extensions
{
	/// <summary>
	/// Data IO extensions.
	/// </summary>
	public static class DataIOExtensions
	{
		/// <summary>
		/// Gets this object represented as a CSV string.
		/// </summary>
		/// <returns>The CSV string.</returns>
		/// <param name="table">Table.</param>
		public static string ToCSVString (this System.Data.DataTable table)
		{
			return DataIOServices.ToCSVString (table);
		}

		/// <summary>
		/// Gets this object represented as a CSV string.
		/// </summary>
		/// <returns>The CSV string.</returns>
		/// <param name="table">Table.</param>
		public static string ToCSVString (this Dataelus.Table.ObjectTable table)
		{
			return DataIOServices.ToCSVString (table);
		}
	}
}

