using System;
using System.Collections.Generic;

namespace Dataelus
{
	/// <summary>
	/// Interface for a generic collection with additional methods and properties.
	/// </summary>
	public interface ICollection2<T> : ICollection<T>
	{
		T this [int index] { get; set; }

		List<T> Items { get; }

		void AddItems (IEnumerable<T> items);

		void Sort (IComparer<T> comparer);
	}
}
