using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

using Dataelus.UI;
using Dataelus.Mono.Extensions;
using Dataelus.EDD;

namespace Dataelus.Mono
{
	/// <summary>
	/// Editor mode.
	/// </summary>
	public enum EditorMode
	{
		/// <summary>
		/// Function as a database table editor, for a single table at a time.
		/// </summary>
		DatabaseTableEditor,

		/// <summary>
		/// Function as an EDD editor, which is based on a set of database tables.
		/// </summary>
		EDDEditor
	}

	/// <summary>
	/// Column source.
	/// </summary>
	public enum ColumnSource
	{
		/// <summary>
		/// Gets the EDD Fields from the columns in the data object (Ex: the System.Data.DataSet).
		/// </summary>
		DataObjectColumns,

		/// <summary>
		/// Gets the EDD Fields from the EDD field list.
		/// </summary>
		EDDFieldList
	}

	/// <summary>
	/// Base data-saver class - just copies the table to a CSV file
	/// </summary>
	public class SaverBase : Dataelus.UI.GridEditor.IDataSaver<Table.ObjectTable>
	{
		/// <summary>
		/// Gets filename; returns true if successful; returns false to cancel.
		/// </summary>
		public delegate bool GetFilename (out string filename);

		/// <summary>
		/// The filename getter.
		/// </summary>
		protected GetFilename _filenameGetter;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.SaverBase"/> class.
		/// </summary>
		/// <param name="filenameGetter">Delegate for getting a filename.</param>
		public SaverBase (GetFilename filenameGetter)
		{
			_filenameGetter = filenameGetter;
		}

		#region IDataSaver implementation

		/// <summary>
		/// Saves the data.
		/// </summary>
		/// <param name="data">Data.</param>
		public void SaveData (Dataelus.Table.ObjectTable data)
		{
			string filename;
			if (_filenameGetter (out filename))
				System.IO.File.WriteAllText (filename, "#" + data.ToCSVString ());
		}

		#endregion
	}

	/// <summary>
	/// Saves an object table to a database.
	/// </summary>
	public class DatabaseSaver : Dataelus.UI.GridEditor.IDataSaver<Table.ObjectTable>
	{
		/// <summary>
		/// Gets or sets the producer for the table.
		/// </summary>
		/// <value>The producer.</value>
		public Dataelus.EDD.Producer.EDDRecordProducerTable Producer { get; set; }

		/// <summary>
		/// Gets or sets the record producer.
		/// </summary>
		/// <value>The record producer.</value>
		public DatabaseRecordProducerTable RecordProducer { get; set; }

		public DatabaseSaver ()
			: this (new Dataelus.EDD.Producer.EDDRecordProducerTable (), new DatabaseRecordProducerTable ())
		{
		}

		public DatabaseSaver (Dataelus.EDD.Producer.EDDRecordProducerTable producer, DatabaseRecordProducerTable recordProducer)
		{
			this.Producer = producer;
			this.RecordProducer = recordProducer;
		}

		
		#region IDataSaver implementation

		public void SaveData (Dataelus.Table.ObjectTable data)
		{
			this.Producer.Table = data;
			this.Producer.Produce (this.RecordProducer);
		}

		#endregion
	}

	/*
	 * Row-level filtering
	 * For every constraint with more than one column, we should have a filter table
	 * We can then build a filterset for every table
	 */
	/// <summary>
	/// Data editor controller.
	/// </summary>
	public class DataEditorController : Dataelus.UI.GridEditor.ControllerObjectTable, IDataEditorController, IEquatable<DataEditorController>
	{
		/// <summary>
		/// The view object.
		/// </summary>
		protected IDataEditorView _viewObject2;

		#region Variables and Properties

		/// <summary>
		/// The querier.
		/// </summary>
		protected Dataelus.Mono.DBQuerier _querier;

		/// <summary>
		/// Gets or sets the querier.
		/// </summary>
		/// <value>The querier.</value>
		public Dataelus.Mono.DBQuerier Querier {
			get { return _querier; }
			set { _querier = value; }
		}

		/// <summary>
		/// The columns schema DataTable.
		/// </summary>
		protected DataTable _dtColumnsSchema;

		/// <summary>
		/// Gets or sets the columns schema datatable.
		/// </summary>
		/// <value>The dt columns schema.</value>
		[Newtonsoft.Json.JsonIgnore]
		[System.Xml.Serialization.XmlIgnore]
		public DataTable DtColumnsSchema {
			get { return _dtColumnsSchema; }
			set { _dtColumnsSchema = value; }
		}

		/// <summary>
		/// The name of the table for editing
		/// </summary>
		protected string _tableName;

		/// <summary>
		/// Gets or sets the name of the table.
		/// </summary>
		/// <value>The name of the table.</value>
		public string TableName {
			get { return _tableName; }
			set { _tableName = value; }
		}

		/// <summary>
		/// The excluded fields.
		/// </summary>
		protected List<string> _excludedFields;

		/// <summary>
		/// Gets or sets the excluded fields.
		/// </summary>
		/// <value>The excluded fields.</value>
		public List<string> ExcludedFields {
			get { return _excludedFields; }
			set { _excludedFields = value; }
		}

		/// <summary>
		/// The dictionary of column names/display labels.
		/// </summary>
		protected Dictionary<string, string> _columnDisplay;

		/// <summary>
		/// Gets or sets the column display dictionary.
		/// </summary>
		/// <value>The column display.</value>
		public Dictionary<string, string> ColumnDisplay {
			get { return _columnDisplay; }
			set { _columnDisplay = value; }
		}

