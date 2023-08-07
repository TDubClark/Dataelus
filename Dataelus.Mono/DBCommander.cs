using System;
using System.Data.SqlClient;

namespace Dataelus.Mono
{
	/// <summary>
	/// Executes commands on a database
	/// </summary>
	public class DBCommander : IDisposable
	{
		protected string _connectionString;

		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value>The connection string.</value>
		public string ConnectionString {
			get { return _connectionString; }
			set { _connectionString = value; }
		}

		protected int _connectionTimeout;

		/// <summary>
		/// Gets or sets the connection timeout.
		/// </summary>
		/// <value>The connection timeout.</value>
		public int ConnectionTimeout {
			get { return _connectionTimeout; }
			set { _connectionTimeout = value; }
		}

		protected int _commandTimeout;

		/// <summary>
		/// Gets or sets the default command timeout.
		/// </summary>
		/// <value>The command timeout.</value>
		public int CommandTimeout {
			get { return _commandTimeout; }
			set { _commandTimeout = value; }
		}

		private SqlConnection _connection;

		/// <summary>
		/// Gets or sets the connection.
		/// </summary>
		/// <value>The connection.</value>
		public SqlConnection Connection {
			get { return _connection; }
			set { _connection = value; }
		}

		private SqlTransaction _transaction;

		/// <summary>
		/// Gets or sets the transaction.
		/// </summary>
		/// <value>The transaction.</value>
		public SqlTransaction Transaction {
			get { return _transaction; }
			set { _transaction = value; }
		}

		/// <summary>
		/// Opens the connection.
		/// </summary>
		/// <returns><c>true</c>, if connection was opened, <c>false</c> otherwise.</returns>
		public bool OpenConnection ()
		{
			if (_connection == null) {
				_connection = new SqlConnection (_connectionString);

//				if (_connectionTimeout > 0)
//					_connection.ConnectionTimeout = _connectionTimeout;
				
				_connection.Open ();
			} else if (_connection.State == System.Data.ConnectionState.Closed) {
				_connection.Open ();
			}
			return true;
		}

