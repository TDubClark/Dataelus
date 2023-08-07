using System;

namespace Dataelus.Table
{
	/// <summary>
	/// Interface for a table.
	/// </summary>
	public interface ITable
	{
		/// <summary>
		/// Finds the index of the column.
		/// </summary>
		/// <returns>The column index.</returns>
		/// <param name="columnName">Column name.</param>
		int FindColumnIndex (string columnName);

		/// <summary>
		/// Determines whether this instance is tracking edits.
		/// </summary>
		/// <returns><c>true</c> if this instance is tracking edits; otherwise, <c>false</c>.</returns>
		bool IsTrackingEdits();

		/// <summary>
		/// Starts the edit tracking.
		/// </summary>
		void StartEditTracking();

		/// <summary>
		/// Stops the edit tracking.
		/// </summary>
		void StopEditTracking();

		void ClearEditState();
	}
}

