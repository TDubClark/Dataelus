using System;

namespace Dataelus.HMVC
{
	/// <summary>
	/// Action type.
	/// </summary>
	public enum ActionType
	{
		/// <summary>
		/// Navigate action.
		/// </summary>
		Navigate
	}

	/// <summary>
	/// Action event.
	/// </summary>
	public class ActionEvent : IActionEventTyped
	{
		public ActionEvent (ActionType action)
			: this (action, null)
		{
		}

		public ActionEvent (ActionType action, object parameter)
		{
			_action = action;
			_actionCode = action.ToString ();
			_actionParameter = parameter;
		}

		#region IActionEventTyped implementation

		protected ActionType _action;

		public ActionType Action {
			get { return _action; }
			set { _action = value; }
		}

		protected object _actionParameter;

		public object ActionParameter {
			get { return _actionParameter; }
			set { _actionParameter = value; }
		}

		#endregion

		#region IActionEvent implementation

		protected string _actionCode;

		public string ActionCode {
			get {
				return _actionCode; }
			set { _actionCode = value;
			}
		}

		#endregion
	}
}

