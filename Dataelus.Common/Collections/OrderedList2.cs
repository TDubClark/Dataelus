using System;
using System.Linq;

namespace Dataelus.Collections
{
	public class OrderedList2<T> : ListBase<T>
	{
		/// <summary>OrderedListItemComparer2
		/// Function which gets the order for a given object.
		/// </summary>
		/// <value>The order getter.</value>
		public Func<T, int> OrderGetter { get; set; }

		/// <summary>
		/// Function which sets the order for a given object.
		/// </summary>
		/// <value>The order setter.</value>
		public Action<T, int> OrderSetter { get; set; }

		public void Sort ()
		{
			_items.Sort (new OrderedListItemComparer2<T> (this.OrderGetter, this.OrderSetter));
		}
	}
}
