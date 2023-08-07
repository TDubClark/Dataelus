using System;

namespace Dataelus.Wizard
{
	/// <summary>
	/// Wizard navigation action event.
	/// </summary>
	public class WizardNavActionEvent : Dataelus.HMVC.ActionEvent
	{
		public WizardNavActionEvent (WizardNavAction action)
			: this (action, -1, StepNumberType.Index)
		{
		}

		public WizardNavActionEvent (WizardNavAction action, int step, StepNumberType stepType)
			: base (Dataelus.HMVC.ActionType.Navigate)
		{
			_navAction = action;
			_stepNumber = step;
			_stepType = stepType;
		}

		/// <summary>
		/// Gets the step index for the Wizard Navigation Action
		/// (For Relative Step, adjusts from the given current Step Index).
		/// </summary>
		/// <returns>The step index.</returns>
		/// <param name="currentStepIndex">Current step index.</param>
		public int GetStepIndex (int currentStepIndex)
		{
			int stepIndex = _stepNumber;
			if (_stepType == StepNumberType.Relative) {
				stepIndex = currentStepIndex + stepIndex;
			}
			return stepIndex;
		}

		protected WizardNavAction _navAction;

		public WizardNavAction NavAction {
			get { return _navAction; }
			set { _navAction = value; }
		}

		protected int _stepNumber;

		public int StepNumber {
			get { return _stepNumber; }
			set { _stepNumber = value; }
		}

		protected StepNumberType _stepType;

		public StepNumberType StepType {
			get {
				return _stepType; }
			set { _stepType = value;
			}
		}
	}
}

