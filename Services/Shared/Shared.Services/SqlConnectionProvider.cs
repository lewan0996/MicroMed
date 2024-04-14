using Microsoft.Data.SqlClient;

namespace Shared.Services;

public class SqlConnectionProvider(string connectionString)
{
    public async Task<TResult> CallAsync<TResult>(Func<SqlConnection, Task<TResult>> func)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            return await func(connection);
        }
    }
}