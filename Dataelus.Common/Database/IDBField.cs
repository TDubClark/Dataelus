using System;

namespace Dataelus.Database
{
	/// <summary>
	/// Interface for a database field.
	/// </summary>
	public interface IDBField : IDBFieldSimple, IEquatable<IDBField, string>
	{
		/// <summary>
		/// Gets or sets the type of the data.
		/// </summary>
		/// <value>The type of the data.</value>
		string DataType{ get; set; }

		/// <summary>
		/// Gets or sets the maximum character length of a string.
		/// </summary>
		/// <value>The length of the max.</value>
		int MaxLength{ get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Dataelus.Database.IDbField"/> is nullable.
		/// </summary>
		/// <value><c>true</c> if nullable; otherwise, <c>false</c>.</value>
		bool Nullable{ get; set; }
	}
}

