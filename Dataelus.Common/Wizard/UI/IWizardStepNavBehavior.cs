using System;

namespace Dataelus.Wizard.UI
{
	/// <summary>
	/// Navigation behavior for an individual wizard step.
	/// </summary>
	public interface IWizardStepNavBehavior
	{
		/// <summary>
		/// Gets the index of the current step.
		/// </summary>
		/// <value>The index of the current step.</value>
		int CurrentStepIndex{ get; }

		/// <summary>
		/// Gets the total step count.
		/// </summary>
		/// <value>The step count total.</value>
		int StepCountTotal{ get; }

		/// <summary>
		/// Whether the given action is permitted for this wizard step.
		/// </summary>
		/// <returns><c>true</c>, if action was permitteded, <c>false</c> otherwise.</returns>
		/// <param name="action">Action.</param>
		bool PermittedAction (WizardNavAction action);

		/// <summary>
		/// Whether the given action (and the given stepIndex) is permitted for this wizard step.
		/// </summary>
		/// <returns><c>true</c>, if action was permitted, <c>false</c> otherwise.</returns>
		/// <param name="action">Action.</param>
		/// <param name="stepIndex">Step index.</param>
		bool PermittedAction (WizardNavAction action, int stepIndex);
	}
}

