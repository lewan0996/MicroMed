using Dapper;
using System.Data;
using Npgsql;

namespace Shared.Services;

public static class DapperExtensions
{
    static DapperExtensions()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
    
    public static Task<IEnumerable<T>> QueryAsync<T>(this NpgsqlConnection cnn, string sql, CancellationToken cancellationToken, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        => cnn.QueryAsync<T>(new CommandDefinition(sql, param, transaction, commandTimeout, commandType, cancellationToken: cancellationToken));
}