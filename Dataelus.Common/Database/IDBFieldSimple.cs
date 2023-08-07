using System;

namespace Dataelus.Database
{
	/// <summary>
	/// Interface for a simple Database field (just Table name and Field name).
	/// </summary>
	public interface IDBFieldSimple : IEquatable<IDBFieldSimple, string>
	{
		/// <summary>
		/// Gets or sets the name of the schema.
		/// </summary>
		/// <value>The name of the schema.</value>
		string SchemaName{ get; set; }

		/// <summary>
		/// Gets or sets the name of the table.
		/// </summary>
		/// <value>The name of the table.</value>
		string TableName{ get; set; }

		/// <summary>
		/// Gets or sets the name of the field.
		/// </summary>
		/// <value>The name of the field.</value>
		string FieldName{ get; set; }

		bool EqualsOrException (IDBFieldSimple other, System.Collections.Generic.IEqualityComparer<string> comparer);
	}
}

