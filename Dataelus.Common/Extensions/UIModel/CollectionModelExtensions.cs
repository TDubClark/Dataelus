using System;
using System.Collections.Generic;

using Dataelus.Generic;

namespace Dataelus.Extensions.UIModel
{
	public static class CollectionModelExtensions
	{
		/// <summary>
		/// Gets a Boolean Intersect UI model around this collection.
		/// </summary>
		/// <returns>The boolean intersect.</returns>
		/// <param name="collection">Collection.</param>
		/// <param name="creator">Creator - creates a new item.</param>
		/// <param name="finder">Finder - finds whether the item matches the given intersection.</param>
		/// <typeparam name="TItem">The list item type.</typeparam>
		/// <typeparam name="TRow">The Row intersection object.</typeparam>
		/// <typeparam name="TColumn">The Column intersection object.</typeparam>
		public static BooleanIntersectList<TItem, TRow, TColumn> ToBooleanIntersect<TItem, TRow, TColumn> (
			this IList<TItem> collection
			, Func<TRow, TColumn, TItem> creator
			, Func<TItem, TRow, TColumn, bool> finder)
		{
			return new BooleanIntersectList<TItem, TRow, TColumn> (collection, creator, finder);
		}

		public static BooleanListIntersector<TRow, TColumn> GetBooleanIntersectWrapper<TItem, TRow, TColumn> (
			this IList<TItem> collection
			, Func<TRow, TColumn, TItem> creator
			, Func<TItem, TRow, TColumn, bool> finder)
		{
			return new BooleanListIntersector<TRow, TColumn> (collection.ToBooleanIntersect (creator, finder));
		}
	}
}

