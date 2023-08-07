using System;

namespace Dataelus
{
	public interface ICollectionSearchable<T> : ICollection2<T>
	{
		bool Contains(T item, System.Collections.Generic.IEqualityComparer<T> comparer);
	}
}

