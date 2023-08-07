using System;

namespace Dataelus.Table
{
	/// <summary>
	/// Interface for a record value collection.
	/// </summary>
	public interface IRecordValueCollection : ICollection2<IRecordValue>
	{
		/// <summary>
		/// Gets or sets the index of the row.
		/// </summary>
		/// <value>The index of the row.</value>
		int RowIndex{ get; set; }
	}
}

