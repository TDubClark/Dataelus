using System;
using System.Text.RegularExpressions;

namespace Dataelus.Database.SQL
{
	/// <summary>
	/// String equality comparer for SQL Fields (which may have brakets).
	/// </summary>
	public class SQLFieldStringEqualityComparer : StringEqualityComparer, System.Collections.Generic.IEqualityComparer<string>
	{
		public SQLFieldStringEqualityComparer ()
			: base ()
		{
		}

		public SQLFieldStringEqualityComparer (IStringComparisonMethod comparisonMethod)
			: base (comparisonMethod)
		{
		}

		public override bool Equals (string x, string y)
		{
			return base.Equals (DBField.RemoveBrackets (x), DBField.RemoveBrackets (y));
		}
	}
}

