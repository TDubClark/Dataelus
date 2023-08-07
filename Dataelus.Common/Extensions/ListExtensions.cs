using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Extensions
{
	public static class ListExtensions
	{
		/// <summary>
		/// Adds all the items into a combined list
		/// </summary>
		/// <returns>The combined list.</returns>
		/// <param name="collection">Collection.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static List<T> ToCombinedList<T> (this IEnumerable<List<T>> collection)
		{
			var lst = new List<T> ();

			foreach (var item in collection) {
				lst.AddRange (item);
			}

			return lst;
		}

		/// <summary>
		/// Adds all the items into a combined list
		/// </summary>
		/// <returns>The combined list.</returns>
		/// <param name="collection">Collection.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static List<T> ToCombinedList<T> (this IEnumerable<T[]> collection)
		{
			var lst = new List<T> ();

			foreach (var item in collection) {
				lst.AddRange (item);
			}

			return lst;
		}

		/// <summary>
		/// Adds all the items into a combined list
		/// </summary>
		/// <returns>The combined list.</returns>
		/// <param name="collection">Collection.</param>
		/// <typeparam name="T">The core type of item.</typeparam>
		/// <typeparam name="TColl">An enumerable type which implements IEnumerable(T).</typeparam>
		public static List<T> ToCombinedList<T, TColl> (this IEnumerable<TColl> collection) where TColl : IEnumerable<T>
		{
			var lst = new List<T> ();

			foreach (var item in collection) {
				lst.AddRange (item);
			}

			return lst;
		}


		/// <summary>
		/// Copies this collection to a new list, and adds the given item to the list, and returns the new list.
		/// </summary>
		/// <returns>The to new.</returns>
		/// <param name="collection">Collection.</param>
		/// <param name="item">Item.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static List<T> AddToNew<T> (this IEnumerable<T> collection, T item)
		{
			var lst = collection.ToList ();
			lst.Add (item);
			return lst;
		}

		/// <summary>
		/// Merge the given items into this list, replacing the subset of the list defined by the mergeSubset predicate.
		/// </summary>
		/// <param name="list">List.</param>
		/// <param name="other">The other items, which will essentially replace the subset of this list.</param>
		/// <param name="comparer">Comparer.</param>
		/// <param name="mergeSubset">Defines the subset of this list into which the other items are merged (added/removed).</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Merge<T> (this IList<T> list, IEnumerable<T> other, IEqualityComparer<T> comparer, Predicate<T> mergeSubset)
		{
			var currentItems = list.Where (x => mergeSubset (x)).ToList ();

			var itemsRemove = currentItems.Where (x => !other.Contains (x, comparer)).ToList ();

			var itemsAdd = other.Where (x => !currentItems.Contains (x, comparer)).ToList ();

			// Remove items
			foreach (var item in itemsRemove) {
				int index = list.FindIndex (x => comparer.Equals (x, item));
				if (index < 0)
					continue;
				list.RemoveAt (index);
			}

			// Add items
			list.AddItems (itemsAdd);
		}
	}
}

