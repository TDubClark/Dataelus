using System;
using System.Linq;
using System.Collections.Generic;

namespace Dataelus
{
	/// <summary>
	/// A String collection, which may be filtered on a set of criteria (including uniqueness)
	/// </summary>
	public class StringCollection : ListBase<string>, System.Collections.Generic.ICollection<string>, ICollectionSearchable<string>
	{
		/// <summary>
		/// The filter criteria.
		/// </summary>
		protected IStringFilterCriteria _criteria;

		/// <summary>
		/// Gets or sets the filter criteria.
		/// </summary>
		/// <value>The filter criteria.</value>
		public IStringFilterCriteria FilterCriteria {
			get { return _criteria; }
			set { _criteria = value; }
		}

		/// <summary>
		/// Gets the items (guaranteed to be non-null).
		/// </summary>
		/// <value>The items non null.</value>
		public System.Collections.Generic.List<string> ItemsNonNull {
			get {
				if (_items == null)
					return new System.Collections.Generic.List<string> ();
				return _items;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.StringCollection"/> class.
		/// Default: Unique, non-null; Ignore case, trim, null=Empty
		/// </summary>
		public StringCollection ()
			: this (new StringFilterCriteria ())
		{
		}

		public StringCollection (IStringFilterCriteria filterCriteria)
			: base ()
		{
			_criteria = filterCriteria;
		}

		#region Constructors: add one or more initial items

		public StringCollection (params string[] items)
			: this (items.ToList ())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.StringCollection"/> class.
		/// </summary>
		/// <param name="items">Items.</param>
		/// <param name="sort">Whether to sort the instance.</param>
		public StringCollection (System.Collections.Generic.IEnumerable<string> items, bool sort)
			: this (items)
		{
			if (sort)
				Sort ();
		}

		public StringCollection (System.Collections.Generic.IEnumerable<string> items)
			: this ()
		{
			Add (items);
		}

		public StringCollection (System.Collections.Generic.IEnumerable<string> items, IStringFilterCriteria filterCriteria)
			: this (filterCriteria)
		{
			Add (items);
		}

		#endregion

		public override void Add (string item)
		{
			if (_criteria.MeetsCriteriaForCollection (item, _items))
				base.Add (item);
		}

		public void Add (System.Collections.Generic.IEnumerable<string> items)
		{
			foreach (var item in items) {
				Add (item);
			}
		}

		public void AddNoFilter (string item)
		{
			_items.Add (item);
		}

		public void AddNoFilter (System.Collections.Generic.IEnumerable<string> items)
		{
			_items.AddRange (items);
		}

		public int RemoveAll (System.Collections.Generic.IEnumerable<string> items)
		{
			int count = 0;
			foreach (var item in items) {
				count += _items.RemoveAll (x => _criteria.EqualityComparer.Equals (x, item));
			}
			return count;
		}

		/// <summary>
		/// Sorts the items using the default String comparer.
		/// </summary>
		public void Sort ()
		{
			Sort (Comparer<string>.Default);
		}

		/// <summary>
		/// Sorts the items using the specified comparer.
		/// </summary>
		/// <param name="comparer">Comparer.</param>
		public void Sort (IComparer<string> sorter)
		{
			_items.Sort (sorter);
		}

		public new bool Contains (string value)
		{
			return _items.Contains (value, _criteria.EqualityComparer);
		}

		/// <summary>
		/// Gets whether any item matches the given pattern.
		/// </summary>
		/// <returns><c>true</c>, if pattern was matched, <c>false</c> otherwise.</returns>
		/// <param name="pattern">Pattern.</param>
		public bool MatchPattern (string pattern, System.Text.RegularExpressions.RegexOptions options)
		{
			return _items.Any (x => System.Text.RegularExpressions.Regex.IsMatch (x, pattern, options));
		}
	}
}

