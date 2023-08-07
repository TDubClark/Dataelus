using System;

namespace Dataelus.Wizard
{
	/// <summary>
	/// Interface for wizard navigation.
	/// </summary>
	public interface IWizardNavigation
	{
		/// <summary>
		/// Gets or sets the index of the current step.
		/// </summary>
		/// <value>The index of the current step.</value>
		int CurrentStepIndex{ get; set; }

		/// <summary>
		/// Gets or sets the step count.
		/// </summary>
		/// <value>The step count.</value>
		int StepCount{ get; set; }

		/// <summary>
		/// Performs the given navigation action
		/// </summary>
		/// <returns><c>true</c>, if action was navigationed, <c>false</c> otherwise.</returns>
		/// <param name="action">Action.</param>
		bool NavigationAction (WizardNavAction action);

		/// <summary>
		/// Performs the given navigation action
		/// </summary>
		/// <returns><c>true</c>, if action jump was navigationed, <c>false</c> otherwise.</returns>
		/// <param name="action">Action.</param>
		/// <param name="newIndex">The Step index (If Jump, this will be the target index; if Start, this will be the start index).</param>
		bool NavigationAction (WizardNavAction action, int stepIndex);

		/// <summary>
		/// Determines whether the specified action is permitted.
		/// </summary>
		/// <returns><c>true</c> if this instance is permitted the specified action; otherwise, <c>false</c>.</returns>
		/// <param name="action">Action.</param>
		bool IsPermitted (WizardNavAction action);

		/// <summary>
		/// Determines whether the specified action is permitted for the given stepIndex.
		/// </summary>
		/// <returns><c>true</c> if this instance is permitted the specified action stepIndex; otherwise, <c>false</c>.</returns>
		/// <param name="action">Action.</param>
		/// <param name="stepIndex">Step index.</param>
		bool IsPermitted (WizardNavAction action, int stepIndex);

		/// <summary>
		/// Gets or sets the behavior object.
		/// </summary>
		/// <value>The behavior.</value>
		IWizardNavBehavior Behavior{ get; set; }
	}
}

