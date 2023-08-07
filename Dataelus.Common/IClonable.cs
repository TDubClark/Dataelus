using System;

namespace Dataelus
{
	/// <summary>
	/// Interface for a Clonable object
	/// </summary>
	public interface IClonable<T>
	{
		T Clone();
	}
}