		/// <summary>
		/// Closes the connection.
		/// </summary>
		/// <returns><c>true</c>, if connection was closed, <c>false</c> otherwise.</returns>
		public bool CloseConnection ()
		{
			if (_connection != null) {
				if (_connection.State != System.Data.ConnectionState.Closed) {
					try {
						_connection.Close ();
					} catch {
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Begins a transaction, and stores a pointer in the "Transaction" property.
		/// </summary>
		public void BeginTransaction ()
		{
			_transaction = _connection.BeginTransaction ();
		}

		/// <summary>
		/// Executes the command within a started transaction; must call OpenConnection() and BeginTransaction() first.
		/// Call Transaction.Commit() or Transaction.RollBack() afterward.
		/// </summary>
		/// <returns>The number of records affected.</returns>
		/// <param name="command">The command object.</param>
		public int ExecuteCommandWithinTransaction (SqlCommand command)
		{
			return ExecuteCommand (_connection, _transaction, command);
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DBCommander"/> class.
		/// </summary>
		public DBCommander ()
		{
			_connectionTimeout = -1;
			_commandTimeout = -1;
			_connectionString = null;

			_connection = null;
			_transaction = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DBCommander"/> class.
		/// </summary>
		/// <param name="connectionString">Connection string (with Execute permissions).</param>
		public DBCommander (string connectionString)
			: this ()
		{
			_connectionString = connectionString;
		}

		/// <summary>
		/// Sets the user login.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		public void SetUserLogin (string username, string password)
		{
			var builder = new System.Data.SqlClient.SqlConnectionStringBuilder (_connectionString);
			builder.UserID = username;
			builder.Password = password;
			_connectionString = builder.ToString ();
		}

		/// <summary>
		/// Executes the given command, using the given Username and Password.
		/// </summary>
		/// <returns>The command result.</returns>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <param name="commandText">Command text.</param>
		public CommandResult ExecuteCommand (string username, string password, string commandText)
		{
			var builder = new SqlConnectionStringBuilder (_connectionString);
			builder.UserID = username;
			builder.Password = password;
			if (_connectionTimeout > 0) {
				builder.ConnectTimeout = _connectionTimeout;
			}
			return ExecuteCommand (builder.ToString (), commandText);
		}

		public CommandResult ExecuteCommandObject (SqlCommand command)
		{
			return ExecuteCommand (_connectionString, command);
		}

		/// <summary>
		/// Executes the given command using the given connection string.
		/// </summary>
		/// <returns><c>true</c>, if command was executed, <c>false</c> otherwise.</returns>
		/// <param name="connectionString">Connection string.</param>
		/// <param name="commandText">Command text.</param>
		public static CommandResult ExecuteCommand (string connectionString, string commandText)
		{
			return ExecuteCommand (connectionString, new SqlCommand (commandText));
		}

		public static CommandResult ExecuteCommand (string connectionString, SqlCommand cmd)
		{
			var result = new CommandResult ();

			using (var conn = new SqlConnection (connectionString)) {
				conn.Open ();

				var trans = conn.BeginTransaction ();

				try {
					result.RowsAffected = ExecuteCommand (conn, trans, cmd);

					trans.Commit ();
					result.Success = true;
					result.Message = "Success";
				} catch (Exception ex) {
					try {
						trans.Rollback ();
						result.Message = String.Format ("Command failed to execute, transaction was rolled back: {0}", ex.Message);
					} catch (Exception ex2) {
						result.Message = String.Format ("Command failed to execute: {0}; transaction roll-back also failed: {1}", ex.Message, ex2.Message);
					}
				}

				conn.Close ();
			}

			return result;
		}

		static int ExecuteCommand (SqlConnection conn, SqlTransaction trans, SqlCommand cmd)
		{
			cmd.Connection = conn;
			cmd.Transaction = trans;
			return cmd.ExecuteNonQuery ();
		}

		/// <summary>
		/// Executes a Bulk Insert.
		/// </summary>
		/// <returns>The insert.</returns>
		/// <param name="connectionString">Connection string.</param>
		/// <param name="connectionTimeout">Connection timeout (for default, set to -1).</param>
		/// <param name="bulkCopyTimeout">SqlBulkCopy timeout (for default, set to -1).</param>
		/// <param name="batchSize">The bulk copy batch size (for default, set to -1).</param>
		/// <param name="tableName">Table name.</param>
		/// <param name="columnMap">Column map.</param>
		/// <param name="dt">The DataTable of values.</param>
		public static CommandResult BulkInsert (string connectionString, int connectionTimeout, int bulkCopyTimeout, int batchSize, string tableName, System.Collections.Generic.Dictionary<string, string> columnMap, System.Data.DataTable dt)
		{
			var result = new CommandResult ();

			if (connectionTimeout > 0) {
				var builder = new SqlConnectionStringBuilder (connectionString);
				builder.ConnectTimeout = connectionTimeout;
				connectionString = builder.ToString ();
			}

			using (var conn = new SqlConnection (connectionString)) {
				conn.Open ();

				var trans = conn.BeginTransaction ();

				SqlBulkCopy bc = new SqlBulkCopy (conn, SqlBulkCopyOptions.CheckConstraints, trans);
				if (batchSize <= 0)
					batchSize = 100;
				bc.BatchSize = batchSize;
				bc.DestinationTableName = tableName;

				if (bulkCopyTimeout > 0)
					bc.BulkCopyTimeout = bulkCopyTimeout;

				foreach (var map in columnMap) {
					bc.ColumnMappings.Add (new SqlBulkCopyColumnMapping (map.Key, map.Value));
				}

				try {
					bc.WriteToServer (dt);
					trans.Commit ();

					result.Success = true;
					result.Message = "Success";
				} catch (Exception ex) {
					try {
						trans.Rollback ();
						result.Message = String.Format ("Command failed to execute, transaction was rolled back: {0}", ex.Message);
					} catch (Exception ex2) {
						result.Message = String.Format ("Command failed to execute: {0}; transaction roll-back also failed: {1}", ex.Message, ex2.Message);
					}
				}

				conn.Close ();
			}

			return result;
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			if (_transaction != null) {
				_transaction.Dispose ();
			}
			if (_connection != null) {
				CloseConnection ();
				_connection.Dispose ();
			}
		}

		#endregion
	}

	/// <summary>
	/// Database Command execution result.
	/// </summary>
	public class CommandResult
	{
		/// <summary>
		/// Gets or sets the rows affected by the command.
		/// </summary>
		/// <value>The rows affected.</value>
		public int RowsAffected{ get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the command was executed successfully.
		/// </summary>
		/// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
		public bool Success{ get; set; }

		/// <summary>
		/// Gets or sets the message resulting from the command.
		/// </summary>
		/// <value>The message.</value>
		public string Message{ get; set; }

		public CommandResult ()
		{
			this.RowsAffected = -1;
			this.Success = false;
			this.Message = null;
		}
	}
}

