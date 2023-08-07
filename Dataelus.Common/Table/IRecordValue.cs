using System;

namespace Dataelus.Table
{
	/// <summary>
	/// Interface for a value from a given record.
	/// </summary>
	public interface IRecordValue
	{
		/// <summary>
		/// Gets or sets the column ID, from which this value was taken.
		/// </summary>
		/// <value>The column ID.</value>
		int ColumnID{ get; set; }

		/// <summary>
		/// Gets or sets the name of the column.
		/// </summary>
		/// <value>The name of the column.</value>
		string ColumnName{ get; set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		object Value{ get; set; }
	}
}