		/// <summary>
		/// The logger.
		/// </summary>
		protected Dataelus.Log.LogManager _logger;

		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		/// <value>The logger.</value>
		public Dataelus.Log.LogManager Logger {
			get { return _logger; }
			set { _logger = value; }
		}

		/// <summary>
		/// The Database constraints.
		/// </summary>
		protected Database.DBConstraintCollection _constraints = null;

		// This is also stored in the editor; do not need to serialize it
		/// <summary>
		/// Gets or sets the constraints.
		/// </summary>
		/// <value>The constraints.</value>
		[Newtonsoft.Json.JsonIgnore]
		[System.Xml.Serialization.XmlIgnore]
		public Database.DBConstraintCollection Constraints {
			get { return _constraints; }
			set { _constraints = value; }
		}

		/// <summary>
		/// The column schema.
		/// </summary>
		protected Database.DBFieldCollection _columnSchema = null;

		// This is also stored in the editor; do not need to serialize it
		/// <summary>
		/// Gets or sets the column schema.
		/// </summary>
		/// <value>The column schema.</value>
		[Newtonsoft.Json.JsonIgnore]
		[System.Xml.Serialization.XmlIgnore]
		public Database.DBFieldCollection ColumnSchema {
			get { return _columnSchema; }
			set { _columnSchema = value; }
		}

		//		protected Dataelus.EDD.EditorData _editor;
		//
		//		/// <summary>
		//		/// Gets or sets the editor data manager.
		//		/// </summary>
		//		/// <value>The editor.</value>
		//		public Dataelus.EDD.EditorData Editor {
		//			get { return _editor; }
		//			set { _editor = value; }
		//		}

		/// <summary>
		/// The type converter for Database types (for use with the schema).
		/// </summary>
		protected ITypeConverter _dbTypeConverter;

		/// <summary>
		/// Gets or sets the db type converter.
		/// </summary>
		/// <value>The db type converter.</value>
		public ITypeConverter DbTypeConverter {
			get { return _dbTypeConverter; }
			set { _dbTypeConverter = value; }
		}

		/// <summary>
		/// The data to be edited.
		/// </summary>
		protected DataSet _data;

		// This is just the initial form of the data - it is copied to an ObjectTable for use with the controller
		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>The data.</value>
		public DataSet Data {
			get { return _data; }
			set { _data = value; }
		}

		/// <summary>
		/// The constraint column sorter.
		/// </summary>
		protected Dataelus.Database.DBConstraintColumnComparer _constraintColumnSorter;

		/// <summary>
		/// Gets or sets the constraint column sorter.
		/// </summary>
		/// <value>The constraint column sorter.</value>
		public Dataelus.Database.DBConstraintColumnComparer ConstraintColumnSorter {
			get { return _constraintColumnSorter; }
			set { _constraintColumnSorter = value; }
		}

		/// <summary>
		/// The constraint filter tables.
		/// </summary>
		protected Dictionary<Dataelus.Database.DBConstraint, Dataelus.FilterCascade.FilterTextTable> _constraintFilterTables;

		/// <summary>
		/// Gets or sets the constraint filter tables.
		/// </summary>
		/// <value>The constraint filter tables.</value>
		public Dictionary<Dataelus.Database.DBConstraint, Dataelus.FilterCascade.FilterTextTable> ConstraintFilterTables {
			get { return _constraintFilterTables; }
			set { _constraintFilterTables = value; }
		}

		/// <summary>
		/// The constraint filter items.
		/// </summary>
		protected Dictionary<Dataelus.Database.DBConstraint, Dataelus.FilterCascade.FilterTextItemCollection> _constraintFilterItems;

		/// <summary>
		/// Gets or sets the constraint filter items.
		/// </summary>
		/// <value>The constraint filter items.</value>
		public Dictionary<Dataelus.Database.DBConstraint, Dataelus.FilterCascade.FilterTextItemCollection> ConstraintFilterItems {
			get { return _constraintFilterItems; }
			set { _constraintFilterItems = value; }
		}

		/// <summary>
		/// Whether to use row-level filters (for drop-down widgets).
		/// </summary>
		protected bool _useRowLevelFilters = true;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Dataelus.Mono.DataEditorController"/> uses row-level filters (for drop-down widgets).
		/// </summary>
		/// <value><c>true</c> if uses row-level filters; otherwise, <c>false</c>.</value>
		public bool UseRowLevelFilters {
			get { return _useRowLevelFilters; }
			set { _useRowLevelFilters = value; }
		}

		/// <summary>
		/// The database fields where existing values are used as suggestions.
		/// </summary>
		protected List<Dataelus.Database.DBFieldSimple> _suggestionFields;

		/// <summary>
		/// Gets or sets the database fields where existing values are used as suggestions.
		/// </summary>
		/// <value>The suggestion fields.</value>
		public List<Dataelus.Database.DBFieldSimple> SuggestionFields {
			get { return _suggestionFields; }
			set { _suggestionFields = value; }
		}

		/// <summary>
		/// The EDD field names where existing values are used as suggestions.
		/// </summary>
		protected List<string> _suggestionEddFieldNames;

		/// <summary>
		/// Gets or sets the EDD field names where existing values are used as suggestions..
		/// </summary>
		/// <value>The suggestion edd field names.</value>
		public List<string> SuggestionEddFieldNames {
			get { return _suggestionEddFieldNames; }
			set { _suggestionEddFieldNames = value; }
		}

		/// <summary>
		/// The editor mode.
		/// </summary>
		protected Dataelus.Mono.EditorMode _mode;

		/// <summary>
		/// Gets or sets the Editor mode.
		/// </summary>
		/// <value>The mode.</value>
		public Dataelus.Mono.EditorMode Mode {
			get { return _mode; }
			set { _mode = value; }
		}

