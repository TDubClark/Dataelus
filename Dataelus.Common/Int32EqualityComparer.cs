using System;

namespace Dataelus
{
	/// <summary>
	/// Integer comparer.
	/// </summary>
	public class Int32EqualityComparer : System.Collections.Generic.IEqualityComparer<int>, System.Collections.IEqualityComparer
	{
		public bool IsReverse {
			get;
			set;
		}

		public Int32EqualityComparer ()
			: this (false)
		{
		}

		public Int32EqualityComparer (bool reverse)
		{
			this.IsReverse = reverse;
		}

		#region System.Collections.IEqualityComparer implementation

		public bool Equals (object x, object y)
		{
			return Equals ((int)x, (int)y);
		}

		public int GetHashCode (object obj)
		{
			return GetHashCode ((int)obj);
		}

		#endregion

		#region IEqualityComparer implementation

		public bool Equals (int x, int y)
		{
			return x.Equals (y);
		}

		public int GetHashCode (int obj)
		{
			return obj.GetHashCode ();
		}

		#endregion
	}
}

