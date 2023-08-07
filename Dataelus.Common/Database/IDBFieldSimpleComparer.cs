using System;
using System.Collections.Generic;

namespace Dataelus.Database
{
	public class IDBFieldSimpleComparer : IEqualityComparer<IDBFieldSimple>
	{
		protected IEqualityComparer<string> _fieldComparer;

		/// <summary>
		/// Gets or sets the comparer for database field names (table names, column names, schema names).
		/// </summary>
		/// <value>The field comparer.</value>
		public IEqualityComparer<string> FieldComparer {
			get { return _fieldComparer; }
			set { _fieldComparer = value; }
		}

		public IDBFieldSimpleComparer (IEqualityComparer<string> comparer)
		{
			_fieldComparer = comparer;
		}

		public IDBFieldSimpleComparer ()
			: this (new Dataelus.Database.SQL.SQLFieldStringEqualityComparer ())
		{
		}

		public bool EqualsOrException (IDBFieldSimple x, IDBFieldSimple y)
		{
			return x.EqualsOrException (y, _fieldComparer);
		}

		#region IEqualityComparer implementation

		public bool Equals (IDBFieldSimple x, IDBFieldSimple y)
		{
			return x.Equals (y, _fieldComparer);
		}

		public int GetHashCode (IDBFieldSimple obj)
		{
			return obj.TableName.GetHashCode () + obj.FieldName.GetHashCode ();
		}

		#endregion
	}
}

