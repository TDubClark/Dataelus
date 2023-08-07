using System;
using System.Collections.Generic;

namespace Dataelus
{
	public interface IGraphNode
	{
		/// <summary>
		/// Gets or sets the references to other nodes.
		/// </summary>
		/// <value>The other nodes.</value>
		List<IGraphNode> OtherNodes{ get; set; }

		/// <summary>
		/// Gets or sets the node class.
		/// </summary>
		/// <value>The node class.</value>
		object NodeClass{ get; set; }
	}
}

