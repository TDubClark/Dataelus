using System;

namespace Dataelus.EDD
{
	/// <summary>
	/// Interface for an EDD record-level validation rule - applies to one record at a time
	/// </summary>
	public interface IEDDRecordValidationRule
	{
		/// <summary>
		/// Gets or sets the column names.
		/// </summary>
		/// <value>The column names.</value>
		string[] ColumnNames { get; set; }

		/// <summary>
		/// Gets or sets the column IDs.
		/// </summary>
		/// <value>The column I ds.</value>
		int[] ColumnIDs { get; set; }

		/// <summary>
		/// Gets whether the specified values are valid.
		/// </summary>
		/// <returns><c>true</c> if the specified values are valid; otherwise, <c>false</c>.</returns>
		/// <param name="values">Values.</param>
		bool IsValid (Table.IRecordValueCollection values);

		/// <summary>
		/// Determines whether the specified values are valid, returning a validation result.
		/// </summary>
		/// <returns><c>true</c> if the specified values are valid; otherwise, <c>false</c>.</returns>
		/// <param name="values">Values.</param>
		/// <param name="message">The validation message.</param>
		ITableValidationResult Validate (Table.IRecordValueCollection values);
	}

	/// <summary>
	/// Methods of Column referencing (Name or ID).
	/// </summary>
	public enum ColumnReferencing
	{
		/// <summary>
		/// Reference the name of the column.
		/// </summary>
		ColumnName,

		/// <summary>
		/// Reference the column ID.
		/// </summary>
		ColumnID
	}
}

