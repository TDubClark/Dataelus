using System;

namespace Dataelus.Collections
{

	public class OrderedListItemComparer2<T> : System.Collections.Generic.IComparer<T>
	{
		/// <summary>
		/// Function which gets the order for a given object.
		/// </summary>
		/// <value>The order getter.</value>
		public Func<T, int> OrderGetter { get; set; }

		/// <summary>
		/// Function which sets the order for a given object.
		/// </summary>
		/// <value>The order setter.</value>
		public Action<T, int> OrderSetter { get; set; }

		public OrderedListItemComparer2 (Func<T, int> orderGetter, Action<T, int> orderSetter)
		{
			this.OrderGetter = orderGetter;
			this.OrderSetter = orderSetter;
		}

		#region IComparer implementation

		public int Compare (T x, T y)
		{
			return OrderGetter (x).CompareTo (OrderGetter (y));
		}

		#endregion
	}
}
