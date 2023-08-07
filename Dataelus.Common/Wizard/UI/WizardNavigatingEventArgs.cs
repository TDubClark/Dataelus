using System;

namespace Dataelus.Wizard.UI
{
	public class WizardNavigatingEventArgs : EventArgs, IWizardNavigatingEventArgs
	{
		public WizardNavigatingEventArgs ()
		{
		}

		#region IWizardNavigatingEventArgs implementation

		IWizardView _view;

		public IWizardView View {
			get { return _view; }
			set { _view = value; }
		}

		WizardNavAction _action;

		public WizardNavAction Action {
			get { return _action; }
			set { _action = value; }
		}

		int _stepIndex;

		public int StepIndex {
			get {
				return _stepIndex; }
			set { _stepIndex = value;
			}
		}

		bool _proceed;

		public bool Proceed {
			get { return _proceed; }
			set { _proceed = value; }
		}

		#endregion
	}
}

