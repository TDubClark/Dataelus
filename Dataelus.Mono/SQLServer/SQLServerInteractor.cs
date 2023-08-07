using System;

namespace Dataelus.Mono.SQLServer
{
	/// <summary>
	/// Interactor class for Microsoft SQL Server; holds a Querier, a Command Builder, and a Command Executor
	/// </summary>
	public class SQLServerInteractor
	{
		/// <summary>
		/// Gets or sets the command executor.
		/// </summary>
		/// <value>The commander.</value>
		public DBCommander Commander { get; set; }

		/// <summary>
		/// Gets or sets the database querier.
		/// </summary>
		/// <value>The querier.</value>
		public SQLServerQuerier Querier { get; set; }

		/// <summary>
		/// Gets or sets the SQL Command builder.
		/// </summary>
		/// <value>The builder.</value>
		public CommandBuilder Builder { get; set; }

		/// <summary>
		/// Gets or sets the column schema.
		/// </summary>
		/// <value>The column schema.</value>
		public Dataelus.Database.DBFieldCollection ColumnSchema { get; set; }

		/// <summary>
		/// Gets the schematic object for the database.
		/// </summary>
		/// <returns>The schematic.</returns>
		public Database.DBSchematic GetSchematic (string dbSchemaName)
		{
			var schematic = new Dataelus.Database.DBSchematic ();

			// Initialize the Column Schema, if necessary
			if (this.ColumnSchema == null)
				InitSchema ();

			schematic.ColumnSchema = this.ColumnSchema;

			schematic.IdentityFields = this.Querier.GetSchemaIdentityColumnList (dbSchemaName);
			schematic.PrimaryKeys = this.Querier.GetPrimaryKeys ();
			schematic.TableConstraints = this.Querier.GetConstraints ();

			return schematic;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.SQLServer.SQLServerInteractor"/> class.
		/// </summary>
		/// <param name="connectionStringReader">Connection string reader.</param>
		/// <param name="connectionStringWriter">Connection string writer.</param>
		public SQLServerInteractor (string connectionStringReader, string connectionStringWriter)
		{
			this.Querier = new SQLServerQuerier (connectionStringReader);
			this.Commander = new DBCommander (connectionStringWriter);

			this.ColumnSchema = null;
			this.Builder = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.SQLServer.SQLServerInteractor"/> class.
		/// </summary>
		/// <param name="connectionStringReader">Connection string reader.</param>
		/// <param name="connectionStringWriter">Connection string writer.</param>
		/// <param name="columnSchema">The Database Column schema object.</param>
		public SQLServerInteractor (string connectionStringReader, string connectionStringWriter, Database.DBFieldCollection columnSchema)
			: this (connectionStringReader, connectionStringWriter)
		{
			InitCommandBuilder (columnSchema);
		}

		/// <summary>
		/// Initializes the column schema and the Command Builder; downloads the schema from the database).
		/// </summary>
		public void InitSchema ()
		{
			InitCommandBuilder (this.Querier.GetColumnSchema ());
		}

		/// <summary>
		/// Inits the command builder.
		/// </summary>
		/// <param name="columnSchema">Column schema.</param>
		public void InitCommandBuilder (Database.DBFieldCollection columnSchema)
		{
			this.ColumnSchema = columnSchema;
			this.Builder = new CommandBuilder (columnSchema);
		}
	}
}

