using System;
using System.Collections.Generic;

namespace Dataelus.EDD
{
	/// <summary>
	/// Table validation result collection.
	/// </summary>
	public class TableValidationResultCollection : ListBase<TableValidationResult>
	{
		public TableValidationResultCollection ()
			: base ()
		{
		}

		public TableValidationResultCollection (System.Collections.Generic.IEnumerable<TableValidationResult> collection)
			: base (collection)
		{
		}

		public void Add (ITableValidationResult item)
		{
			Add (new TableValidationResult (item));
		}

		public void AddRange (IEnumerable<ITableValidationResult> collection)
		{
			foreach (var item in collection) {
				Add (item);
			}
		}
	}
}

