using System;

namespace Dataelus.EDD
{
	/// <summary>
	/// Interface for a validation result.
	/// </summary>
	public interface ITableValidationResult
	{
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Dataelus.EDD.ITableValidationResult"/> is valid.
		/// </summary>
		/// <value><c>true</c> if valid; otherwise, <c>false</c>.</value>
		bool Valid { get; set; }

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
		string Message { get; set; }

		/// <summary>
		/// Gets or sets the index of the row.
		/// </summary>
		/// <value>The index of the row.</value>
		int RowIndex { get; set; }

		/// <summary>
		/// Gets or sets the index of the column.
		/// </summary>
		/// <value>The index of the column.</value>
		int ColumnIndex { get; set; }

		/// <summary>
		/// Gets or sets the name of the column.
		/// </summary>
		/// <value>The name of the column.</value>
		string ColumnName { get; set; }
	}
}
