using System;
using System.Collections.Generic;

namespace Dataelus.Database
{
	/// <summary>
	/// Db field comparer; sorts first by TableName, then by PriorityIndex.
	/// </summary>
	public class DBFieldComparer : IComparer<DBField>
	{
		protected IComparer<IPrioritized> _prioritizer;

		public DBFieldComparer ()
		{
			_prioritizer = new Dataelus.PriorityComparer ();
		}

		#region IComparer implementation

		public int Compare (DBField x, DBField y)
		{
			var ret = x.TableName.CompareTo (y.TableName);
			if (ret == 0)
				return _prioritizer.Compare (x, y);
			return ret;
		}

		#endregion
	}
}

