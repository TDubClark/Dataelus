using System;

namespace Dataelus.Database
{
	/// <summary>
	/// Represents a primary key column in a Database.
	/// </summary>
	public class DBPrimaryKeyColumn : DBFieldSimple, IPrioritized
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBPrimaryKeyColumn"/> class.
		/// </summary>
		public DBPrimaryKeyColumn ()
			: base ()
		{
			_priorityIndex = -1;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBPrimaryKeyColumn"/> class.  Base Copy Constructor
		/// </summary>
		/// <param name="other">Another object which implements IDBFieldSimple.</param>
		public DBPrimaryKeyColumn (IDBFieldSimple other)
			: base (other)
		{
			_priorityIndex = -1;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBPrimaryKeyColumn"/> class.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldName">Field name.</param>
		public DBPrimaryKeyColumn (string tableName, string fieldName)
			: this (tableName, fieldName, -1)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBPrimaryKeyColumn"/> class.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldName">Field name.</param>
		/// <param name="priority">Priority.</param>
		public DBPrimaryKeyColumn (string tableName, string fieldName, int priority)
			: base (tableName, fieldName)
		{
			_priorityIndex = priority;
		}

		#region IPrioritized implementation

		/// <summary>
		/// The index of the priority.
		/// </summary>
		protected int _priorityIndex;

		/// <summary>
		/// Gets or sets the index of the priority.
		/// </summary>
		/// <value>The index of the priority.</value>
		public int PriorityIndex {
			get { return _priorityIndex; }
			set { _priorityIndex = value; }
		}

		#endregion
	}
}

