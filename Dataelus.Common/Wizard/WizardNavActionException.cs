using System;

namespace Dataelus.Wizard
{
	/// <summary>
	/// Exception which occurs as a result of a wizard navigation action.
	/// </summary>
	public class WizardNavActionException : Exception
	{
		public WizardNavActionException ()
		{
		}
		

		public WizardNavActionException (string message) : base (message)
		{
		}
		

		public WizardNavActionException (string message, Exception innerException) : base (message, innerException)
		{
		}
		
	}
}

