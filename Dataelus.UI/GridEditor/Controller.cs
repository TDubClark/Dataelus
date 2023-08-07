using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.UI.GridEditor
{
	/// <summary>
	/// Controller for a grid data editor.
	/// D is the type of DataGrid object.
	/// </summary>
	public abstract class Controller<D> : IController<D>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.UI.GridEditor.Controller`1"/> class.
		/// </summary>
		protected Controller ()
		{
			_eddFieldNameComparer = new StringEqualityComparer ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.UI.GridEditor.Controller`1"/> class.
		/// </summary>
		/// <param name="dbFields">Db fields.</param>
		/// <param name="dbConstraints">Db constraints.</param>
		protected Controller (EDD.EDDFieldCollection dbFields, Database.DBConstraintCollection dbConstraints)
			: this ()
		{
			if (dbFields == null)
				throw new ArgumentNullException ("dbFields");
			if (dbConstraints == null)
				throw new ArgumentNullException ("dbConstraints");

			_eddFields = dbFields;
			_databaseContraints = dbConstraints;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.UI.GridEditor.Controller`1"/> class.
		/// </summary>
		/// <param name="dbFields">Db fields.</param>
		/// <param name="dbConstraints">Db constraints.</param>
		/// <param name="dataObject">Data object.</param>
		protected Controller (EDD.EDDFieldCollection dbFields, Database.DBConstraintCollection dbConstraints, D dataObject)
			: this (dbFields, dbConstraints)
		{
			if (dataObject.Equals (default(D)))
				throw new ArgumentNullException ("dataObject");

			_gridDataObject = dataObject;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.UI.GridEditor.Controller`1"/> class.
		/// </summary>
		/// <param name="dbFields">Db fields.</param>
		/// <param name="dbConstraints">Db constraints.</param>
		/// <param name="dataObject">Data object.</param>
		/// <param name="viewObject">View object.</param>
		protected Controller (EDD.EDDFieldCollection dbFields, Database.DBConstraintCollection dbConstraints, D dataObject, IView<D> viewObject)
			: this (dbFields, dbConstraints, dataObject)
		{
			if (viewObject == null)
				throw new ArgumentNullException ("viewObject");

			_viewObject = viewObject;

			// See if the given view also implements the Grid Item Viewer
			var view = viewObject as Dataelus.UI.GridItemViewer.Generic.IView<D>;
			if (view != null) {
				_issueViewer = view;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.UI.GridEditor.Controller`1"/> class.
		/// </summary>
		/// <param name="dbFields">Db fields.</param>
		/// <param name="dbConstraints">Db constraints.</param>
		/// <param name="dataObject">Data object.</param>
		/// <param name="viewObject">View object.</param>
		/// <param name="saver">Saver.</param>
		/// <param name="validator">Validator.</param>
		protected Controller (EDD.EDDFieldCollection dbFields, Database.DBConstraintCollection dbConstraints
			, D dataObject, IView<D> viewObject
			, IDataSaver<D> saver, IValidator<D> validator)
			: this (dbFields, dbConstraints, dataObject, viewObject)
		{
			_saver = saver;
			_validator = validator;
		}

		#endregion

		#region Variables and Properties

		protected EDD.EditorData _editorMgr;

		/// <summary>
		/// Gets or sets the editor manager.
		/// </summary>
		/// <value>The editor mgr.</value>
		public EDD.EditorData EditorMgr {
			get { return _editorMgr; }
			set { _editorMgr = value; }
		}

		protected IDataServices2 _dataServices;

		/// <summary>
		/// Gets or sets the data services object.
		/// </summary>
		/// <value>The data services.</value>
		public IDataServices2 DataServices {
			get { return _dataServices; }
			set { _dataServices = value; }
		}

		protected D _gridDataObject;

		/// <summary>
		/// Gets or sets the grid data object.
		/// </summary>
		/// <value>The data object.</value>
		public D GridDataObject {
			get { return _gridDataObject; }
			set { _gridDataObject = value; }
		}


		protected IEqualityComparer<string> _eddFieldNameComparer;

		/// <summary>
		/// Gets or sets the EDD field name comparer.
		/// </summary>
		/// <value>The edd field name comparer.</value>
		public IEqualityComparer<string> EddFieldNameComparer {
			get { return _eddFieldNameComparer; }
			set { _eddFieldNameComparer = value; }
		}

		/// <summary>
		/// The EDD fields collection.
		/// </summary>
		protected Dataelus.EDD.EDDFieldCollection _eddFields;

		/// <summary>
		/// Gets or sets the EDD fields collection.
		/// </summary>
		/// <value>The database fields.</value>
		public Dataelus.EDD.EDDFieldCollection EddFields {
			get { return _eddFields; }
			set { _eddFields = value; }
		}

		/// <summary>
		/// The database contraints.
		/// </summary>
		protected Database.DBConstraintCollection _databaseContraints;

		// This is also stored in the editor; do not need to serialize it
		/// <summary>
		/// Gets or sets the database contraints.
		/// </summary>
		/// <value>The database contraints.</value>
		[Newtonsoft.Json.JsonIgnore]
		[System.Xml.Serialization.XmlIgnore]
		public Database.DBConstraintCollection DatabaseContraints {
			get { return _databaseContraints; }
			set { _databaseContraints = value; }
		}

		#endregion

		/// <summary>
		/// To be run the after deserialization.
		/// </summary>
		public virtual void RunAfterDeserialization ()
		{
			_databaseContraints = _editorMgr.ConstraintsList;
		}


		/// <summary>
		/// Gets the widget data for the database fields and constraints.
		/// </summary>
		/// <returns>The widget data.</returns>
		protected virtual FilterCascade.WidgetData.FilterSelectableTextValuesCache GetWidgetData ()
		{
			return Dataelus.FilterCascade.WidgetData.FilterSelectableTextValuesCache.GetValuesCache (this.EddFields, this.DataServices);

			/*var cache = new Dataelus.FilterCascade.WidgetData.FilterSelectableTextValuesCache ();

			foreach (var eddField in this.EddFields) {
				// Get the Data field
				var dbField = eddField.DBDataField;
				Database.DBConstraint[] constraints;
				if (this.DatabaseContraints.GetConstraintsSingleField (dbField, out constraints)) {
					cache.FilterData.Add (dbField.FieldName, GetValues (dbField, constraints));
				}
			}

			return cache;*/
		}

		/// <summary>
		/// Gets the values.
		/// </summary>
		/// <returns>The values.</returns>
		/// <param name="dbField">Db field.</param>
		/// <param name="singleFieldConstraints">Single field constraints - assumed to have only one database source column.</param>
		protected virtual Dictionary<string, string> GetValues (Database.IDBField dbField, Database.DBConstraint[] singleFieldConstraints)
		{
			var dict = new Dictionary<string, string> ();

			var comparer = new StringEqualityComparer ();

			if (singleFieldConstraints.Length >= 1) {
				dict = GetConstraintValues (singleFieldConstraints [0]);

				if (singleFieldConstraints.Length > 1) {
					// Gets the intersected values of all the constraints
					for (int i = 1; i < singleFieldConstraints.Length; i++) {
						var valuesDict = GetConstraintValues (singleFieldConstraints [i]);
						var valuesList = valuesDict.Select (x => x.Key).ToList ();

						// Get the list of keys to remove (keys which are not in the values list)
						var lstRemove = dict.Where (x => !valuesList.Contains (x.Key, comparer)).Select (x => x.Key).ToList ();

						// Remove each one
						foreach (var remove in lstRemove) {
							dict.Remove (remove);
						}
					}

					/*foreach (var cnstr in constraints) {
						var temp = GetDbValues (cnstr.ConstraintColumns.ElementAt (0).Key);
						if (lstValues == null)
							lstValues = temp;
						else
							lstValues = lstValues.Intersect (temp, comparer).ToList ();
					}
					foreach (var value in lstValues) {
						dict.Add (value, value);
					}*/
				}
			}

			return dict;
		}

		/// <summary>
		/// Gets the dictionary of values/display values for the given constraint (only the first column).
		/// </summary>
		/// <returns>The constraint values.</returns>
		/// <param name="obj">Object.</param>
		protected virtual Dictionary<string, string> GetConstraintValues (Dataelus.Database.DBConstraint obj)
		{
			return DataServices.GetValueDisplayDictionary (obj.FirstColumnSource ());
			/*var dict = new Dictionary<string, string> ();
			var firstContCol = obj.ConstraintColumns.ElementAt (0);

			var lst = GetDbValues (firstContCol.Key);
			foreach (var item in lst) {
				dict.Add (item, item);
			}
			return dict;*/
		}
		/*
		/// <summary>
		/// Gets the distinct database values for the given source field.
		/// Not used - retrieves by constructing SQL Statement
		/// </summary>
		/// <returns>The table values.</returns>
		/// <param name="sourceField">Source field.</param>
		protected virtual List<string> GetDbValues (Dataelus.Database.DbFieldSimple sourceField)
		{
			var sql = String.Format ("SELECT DISTINCT {0} FROM {1}", sourceField.FieldName, sourceField.TableName);
			var tbl = DataServices.GetTextTable (sql);
			int colIndex = tbl.Columns.FindIndex (sourceField.FieldName);
			return tbl.Rows.Select (x => x [colIndex]).ToList ();
		}
*/

		/// <summary>
		/// Gets the new index.
		/// </summary>
		/// <returns>The new index.</returns>
		/// <param name="direction">Direction.</param>
		/// <param name="currentIndex">Current index.</param>
		/// <param name="magnitude">Magnitude.</param>
		/// <param name="maxIndex">Max index.</param>
		public static int GetNewIndex (VerticalDirection direction, int currentIndex, int magnitude, int maxIndex)
		{
			int newIndex = -1;
			switch (direction) {
				case VerticalDirection.Up:
					newIndex = currentIndex - magnitude;
					break;
				case VerticalDirection.Down:
					newIndex = currentIndex + magnitude;
					break;
				case VerticalDirection.Bottom:
					newIndex = maxIndex;
					break;
				case VerticalDirection.Top:
					newIndex = 0;
					break;
				default:
					break;
			}
			if (newIndex < 0)
				newIndex = 0;
			if (newIndex > maxIndex)
				newIndex = maxIndex;

			return newIndex;
		}

		/// <summary>
		/// Gets the new index.
		/// </summary>
		/// <returns>The new index.</returns>
		/// <param name="direction">Direction.</param>
		/// <param name="currentIndex">Current index.</param>
		/// <param name="magnitude">Magnitude.</param>
		/// <param name="maxIndex">Max index.</param>
		public static int GetNewIndex (HorizontalDirection direction, int currentIndex, int magnitude, int maxIndex)
		{
			int newIndex = -1;
			switch (direction) {
				case HorizontalDirection.LeftMost:
					newIndex = 0;
					break;
				case HorizontalDirection.Left:
					newIndex = currentIndex - magnitude;
					break;
				case HorizontalDirection.Right:
					newIndex = currentIndex + magnitude;
					break;
				case HorizontalDirection.RightMost:
					newIndex = maxIndex;
					break;
				default:
					break;
			}
			if (newIndex < 0)
				newIndex = 0;
			if (newIndex > maxIndex)
				newIndex = maxIndex;

			return newIndex;
		}

		#region IController implementation

		/// <summary>
		/// The saver for the grid data object.
		/// </summary>
		protected IDataSaver<D> _saver;

		/// <summary>
		/// Gets or sets the saver for the data object.
		/// </summary>
		/// <value>The saver.</value>
		[Newtonsoft.Json.JsonIgnore]
		public IDataSaver<D> Saver {
			get { return _saver; }
			set { _saver = value; }
		}

		/// <summary>
		/// The validator for the grid data object.
		/// </summary>
		protected IValidator<D> _validator;

		/// <summary>
		/// Gets or sets the validator for the data object.
		/// </summary>
		/// <value>The validator.</value>
		[Newtonsoft.Json.JsonIgnore]
		public IValidator<D> Validator {
			get { return _validator; }
			set { _validator = value; }
		}

		/// <summary>
		/// Loads the view.
		/// </summary>
		public virtual void LoadView ()
		{
			_viewObject.LoadForm (_eddFields, GetWidgetData (), _gridDataObject);
		}

		public abstract void MoveRow (int rowIndex, VerticalDirection direction, int magnitude);

		public abstract void MoveColumn (string columnName, HorizontalDirection direction, int magnitude);

		public abstract void MoveColumn (int columnIndex, HorizontalDirection direction, int magnitude);

		public abstract void ValueChanged (int rowIndex, string columnName, string newValue);

		public abstract void ValueChanged (int rowIndex, string columnName, object newValue);

		public abstract void SaveData ();

		public abstract void AppendRow ();

		public abstract void InsertRow (int rowIndex);

		public abstract void DeleteRow (int rowIndex);

		public abstract void ValidateRow (int rowIndex);

		public abstract void ValidateGrid ();

		public abstract void ValidateGrid (Dataelus.UI.GridItemViewer.Generic.IView<D> gridItemViewer);

		/// <summary>
		/// The view object.
		/// </summary>
		protected IView<D> _viewObject;

		/// <summary>
		/// Gets or sets the view object.
		/// </summary>
		/// <value>The view object.</value>
		[Newtonsoft.Json.JsonIgnore]
		public virtual IView<D> ViewObject {
			get { return _viewObject; }
			set { _viewObject = value; }
		}

		/// <summary>
		/// The issue viewer.
		/// </summary>
		protected Dataelus.UI.GridItemViewer.Generic.IView<D> _issueViewer;

		/// <summary>
		/// Gets or sets the view object for the Grid Issue Viewer.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		[Newtonsoft.Json.JsonIgnore]
		public virtual Dataelus.UI.GridItemViewer.Generic.IView<D> IssueViewer {
			get { return _issueViewer; }
			set { _issueViewer = value; }
		}

		#endregion
	}
}

