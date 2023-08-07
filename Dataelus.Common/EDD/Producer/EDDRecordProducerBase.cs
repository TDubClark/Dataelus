using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.EDD.Producer
{
	/// <summary>
	/// EDD record producer to a production database table.
	/// </summary>
	public class EDDRecordProducerBase
	{
		/// <summary>
		/// Gets or sets the record validation rules.
		/// </summary>
		/// <value>The record validation rules.</value>
		public List<IEDDRecordValidationRule> RecordValidationRules { get; set; }

		/// <summary>
		/// Gets or sets the primary keys.
		/// </summary>
		/// <value>The primary keys.</value>
		public Database.DBPrimaryKeyCollection PrimaryKeys { get; set; }

		public EDDRecordProducerBase ()
			: this (new List<IEDDRecordValidationRule> (), new Dataelus.Database.DBPrimaryKeyCollection ())
		{
		}

		public EDDRecordProducerBase (Dataelus.Database.DBPrimaryKeyCollection primaryKeys)
			: this (new List<IEDDRecordValidationRule> (), primaryKeys)
		{
		}

		public EDDRecordProducerBase (List<IEDDRecordValidationRule> recordValidationRules, Dataelus.Database.DBPrimaryKeyCollection primaryKeys)
		{
			this.RecordValidationRules = recordValidationRules;
			this.PrimaryKeys = primaryKeys;
		}
	}

	public class EDDRecordProducerDict : EDDRecordProducerBase
	{
		public List<Dictionary<string, object>> ValueList { get; set; }

		public List<Dictionary<string, object>> DeletedValueList { get; set; }

		protected bool IsValid (Dictionary<string, object> record, out string message)
		{
			var resultList = new TableValidationResultCollection ();
			foreach (var item in RecordValidationRules) {
				//resultList.Add(new TableValidationResult(item.Validate(/* build the Record Value list */)))
			}
			message = null;
			return true;
		}

		public EDDRecordProducerDict ()
			: base ()
		{
			this.ValueList = new List<Dictionary<string, object>> ();
		}

		public void Produce (IDatabaseRecordProducerDict recordProducer)
		{
			foreach (var record in ValueList) {
				if (recordProducer.DoesRecordExist (record)) {
					if (recordProducer.DoesRecordNeedUpdated (record)) {
						recordProducer.UpdateRecord (record);
					}
				} else {
					recordProducer.InsertRecord (record);
				}
			}
			foreach (var record in DeletedValueList) {
				if (recordProducer.DoesRecordExist (record)) {
					recordProducer.DeleteRecord (record);
				}
			}
		}
	}

	/// <summary>
	/// EDD record producer for an ObjectTable.
	/// </summary>
	public class EDDRecordProducerTable : EDDRecordProducerBase
	{
		public EDDRecordProducerTable ()
			: base ()
		{
			_table = null;
		}

		public EDDRecordProducerTable (Dataelus.Database.DBPrimaryKeyCollection primaryKeys)
			: base (primaryKeys)
		{
			_table = null;
		}

		public EDDRecordProducerTable (List<IEDDRecordValidationRule> recordValidationRules, Dataelus.Database.DBPrimaryKeyCollection primaryKeys)
			: base (recordValidationRules, primaryKeys)
		{
			_table = null;
		}


		protected Table.ObjectTable _table;

		public Table.ObjectTable Table {
			get { return _table; }
			set { _table = value; }
		}

		protected bool IsValid (Table.ObjectRow record, out string message)
		{
			return IsValid (_table, _table.Rows.FindIndex (record.ObjectUniqueID), out message);
		}

		protected virtual List<ITableValidationResult> Validate (Dataelus.Table.RecordValueCollection recordValueCollection)
		{
			// Apply each rule to the record
			return RecordValidationRules.Select (rule => rule.Validate (recordValueCollection)).ToList ();
		}

		protected bool IsValid (Table.ObjectTable table, int rowIndex, out string message)
		{
			var recordValueCollection = table.GetRecordValues (rowIndex);

			var resultList = Validate (recordValueCollection);

			if (resultList.Any (x => !x.Valid)) {
				message = resultList.First (x => !x.Valid).Message;
				return false;
			}
			message = null;
			return true;
		}

		public TableValidationResultCollection Validate (Table.ObjectTable table)
		{
			var result = new TableValidationResultCollection ();

			for (int row = 0; row < table.RowCount; row++) {
				result.AddRange (Validate (table.GetRecordValues (row)));
			}

			return result;
		}

		public void Produce (IDatabaseRecordProducer<Table.ObjectRow> producer)
		{
			for (int i = 0, _tableRowsCount = _table.Rows.Count; i < _tableRowsCount; i++) {
				var row = _table.Rows [i];

				switch (row.EditState) {
					case Dataelus.Table.RowEditState.Inserted:
						if (!producer.DoesRecordExist (row)) {
							producer.InsertRecord (row);
						}
						break;
					case Dataelus.Table.RowEditState.Updated:
						if (producer.DoesRecordNeedUpdated (row)) {
							producer.UpdateRecord (row);
						}
						break;
					case Dataelus.Table.RowEditState.Deleted:
						if (producer.DoesRecordExist (row)) {
							producer.DeleteRecord (row);
						}
						break;
					case Dataelus.Table.RowEditState.Unchanged:
					// Skip this row
						continue;
					case Dataelus.Table.RowEditState.Undefined:
					// Do nothing
						break;
					default:
						throw new ArgumentOutOfRangeException ();
				}
			}
			for (int i = 0, _tableRowsDeletedCount = _table.RowsDeleted.Count; i < _tableRowsDeletedCount; i++) {
				var row = _table.RowsDeleted [i];
				if (producer.DoesRecordExist (row)) {
					producer.DeleteRecord (row);
				}
			}
		}
	}

	/// <summary>
	/// Interface for a database record editor.
	/// </summary>
	public interface IDatabaseRecordProducer<R>
	{
		bool InsertRecord (R record);

		bool UpdateRecord (R record);

		bool DeleteRecord (R record);

		bool DoesRecordExist (R record);

		bool DoesRecordNeedUpdated (R record);
	}

	/// <summary>
	/// Interface for a database record editor.
	/// </summary>
	public interface IDatabaseRecordProducerDict : IDatabaseRecordProducer<Dictionary<string, object>>
	{
	}

	/// <summary>
	/// Interface for a database record editor.
	/// </summary>
	public interface IDatabaseRecordProducerTable : IDatabaseRecordProducer<Table.ObjectRow>
	{
	}
}

