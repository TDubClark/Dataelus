using System;

namespace Dataelus
{
	/// <summary>
	/// Interface for a unique identifier.  Objects which implement this can be uniquely identified in a collection.
	/// </summary>
	public interface IUniqueIdentifier
	{
		long ObjectUniqueID { get; set; }
	}
}

