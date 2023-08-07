using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using Dataelus.Database;
using Dataelus.Database.SQL;
using Dataelus.EDD.Producer;
using Dataelus.Mono.SQLServer;

namespace Dataelus.Mono
{
	public class DatabaseRecordProducerTable : DatabaseRecordProducerTableBase
	{
		public DBCommander Commander { get; set; }

		public DBQuerier Querier { get; set; }

		public CommandBuilder Builder { get; set; }

		public ISQLServerCommandBuilder FinalCommandBuilder { get; set; }

		public bool IsSingleTransaction { get; set; }

		public DatabaseRecordProducerTable ()
			: base ()
		{
		}

		public DatabaseRecordProducerTable (
			DBFieldCollection columnSchema
			, DBPrimaryKeyCollection primaryKeys
			, List<DBFieldSimple> identityFields
			, Dataelus.EDD.EDDFieldCollection eddFields
			, string tableName
		)
			: base (columnSchema, primaryKeys, identityFields, eddFields, tableName)
		{
			this.Builder = new CommandBuilder (columnSchema);
			this.IsSingleTransaction = false;
			this.FinalCommandBuilder = this.Builder;
		}

		public DatabaseRecordProducerTable (
			DBFieldCollection columnSchema
			, DBPrimaryKeyCollection primaryKeys
			, List<DBFieldSimple> identityFields
			, Dataelus.EDD.EDDFieldCollection eddFields
			, string tableName
			, DBCommander commander
			, DBQuerier querier
			, CommandBuilder builder
		)
			: base (columnSchema, primaryKeys, identityFields, eddFields, tableName)
		{
			this.Commander = commander;
			this.Querier = querier;
			this.Builder = builder;
			this.IsSingleTransaction = false;
			this.FinalCommandBuilder = builder;
		}

		public DatabaseRecordProducerTable (
			SQLServerInteractor interactor
			, DBSchematic schematic
			, Dataelus.EDD.EDDFieldCollection eddFields
			, string tableName
		)
			: this (schematic.ColumnSchema, schematic.PrimaryKeys, schematic.IdentityFields
				, eddFields, tableName
				, interactor.Commander, interactor.Querier, interactor.Builder)
		{
			
		}

		protected virtual bool ExecuteCommand (SqlCommand command)
		{
			if (this.IsSingleTransaction)
				return this.Commander.ExecuteCommandWithinTransaction (command) > 0;
			else
				return this.Commander.ExecuteCommandObject (command).Success;
		}

		#region implemented abstract members of DatabaseRecordProducerTableBase

		protected override bool InsertRecordCommand (SQLCommandParameterized cmdParam, Dictionary<string, object> insertValues)
		{
			return ExecuteCommand (FinalCommandBuilder.GetCommandInsert (this.TableName, cmdParam, insertValues));
		}

		protected override bool UpdateRecordCommand (SQLCommandParameterized cmdParamValue, SQLCommandParameterized cmdParamCondition, Dictionary<string, object> conditions, Dictionary<string, object> updateValues)
		{
			return ExecuteCommand (FinalCommandBuilder.GetCommandUpdate (this.TableName, cmdParamValue, cmdParamCondition, conditions, updateValues));
		}

		protected override bool DeleteRecordCommand (SQLCommandParameterized cmdParamCondition, Dictionary<string, object> conditions)
		{
			return ExecuteCommand (FinalCommandBuilder.GetCommandDelete (this.TableName, cmdParamCondition, conditions));
		}

		protected override int GetRecordCountCommand (SQLCommandParameterized cmdParamCondition, Dictionary<string, object> conditions)
		{
			return this.Querier.GetCount (FinalCommandBuilder.GetCommandSelectCount (this.TableName, cmdParamCondition, conditions));
		}

		#endregion
	}
}

