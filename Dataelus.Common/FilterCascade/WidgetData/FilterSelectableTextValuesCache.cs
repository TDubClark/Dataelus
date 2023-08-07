using System;
using System.Collections.Generic;
using System.Linq;

using Dataelus.Extensions;

namespace Dataelus.FilterCascade.WidgetData
{
	/// <summary>
	/// Selectable values cache for a filter widget (such as a drop-down box).
	/// </summary>
	public class FilterSelectableTextValuesCache : IFilterSelectableTextValuesCache
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.FilterCascade.WidgetData.FilterSelectableTextValuesCache"/> class.
		/// </summary>
		public FilterSelectableTextValuesCache ()
			: this (new StringEqualityComparer ())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.FilterCascade.WidgetData.FilterSelectableValuesCache"/> class.
		/// </summary>
		/// <param name="comparer">String Comparer for filter codes.</param>
		public FilterSelectableTextValuesCache (IEqualityComparer<string> comparer)
		{
			this.FilterData = new Dictionary<string, Dictionary<string, string>> (comparer);
		}

		/// <summary>
		/// Adds the filter.
		/// </summary>
		/// <param name="filterCode">Filter code.</param>
		/// <param name="values">Values.</param>
		public void AddFilter (string filterCode, Dictionary<string, string> values)
		{
			this.FilterData.Add (filterCode, values);
		}

		#region IFilterSelectableValuesCache implementation

		public Dictionary<string, Dictionary<string, string>> FilterData{ get; set; }

		public void Load (FilterTextItemCollection filters, FilterTextTable table)
		{
			foreach (var filter in filters) {
				string fcode = filter.FilterCode;
				this.FilterData [fcode] = table.GetValueDisplay (filters, fcode);
			}
		}

		public void UpdateCache (string filterCode, FilterTextItemCollection filters, FilterTextTable table)
		{
			// Load, but only for the child filters of the given filter code
			Load (filters.GetChildFilters (filterCode), table);
		}

		public Dictionary<string, string> GetWidgetData (string filterCode)
		{
			Dictionary<string, string> data;
			if (this.FilterData.TryGetValue (filterCode, out data)) {
				return data;
			}
			return null;
		}

		public string[] GetWidgetDataValues (string filterCode)
		{
			Dictionary<string, string> data = GetWidgetData (filterCode);
			if (data != null) {
				return data.Select (x => x.Key).ToArray ();
			}
			return null;
		}

		public string[] GetWidgetDataDisplayText (string filterCode)
		{
			Dictionary<string, string> data = GetWidgetData (filterCode);
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
		public static FilterSelectableTextValuesCache GetValuesCache (Dataelus.EDD.EDDFieldCollection fields, IDataServices2 querier)
		{
			int countLimit = 2000;
			return GetValuesCache (fields, querier, countLimit);
		}

		public static FilterSelectableTextValuesCache GetValuesCache (Dataelus.EDD.EDDFieldCollection fields, IDataServices2 querier, int countLimit)
		{
			return GetValuesCache (fields, querier, countLimit, null, null);
		}

		/// <summary>
		/// function which downloads all the values for a given collection of database fields; puts these into a cache.
		/// </summary>
		/// <returns>The values cache.</returns>
		/// <param name="fields">The EDD Fields.</param>
		/// <param name="querier">The Database Querier.</param>
		/// <param name="valueCountLimit">The limit on the number of items to return per field.</param>
		/// <param name="otherValues">Any other values for EDD Field columns (custom enumerated values).</param>
		/// <param name="suggestionFields">Database fields which take their </param>
		public static FilterSelectableTextValuesCache GetValuesCache (
			Dataelus.EDD.EDDFieldCollection fields
			, IDataServices2 querier
			, int valueCountLimit
			, Dictionary<string, IEnumerable<string>> otherValues
			, IEnumerable<Database.IDBFieldSimple> suggestionFields)
		{
			var fieldsWithRef = fields.Where (x => x.DBReferenceField != null).ToList ();

			var refFields = fieldsWithRef.Select (x => (Dataelus.Database.IDBFieldSimple)x.DBReferenceField).ToList ();

			// Pre-query all the fields at once
			// Get the dictionary of Database Field => Unique Values
			DBFieldTextValueListCollection dictValues = querier.GetUniqueTextValues (refFields, valueCountLimit, new StringEqualityComparer ());

			// Create a new list of filters, which will be populated
			var filters = new FilterSelectableTextValuesCache (new StringEqualityComparer ());

			var existing = new List<string> ();

			// For each field which has a defined Reference field
			foreach (var eddField in fieldsWithRef) {
				string fieldName = eddField.DBDataField.FieldName;

				if (filters.FilterData.ContainsKey (fieldName)) {
					// The list of filters already contains this field
					existing.Add (fieldName);
					continue;
				}

				DBFieldTextValueList valuesList;
				Dictionary<string, string> values; // = querier.GetUniqueValues (eddField.DbReferenceField);
				if (dictValues.TryFind (eddField.DBReferenceField, out valuesList)) {
					// Found the unique values
					filters.FilterData.Add (fieldName, valuesList.FieldValues);
				} else {
					// Not found

					// This scope is just trouble-shooting

					// Perform a custom search of the keys, and throw an exception with the results of each comparison
					var comparer = new Dataelus.Database.IDBFieldSimpleComparer ();

					var sb = new System.Text.StringBuilder ();
					//sb.AppendLine()
					sb.AppendLineFormat ("Comparing Values to field '{0}' (tablename='{1}'; fieldname='{2}')", eddField.DBReferenceField
						, eddField.DBReferenceField.TableName
						, eddField.DBReferenceField.FieldName);
					
					foreach (var item in dictValues) {
						var fieldDef = item.FieldDef;
						string resultText = "";
						try {
							resultText = comparer.EqualsOrException (eddField.DBReferenceField, fieldDef) ? "Equal" : "Not Equal";
						} catch (Exception ex1) {
							resultText = ex1.Message;
						}
						sb.AppendLine (String.Format ("Compared to: [tablename='{0}'; fieldname='{1}']; result: {2}", fieldDef.TableName, fieldDef.FieldName, resultText));
					}

					throw new Exception (String.Format ("Could not find filters for field '{0}' (tablename='{1}'; fieldname='{2}'){3}Existing filters:{3}{4}{3}{5}"
						, eddField.DBReferenceField
						, eddField.DBReferenceField.TableName
						, eddField.DBReferenceField.FieldName
						, Environment.NewLine
						, "  >> " + String.Join (Environment.NewLine + "  >> ", dictValues.Select (x => x.FieldDef.ToString ()).ToArray ())
						, sb.ToString ()
					));
				}
			}

			// Populate the list of other values
			if (otherValues != null) {
				foreach (var otherVal in otherValues) {
					if (!filters.FilterData.ContainsKey (otherVal.Key)) {
						filters.FilterData.Add (otherVal.Key, GetValueDict (otherVal.Value));
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

