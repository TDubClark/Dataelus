using System;

namespace Dataelus.Collections
{
	/// <summary>
	/// Interface for an ordered list item, which can be sticky to other list items.
	/// </summary>
	public interface IOrderedListItemSticky : IOrderedListItem
	{
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Dataelus.TableDisplay.IColumnDefSticky"/> is sticky.
		/// </summary>
		/// <value><c>true</c> if is sticky; otherwise, <c>false</c>.</value>
		bool isSticky { get; set; }

		/// <summary>
		/// Gets or sets the object (a position or other item) to which this item prefers to adhere.
		/// </summary>
		/// <value>The stickiness object.</value>
		StickyTo stickinessObject { get; set; }

		/// <summary>
		/// Gets or sets the absolute position prefered by this item (if applicable).
		/// </summary>
		/// <value>The absolute position.</value>
		int absolutePosition { get; set; }

		/// <summary>
		/// Gets or sets the priority of this object in obtaining it's prefered position (relative to other items with the same preference).
		/// </summary>
		/// <value>The index of the priority.</value>
		int priorityIndex { get; set; }

		/// <summary>
		/// Gets or sets the target item to which this item prefers to adhere (if applicable).
		/// </summary>
		/// <value>The target item.</value>
		object targetItem { get; set; }

		/// <summary>
		/// Copies the values from the given object.
		/// </summary>
		/// <param name="other">Other.</param>
		void CopyFrom(IOrderedListItemSticky other);
	}
}
