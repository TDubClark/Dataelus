using System;

namespace Dataelus.Collections
{
	public class OrderedListItem<T> : IOrderedListItem
	{
		protected T _value;

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public T Value {
			get { return _value; }
			set { _value = value; }
		}

		protected bool _selected;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Dataelus.Collections.OrderedListItem`1"/> is selected.
		/// </summary>
		/// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
		public bool Selected {
			get { return _selected; }
			set { _selected = value; }
		}

		public OrderedListItem ()
		{
		}

		public OrderedListItem (T value, int orderIndex)
		{
			this.Value = value;
			this._orderIndex = orderIndex;
		}

		#region IOrderedListItem implementation

		protected int _orderIndex;

		/// <summary>
		/// Gets or sets the order index.
		/// </summary>
		/// <value>The order index.</value>
		public int OrderIndex {
			get { return _orderIndex; }
			set { _orderIndex = value; }
		}

		#endregion
	}
}

