using System;

namespace Dataelus.Database.SQL
{
	/// <summary>
	/// Interface for sql command value parameters.
	/// </summary>
	public interface ISQLCommandValueParameters
	{
		/// <summary>
		/// Gets or sets the value parameters.
		/// Represents database fields, each with a single value.
		/// </summary>
		/// <value>The value parameters.</value>
		SQLParameterValueCollection ValueParams{ get; set; }

		/// <summary>
		/// Gets or sets the multi value parameters.
		/// Represents database fields, each with multiple values.
		/// </summary>
		/// <value>The multi value parameters.</value>
		SQLParameterMultiValueCollection MultiValueParams{ get; set; }
	}
}

