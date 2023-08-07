using System;

namespace Dataelus.Extensions
{
	/// <summary>
	/// Comma-Separated Values File extensions.
	/// </summary>
	public static class CSVExtensions
	{
		/// <summary>
		/// Gets this string as a CSV-safe format.
		/// </summary>
		/// <returns>The safe CS.</returns>
		/// <param name="value">Value.</param>
		public static string ToSafeCSV (this string value)
		{
			return CSV.CSVServices.ToSafeCSV (value);
		}
	}
}

