using System;

namespace Dataelus.Wizard.UI.HMVC
{
	public class NavigatingEventArgs : EventArgs
	{
		public NavigatingEventArgs ()
			: base ()
		{
			
		}

		IControllerWizardStep _stepController;

		public IControllerWizardStep StepController {
			get { return _stepController; }
			set { _stepController = value; }
		}

		WizardNavAction _action;

		/// <summary>
		/// Gets or sets the Proposed action.
		/// </summary>
		/// <value>The action.</value>
		public WizardNavAction Action {
			get { return _action; }
			set { _action = value; }
		}

		int _stepIndex;

		/// <summary>
		/// Gets or sets the proposed index of the step.
		/// </summary>
		/// <value>The index of the step.</value>
		public int StepIndex {
			get {
				return _stepIndex; }
			set { _stepIndex = value;
			}
		}

		protected bool _proceed;

		/// <summary>
		/// Gets or sets a value indicating whether to proceed with the proposed navigation.
		/// </summary>
		/// <value><c>true</c> if this instance is proceed; otherwise, <c>false</c>.</value>
		public bool Proceed {
			get { return _proceed; }
			set { _proceed = value; }
		}
	}
}

