using System.Collections.Generic;

namespace Dataelus.UI
{
	public interface IDataEditorController : GridEditor.IController<Dataelus.Table.ObjectTable>
	{
		/// <summary>
		/// Gets or sets the view object.
		/// </summary>
		/// <value>The view object.</value>
		new IDataEditorView ViewObject { get; set; }

		/// <summary>
		/// Run this function after deserialization.
		/// </summary>
		/// <param name="view">The View.</param>
		void RunAfterDeserialization (IDataEditorView view);

		/// <summary>
		/// Deletes the row by the given unique object ID
		/// </summary>
		/// <param name="objectUniqueID">The Unique ID for the Object.</param>
		void DeleteRowByID (long objectUniqueID);

		//		/// <summary>
		//		/// Loads the view.
		//		/// </summary>
		//		void LoadView ();
		//
		//		/// <summary>
		//		/// Saves the data to the Data Source.
		//		/// </summary>
		//		void SaveData ();
	}
}

