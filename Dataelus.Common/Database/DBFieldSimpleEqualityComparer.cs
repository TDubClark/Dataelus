using System.Collections.Generic;

namespace Dataelus.Database
{
	/// <summary>
	/// DBFieldSimple equality comparer.
	/// </summary>
	public class DBFieldSimpleEqualityComparer : IEqualityComparer<DBFieldSimple>
	{
		/// <summary>
		/// The comparer.
		/// </summary>
		protected IEqualityComparer<string> _fieldComparer;

		/// <summary>
		/// Gets or sets the comparer.
		/// </summary>
		/// <value>The comparer.</value>
		public IEqualityComparer<string> FieldComparer {
			get { return _fieldComparer; }
			set { _fieldComparer = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBFieldSimpleEqualityComparer"/> class, with a default Comparer.
		/// </summary>
		public DBFieldSimpleEqualityComparer ()
			: this (new StringEqualityComparer ())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBFieldSimpleEqualityComparer"/> class.
		/// </summary>
		/// <param name="fieldComparer">The database field name comparer.</param>
		public DBFieldSimpleEqualityComparer (IEqualityComparer<string> fieldComparer)
		{
			_fieldComparer = fieldComparer;
		}

		#region IEqualityComparer implementation

		/// <summary>
		/// Gets whether the given values are equal.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		public bool Equals (DBFieldSimple x, DBFieldSimple y)
		{
			return x.Equals (y, _fieldComparer);
		}

		/// <Docs>The object for which the hash code is to be returned.</Docs>
		/// <para>Returns a hash code for the specified object.</para>
		/// <returns>A hash code for the specified object.</returns>
		/// <summary>
		/// Gets the hash code.
		/// </summary>
		/// <param name="obj">Object.</param>
		public int GetHashCode (DBFieldSimple obj)
		{
			return obj.SchemaName.GetHashCode () + obj.TableName.GetHashCode () + obj.FieldName.GetHashCode ();
		}

		#endregion
	}
}

