using System;
using System.Collections.Generic;

namespace Dataelus.Extensions.IList
{
	/// <summary>
	/// Extensions for the IList(T) Interface.
	/// </summary>
	public static class IListExtensions
	{
		/// <summary>
		/// Finds the index of the first item which matches the predicate; else returns -1.
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="list">List.</param>
		/// <param name="predicate">Predicate.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static int FindIndex<T> (this IList<T> list, Predicate<T> predicate)
		{
			for (int i = 0; i < list.Count; i++) {
				if (predicate (list [i]))
					return i;
			}
			return -1;
		}
	}
}

