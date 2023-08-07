using System;

namespace Dataelus.Table.Comparison.Generic
{
	/// <summary>
	/// Record evaluator.
	/// </summary>
	public class RecordEvaluator<T> : IRecordEvaluator<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Table.Comparison.Generic.RecordEvaluator`1"/> class.
		/// </summary>
		public RecordEvaluator ()
		{
		}

		#region IRecordEvaluator implementation

		/// <summary>
		/// Evaluate the specified table1Record and table2Record.
		/// </summary>
		/// <param name="table1Record">Table1 record.</param>
		/// <param name="table2Records">Table2 records.</param>
		/// <param name="table1RecordIndex">Table1 record index.</param>
		/// <param name="table2RecordIndexes">Table2 record indexes.</param>
		public virtual IssueItem Evaluate (T table1Record, int table1RecordIndex, T[] table2Records, int[] table2RecordIndexes)
		{
			int id = -1;
			if (table2Records.Length == 0)
				return new IssueItem (id, "Record Issue", "Record Missing", new CellRange (table1RecordIndex), new CellRange (table2RecordIndexes));
			if (table2Records.Length > 1)
				return new IssueItem (id, "Record Issue", "Record Duplicated", new CellRange (table1RecordIndex), new CellRange (table2RecordIndexes));
			return null;
		}

		#endregion
	}
}

