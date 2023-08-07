using System;
using System.Collections.Generic;

namespace Dataelus.Table.Comparison.Generic
{
	/// <summary>
	/// Table comparer.
	/// T = Table Type
	/// R = Record Type
	/// </summary>
	public class TableComparer<T, R>
	{
		public IRecordEqualityComparer<R> RecordComparer{ get; set; }

		/// <summary>
		/// Gets or sets the evaluator, which will evaluate a row and a set of rows which have been determined equal.
		/// </summary>
		/// <value>The evaluator.</value>
		public IRecordEvaluator<R> Evaluator{ get; set; }

		public T Table1{ get; set; }

		public T Table2{ get; set; }

		public ITableIterator<T, R> Table1Iterator{ get; set; }

		public ITableIterator<T, R> Table2Iterator{ get; set; }

		public List<IssueItem> GetIssues ()
		{
			List<IssueItem> issues = new List<IssueItem> ();
			R record1;
			while (this.Table1Iterator.GetNextRecord (this.Table1, out record1)) {
				var t2Records = new List<R> ();
				var t2Indexes = new List<int> ();
				R record2;
				while (this.Table2Iterator.GetNextRecord (this.Table2, out record2)) {
					if (RecordComparer.RecordEquals (record1, record2)) {
						t2Records.Add (record2);
						t2Indexes.Add (this.Table2Iterator.CurrentIndex);
					}
				}
				var issue = this.Evaluator.Evaluate (record1, this.Table1Iterator.CurrentIndex, t2Records.ToArray (), t2Indexes.ToArray ());
				if (issue != null)
					issues.Add (issue);
			}

			return issues;
		}

		public TableComparer ()
		{
		}
	}
}

