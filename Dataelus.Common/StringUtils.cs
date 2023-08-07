using System;

namespace Dataelus
{
	public class StringUtils
	{
		public static bool Equals(string value1, string value2){
			return (new StringEqualityComparer()).Equals(value1, value2);
		}
		public static bool Equals(string value1, string value2, IStringComparisonMethod comparisonMethod){
			return (new StringEqualityComparer(comparisonMethod)).Equals(value1, value2);
		}

		public StringUtils ()
		{
		}
	}
}

