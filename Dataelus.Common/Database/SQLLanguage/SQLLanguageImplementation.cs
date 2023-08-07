using System;

namespace Dataelus.Database.SQLLanguage
{
	public enum SQLLanguageImplementation
	{
		/// <summary>
		/// ODBC SQL Language implementation
		/// </summary>
		ODBC,

		/// <summary>
		/// Oracle SQL Language implementation.
		/// </summary>
		Oracle,

		/// <summary>
		/// MS Access SQL Language implementation.
		/// </summary>
		MSAccess,

		/// <summary>
		/// MS SQL server SQL Language implementation.
		/// </summary>
		MSSQLServer,

		/// <summary>
		/// MySQL SQL Language implementation
		/// </summary>
		MySQL,

		/// <summary>
		/// Generic implementation, which enbrackets any field or table name.
		/// </summary>
		GenericEnbracketAll
	}
}

