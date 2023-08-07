using System;

namespace Dataelus
{
	/// <summary>
	/// Interface for an equatable object, which takes a comparer for a specified type (C).
	/// </summary>
	public interface IEquatable<T, C> : IEquatable<T>
	{
		/// <summary>
		/// Gets whether this instance equals the specified other, using the given comparer.
		/// </summary>
		/// <param name="other">Other.</param>
		/// <param name="comparer">Comparer.</param>
		bool Equals(T other, System.Collections.Generic.IEqualityComparer<C> comparer);
	}
}

