using System;

namespace Dataelus.HMVC
{
	/// <summary>
	/// Interface for a typed action event.
	/// </summary>
	public interface IActionEventTyped : IActionEvent
	{
		/// <summary>
		/// Gets or sets the action.
		/// </summary>
		/// <value>The action.</value>
		ActionType Action{ get; set; }

		/// <summary>
		/// Gets or sets the action parameter.
		/// </summary>
		/// <value>The action parameter.</value>
		object ActionParameter { get; set; }
	}
}

