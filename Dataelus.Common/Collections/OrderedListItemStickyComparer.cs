using System;
using System.Linq;
using System.Collections.Generic;

namespace Dataelus.Collections
{
	/*
	 * TODO
	 * 1) [DONE 9/29/2915] does not account for collisions - same relative position item
	 * 2) [In Progress 9/29/2915] does not properly avoid recursive relative references
	 * 3) [DONE 9/29/2915] if recursively seeking out relative references,
	 *      it should track how many recursion calls, and use that recursion count for comparison if two items track to the same item
	 */

	public class OrderedListItemStickyComparer<T> : OrderedListItemComparer<T>, IComparer<T> where T : IOrderedListItem
	{
		protected bool _withStickiness;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Dataelus.Collections.OrderedListItemStickyComparer`1"/>
		/// should sort with stickiness in consideration.
		/// </summary>
		/// <value><c>true</c> if sorting according to stickiness; otherwise, <c>false</c>.</value>
		public bool WithStickiness {
			get { return _withStickiness; }
			set { _withStickiness = value; }
		}

		protected bool _preferAbsoluteStickiness;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Dataelus.Collections.OrderedListItemStickyComparer`1"/>
		/// will prefer absolute stickiness positions over relative stickiness positions (default is False).
		/// </summary>
		/// <value><c>true</c> if prefer absolute stickiness; otherwise, <c>false</c>.</value>
		public bool PreferAbsoluteStickiness {
			get { return _preferAbsoluteStickiness; }
			set { _preferAbsoluteStickiness = value; }
		}

		protected bool _recursiveTargets;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Dataelus.Collections.OrderedListItemStickyComparer`1"/>
		/// looks for recursive target items.
		/// </summary>
		/// <value><c>true</c> if recursive targets; otherwise, <c>false</c>.</value>
		public bool RecursiveTargets {
			get { return _recursiveTargets; }
			set { _recursiveTargets = value; }
		}

		public OrderedListItemStickyComparer ()
			: this (true)
		{
		}

		public OrderedListItemStickyComparer (bool withStickiness)
			: this (withStickiness, false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Collections.OrderedListItemStickyComparer`1"/> class.
		/// </summary>
		/// <param name="withStickiness">If set to <c>true</c> with stickiness.</param>
		/// <param name="preferAbsoluteStickiness">If set to <c>true</c> prefer absolute stickiness over relative stickiness (default is False).</param>
		public OrderedListItemStickyComparer (bool withStickiness, bool preferAbsoluteStickiness)
			: this (withStickiness, preferAbsoluteStickiness, false)
		{
		}

		public OrderedListItemStickyComparer (bool withStickiness, bool preferAbsoluteStickiness, bool recursiveTargets)
		{
			this._withStickiness = withStickiness;
			this._preferAbsoluteStickiness = preferAbsoluteStickiness;
			this._recursiveTargets = recursiveTargets;
		}

		/// <summary>
		/// Reverse the specified compareResult.
		/// </summary>
		/// <param name="compareResult">Compare result.</param>
		protected int Reverse (int compareResult)
		{
			if (compareResult < 0)
				return 1;
			if (compareResult > 0)
				return -1;
			return compareResult;
		}

		/// <summary>
		/// Compare the specified x and y.
		/// </summary>
		/// <param name="x">The x item.</param>
		/// <param name="y">The y item.</param>
		protected override int Compare (IOrderedListItem x, IOrderedListItem y)
		{
			if (_withStickiness) {
				IOrderedListItemSticky xs = x as IOrderedListItemSticky;
				IOrderedListItemSticky ys = y as IOrderedListItemSticky;

				if (xs != null) {
					if (!xs.isSticky)
						xs = null;
				}
				if (ys != null) {
					if (!ys.isSticky)
						ys = null;
				}

				if (xs != null && ys != null) {
					// Both Sticky
					if (xs.stickinessObject == StickyTo.AbsolutePosition && ys.stickinessObject == StickyTo.AbsolutePosition) {
						// Both are sticky to an absolute position
						return CompareAbsolutes (xs, ys);
					}
					if (xs.stickinessObject == StickyTo.AbsolutePosition) {
						// Only X is absolute
						return CompareAsymetric (xs, ys); //CompareAsymetric (xs.absolutePosition, y.OrderIndex);
					}
					if (ys.stickinessObject == StickyTo.AbsolutePosition) {
						// Only Y is absolute
						// Reverse, since we are putting the Y argument first
						return Reverse (CompareAsymetric (ys, xs)); // CompareAsymetric (ys.absolutePosition, x.OrderIndex);
					}

					// Neither X nor Y are absolute
					return CompareRelative (xs, ys);

				} else if (xs != null) {
					// Only X is sticky
					switch (xs.stickinessObject) {
						case StickyTo.AbsolutePosition:
							return CompareAsymetric (xs.absolutePosition, y.OrderIndex);
						case StickyTo.OtherObject:
							return CompareAsymetric (xs, y);
					}

				} else if (ys != null) {
					// Only Y is sticky
					switch (ys.stickinessObject) {
						case StickyTo.AbsolutePosition:
							// Reverse, since we are putting the Y argument first
							return Reverse (CompareAsymetric (ys.absolutePosition, x.OrderIndex));
						case StickyTo.OtherObject:
							// Reverse, since we are putting the Y argument first
							return Reverse (CompareAsymetric (ys, x));
					}
				}
			}

			return base.Compare (x, y);
		}

