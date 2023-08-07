using System;

namespace Dataelus.Mono.DynamiQuery
{
	/// <summary>
	/// Interface for a controller which allows item/record selection.
	/// </summary>
	public interface IControllerSelection
	{
		IViewSelection ViewObject { get; set; }

		IWizardController WizardControllerObject { get; set; }

		/// <summary>
		/// Loads the view.
		/// </summary>
		void LoadView ();

		/// <summary>
		/// Closes the view.
		/// </summary>
		void CloseView ();
	}
}

