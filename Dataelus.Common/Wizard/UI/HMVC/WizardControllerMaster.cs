using System;
using System.Linq;

namespace Dataelus.Wizard.UI.HMVC
{
	public delegate void NavigatingHandler (object sender, NavigatingEventArgs e);

	/// <summary>
	/// Wizard step definition (includes the controller.
	/// </summary>
	public class WizardStepDef
	{
		public WizardStepDef (int stepIndex)
		{
			_stepIndex = stepIndex;
		}

		protected int _stepIndex;

		public int StepIndex {
			get { return _stepIndex; }
			set { _stepIndex = value; }
		}

		protected IControllerWizardStep _controller;

		public IControllerWizardStep Controller {
			get { return _controller; }
			set { _controller = value; }
		}
	}

	/// <summary>
	/// Master Wizard controller.
	/// </summary>
	public class WizardControllerMaster : IWizardControllerMaster
	{
		public WizardControllerMaster ()
			: base ()
		{
			_isMaster = true;
			_parentController = null;
			_wizardSteps = new System.Collections.Generic.List<WizardStepDef> ();
		}

		public event NavigatingHandler WizardNavigating;

		protected virtual void OnWizardNavigating (NavigatingEventArgs args)
		{
			NavigatingHandler handler = WizardNavigating;
			if (handler != null) {
				handler (this, args);
			}
		}

		protected System.Collections.Generic.List<WizardStepDef> _wizardSteps;

		public System.Collections.Generic.List<WizardStepDef> WizardSteps {
			get {
				return _wizardSteps;
			}
			set {
				_wizardSteps = value;
			}
		}

		protected virtual IControllerWizardStep GetStepController (int stepIndex)
		{
			return _wizardSteps.Where (x => x.StepIndex == stepIndex).Select (x => x.Controller).FirstOrDefault ();
		}

		protected virtual NavigatingEventArgs GetNavEventArgs (WizardNavAction action, int stepIndex)
		{
			var args = new NavigatingEventArgs ();
			args.Action = action;
			args.StepIndex = stepIndex;
			args.Proceed = _navigationObject.IsPermitted (action, stepIndex);
			args.StepController = GetStepController (stepIndex);
			return args;
		}

		protected virtual bool navigate (NavigatingEventArgs args)
		{
			return args.StepController.ShowWizardStep ();
		}

		#region IWizardController implementation

		public bool Navigate (WizardNavAction action)
		{
			return Navigate (action, -1);
		}

		public bool Navigate (WizardNavAction action, int stepIndex)
		{
			var args = GetNavEventArgs (action, stepIndex);

			OnWizardNavigating (args);

			if (args.Proceed) {
				return navigate (args);
			}
			return false;
		}

		public void StartWizard ()
		{
			Navigate (WizardNavAction.Start);
		}

		IWizardNavigation _navigationObject;

		public IWizardNavigation NavigationObject {
			get { return _navigationObject; }
			set { _navigationObject = value; }
		}

		#endregion

		#region IControllerBase implementation

		public void HandleEvent (object sender, object args)
		{
			var action = args as Dataelus.HMVC.ActionEvent;
			if (action != null) {
				HandleAction (sender, action);
			}
		}

		public bool HandleAction (object sender, Dataelus.HMVC.IActionEvent action)
		{
			if (action.ActionCode == "Navigate") {
				var navAction = action as Dataelus.Wizard.WizardNavActionEvent;
				if (navAction != null) {
					this.Navigate (navAction.NavAction, navAction.GetStepIndex (_navigationObject.CurrentStepIndex));
				}
			}
			return false;
		}

		protected Dataelus.HMVC.IControllerBase _parentController;

		public Dataelus.HMVC.IControllerBase ParentController {
			get { return _parentController; }
			set { _parentController = value; }
		}

		#endregion

		#region IControllerMaster implementation

		readonly bool _isMaster;

		public bool IsMaster {
			get { return _isMaster; }
		}

		#endregion
	}
}

