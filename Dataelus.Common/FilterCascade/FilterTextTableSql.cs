using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// Filterable text table, which can build SQL Statements (with Command Parameters).
	/// </summary>
	public class FilterTextTableSql : FilterTextTable, ICodedHierarchyNode
	{
		#region ICodedHierarchyNode implementation

		public string ItemCode { get; set; }

		public System.Collections.Generic.List<string> ParentItemCodes { get; set; }

		#endregion

		public FilterTextTableSql ()
			: base ()
		{
		}

		public FilterTextTableSql (Table.TextTable other)
			: base (other)
		{
		}

		public FilterTextTableSql (FilterTextTable other)
			: base (other)
		{
		}

		/// <summary>
		/// Determines whether the specified filter is applicable to this table.
		/// Uses the given Associator to determine whether there is an association between this table and the filter.
		/// </summary>
		/// <returns><c>true</c> if this instance is filter applicable the specified filter associator; otherwise, <c>false</c>.</returns>
		/// <param name="filter">Filter.</param>
		/// <param name="associator">Associator between tables and filters.</param>
		public bool IsFilterApplicable (FilterTextItem filter, IFilterTableAssociator associator)
		{
			return associator.IsAssociated (this.ItemCode, filter.FilterCode);
		}

		/// <summary>
		/// Gets the command data object, which can be used to easily create a SQL Command object and parameters.
		/// Examples include .NET System.Data.SqlClient.SqlCommand
		/// </summary>
		/// <returns>The command data.</returns>
		/// <param name="filters">Filters.</param>
		/// <param name="fields">Fields, which will be used in the SQL Command.</param>
		public Dataelus.Database.SQL.SQLCommandParameterized GetCommandData (FilterTextItemCollection filters, List<ColumnField> fields)
		{
			return new Dataelus.Database.SQL.SQLCommandParameterized (GetSqlValues (filters, fields), new Dataelus.Database.SQL.SQLParameterNameBuilder ());
		}

		public FilterTextTableSql GetTableSqlInFilter (FilterTextItemCollection filters)
		{
			return new FilterTextTableSql (base.GetTableInFilter (filters));
		}

		/// <summary>
		/// Gets the sql parameter values for a Command.
		/// </summary>
		/// <returns>The sql values.</returns>
		/// <param name="filters">Filters.</param>
		/// <param name="fields">Fields.</param>
		public Dataelus.Database.SQL.SQLCommandValueParameters GetSqlValues (FilterTextItemCollection filters, List<ColumnField> fields)
		{
			// 1) Get the table of rows which are within the filter
			// 2) Get the SQL Command Value Parameters from that table, according to the given fields
			return GetTableSqlInFilter (filters).GetSqlValues (fields);
		}

		/// <summary>
		/// Gets the sql values.
		/// </summary>
		/// <returns>The sql values.</returns>
		/// <param name="fields">Fields.</param>
		public virtual Dataelus.Database.SQL.SQLCommandValueParameters GetSqlValues (List<ColumnField> fields)
		{
			// Builds a list of Values per given Column/Field
			var lstParameters = new Dataelus.Database.SQL.SQLCommandValueParameters ();
			foreach (var oField in fields) {
				if (ContainsColumn (oField.ColumnName)) {
					var values = GetValuesByColumn (oField.ColumnName).ToArray ();

					object[] oValues = new object[values.Length];
					values.CopyTo (oValues, 0);

					lstParameters.MultiValueParams.Add (new Dataelus.Database.SQL.SQLParameterMultiValue (oField.Field, oValues));
				}
			}
			return lstParameters;
		}
	}

	/// <summary>
	/// Associates a Column name and a Database Field.
	/// </summary>
	public class ColumnField
	{
		public ColumnField ()
		{
		}

		public ColumnField (string columnName, Database.IDBField field)
		{
			ColumnName = columnName;
			Field = field;
		}

		/// <summary>
		/// Gets or sets the name of the column.
		/// </summary>
		/// <value>The name of the column.</value>
		public string ColumnName{ get; set; }

		/// <summary>
		/// Gets or sets the database field.
		/// </summary>
		/// <value>The field.</value>
		public Database.IDBField Field{ get; set; }
	}
}

