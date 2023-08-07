using System;
using System.Collections.Generic;

namespace Dataelus.Database
{
	/// <summary>
	/// Represents a Database primary key, which may contain one or more columns.
	/// </summary>
	public class DBPrimaryKey
	{
		/// <summary>
		/// Gets or sets the name of the table.
		/// </summary>
		/// <value>The name of the table.</value>
		public string TableName { get; set; }

		/// <summary>
		/// The columns.
		/// </summary>
		protected DBPrimaryKeyColumnCollection _columns;

		/// <summary>
		/// Gets or sets the columns of this primary key.
		/// </summary>
		/// <value>The columns.</value>
		public DBPrimaryKeyColumnCollection Columns {
			get { return _columns; }
			set { _columns = value; }
		}

		/// <summary>
		/// Gets the field names.
		/// </summary>
		/// <returns>The field names.</returns>
		public List<string> GetFieldNames ()
		{
			return _columns.GetFieldNames ();
		}

		/// <summary>
		/// Adds the field as the next primary key column.
		/// </summary>
		/// <param name="fieldName">Field name.</param>
		public void AddField (string fieldName)
		{
			_columns.Add (new DBPrimaryKeyColumn (this.TableName, fieldName, _columns.Count));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBPrimaryKey"/> class.
		/// </summary>
		public DBPrimaryKey ()
		{
			_columns = new DBPrimaryKeyColumnCollection ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBPrimaryKey"/> class.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		public DBPrimaryKey (string tableName)
			: this ()
		{
			this.TableName = tableName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBPrimaryKey"/> class.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldNames">Field names.</param>
		public DBPrimaryKey (string tableName, params string[] fieldNames)
			: this (tableName, new List<string> (fieldNames))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBPrimaryKey"/> class.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldNames">Field names.</param>
		public DBPrimaryKey (string tableName, IEnumerable<string> fieldNames)
			: this (tableName)
		{
			int i = 0;
			foreach (var fieldName in fieldNames) {
				_columns.Add (new DBPrimaryKeyColumn (tableName, fieldName, i));
				i++;
			}
		}
	}
}