		/// <summary>
		/// The primary source of the EDD columns.
		/// </summary>
		protected ColumnSource _primaryEddColumnSource;

		/// <summary>
		/// Gets or sets the primary source of the EDD columns.
		/// </summary>
		/// <value>The edd column source.</value>
		public ColumnSource PrimaryEddColumnSource {
			get { return _primaryEddColumnSource; }
			set { _primaryEddColumnSource = value; }
		}

		#endregion

		/// <summary>
		/// Gets the user-defined list of simple constraints.
		/// </summary>
		/// <returns>The simple constraints user defined.</returns>
		public Database.DBConstraintColumnCollection GetSimpleConstraintsUserDefined ()
		{
			return _editorMgr.SimpleConstraintsListUserDefined;
		}

		/// <summary>
		/// Sets the user-defined list of simple constraints.
		/// </summary>
		/// <param name="collection">Collection.</param>
		public void SetSimpleConstraintsUserDefined (Database.DBConstraintColumnCollection collection)
		{
			_editorMgr.SimpleConstraintsListUserDefined = collection;
		}

		#region IEquatable implementation

		/// <summary>
		/// Determines whether the specified <see cref="Dataelus.Mono.DataEditorController"/> is equal to the current <see cref="Dataelus.Mono.DataEditorController"/>.
		/// </summary>
		/// <param name="other">The <see cref="Dataelus.Mono.DataEditorController"/> to compare with the current <see cref="Dataelus.Mono.DataEditorController"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="Dataelus.Mono.DataEditorController"/> is equal to the current
		/// <see cref="Dataelus.Mono.DataEditorController"/>; otherwise, <c>false</c>.</returns>
		public bool Equals (DataEditorController other)
		{
			return Equals (other, new StringEqualityComparer ());
		}

		#endregion

		/// <summary>
		/// Determines whether the specified <see cref="Dataelus.Mono.DataEditorController"/> is equal to the current <see cref="Dataelus.Mono.DataEditorController"/>.
		/// </summary>
		/// <param name="other">The <see cref="Dataelus.Mono.DataEditorController"/> to compare with the current <see cref="Dataelus.Mono.DataEditorController"/>.</param>
		/// <param name="tableNameComparer">The Table Name comparer.</param>
		/// <returns><c>true</c> if the specified <see cref="Dataelus.Mono.DataEditorController"/> is equal to the current
		/// <see cref="Dataelus.Mono.DataEditorController"/>; otherwise, <c>false</c>.</returns>
		public bool Equals (DataEditorController other, IEqualityComparer<string> tableNameComparer)
		{
			return (tableNameComparer.Equals (this.Querier.ConnectionString, other.Querier.ConnectionString)
			&& tableNameComparer.Equals (this.TableName, other.TableName)
			&& base.GridDataObject.Equals (other.GridDataObject));
		}

		#region Methods: JSON Serialization

		/// <summary>
		/// Gets the serialization manager for this object.
		/// </summary>
		/// <returns>The serialization manager.</returns>
		public DataEditorControllerSerializationManager GetSerializationManager ()
		{
			return new DataEditorControllerSerializationManager (this.GetType ());
		}

		/// <summary>
		/// Run this function after deserialization.
		/// </summary>
		/// <param name="view">The View.</param>
		public virtual void RunAfterDeserialization (IDataEditorView view)
		{
			this.ViewObject = view;
			RunAfterDeserialization ();
		}

		/// <summary>
		/// To be run the after deserialization. (The ViewObject must also be set)
		/// </summary>
		public override void RunAfterDeserialization ()
		{
			base.RunAfterDeserialization ();
			_columnSchema = _editorMgr.DatabaseFields;
		}

		#endregion

		#region Constructors

