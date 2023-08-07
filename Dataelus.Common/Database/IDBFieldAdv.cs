using System;

namespace Dataelus.Database
{
	/// <summary>
	/// Interface for an Advanced definition of a database field.
	/// </summary>
	public interface IDBFieldAdv
	{
		/// <summary>
		/// Gets or sets whether this field is populated by an auto number.
		/// </summary>
		/// <value><c>true</c> if auto number; otherwise, <c>false</c>.</value>
		bool AutoNumber { get; set; }
	}
}

