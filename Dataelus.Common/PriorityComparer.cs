using System;

namespace Dataelus
{
	/// <summary>
	/// Comparer for items which implement IPrioritized.
	/// </summary>
	public class PriorityComparer : System.Collections.Generic.IComparer<IPrioritized>
	{
		#region IComparer implementation

		public int Compare (IPrioritized x, IPrioritized y)
		{
			return System.Collections.Generic.Comparer<int>.Default.Compare (x.PriorityIndex, y.PriorityIndex);
		}

		#endregion
	}
}

