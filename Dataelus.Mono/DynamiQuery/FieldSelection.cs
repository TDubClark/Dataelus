using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Mono.DynamiQuery
{
	/// <summary>
	/// Field selection.
	/// </summary>
	public class FieldSelection
	{
		public Database.DBField Field { get; set; }

		public bool Selected { get; set; }

		public bool Search { get; set; }

		public string DisplayName { get; set; }

		public FieldSelection (Dataelus.Database.DBField field)
		{
			this.Field = field;
		}

		public FieldSelection (Dataelus.Database.DBField field, bool selected, bool search)
		{
			this.Field = field;
			this.Selected = selected;
			this.Search = search;
		}
	}
	
	/// <summary>
	/// Field selection view model.
	/// </summary>
	public class FieldSelectionViewModel : ListBase<FieldSelection>
	{
		public FieldSelectionViewModel ()
		{
		}

		public FieldSelectionViewModel (IEnumerable<FieldSelection> collection) : base (collection)
		{
		}

		public void Load (IEnumerable<Database.DBField> fields, IEnumerable<string> selected, string searchField, Dictionary<string, string> displayFields)
		{
			var comparer = new StringEqualityComparer ();
			foreach (var field in fields) {
				var obj = new FieldSelection (field, selected.Contains (field.FieldName, comparer), comparer.Equals (searchField, field.FieldName));
				string display;
				if (displayFields.TryGetValue (field.FieldName, out display)) {
					obj.DisplayName = display;
				}
				Add (obj);
			}
		}

		/// <summary>
		/// Gets the selected field names.
		/// </summary>
		/// <returns>The selected field names.</returns>
		public List<string> GetSelectedFieldNames ()
		{
			return _items
				.Where (x => x.Selected)
				.Select (x => x.Field.FieldName)
				.ToList ();
		}

		/// <summary>
		/// Gets the display field names.
		/// </summary>
		/// <returns>The display field names.</returns>
		public List<FieldSelection> GetDisplayFieldNames ()
		{
			return _items
				.Where (x => !String.IsNullOrWhiteSpace (x.DisplayName))
				.ToList ();
		}

		/// <summary>
		/// Gets the name of the search field.
		/// </summary>
		/// <returns>The search field name.</returns>
		public string GetSearchFieldName ()
		{
			foreach (var item in _items) {
				if (item.Search)
					return item.Field.FieldName;
			}
			return null;
		}
	}
}
