using System;

namespace Dataelus.Table.Comparison.Generic
{
	public interface ITableIterator<T, R>
	{
		/// <summary>
		/// Starts the iterator.
		/// </summary>
		void Start ();

		/// <summary>
		/// Gets the next record.
		/// </summary>
		/// <returns><c>true</c>, if next record was gotten, <c>false</c> otherwise.</returns>
		/// <param name="table">Table.</param>
		/// <param name="record">Record.</param>
		bool GetNextRecord (T table, out R record);

		/// <summary>
		/// Gets the current index.
		/// </summary>
		/// <value>The index of the current.</value>
		int CurrentIndex{ get; }
	}
}

