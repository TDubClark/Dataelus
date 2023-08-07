using System;

namespace Dataelus.Wizard
{
	/// <summary>
	/// Defines wizrd navigation behavior.
	/// </summary>
	public interface IWizardNavBehavior
	{
		/// <summary>
		/// Gets or sets a value indicating whether to allow Jump navigation.
		/// </summary>
		/// <value><c>true</c> if this instance is allow jump; otherwise, <c>false</c>.</value>
		bool AllowJump{ get; set; }

		/// <summary>
		/// Determines whether the given action is permitted for the given wizard navigation.
		/// </summary>
		/// <returns><c>true</c> if this instance is valid action the specified nav action; otherwise, <c>false</c>.</returns>
		/// <param name="nav">Nav.</param>
		/// <param name="action">Action.</param>
		bool IsPermitted (IWizardNavigation nav, WizardNavAction action);

		/// <summary>
		/// Determines whether the given action is permitted for the given wizard navigation.
		/// </summary>
		/// <returns><c>true</c> if this instance is permitted the specified nav action stepIndex; otherwise, <c>false</c>.</returns>
		/// <param name="nav">Nav.</param>
		/// <param name="action">Action.</param>
		/// <param name="stepIndex">Step index.</param>
		bool IsPermitted (IWizardNavigation nav, WizardNavAction action, int stepIndex);
	}
}

