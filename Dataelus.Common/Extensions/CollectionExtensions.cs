using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Reflection;
using System;

namespace Dataelus.Extensions
{
	public static class CollectionExtensions
	{
		public static List<T> ToList<T> (this IEnumerable collection)
		{
			var lst = new List<T> ();
			foreach (T item in collection) {
				lst.Add (item);
			}
			return lst;
		}

		public static int FindIndex<T> (this IList<T> collection, Predicate<T> match)
		{
			for (int i = 0; i < collection.Count; i++) {
				if (match (collection [i]))
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Applies the given action to each item
		/// </summary>
		/// <param name="collection">Collection.</param>
		/// <param name="action">Action.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void ForEach<T> (this IEnumerable<T> collection, Action<T> action)
		{
			foreach (var item in collection) {
				action (item);
			}
		}

		/// <summary>
		/// Gets this collection as an ObjectTable.
		/// </summary>
		/// <returns>The ObjectTable.</returns>
		/// <param name="collection">Collection.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static Table.ObjectTable ToObjectTable<T> (this IEnumerable<T> collection)
		{
			return collection.ToObjectTable (true);
		}

		/// <summary>
		/// Tos the object table.
		/// </summary>
		/// <returns>The object table.</returns>
		/// <param name="collection">Collection.</param>
		/// <param name="assignPointersToRowTags">If set to <c>true</c> assigns pointers to row tags.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static Table.ObjectTable ToObjectTable<T> (this IEnumerable<T> collection, bool assignPointersToRowTags)
		{
			PropertyInfo[] props = typeof(T).GetProperties ();

			var table = new Dataelus.Table.ObjectTable ();

			foreach (var item in props) {
				table.AddColumn (item.Name, item.PropertyType);
			}

			foreach (var item in collection) {
				var row = table.CreateRow ();
				if (assignPointersToRowTags)
					row.CustomTag = item;  // Assign pointer to Custom Tag

				foreach (var prop in props) {
					row [prop.Name] = prop.GetValue (item, null);
				}

				table.AddRow (row);
			}

			return table;
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <returns>The count.</returns>
		/// <param name="collection">Collection.</param>
		public static int GetCount (this System.Collections.IEnumerable collection)
		{
			int count = 0;
			foreach (var item in collection) {
				count++;
			}
			return count;
		}

		/// <summary>
		/// Adds the given range of items to this collection.
		/// </summary>
		/// <param name="list">List.</param>
		/// <param name="items">Items.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void AddRange<T> (this ICollection<T> list, params T[] items)
		{
			foreach (var item in items) {
				list.Add (item);
			}
		}

		/// <summary>
		/// Adds the given items to this collection.
		/// </summary>
		/// <param name="collection">Collection.</param>
		/// <param name="items">Items.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void AddItems<T> (this ICollection<T> collection, IEnumerable<T> items)
		{
			foreach (var item in items) {
				collection.Add (item);
			}
		}
	}
}

