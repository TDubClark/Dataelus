using System;

namespace Dataelus.Database.SQL
{
	/// <summary>
	/// Interface for a SQL parameter, with a value.
	/// </summary>
	public interface ISQLParameterValue
	{
		/// <summary>
		/// Gets or sets the database field.
		/// </summary>
		/// <value>The field.</value>
		IDBField Field{ get; set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		object Value{ get; set; }
	}
}

