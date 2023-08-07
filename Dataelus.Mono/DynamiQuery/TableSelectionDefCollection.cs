using System;
using System.Collections.Generic;

namespace Dataelus.Mono.DynamiQuery
{
	/// <summary>
	/// Table selection definitions collection.
	/// </summary>
	[Serializable]
	public class TableSelectionDefCollection : ListBase<TableSelectionDef>
	{
		/// <summary>
		/// The identifier manager.
		/// </summary>
		protected UniqueIdentifierManager _idMgr;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DynamiQuery.TableSelectionDefCollection"/> class.
		/// </summary>
		public TableSelectionDefCollection ()
			: base ()
		{
			_idMgr = new UniqueIdentifierManager ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DynamiQuery.TableSelectionDefCollection"/> class.
		/// </summary>
		/// <param name="collection">Collection.</param>
		public TableSelectionDefCollection (IEnumerable<TableSelectionDef> collection)
			: base (collection)
		{
			_idMgr = new UniqueIdentifierManager ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DynamiQuery.TableSelectionDefCollection"/> class; copying the values from the given other instance.
		/// </summary>
		/// <param name="other">Other instance, from which to copy.</param>
		public TableSelectionDefCollection (TableSelectionDefCollection other)
			: this ()
		{
			foreach (var item in other) {
				Add (new TableSelectionDef (item));
			}
		}

		/// <Docs>The item to add to the current collection.</Docs>
		/// <para>Adds an item to the current collection.</para>
		/// <remarks>To be added.</remarks>
		/// <exception cref="System.NotSupportedException">The current collection is read-only.</exception>
		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public override void Add (TableSelectionDef item)
		{
			item.ObjectUniqueID = _idMgr.GetUniqueID ();
			base.Add (item);
		}

		/// <summary>
		/// Insert the item a the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="item">Item.</param>
		public override void Insert (int index, TableSelectionDef item)
		{
			item.ObjectUniqueID = _idMgr.GetUniqueID ();
			base.Insert (index, item);
		}

		/// <summary>
		/// Sort the items according to Display Order.
		/// </summary>
		public void Sort ()
		{
			_items.Sort (new TableSelectionDefComparer ());
		}

		/// <summary>
		/// Finds the index.
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="objectID">The unique Object ID.</param>
		public int FindIndex (long objectID)
		{
			return FindIndex (x => x.ObjectUniqueID == objectID);
		}

		/// <summary>
		/// Find the specified item.
		/// </summary>
		/// <param name="objectID">The unique Object ID.</param>
		public TableSelectionDef Find (long objectID)
		{
			return Find (x => x.ObjectUniqueID == objectID);
		}

		/// <summary>
		/// Update the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Update (TableSelectionDef item)
		{
			int index = FindIndex (item.ObjectUniqueID);
			if (index < 0)
				Add (item);
			else
				_items [index].CopyFrom (item);
		}

		public TableSelectionDefCollection Clone ()
		{
			return new TableSelectionDefCollection (this);
		}

		#region Not necessarily belonging to this class

		/// <summary>
		/// Gets the ObjectTable for the TableSelectionDef at the given index.
		/// </summary>
		/// <returns>The object table.</returns>
		/// <param name="index">Index of this collection.</param>
		/// <param name="editor">Data Editor.</param>
		/// <param name="typeConverter">Type converter.</param>
		public Table.ObjectTable GetObjectTable (int index, Dataelus.EDD.EditorData editor, ITypeConverter typeConverter)
		{
			var tbldef = _items [index];
			var fields = editor.GetEddFieldsInclude (tbldef.DBTableName, tbldef.DBFieldNames, typeConverter);
			var tbl = Dataelus.EDD.EditorData.GetObjectTable (fields, typeConverter);

			return tbl;
		}

		/// <summary>
		/// Gets the ObjectTable for the TableSelectionDef at the given index; loads it from the database using the given querier.
		/// </summary>
		/// <returns>The object table loaded.</returns>
		/// <param name="index">Index of this collection.</param>
		/// <param name="editor">Data Editor.</param>
		/// <param name="typeConverter">Type converter.</param>
		/// <param name="querier">Database Querier.</param>
		public Table.ObjectTable GetObjectTableLoaded (int index, Dataelus.EDD.EditorData editor, ITypeConverter typeConverter, DBQuerier querier)
		{
			var tbl = GetObjectTable (index, editor, typeConverter);

			LoadTable (index, querier, tbl);

			return tbl;
		}

		/// <summary>
		/// Loads the given table.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="querier">Querier.</param>
		/// <param name="tbl">Tbl.</param>
		public void LoadTable (int index, DBQuerier querier, Dataelus.Table.ObjectTable tbl)
		{
			var tbldef = _items [index];
			string sql = new Dataelus.Database.SQL.SQLBuilder ().BuildSelectSingleTable (tbldef.DBTableName, tbldef.DBFieldNames);

			var ds = querier.GetDs (sql);
			LoadTable (tbl, ds, tbldef.DBFieldNames);

		}

		/// <summary>
		/// Loads the table.
		/// </summary>
		/// <param name="tbl">Tbl.</param>
		/// <param name="ds">Ds.</param>
		/// <param name="fieldNames">Field names.</param>
		public static void LoadTable (Dataelus.Table.ObjectTable tbl, System.Data.DataSet ds, List<string> fieldNames)
		{
			var dt = ds.Tables [0];
			foreach (System.Data.DataRow dr in dt.Rows) {
				var trow = tbl.CreateRow ();
				foreach (var fieldName in fieldNames) {
					trow [fieldName] = DBQuerier.GetNullable (dr [fieldName]);
				}
				tbl.AddRow (trow);
			}
		}

		#endregion
	}
}

