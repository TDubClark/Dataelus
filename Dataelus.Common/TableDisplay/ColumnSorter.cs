using System;

namespace Dataelus.TableDisplay
{
	/// <summary>
	/// Column sorter.
	/// </summary>
	public class ColumnSorter<T> : System.Collections.Generic.IComparer<T> where T : IColumnDef
	{
		public ColumnSorter ()
		{
		}

		public virtual int Compare (IColumnDef x, IColumnDef y)
		{
			return x.columnOrderIndex.CompareTo (y.columnOrderIndex);
		}

		#region IComparer implementation

		public virtual int Compare (T x, T y)
		{
			return Compare ((IColumnDef)x, (IColumnDef)y);
		}

		#endregion
	}
}

