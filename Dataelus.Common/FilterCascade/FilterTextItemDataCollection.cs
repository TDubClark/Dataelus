using System;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// Filter text item data collection; intended just for storing the data parts of Filter Text Items.
	/// </summary>
	public class FilterTextItemDataCollection : CollectionBase<FilterTextItemData>
	{
		public FilterTextItemDataCollection ()
			: base ()
		{
		}

		public static string SerializeJson (FilterTextItemDataCollection obj)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject (obj);
		}

		public static FilterTextItemDataCollection DeserializeJson (string jsonString)
		{
			return (FilterTextItemDataCollection)Newtonsoft.Json.JsonConvert.DeserializeObject (jsonString);
		}
	}
}

