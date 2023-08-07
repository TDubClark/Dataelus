using System;

namespace Dataelus.Collections
{
	public class OrderedListItemSticky<T> : OrderedListItem<T>, IOrderedListItemSticky
	{
		public OrderedListItemSticky ()
			: base ()
		{
			Init ();
		}

		public OrderedListItemSticky (T value, int orderIndex)
			: base (value, orderIndex)
		{
			Init ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Collections.OrderedListItemSticky`1"/> class.
		/// </summary>
		/// <param name="other">Other.</param>
		public OrderedListItemSticky (OrderedListItem<T> other)
			: base (other.Value, other.OrderIndex)
		{
			Init ();
		}

		public OrderedListItemSticky (OrderedListItemSticky<T> other)
			: this (other.Value, other.OrderIndex, other.isSticky, other.stickinessObject, other.absolutePosition, other.priorityIndex, other.targetItem)
		{
		}

		public OrderedListItemSticky (T value, int orderIndex, bool isSticky, StickyTo stickinessObject, int absolutePosition, int priorityIndex, object targetItem)
			: base (value, orderIndex)
		{
			this._isSticky = isSticky;
			this._stickinessObject = stickinessObject;
			this._absolutePosition = absolutePosition;
			this._priorityIndex = priorityIndex;
			this._targetItem = targetItem;
		}



		/// <summary>
		/// Init this instance with default values.
		/// </summary>
		void Init ()
		{
			_isSticky = false;
			_absolutePosition = -1;
			_priorityIndex = -1;
			_stickinessObject = StickyTo.AbsolutePosition;
			_targetItem = null;
		}


		#region IOrderedListItemSticky implementation

		public void CopyFrom (IOrderedListItemSticky other)
		{
			this.absolutePosition = other.absolutePosition;
			this.isSticky = other.isSticky;
			this.OrderIndex = other.OrderIndex;
			this.priorityIndex = other.priorityIndex;
			this.stickinessObject = other.stickinessObject;
			this.targetItem = other.targetItem;
		}

		protected bool _isSticky;

		public bool isSticky {
			get { return _isSticky; }
			set { _isSticky = value; }
		}

		protected StickyTo _stickinessObject;

		public StickyTo stickinessObject {
			get { return _stickinessObject; }
			set { _stickinessObject = value; }
		}

		protected int _absolutePosition;

		public int absolutePosition {
			get { return _absolutePosition; }
			set { _absolutePosition = value; }
		}

		protected int _priorityIndex;

		public int priorityIndex {
			get { return _priorityIndex; }
			set { _priorityIndex = value; }
		}

		protected object _targetItem;

		public object targetItem {
			get { return _targetItem; }
			set { _targetItem = value; }
		}

		#endregion

		//		#region IOrderedListItem implementation
		//
		//		public int OrderIndex {
		//			get { return _orderIndex; }
		//			set { _orderIndex = value; }
		//		}
		//
		//		#endregion
	}
}

