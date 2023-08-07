using System;
using System.Collections.Generic;

namespace Dataelus.Table
{
	public enum RowDeletedAction
	{
		/// <summary>
		/// Only mark the row as "deleted"; do not remove from the list of rows
		/// </summary>
		MarkDeletedOnly,

		/// <summary>
		/// Mark the row as deleted, and move it to a special "RowsDeleted" list
		/// </summary>
		MoveToDeletedList,

		/// <summary>
		/// Remove the row completely, and do not store it anywhere
		/// </summary>
		RemoveCompletely
	}
}

