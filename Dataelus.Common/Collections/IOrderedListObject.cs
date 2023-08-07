using System;

namespace Dataelus.Collections
{
	public interface IOrderedListObject : IOrderedListItem
	{
		object Value { get; set; }
	}
}