		/// <summary>
		/// Compares two items which are sticky to absolute positions.
		/// </summary>
		/// <returns>The absolutes.</returns>
		/// <param name="xs">Xs.</param>
		/// <param name="ys">Ys.</param>
		protected virtual int CompareAbsolutes (IOrderedListItemSticky xs, IOrderedListItemSticky ys)
		{
			int result = xs.absolutePosition.CompareTo (ys.absolutePosition);
			if (result == 0) {
				// Both are sticky to the SAME absolute position
				result = xs.priorityIndex.CompareTo (ys.priorityIndex);
				if (result == 0) {
					// Nothing to do - same preference, same priority -- someone screwed up the definitions
				}
			}
			return result;
		}

		/// <summary>
		/// Compares two items which are both sticky to other items.
		/// </summary>
		/// <returns>The relative.</returns>
		/// <param name="xs">Xs.</param>
		/// <param name="ys">Ys.</param>
		protected virtual int CompareRelative (IOrderedListItemSticky xs, IOrderedListItemSticky ys)
		{
			int xrecursionCount = 0;
			int yrecursionCount = 0;


			var xtarget = GetStickyTarget (xs, ref xrecursionCount);
			var ytarget = GetStickyTarget (ys, ref yrecursionCount);

			// Compare the target items
			int result = base.Compare (xtarget, ytarget);
			if (result == 0) {
				// Target items are both of the same order
				result = xs.priorityIndex.CompareTo (ys.priorityIndex);
				if (result == 0) {
					// Items both have the same Priority
					// Use the recursion count - the recursive distance from the target
					return xrecursionCount.CompareTo (yrecursionCount);
				}
			}
			return result;
		}

		/// <summary>
		/// Asymetric comparison between an Absolute position and a non-absolute position.
		/// Absolute position wins a tie.
		/// </summary>
		/// <returns>The asymetric.</returns>
		/// <param name="absoluteItem">Item sticky to an absolute position.</param>
		/// <param name="item">Item NOT sticky to an absolute position.</param>
		protected virtual int CompareAsymetric (IOrderedListItemSticky absoluteItem, IOrderedListItemSticky item)
		{
			if (item.isSticky) {
				switch (item.stickinessObject) {
					case StickyTo.AbsolutePosition:
						// Should not happen
						return CompareAbsolutes (absoluteItem, item);
					case StickyTo.OtherObject:
						var target = GetStickyTarget (item);

						if (_recursiveTargets) {
							// Go back to the comparer function
							return Compare (absoluteItem, target);
						} else {
							// TODO prevent recursion unless switched on
							var targetSticky = target as IOrderedListItemSticky;
							if (targetSticky != null) {
								if (targetSticky.isSticky && targetSticky.stickinessObject == StickyTo.OtherObject) {
									// This item would recurse if put back to the normal compare function
									// Prevent recursion
									// Treat like it is a normal item

									// TODO is this the correct way to get the target's order index?
									//    Should we look at the target's target, and add 1?
									return CompareAsymetric (absoluteItem.absolutePosition, target.OrderIndex);
								}
							}
							return Compare (absoluteItem, target);
						}
				}

				throw new NotImplementedException ();
			}
			return CompareAsymetric (absoluteItem.absolutePosition, item.OrderIndex);
		}

