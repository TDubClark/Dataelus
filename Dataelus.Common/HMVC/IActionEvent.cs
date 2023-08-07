using System;

namespace Dataelus.HMVC
{
	/// <summary>
	/// Interface for an action event.
	/// </summary>
	public interface IActionEvent
	{
		/// <summary>
		/// Gets or sets the action code.
		/// </summary>
		/// <value>The action code.</value>
		string ActionCode{ get; set; }
	}
}

