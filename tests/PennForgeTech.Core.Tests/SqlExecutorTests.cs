using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using PennForgeTech.Core.Data.SqlServer;
using Xunit;

namespace PennForgeTech.Core.Tests
{
    public sealed class SqlExecutorTests
    {
        private const string ConnectionString = "Data Source=file:sqlexecutortests?mode=memory&cache=shared";

        [Fact]
        public async Task ExecuteAsync_ThenQueryAsync_RoundTripsRowsWithParameters()
        {
            using (SqliteConnection keepAlive = new SqliteConnection(ConnectionString))
            {
                keepAlive.Open();
                SqlExecutor executor = new SqlExecutor(SqliteFactory.Instance, ConnectionString);

                await executor.ExecuteAsync("CREATE TABLE People (Id INTEGER, Name TEXT)");
                await executor.ExecuteAsync(
                    "INSERT INTO People (Id, Name) VALUES (@id, @name)",
                    new Dictionary<string, object?> { ["@id"] = 1, ["@name"] = "Eric" });
                await executor.ExecuteAsync(
                    "INSERT INTO People (Id, Name) VALUES (@id, @name)",
                    new Dictionary<string, object?> { ["@id"] = 2, ["@name"] = null });

                IReadOnlyList<string> names = await executor.QueryAsync(
                    "SELECT Name FROM People WHERE Id = @id",
                    ReadName,
                    new Dictionary<string, object?> { ["@id"] = 1 });

                Assert.Equal(new[] { "Eric" }, names);
            }
        }

        [Fact]
        public async Task QueryAsync_RejectsBlankCommandText()
        {
            SqlExecutor executor = new SqlExecutor(SqliteFactory.Instance, ConnectionString);

            await Assert.ThrowsAsync<System.ArgumentException>(() => executor.QueryAsync(" ", ReadName));
        }

        private static string ReadName(DbDataReader reader)
        {
            return reader.GetString(0);
        }
    }
}