		/// <summary>
		/// Asymetric comparison between a Sticky position and a non-sticky position.
		/// Absolute position wins a tie.
		/// </summary>
		/// <returns>The asymetric.</returns>
		/// <param name="sticky">A sticky object.</param>
		/// <param name="normal">A non-sticky object.</param>
		protected virtual int CompareAsymetric (IOrderedListItemSticky sticky, IOrderedListItem normal)
		{
			if (sticky.isSticky) {
				switch (sticky.stickinessObject) {
					case StickyTo.AbsolutePosition:
						return CompareAsymetric (sticky.absolutePosition, normal.OrderIndex);
					case StickyTo.OtherObject:
						var target = GetStickyTarget (sticky);

						if (_recursiveTargets) {
							// Go back to the comparer function
							return Compare (target, normal);
						} else {
							// TODO prevent recursion unless switched on
							//throw new NotImplementedException ();

							// In this case, since:
							//   1) we are not recursing, and
							//   2) we already got the Sticky target
							// then we just have to compare the order index of the target and the normal
							var stickyTarget = target as IOrderedListItemSticky;
							if (stickyTarget != null) {
								if (stickyTarget.stickinessObject == StickyTo.AbsolutePosition) {
									// this case is not accounted for - what if the target column is an Absolute?
									//throw new NotImplementedException ();

									// TODO verify this
									return CompareAsymetric (stickyTarget.absolutePosition, normal.OrderIndex);
								}
							}
							return base.Compare (target, normal);
						}
				}
			}
			return base.Compare (sticky, normal);
		}

		/// <summary>
		/// Asymetric comparison between an Absolute position and a normal position.
		/// Absolute position wins a tie.
		/// </summary>
		/// <returns>The asymetric.</returns>
		/// <param name="absPos">Abs position.</param>
		/// <param name="pos">Position.</param>
		protected virtual int CompareAsymetric (int absPos, int pos)
		{
			int result = absPos.CompareTo (pos);
			if (result == 0) {
				result = absPos.CompareTo (pos + 1);
			}
			return result;
		}

		/// <summary>
		/// Gets the target of stickiness.
		/// </summary>
		/// <returns>The sticky target.</returns>
		/// <param name="item">Item.</param>
		protected virtual IOrderedListItem GetStickyTarget (IOrderedListItemSticky item)
		{
			int recursionCount = 0;
			return GetStickyTarget (item, ref recursionCount);
		}

		protected virtual IOrderedListItem GetStickyTarget (IOrderedListItemSticky item, ref int recursionCount)
		{
			if (item.isSticky && item.stickinessObject == StickyTo.OtherObject) {
				return GetStickyTargetRecursive (item, ref recursionCount);
			}
			return item;
		}

		/// <summary>
		/// Gets the sticky target recursive.
		/// </summary>
		/// <returns>The sticky target recursive.</returns>
		/// <param name="item">Item.</param>
		protected virtual IOrderedListItem GetStickyTargetRecursive (IOrderedListItem item, ref int recursionCount)
		{
			var sticky = item as IOrderedListItemSticky;
			if (sticky != null) {
				if (sticky.isSticky && sticky.stickinessObject == StickyTo.OtherObject) {
					// This is sticky to another object.
					var targetItem = sticky.targetItem as IOrderedListItem;
					if (targetItem == null)
						throw new NullReferenceException (String.Format ("Target item of {0} could not be cast to IOrderedListItem", sticky));
					
					if (_recursiveTargets) {
						recursionCount++;
						return GetStickyTargetRecursive (targetItem, ref recursionCount);
					} else {
						return targetItem;
					}
				}
			}
			return item;
		}

		/// <summary>
		/// Finds all indexes in the given list which match the given Predicate
		/// </summary>
		/// <returns>The index all.</returns>
		/// <param name="lstItems">Lst items.</param>
		/// <param name="predicate">Predicate.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		protected static int[] FindIndexAll (List<T> lstItems, Predicate<T> predicate)
		{
			var indexes = new List<int> ();
			for (int i = 0; i < lstItems.Count; i++) {
				if (predicate (lstItems [i]))
					indexes.Add (i);
			}
			return indexes.ToArray ();
		}


		/// <summary>
		/// Reindexs the columns, according to their current order.
		/// </summary>
		/// <param name="lstItems">Lst items.</param>
		public static void ReindexColumns (List<T> lstItems)
		{
			for (int i = 0; i < lstItems.Count; i++) {
				lstItems [i].OrderIndex = i;
			}
		}
	}
}

