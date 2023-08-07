using System;

namespace Dataelus.TableIssue
{
	public interface ICellIssueItemPaged : ICellIssueItem, IUniqueIdentifier
	{
		/// <summary>
		/// Gets or sets the page ID to which this issue applies.
		/// </summary>
		/// <value>The page ID.</value>
		int PageID { get; set; }
	}
}