		// Primary constructor - all other constructors should call this one
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DataEditorController"/> class.
		/// </summary>
		public DataEditorController ()
			: base ()
		{
			_columnDisplay = new Dictionary<string, string> (new StringEqualityComparer ());
			_suggestionFields = new List<Dataelus.Database.DBFieldSimple> ();
			_suggestionEddFieldNames = new List<string> ();
			_filterValues = null;
			_suggestionValues = null;
			_initializeOnLoadView = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DataEditorController"/> class.
		/// The following properties must be set: the View object, the Saver, and the Validator
		/// </summary>
		/// <param name="logger">Logger.</param>
		/// <param name="querier">Querier.</param>
		/// <param name="dtColumnsSchema">Columns schema datatable.</param>
		/// <param name="tableName">Table name.</param>
		/// <param name="data">Data.</param>
		/// <param name="excludedFields">Excluded fields.</param>
		public DataEditorController (Dataelus.Log.LogManager logger, Dataelus.Mono.DBQuerier querier, DataTable dtColumnsSchema, string tableName, DataSet data, List<string> excludedFields)
			: this (logger, querier
					, SQLServer.SQLServerDataServices.LoadColumnSchemaSqlServer (dtColumnsSchema)
					, tableName, data, excludedFields)
		{
			//			if (_databaseContraints == null)
			//				_databaseContraints = SQLServer.SQLServerDataServices.LoadConstraintsSqlServer (_querier);
			//
			//			if (_columnSchema == null)
			//				_columnSchema = SQLServer.SQLServerDataServices.LoadColumnSchemaSqlServer (_dtColumnsSchema);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DataEditorController"/> class.
		/// </summary>
		/// <param name="logger">Logger.</param>
		/// <param name="querier">Querier.</param>
		/// <param name="columnSchema">Column schema.</param>
		/// <param name="tableName">Table name.</param>
		/// <param name="data">Data.</param>
		/// <param name="excludedFields">Excluded fields.</param>
		public DataEditorController (Dataelus.Log.LogManager logger, Dataelus.Mono.DBQuerier querier, Database.DBFieldCollection columnSchema, string tableName, DataSet data, List<string> excludedFields)
			: this (logger
				, querier
				, columnSchema
				, SQLServer.SQLServerDataServices.LoadConstraintsSqlServer (querier)
				, tableName, data, excludedFields)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DataEditorController"/> class.
		/// </summary>
		/// <param name="logger">Logger.</param>
		/// <param name="interactor">The Database Interactor.</param>
		/// <param name="schematic">The Database Schematic.</param>
		/// <param name="tableName">Table name.</param>
		/// <param name="data">Data.</param>
		/// <param name="excludedFields">Excluded fields.</param>
		public DataEditorController (Dataelus.Log.LogManager logger
			, SQLServer.SQLServerInteractor interactor
			, Database.DBSchematic schematic
			, string tableName, DataSet data, List<string> excludedFields)
			: this (logger
				, interactor.Querier
				, schematic.ColumnSchema
				, schematic.TableConstraints
				, tableName, data, excludedFields)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DataEditorController"/> class.
		/// </summary>
		/// <param name="logger">Logger.</param>
		/// <param name="querier">Querier.</param>
		/// <param name="columnSchema">Column schema.</param>
		/// <param name="databaseContraints">Database contraints.</param>
		/// <param name="tableName">Table name.</param>
		/// <param name="data">Data.</param>
		/// <param name="excludedFields">Excluded fields.</param>
		public DataEditorController (Dataelus.Log.LogManager logger, Dataelus.Mono.DBQuerier querier, Database.DBFieldCollection columnSchema, Database.DBConstraintCollection databaseContraints, string tableName, DataSet data, List<string> excludedFields)
			: this ()
		{
			_logger = logger;
			_querier = querier;
			_dtColumnsSchema = null;
			_tableName = tableName;
			_excludedFields = excludedFields;
			_data = data;

			_columnSchema = columnSchema;
			_databaseContraints = databaseContraints;

			if (String.IsNullOrWhiteSpace (tableName))
				_mode = EditorMode.EDDEditor;
			else
				_mode = EditorMode.DatabaseTableEditor;

			Init ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DataEditorController"/> class.
		/// </summary>
		/// <param name="logger">Logger.</param>
		/// <param name="querier">Querier.</param>
		/// <param name="columnSchema">Column schema.</param>
		/// <param name="databaseContraints">Database contraints.</param>
		/// <param name="eddFields">Edd fields.</param>
		/// <param name="funcGetTypeString">Func get type string.</param>
		/// <param name="data">Data.</param>
		/// <param name="excludedFields">Excluded fields.</param>
		public DataEditorController (Dataelus.Log.LogManager logger, Dataelus.Mono.DBQuerier querier, Database.DBFieldCollection columnSchema, Database.DBConstraintCollection databaseContraints, IEnumerable<Dataelus.EDD.EDDField> eddFields, Func<Dataelus.EDD.EDDField, string> funcGetTypeString, DataSet data, List<string> excludedFields)
			: this (logger, querier, columnSchema, databaseContraints, null, data, excludedFields)
		{
			_mode = EditorMode.EDDEditor;
			BuildEDDFields (eddFields, funcGetTypeString);
		}

		public DataEditorController (Log.LogManager logger, SQLServer.SQLServerInteractor interactor, Database.DBSchematic schematic, IEnumerable<Dataelus.EDD.EDDField> eddFields, Func<Dataelus.EDD.EDDField, string> funcGetTypeString, DataSet data, List<string> excludedFields)
			: this (logger, interactor.Querier, schematic.ColumnSchema, schematic.TableConstraints, eddFields, funcGetTypeString, data, excludedFields)
		{
		}

		#endregion

		/// <summary>
		/// Inititializes this instance.
		/// </summary>
		protected virtual void Init ()
		{
			_editorMgr = new Dataelus.EDD.EditorData (_columnSchema, _databaseContraints);
			_dbTypeConverter = new Mono.SQLServer.SQLServerDataTypeConverter ();

			_constraintColumnSorter = new Dataelus.Database.DBConstraintColumnComparer (_querier);
			var constraintEqComparer = new Database.DBConstraintEqualityComparer (new StringEqualityComparer ());
			_constraintFilterTables = new Dictionary<Dataelus.Database.DBConstraint, Dataelus.FilterCascade.FilterTextTable> (
				constraintEqComparer
			);
			_constraintFilterItems = new Dictionary<Dataelus.Database.DBConstraint, Dataelus.FilterCascade.FilterTextItemCollection> (
				constraintEqComparer
			);

			_eddFields = null;

			base._dataServices = new Mono.DataServices (_querier);
			// Set in the default base constructor
			//base._eddFieldNameComparer = new StringEqualityComparer ();
			// Set in the Load View method
			//base._eddFields = null;
			// Set in the Load View method
			//base._gridDataObject = null;

			// There must be a DataSet for the JSON Serializer to work
			if (_data == null) {
				_data = new DataSet ();
			}

			// These need to be set at some point
			base._issueViewer = null;
			//base._saver = null;
			base._validator = null;
			base._viewObject = null;
		}

		/// <summary>
		/// Gets the EDD fields; uses the default SQL Server type converter.
		/// </summary>
		/// <returns>The EDD fields.</returns>
		public virtual Dataelus.EDD.EDDFieldCollection GetEDDFields ()
		{
			return GetEDDFields (_dbTypeConverter);
		}

		/// <summary>
		/// Gets the EDD fields for the Table name.
		/// </summary>
		/// <returns>The EDD fields.</returns>
		/// <param name="dbTypeConverter">Type converter.</param>
		public virtual Dataelus.EDD.EDDFieldCollection GetEDDFields (ITypeConverter dbTypeConverter)
		{
			return _editorMgr.GetEddFields (_tableName, _excludedFields, dbTypeConverter);
		}

		/// <summary>
		/// Gets the EDD fields based on the given list.
		/// </summary>
		/// <returns>The EDD fields.</returns>
		/// <param name="fields">Fields.</param>
		/// <param name="funcGetTypeString">Func get type string.</param>
		public virtual Dataelus.EDD.EDDFieldCollection GetEDDFields (IEnumerable<Dataelus.EDD.EDDField> fields, Func<Dataelus.EDD.EDDField, string> funcGetTypeString)
		{
			return Dataelus.EDD.EditorData.GetEDDFields (_dbTypeConverter, fields, funcGetTypeString);
		}

		/// <summary>
		/// Builds the EDD fields based on the given fields.
		/// </summary>
		/// <param name="fields">Fields.</param>
		/// <param name="funcGetTypeString">Func get type string.</param>
		public virtual void BuildEDDFields (IEnumerable<Dataelus.EDD.EDDField> fields, Func<Dataelus.EDD.EDDField, string> funcGetTypeString)
		{
			_eddFields = GetEDDFields (fields, funcGetTypeString);
		}

		/// <summary>
		/// Applies the field constraints to the EDD fields, where the Reference field is Null.
		/// </summary>
		public virtual void ApplyFieldConstraints ()
		{
			_editorMgr.ApplyConstraintsSorted (_eddFields);
		}

		/// <summary>
		/// Applies the field constraints to the EDD fields, where the Reference field is Null.
		/// Uses the given table dependency sorter (should make the most dependent table first).
		/// </summary>
		/// <param name="tableDepenencyComparer">Table depenency comparer (most dependent table first).</param>
		public virtual void ApplyFieldConstraints (IComparer<string> tableDepenencyComparer)
		{
			ApplyFieldConstraints (new Database.DBConstraintPrioritizerComparer (tableDepenencyComparer));
		}

		/// <summary>
		/// Applies the field constraints to the EDD fields, where the Reference field is Null.
		/// Uses the given constraint sorter (should make the preferred constraint first).
		/// </summary>
		/// <param name="constraintSorter">Constraint sorter, which makes the best constraint first.</param>
		public virtual void ApplyFieldConstraints (IComparer<Database.DBConstraint> constraintSorter)
		{
			_editorMgr.ApplyConstraintsSorted (_eddFields, constraintSorter);
		}

		/// <summary>
		/// Called when the value of the given row index is changed, for the given column name.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnName">Column name.</param>
		/// <param name="newValue">New value of the given cell.</param>
		public override void ValueChanged (int rowIndex, string columnName, object newValue)
		{
			base.ValueChanged (rowIndex, columnName, newValue);
			LoadRowFiltersAll (rowIndex, columnName, newValue);
		}

		/// <summary>
		/// Called when the value of the given row index is changed, for the given column name.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnName">Column name.</param>
		/// <param name="newValue">New value of the given cell.</param>
		public override void ValueChanged (int rowIndex, string columnName, string newValue)
		{
			base.ValueChanged (rowIndex, columnName, newValue);
			LoadRowFiltersAll (rowIndex, columnName, newValue);
		}

		/// <summary>
		/// Gets the widget data.
		/// </summary>
		/// <returns>The widget data.</returns>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="oConstraint">O constraint.</param>
		/// <param name="cols">Cols.</param>
		protected virtual Dataelus.FilterCascade.WidgetData.FilterSelectableTextValuesCache GetWidgetData (int rowIndex, Dataelus.Database.DBConstraint oConstraint, Dataelus.Database.DBConstraintColumnCollection cols)
		{
			Dataelus.FilterCascade.FilterTextTable filterTable;
			if (!_constraintFilterTables.TryGetValue (oConstraint, out filterTable)) {
				// Download the table, using the given SQL statement
				filterTable = new Dataelus.FilterCascade.FilterTextTable (_dataServices.GetTextTable (cols.GetSqlReferenceTable ()));
				_constraintFilterTables.Add (oConstraint, filterTable);
			}
			Dataelus.FilterCascade.FilterTextItemCollection filters;
			if (!_constraintFilterItems.TryGetValue (oConstraint, out filters)) {
				filters = new Dataelus.FilterCascade.FilterTextItemCollection ();
				string codePrior = null;
				for (int i = 0; i < cols.Count; i++) {
					var refCol = cols [i].ReferencedColumn;
					// Use the column field name as the code
					string newCode = cols [i].Column.FieldName;
					if (i == 0) {
						filters.Add (refCol.FieldName, newCode);
					} else {
						filters.Add (filters.CreateNew (refCol.FieldName, newCode, new string[] {
									codePrior
								}));
					}
					codePrior = newCode;
				}

				//filters.Add ("facility_id", "facility_code", "codeFacility");
				//filters.Add (filters.CreateNew ("loc_type", "codeLocType", new string[] { "codeFacility" }));
				//filters.Add (filters.CreateNew ("sys_loc_code", "codeLocationCode", new string[] { "codeLocType" }));

				_constraintFilterItems.Add (oConstraint, filters);
			}

			// Load the filter values from the table
			LoadFilters (rowIndex, filters);

			var widgetData = new Dataelus.FilterCascade.WidgetData.FilterSelectableTextValuesCache ();
			widgetData.Load (filters, filterTable);
			return widgetData;
		}

		/// <summary>
		/// Loads the filters from the Object Table.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="filters">Filters.</param>
		protected virtual void LoadFilters (int rowIndex, Dataelus.FilterCascade.FilterTextItemCollection filters)
		{
			for (int i = 0; i < filters.Count; i++) {
				filters [i].SelectedValues.Clear ();
				filters [i].SelectedValues.Add (NullableString (_gridDataObject [rowIndex, filters [i].ColumnName]));
			}
		}

		/// <summary>
		/// Gets a Nullable string.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="value">Value.</param>
		protected string NullableString (object value)
		{
			return (value == null) ? String.Empty : value.ToString ();
		}

		/// <summary>
		/// Gets the suggestion values.
		/// </summary>
		/// <returns>The suggestion values.</returns>
		/// <param name="eddFields">Edd fields.</param>
		protected virtual Dataelus.FilterCascade.WidgetData.TextValuesCache GetSuggestionValues (Dataelus.EDD.EDDFieldCollection eddFields)
		{
			FilterCascade.WidgetData.TextValuesCache suggestionValues = null;

			if (_suggestionEddFieldNames.Count > 0) {
				var eddFieldsSuggest = eddFields.Where (x => _suggestionEddFieldNames.Contains (x.EDDColumnName, new StringEqualityComparer ())).ToList ();
				suggestionValues = FilterCascade.WidgetData.TextValuesCache.GetValuesCache (eddFieldsSuggest, _dataServices);
			} else if (_suggestionFields.Count > 0) {
				suggestionValues = FilterCascade.WidgetData.TextValuesCache.GetValuesCache (_suggestionFields.Cast<Dataelus.Database.IDBFieldSimple> ().ToArray (), _dataServices, null);
			}

			return suggestionValues;
		}

		/// <summary>
		/// Loads new data into the control, assuming the same format.
		/// </summary>
		/// <param name="ds">Ds.</param>
		public void LoadNewData (System.Data.DataSet ds)
		{
			_data = ds;
			LoadGridData ();
		}

		/// <summary>
		/// Builds the ObjectTable from the DataSet parameter.
		/// </summary>
		/// <returns>The table.</returns>
		/// <param name="eddFields">Edd fields.</param>
		protected virtual Dataelus.Table.ObjectTable BuildTable (Dataelus.EDD.EDDFieldCollection eddFields)
		{
			Dataelus.Table.ObjectTable table = null;

			switch (_primaryEddColumnSource) {
				case ColumnSource.DataObjectColumns:
				// Start with the columns of the Data Object
					table = Mono.DataServices.GetObjectTable (_data);
					break;
				case ColumnSource.EDDFieldList:
				// Start with the list of EDD Fields
					var columns = EditorData.GetObjectColumns (eddFields, _dbTypeConverter);
					table = new Dataelus.Table.ObjectTable (columns, new Dataelus.Table.ObjectRowCollection ());

				// Add in the data
				//var ds = _querier.GetDs ("SELECT TOP 10 * FROM " + _tableName);
					if (_data != null && _data.Tables.Count > 0)
						table.AddTableRecords (Mono.DataServices.GetObjectTable (_data), this.Logger);
					break;
				default:
					break;
			}

			return table;
		}

		protected Dataelus.FilterCascade.WidgetData.FilterSelectableTextValuesCache _filterValues;

		// Note: these are for performance only, when using a desktop application
		/// <summary>
		/// Gets or sets the filter values.
		/// </summary>
		/// <value>The filter values.</value>
		[Newtonsoft.Json.JsonIgnore]
		[System.Xml.Serialization.XmlIgnore]
		public Dataelus.FilterCascade.WidgetData.FilterSelectableTextValuesCache FilterValues {
			get { return _filterValues; }
			set { _filterValues = value; }
		}

		protected Dataelus.FilterCascade.WidgetData.TextValuesCache _suggestionValues;

		// Note: these are for performance only, when using a desktop application
		/// <summary>
		/// Gets or sets the suggestion values.
		/// </summary>
		/// <value>The suggestion values.</value>
		[Newtonsoft.Json.JsonIgnore]
		[System.Xml.Serialization.XmlIgnore]
		public Dataelus.FilterCascade.WidgetData.TextValuesCache SuggestionValues {
			get { return _suggestionValues; }
			set { _suggestionValues = value; }
		}

		void LoadGridData ()
		{
			_gridDataObject = BuildTable (_eddFields);
			_gridDataObject.StartEditTracking ();
		}

		/// <summary>
		/// Intitialize this instance, including EDD fields and Filter values.
		/// </summary>
		public virtual void IntitializeData ()
		{
			if (_eddFields == null) {
				_eddFields = GetEDDFields ();
			}

			LoadGridData ();

			// Load the filters (if needed)
			if (_filterValues == null)
				_filterValues = FilterCascade.WidgetData.FilterSelectableTextValuesCache.GetValuesCache (_eddFields, _dataServices);
			//ed.GetOutputForEditor (tableName, excludedFields, out columns, out filterValues);

			// Load the Suggestion values (if needed)
			if (_suggestionValues == null)
				_suggestionValues = GetSuggestionValues (_eddFields);
		}

		protected bool _initializeOnLoadView;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Dataelus.Mono.DataEditorController"/> initialize on load view.
		/// </summary>
		/// <value><c>true</c> if initialize on load view; otherwise, <c>false</c>.</value>
		public bool InitializeOnLoadView {
			get { return _initializeOnLoadView; }
			set { _initializeOnLoadView = value; }
		}

		/// <summary>
		/// Determines whether any changes have been made.
		/// </summary>
		/// <returns><c>true</c> if this instance is any changes; otherwise, <c>false</c>.</returns>
		public bool IsAnyChanges ()
		{
			return _gridDataObject.IsEdits ();
		}

		#region IDataEditorController implementation

		/// <summary>
		/// Loads the view.
		/// </summary>
		public override void LoadView ()
		{
			if (_initializeOnLoadView)
				IntitializeData ();

			var table = _gridDataObject;

			_viewObject2.LoadForm (table.Columns, _columnDisplay, _filterValues, _suggestionValues, table);

			if (_useRowLevelFilters) {
				// Load the row filters
				LoadRowFiltersAll (table);
			}
		}

		/// <summary>
		/// Deletes the row by the given unique object ID
		/// </summary>
		/// <param name="objectUniqueID">The Unique ID for the Object.</param>
		public void DeleteRowByID (long objectUniqueID)
		{
			int index = _gridDataObject.Rows.FindIndex (objectUniqueID);
			if (index < 0)
				return;
			DeleteRow (index);
		}

		/// <summary>
		/// Gets or sets the view object.
		/// </summary>
		/// <value>The view object.</value>
		[Newtonsoft.Json.JsonIgnore]
		[System.Xml.Serialization.XmlIgnore]
		public new IDataEditorView ViewObject {
			get { return _viewObject2; }
			set {
				_viewObject2 = value;
				base._viewObject = value;
			}
		}

		#endregion


		/// <summary>
		/// Loads the all of the row filters for the given row index.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnName">Column name.</param>
		/// <param name="newValue">New value.</param>
		protected virtual void LoadRowFiltersAll (int rowIndex, string columnName, object newValue)
		{
			// Exit unless using row-level filters
			if (!_useRowLevelFilters)
				return;

			string table = _tableName;
			string field = columnName;

			Database.DBConstraint[] constraints;
			if (_databaseContraints.GetConstraintsMultiField (table, field, out constraints)) {
				if (constraints.Length >= 1) {

					if (constraints.Length > 1) {
						// Not sure what to do here
						// Do we need to link all of these cascades together?
					}

					var oConstraint = constraints [0];
					var cols = oConstraint.ConstraintColumns;
					cols.Prioritize (_constraintColumnSorter, true);

					var widgetData = GetWidgetData (rowIndex, oConstraint, cols);

					_viewObject.ReloadRowFilters (rowIndex, cols.GetColumnFields ().ToArray (), widgetData);
				}
			}
		}

		/// <summary>
		/// Loads all row filters.
		/// </summary>
		/// <param name="table">Table.</param>
		protected void LoadRowFiltersAll (Dataelus.Table.ObjectTable table)
		{
			if (table == null)
				throw new ArgumentNullException ("table");
			
			//  For each multi-column constraint...
			//     ViewObject.ReloadRowFilters(...)
			Dataelus.Database.DBConstraint[] constraints;
			if (_databaseContraints.GetConstraintsMultiField (_tableName, out constraints)) {

				foreach (Dataelus.Database.DBConstraint constraint in constraints) {
					var cols = constraint.ConstraintColumns;
					cols.Prioritize (_constraintColumnSorter, true);

					// Get the column fields
					var columnFields = cols.GetColumnFields ().ToArray ();

					// Apply this constraint to every row
					for (int rowIndex = 0; rowIndex < table.RowCount; rowIndex++) {
						var widgetData = GetWidgetData (rowIndex, constraint, cols);
						_viewObject.ReloadRowFilters (rowIndex, columnFields, widgetData);
					}
				}
			}
		}
	}

	/// <summary>
	/// Interface for a JSON serialization manager.
	/// </summary>
	public interface IJsonSerializationManager<T>
	{
		/// <summary>
		/// Gets or sets the type of the controller.
		/// </summary>
		/// <value>The type of the controller.</value>
		Type ControllerType {
			get;
			set;
		}

		/// <summary>
		/// Gets the JSON string from the given controller object
		/// </summary>
		/// <returns>The JSON.</returns>
		/// <param name="value">The object to serialize to JSON.</param>
		/// <param name="isIndented">If set to <c>true</c> is indented.</param>
		string ToJSON (T value, bool isIndented);

		/// <summary>
		/// Gets the de-serialized object from the JSON text.
		/// </summary>
		/// <returns>The de-serialized object.</returns>
		/// <param name="jsonText">JSON string.</param>
		T FromJSON (string jsonText);
	}

	/// <summary>
	/// Data editor controller serializer.
	/// </summary>
	[Serializable]
	public class DataEditorControllerSerializationManager
	{
		/// <summary>
		/// Gets or sets the type of the controller.
		/// </summary>
		/// <value>The type of the controller.</value>
		public Type ControllerType {
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DataEditorControllerSerializer"/> class.
		/// </summary>
		public DataEditorControllerSerializationManager ()
		{
			ControllerType = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DataEditorControllerSerializer"/> class.
		/// </summary>
		/// <param name="controllerType">Controller type.</param>
		public DataEditorControllerSerializationManager (Type controllerType)
			: this ()
		{
			ControllerType = controllerType;
		}

		/// <summary>
		/// Gets the settings.
		/// </summary>
		/// <returns>The settings.</returns>
		static Newtonsoft.Json.JsonSerializerSettings GetSettings ()
		{
			// Approach: list the object type
			// Source: http://stackoverflow.com/questions/2254872/using-json-net-converters-to-deserialize-properties

			// Alternate approach, with specific type converters:
			//  http://stackoverflow.com/questions/24644464/json-net-type-is-an-interface-or-abstract-class-and-cannot-be-instantiated
			var settings = new Newtonsoft.Json.JsonSerializerSettings ();
			settings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects;
			settings.Converters.Add (new Dataelus.Database.DBFieldSimpleJsonConverter ());
			settings.Converters.Add (new Dataelus.JSON.GenericDictionaryJsonConverter<Dataelus.Database.DBFieldSimple, int> ());
			settings.Converters.Add (new Dataelus.JSON.GenericDictionaryJsonConverter<Dataelus.Database.DBConstraint, Dataelus.FilterCascade.FilterTextTable> ());
			settings.Converters.Add (new Dataelus.JSON.GenericDictionaryJsonConverter<Dataelus.Database.DBConstraint, Dataelus.FilterCascade.FilterTextItemCollection> ());
			return settings;
		}


		/// <summary>
		/// Gets the JSON string from the given controller object
		/// </summary>
		/// <returns>The JSON.</returns>
		/// <param name="controllerObject">Controller object.</param>
		/// <param name="isIndented">If set to <c>true</c> is indented.</param>
		public string ToJSON (IDataEditorController controllerObject, bool isIndented)
		{
			var settings = GetSettings ();

			var formatting = Newtonsoft.Json.Formatting.None;
			if (isIndented)
				formatting = Newtonsoft.Json.Formatting.Indented;

			ControllerType = controllerObject.GetType ();
			
			string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject (controllerObject, formatting, settings);

			return jsonText;
		}

		/// <summary>
		/// Gets the Controller from the JSON text.
		/// </summary>
		/// <returns>The JSON.</returns>
		/// <param name="jsonText">JSON string.</param>
		/// <param name="viewObject">The view object (must be re-set after Deserialization)</param>
		public IDataEditorController FromJSON (string jsonText, IDataEditorView viewObject)
		{
			return FromJSON (jsonText, ControllerType, viewObject);
		}

		/// <summary>
		/// Gets the Controller from the JSON text.
		/// </summary>
		/// <returns>The JSON.</returns>
		/// <param name="jsonText">JSON string.</param>
		/// <param name="controllerType">Controller type.</param>
		/// <param name="viewObject">The view object (must be re-set after Deserialization)</param>
		public IDataEditorController FromJSON (string jsonText, Type controllerType, IDataEditorView viewObject)
		{
			var settings = GetSettings ();

			var controllerObject = (Dataelus.UI.IDataEditorController)Newtonsoft.Json.JsonConvert.DeserializeObject (jsonText, controllerType, settings);
			controllerObject.RunAfterDeserialization (viewObject);

			return controllerObject;
		}
	}

	/// <summary>
	/// Data editor controller serialization data.
	/// </summary>
	[Serializable]
	public class DataEditorControllerSerializationData
	{
		/// <summary>
		/// Gets or sets the querier.
		/// </summary>
		/// <value>The querier.</value>
		public Dataelus.Mono.DBQuerier Querier {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the name of the table.
		/// </summary>
		/// <value>The name of the table.</value>
		public string TableName {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the excluded fields.
		/// </summary>
		/// <value>The excluded fields.</value>
		public List<string> ExcludedFields {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the column display dictionary.
		/// </summary>
		/// <value>The column display.</value>
		public Dictionary<string, string> ColumnDisplay {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		/// <value>The logger.</value>
		public Dataelus.Log.LogManager Logger {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the constraints.
		/// </summary>
		/// <value>The constraints.</value>
		public Database.DBConstraintCollection Constraints {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the column schema.
		/// </summary>
		/// <value>The column schema.</value>
		public Database.DBFieldCollection ColumnSchema {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the editor data manager.
		/// </summary>
		/// <value>The editor.</value>
		public Dataelus.EDD.EditorData Editor {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the db type converter.
		/// </summary>
		/// <value>The db type converter.</value>
		public ITypeConverter DbTypeConverter {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>The data.</value>
		[Newtonsoft.Json.JsonIgnore]
		[System.Xml.Serialization.XmlIgnore]
		public DataSet Data {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the constraint column sorter.
		/// </summary>
		/// <value>The constraint column sorter.</value>
		public Dataelus.Database.DBConstraintColumnComparer ConstraintColumnSorter {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the constraint filter tables.
		/// </summary>
		/// <value>The constraint filter tables.</value>
		public Dictionary<Dataelus.Database.DBConstraint, Dataelus.FilterCascade.FilterTextTable> ConstraintFilterTables {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the constraint filter items.
		/// </summary>
		/// <value>The constraint filter items.</value>
		public Dictionary<Dataelus.Database.DBConstraint, Dataelus.FilterCascade.FilterTextItemCollection> ConstraintFilterItems {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Dataelus.Mono.DataEditorController"/> uses row-level filters.
		/// </summary>
		/// <value><c>true</c> if uses row-level filters; otherwise, <c>false</c>.</value>
		public bool UseRowLevelFilters {
			get;
			set;
		}

		public DataEditorControllerSerializationData ()
		{
			this.ColumnDisplay = new Dictionary<string, string> (new StringEqualityComparer ());
			this.ColumnSchema = new Dataelus.Database.DBFieldCollection ();
			this.ConstraintColumnSorter = new Dataelus.Database.DBConstraintColumnComparer ();

			var constraintComparer = new Dataelus.Database.DBConstraintEqualityComparer (new StringEqualityComparer ());
			this.ConstraintFilterItems = new Dictionary<Dataelus.Database.DBConstraint, Dataelus.FilterCascade.FilterTextItemCollection> (constraintComparer);
			this.ConstraintFilterTables = new Dictionary<Dataelus.Database.DBConstraint, Dataelus.FilterCascade.FilterTextTable> (constraintComparer);
			this.Constraints = new Dataelus.Database.DBConstraintCollection ();
			this.Data = new DataSet ();
			this.Editor = new Dataelus.EDD.EditorData ();
			this.ExcludedFields = new List<string> ();
			this.Logger = new Dataelus.Log.LogManager ();
			this.Querier = new DBQuerier ();
		}
	}
}

