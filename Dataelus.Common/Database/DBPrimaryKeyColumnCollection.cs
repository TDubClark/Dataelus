using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Database
{
	/// <summary>
	/// A collection of Database Primary Key Columns.
	/// </summary>
	public class DBPrimaryKeyColumnCollection : ListBase<DBPrimaryKeyColumn>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBPrimaryKeyColumnCollection"/> class.
		/// </summary>
		public DBPrimaryKeyColumnCollection ()
			: base ()
		{
		}

		/// <summary>
		/// Sorts by priority.
		/// </summary>
		public void SortByPriority ()
		{
			throw new NotImplementedException ();
		}

		public List<string> GetFieldNames ()
		{
			return _items.Select (x => x.FieldName).ToList ();
		}
	}
}

