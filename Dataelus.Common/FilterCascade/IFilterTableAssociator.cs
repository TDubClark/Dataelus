using System;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// Interface for an associator between filters and tables.
	/// </summary>
	public interface IFilterTableAssociator
	{
		bool IsAssociated (string tableCode, string filterCode);
	}
}

