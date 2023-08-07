using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus
{
	public class GraphNode
	{
		public List<GraphNode> OtherNodes{ get; set; }

		public object NodeClass{ get; set; }

		public GraphNode ()
		{
			this.OtherNodes = new List<GraphNode> ();
		}
	}
}

