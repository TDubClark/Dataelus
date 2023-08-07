using System;
using System.Text.RegularExpressions;

namespace Dataelus.CSV
{
	/// <summary>
	/// Comma-Separated Values File services.
	/// </summary>
	public static class CSVServices
	{
		/// <summary>
		/// Gets the given string as a CSV-safe format.
		/// </summary>
		/// <returns>The safe CS.</returns>
		/// <param name="value">Value.</param>
		public static string ToSafeCSV (string value)
		{
			if (value != null) {
				// Surround with Double-quotes when a field contains any of:
				//   - Double-quote
				//   - Carriage Return and/or New Line
				//   - Comma
				if (Regex.IsMatch (value, "\"|\r|\n|[,]")) {
					// Escape the Double-quotes
					value = Regex.Replace (value, "\"", "\"\"");
					// Surround with Double-quotes
					value = String.Format ("\"{0}\"", value);
				}
			}
			return value;
		}
	}

}

