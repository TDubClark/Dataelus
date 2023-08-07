using System;
using System.Collections.Generic;

namespace Dataelus.UI.MatchTableEditor
{
	/// <summary>
	/// View control manager.
	/// </summary>
	public class ViewWidgetManager : ListBase<ViewWidget>
	{
		/// <summary>
		/// Gets or sets the widget identifier comparer.
		/// </summary>
		/// <value>The widget identifier comparer.</value>
		public IEqualityComparer<string> WidgetIDComparer { get; set; }

		public ViewWidgetManager ()
			: this (new StringEqualityComparer ())
		{
		}

		public ViewWidgetManager (IEqualityComparer<string> widgetIDComparer)
		{
			if (widgetIDComparer == null)
				throw new ArgumentNullException ("widgetIDComparer");
			this.WidgetIDComparer = widgetIDComparer;
		}

		public ViewWidget AddNew (string widgetID, int rowID, int columnID)
		{
			var item = new ViewWidget (widgetID, rowID, columnID);
			Add (item);
			return item;
		}

		public int FindIndex (string widgetID)
		{
			return _items.FindIndex (x => WidgetIDComparer.Equals (x.WidgetID, widgetID));
		}
	}
}
