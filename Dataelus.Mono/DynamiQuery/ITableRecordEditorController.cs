using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Mono.DynamiQuery
{
	/// <summary>
	/// Interface for a table record editor controller.
	/// </summary>
	public interface ITableRecordEditorController
	{
		void LoadView ();

		void TableChanged (string tableName);

		void Save (string tableName, IEnumerable<FieldSelection> fields, int displayOrder);
	}

	/// <summary>
	/// Table record editor controller.
	/// </summary>
	public class TableRecordEditorController : ITableRecordEditorController
	{
		Dataelus.Database.DBFieldCollection _schema;

		/// <summary>
		/// Gets or sets the schema.
		/// </summary>
		/// <value>The schema.</value>
		public Dataelus.Database.DBFieldCollection Schema {
			get { return _schema; }
			set { _schema = value; }
		}

		/// <summary>
		/// Gets or sets the view object.
		/// </summary>
		/// <value>The view object.</value>
		public ITableRecordEditorView ViewObject { get; set; }

		public TableSelectionUIDef Model { get; set; }

		public FieldSelectionViewModel ViewModel { get; set; }

		string[] _tableNames;

		/// <summary>
		/// Occurs when the View data is saved to the model.
		/// </summary>
		public event EventHandler Saved;

		protected void OnSave ()
		{
			try {
				if (Saved != null)
					Saved (this, new EventArgs ());
			} catch (Exception ex) {
			}
		}

		public void SetView (ITableRecordEditorView viewObject)
		{
			if (viewObject == null)
				throw new ArgumentNullException ("viewObject");
			
			this.ViewObject = viewObject;

			viewObject.Controller = this;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DynamiQuery.TableRecordEditorController"/> class.
		/// </summary>
		/// <param name="schema">Schema.</param>
		public TableRecordEditorController (Dataelus.Database.DBFieldCollection schema)
		{
			if (schema == null)
				throw new ArgumentNullException ("schema");

			_schema = schema;

			// Load the Table Names
			_tableNames = _schema.GetTableNames ();

			this.Model = new TableSelectionUIDef ();
			this.ViewModel = new FieldSelectionViewModel ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DynamiQuery.TableRecordEditorController"/> class.
		/// </summary>
		/// <param name="schema">Schema.</param>
		/// <param name="viewObject">View object.</param>
		public TableRecordEditorController (Dataelus.Database.DBFieldCollection schema, ITableRecordEditorView viewObject)
			: this (schema)
		{
			SetView (viewObject);
		}

		public void LoadView ()
		{
			this.ViewObject.LoadTableSelector (new DBTableCollection (_tableNames), this.Model.DBTableName);
			this.ViewObject.LoadOrderNumber (this.Model.WizardDisplayOrder);
		}

		public void TableChanged (string tableName)
		{
			var comparer = new StringEqualityComparer ();
			this.ViewModel.Clear ();
			this.ViewModel.Load (_schema.GetFields (tableName, comparer), this.Model.DBFieldNames, this.Model.SearchDBFieldName, this.Model.DisplayFieldNames);

			this.ViewObject.LoadFieldList (this.ViewModel);
		}

		public void Save (string tableName, IEnumerable<FieldSelection> fields, int displayOrder)
		{
			var viewModel = new FieldSelectionViewModel (fields);

			this.Model.Clear ();
			this.Model.DBTableName = tableName;
			this.Model.DBFieldNames.AddRange (viewModel.GetSelectedFieldNames ());
			this.Model.WizardDisplayOrder = displayOrder;

			foreach (var item in viewModel.GetDisplayFieldNames()) {
				this.Model.DisplayFieldNames.Add (item.Field.FieldName, item.DisplayName);
			}

			this.Model.SearchDBFieldName = viewModel.GetSearchFieldName ();

			OnSave ();
		}
	}
}
