using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.FilterCascade.WidgetData
{
	/// <summary>
	/// Text values cache.
	/// </summary>
	public class TextValuesCache : ITextValuesCache
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.FilterCascade.WidgetData.SelectableTextValuesCache"/> class.
		/// </summary>
		public TextValuesCache ()
			: this (new StringEqualityComparer ())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.FilterCascade.WidgetData.SelectableTextValuesCache"/> class.
		/// </summary>
		/// <param name="filterCodeComparer">Filter code comparer.</param>
		public TextValuesCache (IEqualityComparer<string> filterCodeComparer)
		{
			this.SuggestionData = new Dictionary<string, Dictionary<string, string>> (filterCodeComparer);
		}

		#region ISelectableTextValuesCache implementation

		public Dictionary<string, string> GetWidgetData (string filterCode)
		{
			Dictionary<string, string> data;
			if (this.SuggestionData.TryGetValue (filterCode, out data)) { return data; }
			return null;
		}

		/// <summary>
		/// Gets the widget data values.
		/// </summary>
		/// <returns>The widget data values.</returns>
		/// <param name="filterCode">Filter code.</param>
		public string[] GetWidgetDataValues (string filterCode)
		{
			Dictionary<string, string> data = GetWidgetData (filterCode);
			if (data != null) {
				return data.Select (x => x.Key).ToArray ();
			}
			return null;
		}

		/// <summary>
		/// Gets the widget data display text.
		/// </summary>
		/// <returns>The widget data display text.</returns>
		/// <param name="filterCode">Filter code.</param>
		public string[] GetWidgetDataDisplayText (string filterCode)
		{
			Dictionary<string, string> data = GetWidgetData (filterCode);
			if (data != null) {
				return data.Select (x => x.Value).ToArray ();
			}
			return null;
		}

		/// <summary>
		/// Gets or sets the suggestion data.
		/// It is a dictionary of filter codes/suggestion data; the suggestion data is itself a dictionary of value/display.
		/// </summary>
		/// <value>The filter data.</value>
		public Dictionary<string, Dictionary<string, string>> SuggestionData {
			get;
			set;
		}

		#endregion

		#region Static Members

		/// <summary>
		/// function which downloads all the values for a given collection of database fields; puts these into a cache.
		/// </summary>
		public static TextValuesCache GetValuesCache (IEnumerable<Dataelus.EDD.EDDField> fields, IDataServices querier)
		{
			return GetValuesCache (fields, querier, null);
		}

		/// <summary>
		/// function which downloads all the values for a given collection of database fields; puts these into a cache.
		/// </summary>
		/// <returns>The values cache.</returns>
		/// <param name="fields">The EDD Fields.</param>
		/// <param name="querier">The Database Querier.</param>
		/// <param name="otherValues">Any other values for EDD Field columns (custom enumerated values).</param>
		public static TextValuesCache GetValuesCache (
			IEnumerable<Dataelus.EDD.EDDField> fields
			, IDataServices querier
			, Dictionary<string, IEnumerable<string>> otherValues)
		{
			var filters = new TextValuesCache (new StringEqualityComparer ());

			var existing = new List<string> ();

			var fieldsWithSrc = fields.Where (x => x.DBDataField != null).ToList ();

			// Pre-query all the fields at once
			var dictValues = querier.GetUniqueValues (fieldsWithSrc.Select (x => (Dataelus.Database.IDBFieldSimple)x.DBDataField).ToList (), new StringEqualityComparer ());

			foreach (var eddField in fieldsWithSrc) {
				string fieldName = eddField.DBDataField.FieldName;
				if (filters.SuggestionData.ContainsKey (fieldName)) {
					existing.Add (fieldName);
					continue;
				}

				Dictionary<string, string> values; // = querier.GetUniqueValues (eddField.DbReferenceField);
				if (dictValues.TryGetValue (eddField.DBReferenceField, out values)) {
					filters.SuggestionData.Add (fieldName, values);
				}
			}
			if (otherValues != null) {
				foreach (var otherVal in otherValues) {
					if (!filters.SuggestionData.ContainsKey (otherVal.Key)) {
						filters.SuggestionData.Add (otherVal.Key, GetValueDict (otherVal.Value));
					}
				}
			}

			if (existing.Count > 0) {
				throw new Exception (String.Format ("The following fields were found more than once: [{0}]", String.Join (", ", existing)));
			}

			return filters;
		}

		/// <summary>
		/// function which downloads all the values for a given collection of database fields; puts these into a cache.
		/// </summary>
		/// <returns>The values cache.</returns>
		/// <param name="fields">The Database Fields.</param>
		/// <param name="querier">The Database Querier.</param>
		/// <param name="otherValues">Any other values for EDD Field columns (custom enumerated values).</param>
		public static TextValuesCache GetValuesCache (
			IEnumerable<Dataelus.Database.IDBFieldSimple> fields
			, IDataServices querier
			, Dictionary<string, IEnumerable<string>> otherValues)
		{
			var filters = new TextValuesCache (new StringEqualityComparer ());

			var existing = new List<string> ();

			// Pre-query all the fields at once
			var dictValues = querier.GetUniqueValues (fields, new StringEqualityComparer ());

			foreach (var eddField in fields) {
				string fieldName = eddField.FieldName;
				if (filters.SuggestionData.ContainsKey (fieldName)) {
					existing.Add (fieldName);
					continue;
				}

				Dictionary<string, string> values; // = querier.GetUniqueValues (eddField.DbReferenceField);
				if (dictValues.TryGetValue (eddField, out values)) {
					filters.SuggestionData.Add (fieldName, values);
				}
			}
			if (otherValues != null) {
				foreach (var otherVal in otherValues) {
					if (!filters.SuggestionData.ContainsKey (otherVal.Key)) {
						filters.SuggestionData.Add (otherVal.Key, GetValueDict (otherVal.Value));
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
		public static Dictionary<string,string> GetValueDict (IEnumerable<string> values)
		{
			var dict = new Dictionary<string, string> (new Dataelus.StringEqualityComparer ());
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

