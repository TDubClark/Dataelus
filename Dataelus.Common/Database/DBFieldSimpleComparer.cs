using System;
using System.Collections.Generic;

namespace Dataelus.Database
{
	/// <summary>
	/// Database Field (Simple) comparer.
	/// </summary>
	public class DBFieldSimpleComparer : IDBFieldSimpleComparer, IEqualityComparer<DBFieldSimple>
	{
		public DBFieldSimpleComparer (IEqualityComparer<string> comparer)
			: base (comparer)
		{
		}

		public DBFieldSimpleComparer ()
			: base()
		{
		}

		#region IEqualityComparer implementation

		public bool Equals (DBFieldSimple x, DBFieldSimple y)
		{
			return base.Equals (x, y);
		}

		public int GetHashCode (DBFieldSimple obj)
		{
			return base.GetHashCode (obj);
		}

		#endregion
	}
}

