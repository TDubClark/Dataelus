using System;

namespace Dataelus
{
	/// <summary>
	/// Int comparer, which reverses the order.
	/// </summary>
	public class IntComparerReverse : System.Collections.Generic.IComparer<int>
	{

		#region IComparer implementation

		public int Compare (int x, int y)
		{
			return y.CompareTo (x);
		}

		#endregion
	}
}
