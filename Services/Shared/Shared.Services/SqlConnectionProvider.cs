using Npgsql;

namespace Shared.Services;

public class PostgresConnectionProvider(string connectionString)
{
    public async Task<TResult> CallAsync<TResult>(Func<NpgsqlConnection, Task<TResult>> func)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        
        return await func(connection);
    }
}