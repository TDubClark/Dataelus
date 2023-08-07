using System;

namespace Dataelus
{
	/// <summary>
	/// Graph value/display node.
	/// </summary>
	public class GraphValueDisplayNode : GraphNode
	{
		public object Value{ get; set; }

		public string Display{ get; set; }

		public GraphValueDisplayNode ()
			: base ()
		{

		}
	}
}

