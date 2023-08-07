using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Database
{
	/// <summary>
	/// Stores a collection of database field associations.
	/// </summary>
	public class DBFieldAssociationCollection : ListBase<DBFieldAssociation>
	{
		public DBFieldAssociationCollection ()
			: base ()
		{
		}

		/// <summary>
		/// Gets the associated fields for the given field.
		/// </summary>
		/// <returns>The associated fields.</returns>
		/// <param name="field">Field.</param>
		public List<DBFieldAssociation> GetAssociatedFields (IDBFieldSimple field)
		{
			return _items.Where (x => x.Field.Equals (field)).ToList ();
		}
	}
}

