using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Mono
{
	public static class CollectionServices
	{
		/// <summary>
		/// Gets the value dictionary
		/// </summary>
		/// <returns>The value dictionary.</returns>
		/// <param name="values">Values.</param>
		public static Dictionary<string,string> GetValueDict (this IEnumerable<string> values)
		{
			return GetValueDict (values, new Dataelus.StringEqualityComparer ());
		}

		/// <summary>
		/// Gets the value dictionary
		/// </summary>
		/// <returns>The value dictionary.</returns>
		/// <param name="values">Values.</param>
		/// <param name="keyComparer">Dictionary Key comparer.</param>
		public static Dictionary<string, string> GetValueDict (this IEnumerable<string> values, IEqualityComparer<string> keyComparer)
		{
			var dict = new Dictionary<string, string> (keyComparer);
			foreach (var item in values) {
				if (dict.ContainsKey (item))
					continue;
				dict.Add (item, item);
			}
			return dict;
		}

		/// <summary>
		/// Gets the value dictionary
		/// </summary>
		/// <returns>The value dict.</returns>
		/// <param name="values">Values.</param>
		public static Dictionary<T, string> GetValueDict<T> (this IEnumerable<T> values, IEqualityComparer<T> comparer)
		{
			var dict = new Dictionary<T, string> (comparer); //new Dataelus.StringEqualityComparer ());
			foreach (var item in values) {
				if (dict.ContainsKey (item))
					continue;
				dict.Add (item, item.ToString ());
			}
			return dict;
		}
	}
}

