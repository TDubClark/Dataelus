using System;

namespace Dataelus.Mono.DynamiQuery
{
	/*
	 * This defines a SQL Query, and the associated parameters
	 * The assumptions here are:
	 *  - There is a final query, which takes some parameters
	 *  - There may be one or more selected parameter sets (ex: locations and chemicals), which the user selects one or more items
	 *  - There may be a date range
	 *  - There may be other single-value parameters
	 */

	/// <summary>
	/// Definition for a Dynamic query (including selections).
	/// </summary>
	[Serializable]
	public class DynamicQueryDef
	{
		/// <summary>
		/// Gets or sets the collection of selection tables.
		/// </summary>
		/// <value>The selection tables.</value>
		public TableSelectionDefCollection SelectionTables { get; set; }

		/// <summary>
		/// Gets or sets the DB table name of the Root Data Table (ex: for Chemistry data, this might be the Samples table).
		/// </summary>
		/// <value>The DB table name data root.</value>
		public string DBTableNameDataRoot { get; set; }

		/// <summary>
		/// Gets or sets the SQL data query join statement (the FROM part of the SQL query).
		/// </summary>
		/// <value>The SQL data query join.</value>
		public string SQLDataQueryJoin { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DynamiQuery.DynamicQueryDef"/> class.
		/// </summary>
		public DynamicQueryDef ()
		{
			this.SelectionTables = new TableSelectionDefCollection ();
		}
	}
}

