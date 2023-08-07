using System;
using Dataelus.HMVC;

namespace Dataelus.Wizard.UI.HMVC
{
	/// <summary>
	/// Interface for a controller of an individual wizard step.
	/// </summary>
	public interface IControllerWizardStep : IControllerBase
	{
		/// <summary>
		/// Gets or sets the navigation controller.
		/// </summary>
		/// <value>The nav controller.</value>
		IControllerWizardNav NavController{ get; set; }

		/// <summary>
		/// Shows the wizard step.
		/// </summary>
		bool ShowWizardStep ();
	}

	/// <summary>
	/// Interface for a wizard step controller, with Data.
	/// </summary>
	public interface IControllerWizardStepData<T> : IControllerWizardStep
	{
		/// <summary>
		/// Gets or sets the view object.
		/// </summary>
		/// <value>The view object.</value>
		IViewWizardStep<T> ViewObject{ get; set; }

		/// <summary>
		/// Gets or sets the data object.
		/// </summary>
		/// <value>The data object.</value>
		T DataObject{ get; set; }
	}

	/// <summary>
	/// Interface for a wizard step View.
	/// </summary>
	public interface IViewWizardStep<T>
	{
		/*
		 * What exactly do you need to do in this view?
		 * This is just for the Wizard Step - just selecting data
		 * All this view does is Load Data, and then perform navigation stuff (handled by a sub-controller)
		 * ...and then allow Unload
		 */

		/// <summary>
		/// Loads the data into the view.
		/// </summary>
		/// <param name="dataObject">Data object.</param>
		void LoadData (T dataObject);

		/// <summary>
		/// Unloads the data from the view.
		/// </summary>
		/// <returns><c>true</c>, if data was unloaded, <c>false</c> otherwise.</returns>
		/// <param name="dataObject">Data object.</param>
		bool UnloadData (T dataObject);
	}
}

