using System;

namespace Dataelus.TableIssue
{
	/// <summary>
	/// A collection of issue items
	/// </summary>
	public interface ICellIssueItemCollection : ICollection2<ICellIssueItem>
	{
		int FindIndexByObjectId(int objectID);
	}

	/// <summary>
	/// A collection of issue items
	/// </summary>
	public interface ICellIssueItemPagedCollection : ICollection2<ICellIssueItemPaged>
	{
		int FindIndexByObjectId(int objectID);
	}
}

