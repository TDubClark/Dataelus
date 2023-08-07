using System;
using System.Collections.Generic;

namespace Dataelus
{
	/// <summary>
	/// Graph value display root node.
	/// Represents a Root node.
	/// </summary>
	public class GraphValueDisplayRootNode : GraphNode
	{
		/// <summary>
		/// Gets or sets the type of the values in the child nodes.
		/// </summary>
		/// <value>The type of the value.</value>
		public Type ValueType{ get; set; }

		public GraphValueDisplayNode AddNode (object value, string display)
		{
			var newNode = new GraphValueDisplayNode ();
			newNode.Value = value;
			newNode.Display = display;
			newNode.NodeClass = this.NodeClass;
			this.OtherNodes.Add (newNode);
			return newNode;
		}

		public GraphValueDisplayRootNode ()
			: base ()
		{
		}

		public GraphValueDisplayRootNode (object nodeClass, Type valueType)
			: this ()
		{
			this.NodeClass = nodeClass;
			this.ValueType = valueType;
		}
	}
}

