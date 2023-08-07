using System;
using System.Collections.Generic;

namespace Dataelus
{
	/// <summary>
	/// Interface for filter criteria on a string collection.
	/// </summary>
	public interface IStringFilterCriteria
	{
		/// <summary>
		/// Gets or sets whether the filter criteria only allows addition of unique values
		/// </summary>
		/// <value><c>true</c> if unique; otherwise, <c>false</c>.</value>
		bool Unique{ get; set; }

		/// <summary>
		/// Gets or sets the equality comparer for the collection (also used for determining uniqueness).
		/// </summary>
		/// <value>The equality comparer.</value>
		IEqualityComparer<string> EqualityComparer{ get; set; }

		/// <summary>
		/// Gets or sets the filterer, which can filter out any values (such as NULL or empty strings)
		/// </summary>
		/// <value>The filterer.</value>
		IStringFilterer Filterer{ get; set; }

		/// <summary>
		/// Whether the given value meets all criteria for being added to the given collection
		/// </summary>
		/// <returns><c>true</c>, if criteria for collection was meetsed, <c>false</c> otherwise.</returns>
		/// <param name="value">Value.</param>
		/// <param name="collection">Collection.</param>
		bool MeetsCriteriaForCollection (string value, IEnumerable<string> collection);
	}

	/// <summary>
	/// String filter criteria.
	/// </summary>
	public class StringFilterCriteria : IStringFilterCriteria
	{
		protected bool _unique;

		public bool Unique {
			get { return _unique; }
			set { _unique = value; }
		}

		protected IEqualityComparer<string> _equalityComparer;

		public IEqualityComparer<string> EqualityComparer {
			get { return _equalityComparer; }
			set { _equalityComparer = value; }
		}

		protected IStringFilterer _filterer;

		public IStringFilterer Filterer {
			get {
				return _filterer; }
			set { _filterer = value;
			}
		}

		public StringFilterCriteria ()
			: this (true, new StringEqualityComparer ())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.StringFilterCriteria"/> class (by default, does not allow NULL).
		/// </summary>
		/// <param name="unique">If set to <c>true</c> unique.</param>
		/// <param name="comparer">Comparer.</param>
		public StringFilterCriteria (bool unique, IEqualityComparer<string> comparer)
			: this (unique, comparer, false, false)
		{
		}

		public StringFilterCriteria (bool unique, IEqualityComparer<string> comparer, bool allowAll, bool allowNull)
			: this (unique, comparer, new StringFilterer (allowAll, allowNull, comparer))
		{
		}

		public StringFilterCriteria (bool unique, IEqualityComparer<string> comparer, IStringFilterer filterer)
		{
			_unique = unique;
			_equalityComparer = comparer;
			_filterer = filterer;
		}

		public bool MeetsCriteriaForCollection (string value, IEnumerable<string> collection)
		{
			if (_filterer.MeetsFilterCriteria (value)) {
				// Meets filter criteria
				if (_unique && IsContains (collection, value)) {
					// Uniqueness is required, and the collection already contains the value
					// Therefore, [value] Does not meet uniqueness criteria
					return false;
				}
				return true;
			}
			return false;
		}

		private bool IsContains (IEnumerable<string> collection, string value)
		{
			return ((new List<string> (collection)).FindIndex (x => _equalityComparer.Equals (x, value)) >= 0);
		}
	}
}

