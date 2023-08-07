using System;

namespace Dataelus.Wizard.UI
{
	/// <summary>
	/// Interface for an overall wizard View.
	/// </summary>
	public interface IWizardView
	{
		/// <summary>
		/// Gets or sets the controller object.
		/// </summary>
		/// <value>The controller object.</value>
		IWizardController2 ControllerObject{ get; set; }

		/// <summary>
		/// Navigate the wizard, using the specified action and stepIndex.
		/// </summary>
		/// <param name="action">Action.</param>
		/// <param name="stepIndex">Step index.</param>
		bool Navigate (WizardNavAction action, int stepIndex);

		void LoadStep (int stepIndex);
	}
}

