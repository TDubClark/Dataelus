using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Mono.DynamiQuery
{
	/// <summary>
	/// Interface for an editor controller.
	/// </summary>
	public interface IEditorController
	{
		void ShowCreateNew (ITableRecordEditorView view);

		void ShowEdit (ITableRecordEditorView view, long id);

		void AddNew (TableSelectionDef item);

		void Edit (long id);

		void Update (TableSelectionDef item);

		void Save ();
	}

	/// <summary>
	/// Editor controller.
	/// </summary>
	public class EditorController : IEditorController
	{
		/// <summary>
		/// Gets or sets the view object.
		/// </summary>
		/// <value>The view object.</value>
		public IEditorView ViewObject { get; set; }

		/// <summary>
		/// Gets or sets the model.
		/// </summary>
		/// <value>The model.</value>
		public TableSelectionDefCollection Model { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to accept changes.
		/// </summary>
		/// <value><c>true</c> if changes should be accepted; otherwise, <c>false</c>.</value>
		public bool IsAcceptChanges { get; set; }

		/// <summary>
		/// Gets or sets the Database field schema.
		/// </summary>
		/// <value>The field schema.</value>
		public Database.DBFieldCollection FieldSchema { get; set; }

		TableRecordEditorController _editorController;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DynamiQuery.EditorController"/> class.
		/// </summary>
		/// <param name="viewObject">View object.</param>
		/// <param name="fieldSchema">The Database Field Schema</param>
		public EditorController (IEditorView viewObject, Database.DBFieldCollection fieldSchema)
			: this (viewObject, fieldSchema, new TableSelectionDefCollection ())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DynamiQuery.EditorController"/> class.
		/// </summary>
		/// <param name="viewObject">View object.</param>
		/// <param name="fieldSchema">The Database Field Schema</param>
		/// <param name="model">Model.</param>
		public EditorController (IEditorView viewObject, Database.DBFieldCollection fieldSchema, TableSelectionDefCollection model)
		{
			if (model == null)
				throw new ArgumentNullException ("model");
			if (fieldSchema == null)
				throw new ArgumentNullException ("fieldSchema");
			if (viewObject == null)
				throw new ArgumentNullException ("viewObject");
			
			this.ViewObject = viewObject;
			this.Model = model;
			this.FieldSchema = fieldSchema;

			this.IsAcceptChanges = false;

			viewObject.Controller = this;

			_editorController = new TableRecordEditorController (fieldSchema);
		}

		/// <summary>
		/// Loads the view.
		/// </summary>
		public void LoadView ()
		{
			this.ViewObject.Load (this.Model);
		}

		#region IEditorController implementation

		/// <summary>
		/// Adds the new item.
		/// </summary>
		/// <param name="item">Item.</param>
		public void AddNew (TableSelectionDef item)
		{
			this.Model.Add (item);
			this.ViewObject.Update (item);
		}

		/// <summary>
		/// Edit the specified id.
		/// </summary>
		/// <param name="id">Identifier.</param>
		public void Edit (long id)
		{
			this.ViewObject.Edit (this.Model.Find (id));
		}

		/// <summary>
		/// Update the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Update (TableSelectionDef item)
		{
			this.Model.Update (item);
			this.ViewObject.Update (item);
		}

		/// <summary>
		/// Save this instance.
		/// </summary>
		public void Save ()
		{
			this.IsAcceptChanges = true;
		}

		public void ShowCreateNew (ITableRecordEditorView view)
		{
			_editorController = new TableRecordEditorController (this.FieldSchema);
			_editorController.Model = new TableSelectionUIDef ();
			_editorController.SetView (view);
			_editorController.LoadView ();
			_editorController.Saved += delegate(object sender, EventArgs e) {
				this.AddNew (((TableRecordEditorController)sender).Model);
			};
		}

		public void ShowEdit (ITableRecordEditorView view, long id)
		{
			_editorController = new TableRecordEditorController (this.FieldSchema);
			_editorController.Model = new TableSelectionUIDef (this.Model.Find (id));
			_editorController.SetView (view);
			_editorController.LoadView ();
			_editorController.Saved += delegate(object sender, EventArgs e) {
				this.Update (((TableRecordEditorController)sender).Model);
			};
		}

		#endregion
	}
}

