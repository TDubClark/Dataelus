using System;

namespace Dataelus
{
	/// <summary>
	/// Interface for a globally unique identifier.  Objects which implement this can be uniquely identified in a collection.
	/// </summary>
	public interface IGlobalUniqueIdentifier
	{
		Guid ObjectGUID{ get; set; }
	}
}

