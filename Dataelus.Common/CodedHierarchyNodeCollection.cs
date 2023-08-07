using System;

namespace Dataelus.Generic
{
	/// <summary>
	/// Represents a hierarchy of items, where each item has a unique Item Code.
	/// </summary>
	public class CodedHierarchyNodeCollection<T> : CollectionBase<CodedHierarchyNode<T>>, System.Collections.IEnumerable
	{
		public CodedHierarchyNodeCollection ()
			: base ()
		{
		}
	}
}

