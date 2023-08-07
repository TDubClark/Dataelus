using System;

namespace Dataelus.Wizard.UI
{
	public delegate void WizardNavigatingHandler (object sender, WizardNavigatingEventArgs e);

	/// <summary>
	/// Interface for a wizard Controller.
	/// </summary>
	public interface IWizardController2 : IWizardController
	{
		/// <summary>
		/// Occurs when on wizard is navigating (just before the navigation occurs).
		/// </summary>
		event WizardNavigatingHandler WizardNavigating;

		/// <summary>
		/// Gets or sets the view object.
		/// </summary>
		/// <value>The view object.</value>
		IWizardView ViewObject{ get; set; }
	}
}

