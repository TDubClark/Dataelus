using System;

namespace Dataelus.Collections
{
	public class OrderedListItemComparer<T> : System.Collections.Generic.IComparer<T> where T : IOrderedListItem
	{
		public OrderedListItemComparer ()
		{
		}

		protected virtual int Compare (IOrderedListItem x, IOrderedListItem y)
		{
			return x.OrderIndex.CompareTo (y.OrderIndex);
		}

		#region IComparer implementation

		public int Compare (T x, T y)
		{
			return Compare ((IOrderedListItem)x, (IOrderedListItem)y);
		}

		#endregion
	}
}

