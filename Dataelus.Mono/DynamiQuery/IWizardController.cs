using System;

namespace Dataelus.Mono.DynamiQuery
{
	/// <summary>
	/// Interface for a wizard controller: the overall controller of the wizard
	/// </summary>
	public interface IWizardController
	{
		/// <summary>
		/// Starts the wizard at the specified index.
		/// </summary>
		/// <param name="startIndex">Start index.</param>
		void StartWizard (int startIndex);

		/// <summary>
		/// Communicates a Navigate command from the specified sender, using the given action and stepIndex.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="action">Action.</param>
		/// <param name="stepIndex">Step index.</param>
		bool Navigate (object sender, Dataelus.Wizard.WizardNavAction action, int stepIndex);
	}
}

