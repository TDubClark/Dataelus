using System;
using System.Collections.Generic;

namespace Dataelus.UI.MatchTableEditor
{

	public class IDTagComparer : System.Collections.IEqualityComparer
	{
		public bool Equals (object value1, object value2)
		{
			return ((long)value1 == (long)value2);
		}

		public int GetHashCode (object obj)
		{
			return ((long)obj).GetHashCode ();
		}
	}
	
}
