using System;
using System.Collections.Generic;

namespace Dataelus
{
	class Int64EqualityComparer : System.Collections.IEqualityComparer, IEqualityComparer<long>
	{
		public Int64EqualityComparer ()
		{
			
		}

		public bool Equals (object value1, object value2)
		{
			return Equals ((long)value1, (long)value2);
		}

		public int GetHashCode (object obj)
		{
			return GetHashCode ((long)obj);
		}

		public bool Equals (long value1, long value2)
		{
			return value1.Equals (value2);
		}

		public int GetHashCode (long obj)
		{
			return obj.GetHashCode ();
		}
	}
}

