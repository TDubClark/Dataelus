using System;

namespace Dataelus.Table
{
	/// <summary>
	/// Row edit state.  The state of a row defined by editing.
	/// </summary>
	public enum RowEditState
	{
		/// <summary>
		/// The row was inserted since editing started.
		/// </summary>
		Inserted,
		/// <summary>
		/// The row was updated since editing started.
		/// </summary>
		Updated,
		/// <summary>
		/// The row was deleted since editing started.
		/// </summary>
		Deleted,
		/// <summary>
		/// The row is unchanged since editing started.
		/// </summary>
		Unchanged,

		/// <summary>
		/// Undefined State.
		/// </summary>
		Undefined
	}
}

