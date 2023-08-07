using System;

namespace Dataelus.Table.Comparison
{
	/// <summary>
	/// Interface for a record evaluator.
	/// </summary>
	public interface IRecordEvaluator
	{
		/// <summary>
		/// Evaluate the specified table1Record and table2Record.
		/// </summary>
		/// <param name="table1Record">Table1 record.</param>
		/// <param name="table2Records">Table2 records.</param>
		IssueItem Evaluate(object table1Record, object[] table2Records);
	}
}

