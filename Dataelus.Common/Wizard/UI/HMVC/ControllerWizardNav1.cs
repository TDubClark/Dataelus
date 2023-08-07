using System;
using Dataelus.HMVC;

namespace Dataelus.Wizard.UI.HMVC
{
	/// <summary>
	/// Controller for wizard navigation (Version 1).
	/// </summary>
	public class ControllerWizardNav1 : IControllerWizardNav
	{
		public ControllerWizardNav1 (IControllerWizardStep parent)
		{
			_parent = parent;
		}

		public void NavCancel ()
		{
			_parent.HandleEvent (this, Wizard.WizardNavAction.Cancel);
		}

		public void NavBack ()
		{
			_parent.HandleEvent (this, Wizard.WizardNavAction.Back);
		}

		public void NavNext ()
		{
			_parent.HandleEvent (this, new Wizard.WizardNavActionEvent (Wizard.WizardNavAction.Next));
		}

		public void NavFinish ()
		{
			_parent.HandleEvent (this, new Wizard.WizardNavActionEvent (Wizard.WizardNavAction.Finish));
		}

		public void NavJump (int index)
		{
			_parent.HandleAction (this, new Wizard.WizardNavActionEvent (Wizard.WizardNavAction.Jump, index, StepNumberType.Index));
		}

		public void NavJumpRelative (int relativeNumber)
		{
			_parent.HandleAction (this, new Wizard.WizardNavActionEvent (Wizard.WizardNavAction.Jump, relativeNumber, StepNumberType.Relative));
		}

		#region IControllerWizardNav implementation

		protected IControllerWizardStep _parent;

		public IControllerWizardStep Parent {
			get { return _parent; }
			set { _parent = value; }
		}

		#endregion

		#region IControllerBase implementation

		public bool HandleAction (object sender, Dataelus.HMVC.IActionEvent action)
		{
			var item = action as Dataelus.Wizard.WizardNavActionEvent;
			if (item != null) {
				return _parent.HandleAction (sender, item);
			}
			return _parent.HandleAction (sender, action);
		}

		public void HandleEvent (object sender, object args)
		{
			_parent.HandleEvent (sender, args);
		}

		public IControllerBase ParentController {
			get { return _parent; }
			set { _parent = (IControllerWizardStep)value; }
		}

		#endregion
	}
}

