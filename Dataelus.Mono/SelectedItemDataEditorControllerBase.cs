using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Mono
{
	public abstract class SelectedItemDataEditorControllerBase
	{
		protected DataEditorController _editorController;

		/// <summary>
		/// Gets or sets the editor controller.
		/// </summary>
		/// <value>The editor controller.</value>
		public DataEditorController EditorController {
			get { return _editorController; }
			set { _editorController = value; }
		}

		protected ISelectedItemDataEditorView _viewObject;

		/// <summary>
		/// Gets or sets the view object.
		/// </summary>
		/// <value>The view object.</value>
		public ISelectedItemDataEditorView ViewObject {
			get { return _viewObject; }
			set { _viewObject = value; }
		}

		protected List<int> _selectedRowIndexValues;

		/// <summary>
		/// Gets or sets the selected row index values.
		/// </summary>
		/// <value>The selected row index values.</value>
		public List<int> SelectedRowIndexValues {
			get { return _selectedRowIndexValues; }
			set { _selectedRowIndexValues = value; }
		}

		private System.Data.DataSet _selectableItems;

		public System.Data.DataSet SelectableItems {
			get { return _selectableItems; }
			set { _selectableItems = value; }
		}

		/// <summary>
		/// Loads the view.
		/// </summary>
		public void LoadView ()
		{
			_viewObject.LoadSelectableData (_selectableItems);
		}

		/// <summary>
		/// Loads the new data when the selected data is changed
		/// </summary>
		/// <param name="values">Values.</param>
		public void SelectedItemChanged (Dictionary<int, object> values)
		{
			if (EditorController.IsAnyChanges ()) {

				var unsavedDataAction = _viewObject.PromptUnsaved ();
				switch (unsavedDataAction) {
					case UnsavedDataAction.Save:
						// Save the data
						_editorController.SaveData ();

						// Continue...
						break;
					case UnsavedDataAction.DoNotSave:
						// Continue...
						break;
					case UnsavedDataAction.Cancel:
						// Do not continue...
						return;
					default:
						throw new ArgumentOutOfRangeException ("unsavedDataAction", unsavedDataAction, "Invalid value");
				}
			}

			_editorController.LoadNewData (GetSelectedItemData (values));
			_editorController.LoadView ();
		}

		/// <summary>
		/// Gets the selected item data.
		/// </summary>
		/// <returns>The selected item data.</returns>
		/// <param name="values">Values.</param>
		protected abstract System.Data.DataSet GetSelectedItemData (Dictionary<int, object> values);

		protected SelectedItemDataEditorControllerBase (DataEditorController editorController, ISelectedItemDataEditorView viewObject, List<int> selectedRowIndexValues, System.Data.DataSet selectableItems)
		{
			if (editorController == null)
				throw new ArgumentNullException ("editorController");
			if (viewObject == null)
				throw new ArgumentNullException ("viewObject");
			if (selectedRowIndexValues == null)
				throw new ArgumentNullException ("selectedRowIndexValues");
			if (selectableItems == null)
				throw new ArgumentNullException ("selectableItems");
			
			this._editorController = editorController;
			this._viewObject = viewObject;
			this._selectedRowIndexValues = selectedRowIndexValues;
			this._selectableItems = selectableItems;
		}
		
	}

	/// <summary>
	/// Selected item data editor controller, for SQL Server.
	/// </summary>
	public class SelectedItemDataEditorControllerSQL : SelectedItemDataEditorControllerBase
	{
		/// <summary>
		/// Gets or sets map between column index of the view, and the database fieldname.
		/// </summary>
		/// <value>The column index to field map.</value>
		public Dictionary<int, string> ColumnIndexToFieldMap { get; set; }

		public string TableName { get; set; }

		public IEnumerable<string> SelectFields { get; set; }

		public DBQuerier Querier { get; set; }

		protected virtual System.Data.SqlClient.SqlCommand GetCommand (Dictionary<int, object> columnValues)
		{
			var dictValues = new Dictionary<string, object> ();
			foreach (var item in columnValues) {
				string fieldname;
				if (!ColumnIndexToFieldMap.TryGetValue (item.Key, out fieldname))
					throw new ArgumentOutOfRangeException ("[column index]", item.Key, "Invalid column index: not found in the column-to-field map.");
				
				dictValues.Add (fieldname, item.Value);
			}

			return SelectCommandBuilder (dictValues);
		}

		public Func<Dictionary<string, object>, System.Data.SqlClient.SqlCommand> SelectCommandBuilder { get; set; }

		#region implemented abstract members of SelectedItemDataEditorControllerBase

		protected override System.Data.DataSet GetSelectedItemData (Dictionary<int, object> values)
		{
			return this.Querier.GetDs (GetCommand (values));
		}

		#endregion

		public SelectedItemDataEditorControllerSQL (
			DataEditorController editorController, ISelectedItemDataEditorView viewObject, List<int> selectedRowIndexValues, System.Data.DataSet selectableItems
			, Dictionary<int, string> columnIndexToFieldMap, string tableName, IEnumerable<string> selectFields, DBQuerier querier)
			: base (editorController, viewObject, selectedRowIndexValues, selectableItems)
		{
			this.ColumnIndexToFieldMap = columnIndexToFieldMap;
			this.TableName = tableName;
			this.SelectFields = selectFields;
			this.Querier = querier;

			_cmdBuilder = new Dataelus.Mono.SQLServer.CommandBuilder (editorController.ColumnSchema);

			this.SelectCommandBuilder = new Func<Dictionary<string, object>, System.Data.SqlClient.SqlCommand> (GetSelectCommand);
		}

		public System.Data.SqlClient.SqlCommand GetSelectCommand (Dictionary<string, object> conditions)
		{
			return _cmdBuilder.GetCommandSelect (this.TableName, false, this.SelectFields, conditions);
		}

		protected Mono.SQLServer.CommandBuilder _cmdBuilder;

		public SelectedItemDataEditorControllerSQL (
			DataEditorController editorController, ISelectedItemDataEditorView viewObject, List<int> selectedRowIndexValues, System.Data.DataSet selectableItems
			, Dictionary<int, string> columnIndexToFieldMap, DBQuerier querier, Func<Dictionary<string, object>, System.Data.SqlClient.SqlCommand> selectCommandBuilder)
			: base (editorController, viewObject, selectedRowIndexValues, selectableItems)
		{
			if (columnIndexToFieldMap == null)
				throw new ArgumentNullException ("columnIndexToFieldMap");
			if (querier == null)
				throw new ArgumentNullException ("querier");
			if (selectCommandBuilder == null)
				throw new ArgumentNullException ("selectCommandBuilder");
			
			this.ColumnIndexToFieldMap = columnIndexToFieldMap;
			this.Querier = querier;
			this.SelectCommandBuilder = selectCommandBuilder;
		}
	}

	public enum UnsavedDataAction
	{
		/// <summary>
		/// Save the data.
		/// </summary>
		Save,

		/// <summary>
		/// Do not save the data.
		/// </summary>
		DoNotSave,

		/// <summary>
		/// Cancel whatever action would cause the loss of unsaved data.
		/// </summary>
		Cancel
	}

	public interface ISelectedItemDataEditorView
	{
		SelectedItemDataEditorControllerBase ControllerObject { get; set; }

		UnsavedDataAction PromptUnsaved ();

		void LoadSelectableData (System.Data.DataSet data);
	}
}

