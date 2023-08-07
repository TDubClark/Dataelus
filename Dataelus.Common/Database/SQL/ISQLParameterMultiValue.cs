using System;

namespace Dataelus.Database.SQL
{
	/// <summary>
	/// Interface for a SQL parameter, with multiple values.
	/// </summary>
	public interface ISQLParameterMultiValue
	{
		/// <summary>
		/// Gets or sets the database field.
		/// </summary>
		/// <value>The field.</value>
		IDBField Field{ get; set; }

		/// <summary>
		/// Gets or sets the values.
		/// </summary>
		/// <value>The values.</value>
		object[] Values{ get; set; }
	}
}

