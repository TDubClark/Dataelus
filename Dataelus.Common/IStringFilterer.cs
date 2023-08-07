using System;

namespace Dataelus
{
	/// <summary>
	/// Inteface for an object which can filter out strings
	/// </summary>
	public interface IStringFilterer
	{
		/// <summary>
		/// Determines whether the given value meets the filter criteria.
		/// </summary>
		/// <returns><c>true</c> if this instance is meet filter criteria the specified value; otherwise, <c>false</c>.</returns>
		/// <param name="value">Value.</param>
		bool MeetsFilterCriteria (string value);
	}

	public class StringFilterer : IStringFilterer
	{
		protected bool _allowAll;

		public bool AllowAll {
			get { return _allowAll; }
			set { _allowAll = value; }
		}

		protected bool _allowNull;

		public bool AllowNull {
			get { return _allowNull; }
			set { _allowNull = value; }
		}

		protected System.Collections.Generic.IEqualityComparer<string> _equalityComparer;

		public System.Collections.Generic.IEqualityComparer<string> EqualityComparer {
			get {
				return _equalityComparer; }
			set { _equalityComparer = value;
			}
		}

		public StringFilterer ()
			: this (true)
		{

		}

		public StringFilterer (bool allowAll)
			: this (allowAll, false, new StringEqualityComparer ())
		{
		}

		public StringFilterer (bool allowAll, bool allowNull, System.Collections.Generic.IEqualityComparer<string> equalityComparer)
		{
			_allowAll = allowAll;
			_allowNull = allowNull;
			_equalityComparer = equalityComparer;
		}

		#region IStringFilterer implementation

		public virtual bool MeetsFilterCriteria (string value)
		{
			if (!_allowAll) {
				// Not all values are allowed
				if (!_allowNull) {
					// Not allowing NULL, so if value = NULL, then doesn't meet criteria
					if (_equalityComparer.Equals (null, value))
						return false;
				}
			}
			return true;
		}

		#endregion
	}
}

