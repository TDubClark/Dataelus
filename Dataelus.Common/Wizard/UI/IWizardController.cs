using System;

namespace Dataelus.Wizard.UI
{
	public interface IWizardController
	{
		/// <summary>
		/// Gets or sets the navigation object.
		/// </summary>
		/// <value>The navigation object.</value>
		IWizardNavigation NavigationObject{ get; set; }

		/// <summary>
		/// Navigate the wizard, using the specified action.
		/// </summary>
		/// <param name="action">Action.</param>
		bool Navigate (WizardNavAction action);

		/// <summary>
		/// Navigate the wizard, using the specified action and stepIndex.
		/// </summary>
		/// <param name="action">Action.</param>
		/// <param name="stepIndex">Step index.</param>
		bool Navigate (WizardNavAction action, int stepIndex);

		/// <summary>
		/// Starts the wizard.
		/// </summary>
		void StartWizard ();
	}
}

