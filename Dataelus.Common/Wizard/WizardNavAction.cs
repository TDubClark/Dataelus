using System;

namespace Dataelus.Wizard
{
	/// <summary>
	/// Wizard navigation action.
	/// </summary>
	public enum WizardNavAction
	{
		/// <summary>
		/// Cancel
		/// </summary>
		Cancel,

		/// <summary>
		/// Start
		/// </summary>
		Start,

		/// <summary>
		/// Go to the next step
		/// </summary>
		Next,

		/// <summary>
		/// Go back, to the prior step
		/// </summary>
		Back,

		/// <summary>
		/// Finish
		/// </summary>
		Finish,

		/// <summary>
		/// Jump to a given wizard index
		/// </summary>
		Jump
	}
}

