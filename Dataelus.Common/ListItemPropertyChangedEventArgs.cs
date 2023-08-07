using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus
{
	/// <summary>
	/// List item property changed event arguments.
	/// </summary>
	public class ListItemPropertyChangedEventArgs<T> : EventArgs
	{
		/// <summary>
		/// Gets or sets the list item.
		/// </summary>
		/// <value>The list item.</value>
		public T ListItem { get; set; }

		/// <summary>
		/// Gets or sets the new value.
		/// </summary>
		/// <value>The new value.</value>
		public object NewValue { get; set; }

		/// <summary>
		/// Gets or sets the prior value.
		/// </summary>
		/// <value>The prior value.</value>
		public object PriorValue { get; set; }

		/// <summary>
		/// Gets or sets the index of the item set.
		/// </summary>
		/// <value>The index set.</value>
		public int IndexSet { get; set; }

		public ListItemPropertyChangedEventArgs ()
		{
		}

		public ListItemPropertyChangedEventArgs (T listItem, object newValue, object priorValue, int indexSet)
		{
			this.ListItem = listItem;
			this.NewValue = newValue;
			this.PriorValue = priorValue;
			this.IndexSet = indexSet;
		}
	}

	public delegate void ListItemPropertyChangedEventHandler<T> (object sender, ListItemPropertyChangedEventArgs<T> args);
}

