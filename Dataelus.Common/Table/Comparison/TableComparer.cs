using System;
using System.Collections.Generic;

namespace Dataelus.Table.Comparison
{
	public class TableComparer
	{
		public IRecordEqualityComparer RecordComparer{ get; set; }

		public IRecordEvaluator Evaluator{ get; set; }

		public object Table1{ get; set; }

		public object Table2{ get; set; }

		public ITableIterator Table1Iterator{ get; set; }

		public ITableIterator Table2Iterator{ get; set; }

		public virtual IssueItem[] GetIssues ()
		{
			List<IssueItem> issues = new List<IssueItem> ();
			object record1;
			while (this.Table1Iterator.GetNextRecord (this.Table1, out record1)) {
				List<object> records2 = new List<object> ();
				object record2;
				while (this.Table2Iterator.GetNextRecord (this.Table2, out record2)) {
					if (RecordComparer.RecordEquals (record1, record2)) {
						records2.Add (record2);
					}
				}
				var issue = this.Evaluator.Evaluate (record1, records2.ToArray ());
				if (issue != null)
					issues.Add (issue);
			}

			return issues.ToArray ();
		}

		public TableComparer ()
		{
		}
	}

	public interface ITableIterator
	{
		bool GetNextRecord (object table, out object record);
	}
}

