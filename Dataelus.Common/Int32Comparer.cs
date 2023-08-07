using System;

namespace Dataelus
{
	/// <summary>
	/// Int32 comparer, which allows reversing the order.
	/// </summary>
	public class Int32Comparer : System.Collections.Generic.IComparer<int>
	{
		public bool IsReverse {
			get;
			set;
		}

		public Int32Comparer ()
			: this (false)
		{
		}

		public Int32Comparer (bool reverse)
		{
			this.IsReverse = reverse;
		}

		#region IComparer implementation

		public int Compare (int x, int y)
		{
			if (this.IsReverse) {
				return y.CompareTo (x);
			}
			return x.CompareTo (y);
		}

		#endregion
	}
}

