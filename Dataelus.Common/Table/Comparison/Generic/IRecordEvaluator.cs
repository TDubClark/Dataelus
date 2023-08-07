using System;

namespace Dataelus.Table.Comparison.Generic
{
	public interface IRecordEvaluator<R>
	{
		/// <summary>
		/// Evaluate the specified table1Record and table2Record.
		/// </summary>
		/// <param name="table1Record">Table1 record.</param>
		/// <param name="table2Records">Table2 records.</param>
		IssueItem Evaluate(R table1Record, int table1RecordIndex, R[] table2Records, int[] table2RecordIndexes);
	}
}

