using System;

namespace Dataelus.FilterCascade.UI
{
	/*
	 * This is essentially a tree-navigator, where you can only select one node at a time
	 * 
	 * Q: is this really necessary, when we can just use a tree-view?
	 * 
	 * Well, we may need some formal, easy method for defining how items are nested, 
	 * such that a tree-view can be easily built from a model.
	 */

	/// <summary>
	/// Interface for a filter cascade controller, where all items are text values.
	/// </summary>
	public interface IFilterTextCascadeController
	{
		
	}
}

