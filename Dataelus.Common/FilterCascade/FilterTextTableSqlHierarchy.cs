using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// Filter text table sql hierarchy.
	/// </summary>
	public class FilterTextTableSqlHierarchy : CollectionBase<FilterTextTableSql>, System.Collections.IEnumerable
	{
		private Dataelus.Generic.CodedHierarchy<FilterTextTableSql> _hierarchy;
		private IEqualityComparer<string> _comparer;

		public FilterTextTableSqlHierarchy (IEqualityComparer<string> comparer)
			: base ()
		{
			_comparer = comparer;
			_hierarchy = new Dataelus.Generic.CodedHierarchy<FilterTextTableSql> (_comparer);
		}

		public FilterTextTableSqlHierarchy ()
			: this (new StringEqualityComparer ())
		{
		}

		/// <summary>
		/// Gets the children.
		/// </summary>
		/// <returns>The children.</returns>
		/// <param name="itemCode">Item code.</param>
		public List<FilterTextTableSql> GetChildren (string itemCode)
		{
			return _hierarchy.GetChildren (_items, itemCode);
		}

		/// <summary>
		/// Gets the parents.
		/// </summary>
		/// <returns>The parents.</returns>
		/// <param name="itemCode">Item code.</param>
		public List<FilterTextTableSql> GetParents (string itemCode)
		{
			return _hierarchy.GetParents (_items, itemCode);
		}
	}
}

