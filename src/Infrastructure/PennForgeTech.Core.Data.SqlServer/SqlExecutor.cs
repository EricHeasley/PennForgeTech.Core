using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using PennForgeTech.Core.Primitives;

namespace PennForgeTech.Core.Data.SqlServer
{
    public sealed class SqlExecutor
    {
        private readonly DbProviderFactory _providerFactory;
        private readonly string _connectionString;

        public SqlExecutor(DbProviderFactory providerFactory, string connectionString)
        {
            this._providerFactory = Guard.AgainstNull(providerFactory, nameof(providerFactory));
            this._connectionString = Guard.AgainstNullOrWhiteSpace(connectionString, nameof(connectionString));
        }

        public async Task<int> ExecuteAsync(
            string commandText,
            IReadOnlyDictionary<string, object?>? parameters = null,
            CommandType commandType = CommandType.Text,
            CancellationToken cancellationToken = default)
        {
            using (DbConnection connection = await this.OpenConnectionAsync(cancellationToken).ConfigureAwait(false))
            using (DbCommand command = CreateCommand(connection, commandText, parameters, commandType))
            {
                return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<IReadOnlyList<TRow>> QueryAsync<TRow>(
            string commandText,
            Func<DbDataReader, TRow> mapRow,
            IReadOnlyDictionary<string, object?>? parameters = null,
            CommandType commandType = CommandType.Text,
            CancellationToken cancellationToken = default)
        {
            Guard.AgainstNull(mapRow, nameof(mapRow));

            using (DbConnection connection = await this.OpenConnectionAsync(cancellationToken).ConfigureAwait(false))
            using (DbCommand command = CreateCommand(connection, commandText, parameters, commandType))
            using (DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
            {
                List<TRow> rows = new List<TRow>();

                while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    rows.Add(mapRow(reader));
                }

                return rows;
            }
        }

        private static DbCommand CreateCommand(
            DbConnection connection,
            string commandText,
            IReadOnlyDictionary<string, object?>? parameters,
            CommandType commandType)
        {
            Guard.AgainstNullOrWhiteSpace(commandText, nameof(commandText));

            DbCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;

            if (parameters is not null)
            {
                foreach (KeyValuePair<string, object?> pair in parameters)
                {
                    DbParameter parameter = command.CreateParameter();
                    parameter.ParameterName = pair.Key;
                    parameter.Value = pair.Value ?? DBNull.Value;
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        private async Task<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken)
        {
            DbConnection? connection = this._providerFactory.CreateConnection();

            if (connection is null)
            {
                throw new InvalidOperationException("The provider factory could not create a connection.");
            }

            connection.ConnectionString = this._connectionString;

            try
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                connection.Dispose();
                throw;
            }

            return connection;
        }
    }
}
