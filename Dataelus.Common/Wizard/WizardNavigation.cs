using System;

namespace Dataelus.Wizard
{
	/// <summary>
	/// Wizard navigation.
	/// </summary>
	public class WizardNavigation : IWizardNavigation
	{
		public WizardNavigation ()
		{
			_startIndex = 0;
		}

		protected int _startIndex;

		/// <summary>
		/// Gets or sets the start index.
		/// </summary>
		/// <value>The start index.</value>
		public int StartIndex {
			get { return _startIndex; }
			set { _startIndex = value; }
		}

		/// <summary>
		/// Applies the nav action.
		/// </summary>
		/// <param name="action">Action.</param>
		/// <param name="stepIndex">Step index.</param>
		public virtual void ApplyNavAction (WizardNavAction action, int stepIndex)
		{
			switch (action) {
			case WizardNavAction.Back:
				_currentStepIndex--;
				break;
			case WizardNavAction.Cancel:
				break;
			case WizardNavAction.Finish:
				_currentStepIndex = _stepCount;
				break;
			case WizardNavAction.Jump:
				_currentStepIndex = stepIndex;
				break;
			case WizardNavAction.Next:
				_currentStepIndex++;
				break;
			case WizardNavAction.Start:
				_currentStepIndex = _startIndex;
				break;
			default:
				break;
			}
		}

		#region IWizardNavigation implementation

		public virtual bool NavigationAction (WizardNavAction action)
		{
			if (IsPermitted (action)) {
				ApplyNavAction (action, -1);
			}
			return false;
		}

		public virtual bool NavigationAction (WizardNavAction action, int stepIndex)
		{
			if (IsPermitted (action, stepIndex)) {
				ApplyNavAction (action, stepIndex);
			}
			return false;
		}

		public bool IsPermitted (WizardNavAction action)
		{
			return _navigationBehavior.IsPermitted (this, action);
		}

		public bool IsPermitted (WizardNavAction action, int stepIndex)
		{
			return _navigationBehavior.IsPermitted (this, action, stepIndex);
		}

		protected int _currentStepIndex;

		public int CurrentStepIndex {
			get { return _currentStepIndex; }
			set { _currentStepIndex = value; }
		}

		protected int _stepCount;

		public int StepCount {
			get { return _stepCount; }
			set { _stepCount = value; }
		}

		protected IWizardNavBehavior _navigationBehavior;

		public IWizardNavBehavior Behavior {
			get { return _navigationBehavior; }
			set { _navigationBehavior = value; }
		}

		#endregion
	}
}

