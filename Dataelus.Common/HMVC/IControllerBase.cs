using System;

namespace Dataelus.HMVC
{
	/// <summary>
	/// Base Controller Interface
	/// </summary>
	public interface IControllerBase
	{
		IControllerBase ParentController{ get; set; }

		/// <summary>
		/// Handles the event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		void HandleEvent(object sender, object args);

		/// <summary>
		/// Handles the action.
		/// </summary>
		/// <returns><c>true</c>, if action was handled, <c>false</c> otherwise.</returns>
		/// <param name="sender">Sender.</param>
		/// <param name="action">Action.</param>
		bool HandleAction(object sender, Dataelus.HMVC.IActionEvent action);
	}
}

