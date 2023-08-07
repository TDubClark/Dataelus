using System;

namespace Dataelus.Table.Comparison
{
	/// <summary>
	/// Issue item for comparison between two cell ranges (typically from two different tables).
	/// </summary>
	public class IssueItem : TableIssue.IssueBase
	{
		/// <summary>
		/// Gets or sets the table1 range.
		/// </summary>
		/// <value>The table1 range.</value>
		public CellRange Table1Range{ get; set; }

		/// <summary>
		/// Gets or sets the table2 range.
		/// </summary>
		/// <value>The table2 range.</value>
		public CellRange Table2Range{ get; set; }

		public IssueItem ()
			: base ()
		{
		}

		public IssueItem (int itemId, string category, string description)
			: base (itemId, category, description)
		{
		}

		public IssueItem (int itemId, string category, string description, CellRange t1, CellRange t2)
			: this (itemId, category, description)
		{
			this.Table1Range = t1;
			this.Table2Range = t2;
		}

		public IssueItem (Dataelus.TableIssue.IIssue other)
			: base (other)
		{
		}
	}
}

