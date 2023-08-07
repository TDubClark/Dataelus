using System;

namespace Dataelus.EDD
{
	using Dataelus.Database;

	/// <summary>
	/// Interface for an EDD field.
	/// </summary>
	public interface IEDDField
	{
		/// <summary>
		/// Gets or sets the name of the edd column.
		/// </summary>
		/// <value>The name of the edd column.</value>
		string EDDColumnName{ get; set; }

		/// <summary>
		/// Gets or sets the database data field.
		/// </summary>
		/// <value>The db data field.</value>
		IDBField DBDataField{ get; set; }

		/// <summary>
		/// Gets or sets the database upload field.
		/// </summary>
		/// <value>The db upload field.</value>
		IDBField DBUploadField{ get; set; }

		/// <summary>
		/// Gets or sets the database reference field.
		/// </summary>
		/// <value>The db reference field.</value>
		IDBField DBReferenceField{ get; set; }
	}
}

