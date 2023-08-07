using System;

namespace Dataelus.Table
{
	/// <summary>
	/// Invalid value exception.
	/// </summary>
	public class InvalidValueException : FormatException
	{
		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value { get; set; }

		/// <summary>
		/// Gets or sets the name of the column.
		/// </summary>
		/// <value>The name of the column.</value>
		public string ColumnName { get; set; }

		public InvalidValueException ()
			: base ()
		{
		}

		public InvalidValueException (string message)
			: this (message, null)
		{
		}

		public InvalidValueException (string message, string value)
			: base (message)
		{
			this.Value = value;
		}

		public InvalidValueException (string message, string value, string columnName)
			: this (message, value)
		{
			this.ColumnName = columnName;
		}

		/// <summary>
		/// Gets the full message (format: "Column: [columnName]; Value: [value]; Message: [message]").
		/// </summary>
		/// <returns>The full message.</returns>
		public string GetFullMessage ()
		{
			return String.Format ("Column: {2}; Value: {1}; Message: {0}", base.Message, this.Value, this.ColumnName);
		}
	}
}

