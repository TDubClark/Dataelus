using System;
using Dataelus.HMVC;

namespace Dataelus.Wizard.UI.HMVC
{
	/// <summary>
	/// Interface for a controller of wizard navigation controls.
	/// </summary>
	public interface IControllerWizardNav : IControllerBase
	{
		/// <summary>
		/// Gets or sets the parent - the wizard step.
		/// </summary>
		/// <value>The parent.</value>
		IControllerWizardStep Parent{ get; set; }
	}
}

