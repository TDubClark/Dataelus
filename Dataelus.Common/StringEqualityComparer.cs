using System;
using System.Linq;

namespace Dataelus
{
	/// <summary>
	/// String equality comparer.
	/// </summary>
	public class StringEqualityComparer : System.Collections.Generic.IEqualityComparer<string>
	{
		protected IStringComparisonMethod _comparisonMethod;

		public IStringComparisonMethod ComparisonMethod {
			get { return _comparisonMethod; }
			set { _comparisonMethod = value; }
		}

		public StringEqualityComparer ()
			: this (new StringComparisonMethod ())
		{
		}

		public StringEqualityComparer (IStringComparisonMethod comparisonMethod)
		{
			_comparisonMethod = comparisonMethod;
		}

		public bool EqualsAny (string item, params string[] otherItems)
		{
			return EqualsAny (item, otherItems.ToList ());
		}

		public bool EqualsAny (string item, System.Collections.Generic.IEnumerable<string> other)
		{
			return other.Any (x => this.Equals (item, x));
		}

		#region IEqualityComparer implementation

		public virtual bool Equals (string x, string y)
		{
			x = _comparisonMethod.GetComparableString (x);
			y = _comparisonMethod.GetComparableString (y);

			// If both NULL, then equal
			if (x == null && y == null)
				return true;

			// If either is null (not both), then not equal
			if (x == null || y == null)
				return false;

			return x.Equals (y);
		}

		public virtual int GetHashCode (string obj)
		{
			return obj.GetHashCode ();
		}

		#endregion
	}
}

