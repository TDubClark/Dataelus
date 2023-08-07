using System;

namespace Dataelus.Wizard.UI
{
	public interface IWizardNavigatingEventArgs
	{
		/// <summary>
		/// Gets or sets the View object.
		/// </summary>
		/// <value>The view.</value>
		IWizardView View{ get; set; }

		/// <summary>
		/// Gets or sets the Proposed action.
		/// </summary>
		/// <value>The action.</value>
		WizardNavAction Action{ get; set; }

		/// <summary>
		/// Gets or sets the proposed index of the step.
		/// </summary>
		/// <value>The index of the step.</value>
		int StepIndex{ get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to proceed with the proposed navigation.
		/// </summary>
		/// <value><c>true</c> if this instance is proceed; otherwise, <c>false</c>.</value>
		bool Proceed{ get; set; }
	}
}

