using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Mono.DynamiQuery
{
	/// <summary>
	/// Stores a Database table name and description.
	/// </summary>
	public class DBTable
	{
		public string TableName { get; set; }

		public string TableDescription { get; set; }

		public DBTable ()
		{

		}

		public DBTable (string tableName, string tableDescription)
		{
			this.TableName = tableName;
			this.TableDescription = tableDescription;
		}
	}

	/// <summary>
	/// DB table collection.
	/// </summary>
	public class DBTableCollection : ListBase<DBTable>
	{
		public DBTableCollection ()
		{
		}

		public DBTableCollection (IEnumerable<string> tableNames)
		{
			AddTableNames (tableNames);
		}

		public void AddTableNames (IEnumerable<string> tableNames)
		{
			foreach (var name in tableNames) {
				Add (new DBTable (name, null));
			}
		}
	}

	public class DBTableComparer : IEqualityComparer<DBTable>
	{
		IEqualityComparer<string> _comparer;

		public DBTableComparer ()
		{
			_comparer = new StringEqualityComparer ();
		}

		#region IEqualityComparer implementation

		public bool Equals (DBTable x, DBTable y)
		{
			return _comparer.Equals (x.TableName, y.TableName);
		}

		public int GetHashCode (DBTable obj)
		{
			return obj.TableName.GetHashCode ();
		}

		#endregion
		
	}
}
