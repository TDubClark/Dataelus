using System;

namespace Dataelus.Wizard.UI
{
	/// <summary>
	/// Wizard controller.
	/// </summary>
	public class WizardController : IWizardController2
	{
		public WizardController ()
		{

		}

		#region IWizardController implementation

		public event WizardNavigatingHandler WizardNavigating;

		protected virtual void OnWizardNavigating (WizardNavigatingEventArgs args)
		{
			WizardNavigatingHandler handler = WizardNavigating;
			if (handler != null) {
				handler (this, args);
			}
		}

		public virtual void StartWizard ()
		{
			Navigate (WizardNavAction.Start);
		}

		public virtual bool Navigate (WizardNavAction action)
		{
			return Navigate (action, -1);
		}

		protected virtual WizardNavigatingEventArgs GetNavEventArgs (WizardNavAction action, int stepIndex)
		{
			var args = new WizardNavigatingEventArgs ();
			args.Action = action;
			args.Proceed = _navigationObject.IsPermitted (action, stepIndex);
			args.StepIndex = stepIndex;
			args.View = _viewObject;
			return args;
		}

		protected virtual bool navigate (WizardNavigatingEventArgs args)
		{
			return _viewObject.Navigate (args.Action, args.StepIndex);
		}

		public virtual bool Navigate (WizardNavAction action, int stepIndex)
		{
			var args = GetNavEventArgs (action, stepIndex);

			OnWizardNavigating (args);

			if (args.Proceed) {
				navigate (args);
			}
			return false;
		}

		protected IWizardView _viewObject;

		public virtual IWizardView ViewObject {
			get { return _viewObject; }
			set { _viewObject = value; }
		}

		protected IWizardNavigation _navigationObject;

		public virtual IWizardNavigation NavigationObject {
			get { return _navigationObject; }
			set { _navigationObject = value; }
		}

		#endregion
	}
}

