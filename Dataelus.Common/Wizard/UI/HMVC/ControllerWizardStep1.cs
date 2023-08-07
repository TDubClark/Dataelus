using System;

namespace Dataelus.Wizard.UI.HMVC
{
	/// <summary>
	/// Controller for a wizard step (Version 1).
	/// </summary>
	public abstract class ControllerWizardStep1 : IControllerWizardStep
	{
		public ControllerWizardStep1 ()
		{
		}

		public abstract bool ShowWizardStep ();

		#region IControllerBase implementation

		public void HandleEvent (object sender, object args)
		{
			throw new NotImplementedException ();
		}

		public bool HandleAction (object sender, Dataelus.HMVC.IActionEvent action)
		{
			throw new NotImplementedException ();
		}

		public Dataelus.HMVC.IControllerBase ParentController {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		#endregion

		#region IControllerWizardStep implementation

		public IControllerWizardNav NavController {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		#endregion
	}
}

