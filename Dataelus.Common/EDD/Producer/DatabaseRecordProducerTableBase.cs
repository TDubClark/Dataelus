using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.EDD.Producer
{
	public abstract class DatabaseRecordProducerTableBase : IDatabaseRecordProducerTable
	{
		public Database.DBFieldCollection ColumnSchema { get; set; }

		public Database.DBPrimaryKeyCollection PrimaryKeys { get; set; }

		public EDDFieldCollection EddFields { get; set; }

		public string TableName { get; set; }

		public List<Database.DBFieldSimple> IdentityFields { get; set; }

		protected DatabaseRecordProducerTableBase ()
		{
		}

		protected DatabaseRecordProducerTableBase (
			Dataelus.Database.DBFieldCollection columnSchema
			, Dataelus.Database.DBPrimaryKeyCollection primaryKeys
			, List<Dataelus.Database.DBFieldSimple> identityFields
			, EDDFieldCollection eddFields
			, string tableName
		)
			: this ()
		{
			this.ColumnSchema = columnSchema;
			this.PrimaryKeys = primaryKeys;
			this.EddFields = eddFields;
			this.TableName = tableName;
			this.IdentityFields = identityFields;
		}


		protected Database.DBPrimaryKey GetPrimaryKey ()
		{
			var list = this.PrimaryKeys.GetPrimaryKeys (this.TableName);

			if (list.Count > 1) {
				throw new Exception (String.Format ("Table '{0}' has {1:d} primary key constraints.", this.TableName, list.Count));
			} else if (list.Count == 1) {
				return list [0];
			}

			return null;
		}

		protected List<string> GetIdentityFieldNamesByTable ()
		{
			return GetIdentityFieldsByTable ().Select (x => x.FieldName).ToList ();
		}

		protected List<Database.DBFieldSimple> GetIdentityFieldsByTable ()
		{
			return GetIdentityFieldsByTable (this.TableName);
		}

		protected List<Database.DBFieldSimple> GetIdentityFieldsByTable (string tablename)
		{
			var comparer = new StringEqualityComparer ();
			return this.IdentityFields.Where (x => comparer.Equals (x.TableName, tablename)).ToList ();
		}

		protected List<string> GetPrimaryKeyFields ()
		{
			return GetPrimaryKey ().GetFieldNames ();
		}

		/// <summary>
		/// Gets the primary key fields which are not Identity columns (i.e. auto-number).
		/// Best for inserting.
		/// </summary>
		/// <returns>The primary key non identity fields.</returns>
		protected List<string> GetPrimaryKeyNonIdentityFields ()
		{
			var pk = GetPrimaryKey ();
			var identCols = GetIdentityFieldNamesByTable ();
			var comparer = new StringEqualityComparer ();
			return pk.GetFieldNames ()
				.Where (x => !identCols.Contains (x, comparer))
				.ToList ();
		}

		#region Abstract Members

		protected abstract bool InsertRecordCommand (Dataelus.Database.SQL.SQLCommandParameterized cmdParam, Dictionary<string, object> insertValues);

		protected abstract bool UpdateRecordCommand (Dataelus.Database.SQL.SQLCommandParameterized cmdParamValue, Dataelus.Database.SQL.SQLCommandParameterized cmdParamCondition, Dictionary<string, object> conditions, Dictionary<string, object> updateValues);

		protected abstract bool DeleteRecordCommand (Dataelus.Database.SQL.SQLCommandParameterized cmdParamCondition, Dictionary<string, object> conditions);

		protected abstract int GetRecordCountCommand (Dataelus.Database.SQL.SQLCommandParameterized cmdParamCondition, Dictionary<string, object> conditions);

		#endregion

		#region IDatabaseRecordProducer implementation

		public bool InsertRecord (Dataelus.Table.ObjectRow record)
		{
			// Assumes that the ObjectTable column names are the same as the upload database table field names
			var pkFields = GetPrimaryKeyFields ();
			Dictionary<string, object> insertValues = record.GetValuesInsertNonNull (pkFields, GetIdentityFieldNamesByTable ());

			var cmdParam = new Database.SQL.SQLCommandParameterized (this.TableName, insertValues, this.ColumnSchema);

			return InsertRecordCommand (cmdParam, insertValues);
		}

		public bool UpdateRecord (Dataelus.Table.ObjectRow record)
		{
			// Assumes that the ObjectTable column names are the same as the upload database table field names
			var pkFields = GetPrimaryKeyFields ();
			var pkConditions = record.GetValuesByFields (pkFields);

			List<string> exclude = new List<string> ();
			exclude.AddRange (pkFields);
			exclude.AddRange (GetIdentityFieldNamesByTable ());
			var arrExclude = exclude
				.Distinct (new StringEqualityComparer ())
				.ToArray ();

			var updateValues = record.GetValuesChanged (arrExclude);

			var paramNameBuilder = new Dataelus.Database.SQL.SQLParameterNameBuilder ();
			var cmdParamValue = new Database.SQL.SQLCommandParameterized (this.TableName, updateValues, this.ColumnSchema, paramNameBuilder);
			var cmdParamCondition = new Database.SQL.SQLCommandParameterized (this.TableName, pkConditions, this.ColumnSchema, paramNameBuilder);

			return UpdateRecordCommand (cmdParamValue, cmdParamCondition, pkConditions, updateValues);
		}

		public bool DeleteRecord (Dataelus.Table.ObjectRow record)
		{
			// Assumes that the ObjectTable column names are the same as the upload database table field names
			var pkFields = GetPrimaryKeyFields ();
			var pkConditions = record.GetValuesByFields (pkFields);

			var cmdParamCondition = new Database.SQL.SQLCommandParameterized (this.TableName, pkConditions, this.ColumnSchema);

			return DeleteRecordCommand (cmdParamCondition, pkConditions);
		}

		public bool DoesRecordExist (Dataelus.Table.ObjectRow record)
		{
			// Assumes that the ObjectTable column names are the same as the upload database table field names
			var pkFields = GetPrimaryKeyFields ();
			var pkConditions = record.GetValuesByFields (pkFields);

			var cmdParamCondition = new Database.SQL.SQLCommandParameterized (this.TableName, pkConditions, this.ColumnSchema);

			return GetRecordCountCommand (cmdParamCondition, pkConditions) > 0;
		}

		public bool DoesRecordNeedUpdated (Dataelus.Table.ObjectRow record)
		{
			// Assumes that the ObjectTable column names are the same as the upload database table field names
			if (DoesRecordExist (record)) {
				// Do not exclude any fields
				var allValues = record.GetValuesAll (true);

				var cmdParam = new Database.SQL.SQLCommandParameterized (this.TableName, allValues, this.ColumnSchema);

				// If the record is not found exactly, then it must have been changed
				var count = GetRecordCountCommand (cmdParam, allValues);
				if (count == 0)
					return true;
			}
			return false;
		}

		#endregion
	}
}

