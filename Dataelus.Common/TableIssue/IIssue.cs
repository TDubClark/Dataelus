using System;

namespace Dataelus.TableIssue
{
	/// <summary>
	/// Represents an issue (with an ID, a category, and a description).
	/// </summary>
	public interface IIssue
	{
		int ItemID { get; }

		string Category { get; }

		string Description { get; }
	}
}

