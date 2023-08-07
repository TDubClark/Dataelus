using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.FilterCascade.WidgetData
{
	/// <summary>
	/// Selectable values cache for a filter widget (such as a drop-down box).
	/// </summary>
	public class FilterSelectableValuesCache : IFilterSelectableValuesCache
	{
		public FilterSelectableValuesCache ()
			: this (new StringEqualityComparer ())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.FilterCascade.WidgetData.FilterSelectableValuesCache"/> class.
		/// </summary>
		/// <param name="comparer">String Comparer for filter codes.</param>
		public FilterSelectableValuesCache (IEqualityComparer<string> comparer)
		{
			this.FilterData = new Dictionary<string, Dictionary<object, string>> (comparer);
		}

		/// <summary>
		/// Adds the filter.
		/// </summary>
		/// <param name="filterCode">Filter code.</param>
		/// <param name="values">Values.</param>
		public void AddFilter (string filterCode, Dictionary<object, string> values)
		{
			this.FilterData.Add (filterCode, values);
		}

		#region IFilterSelectableValuesCache implementation

		public Dictionary<string, Dictionary<object, string>> FilterData{ get; set; }

		public void Load (FilterValueItemCollection filters, FilterObjectTable table)
		{
			foreach (var filter in filters) {
				string fcode = filter.FilterCode;
				this.FilterData [fcode] = table.GetValueDisplay (filters, fcode);
			}
		}

		public void UpdateCache (string filterCode, FilterValueItemCollection filters, FilterObjectTable table)
		{
			// Load, but only for the child filters of the given filter code
			Load (filters.GetChildFilters (filterCode), table);
		}

		public Dictionary<object, string> GetWidgetData (string filterCode)
		{
			Dictionary<object, string> data;
			if (this.FilterData.TryGetValue (filterCode, out data)) {
				return data;
			}
			return null;
		}

		public object[] GetWidgetDataValues (string filterCode)
		{
			Dictionary<object, string> data = GetWidgetData (filterCode);
			if (data != null) {
				return data.Select (x => x.Key).ToArray ();
			}
			return null;
		}

		public string[] GetWidgetDataDisplayText (string filterCode)
		{
			Dictionary<object, string> data = GetWidgetData (filterCode);
			if (data != null) {
				return data.Select (x => x.Value).ToArray ();
			}
			return null;
		}

		#endregion

		#region Static Members

		/// <summary>
		/// function which downloads all the values for a given collection of database fields; puts these into a cache.
		/// </summary>
		public static FilterSelectableValuesCache GetValuesCache (Dataelus.EDD.EDDFieldCollection fields, IDataServices querier, IEqualityComparer<object> valueComparer)
		{
			return GetValuesCache (fields, querier, null, valueComparer);
		}

		/// <summary>
		/// function which downloads all the values for a given collection of database fields; puts these into a cache.
		/// </summary>
		/// <returns>The values cache.</returns>
		/// <param name="fields">The EDD Fields.</param>
		/// <param name="querier">The Database Querier.</param>
		/// <param name="otherValues">Any other values for EDD Field columns (custom enumerated values).</param>
		public static FilterSelectableValuesCache GetValuesCache (
			Dataelus.EDD.EDDFieldCollection fields
			, IDataServices querier
			, Dictionary<string, IEnumerable<string>> otherValues
			, IEqualityComparer<object> valueComparer)
		{
			var filters = new FilterSelectableValuesCache (new StringEqualityComparer ());

			var existing = new List<string> ();

			var fieldsWithRef = fields.Where (x => x.DBReferenceField != null).ToList ();

			// Pre-query all the fields at once
			var dictValues = querier.GetUniqueValueObjects (fieldsWithRef.Select (x => (Dataelus.Database.IDBFieldSimple)x.DBReferenceField).ToList (), EqualityComparer<object>.Default);

			foreach (var eddField in fieldsWithRef) {
				string fieldName = eddField.DBDataField.FieldName;
				if (filters.FilterData.ContainsKey (fieldName)) {
					existing.Add (fieldName);
					continue;
				}

				Dictionary<object, string> values; // = querier.GetUniqueValues (eddField.DbReferenceField);
				if (dictValues.TryGetValue (eddField.DBReferenceField, out values)) {
					filters.FilterData.Add (fieldName, values);
				}
			}
			if (otherValues != null) {
				foreach (var otherVal in otherValues) {
					if (!filters.FilterData.ContainsKey (otherVal.Key)) {
						filters.FilterData.Add (otherVal.Key, GetValueDict (otherVal.Value, valueComparer));
					}
				}
			}

			if (existing.Count > 0) {
				throw new Exception (String.Format ("The following fields were found more than once: [{0}]", String.Join (", ", existing)));
			}

			return filters;
		}

		/// <summary>
		/// Gets the value dictionary
		/// </summary>
		/// <returns>The value dict.</returns>
		/// <param name="values">Values.</param>
		/// <param name = "valueComparer">The comparer between two values</param>
		public static Dictionary<object, string> GetValueDict (IEnumerable<string> values, IEqualityComparer<object> valueComparer)
		{
			var dict = new Dictionary<object, string> (valueComparer);
			foreach (var item in values) {
				if (dict.ContainsKey (item))
					continue;
				dict.Add (item, item);
			}
			return dict;
		}

		#endregion
	}
}

