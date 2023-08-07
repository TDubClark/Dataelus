using System;

namespace Dataelus.Wizard
{
	/// <summary>
	/// Step number type (for wizard Navigation).
	/// </summary>
	public enum StepNumberType
	{
		/// <summary>
		/// The step number is relative to the current step number (ex: 2 = move ahead 2; -1 = move back 1)
		/// </summary>
		Relative,

		/// <summary>
		/// The step number is an index of all steps (0 = step number 1)
		/// </summary>
		Index
	}
}

