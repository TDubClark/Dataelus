using System;

namespace Dataelus.FilterCascade
{
	/*
	 * Here's the idea behind a non-text filter:
	 * It does not associate with a normal column, because it does not have an enumerated selection of values
	 * ...not the same way a text-filter does
	 * 
	 * The non-text filter is not filtered in the same way: it is not a cascade of enumerated values.
	 * Rather, it is either:
	 *  1) a preset enumeration (True|False), (Active|Resolved|Closed), or
	 *  2) a bounded range (Date/Time: Start & End), (concentration value: minimum & maximum).
	 * 
	 * Either way, these are not filters that need to cascade themselves: they need to be *applied to* the cascades.
	 * 
	 * The user may change these, but they will need to click a corresponding "Apply" button,
	 * which will re-query/re-build the cascade tables.
	 */

	/// <summary>
	/// Interface for a non text filter.
	/// </summary>
	public interface INonTextFilter : IFilterItem, ICodedHierarchyNode
	{
		Table.TypeClass DataTypeClass{ get; set; }

		Type DataType{ get; set; }
	}
}

