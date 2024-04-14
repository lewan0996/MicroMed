using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Shared.Services;

public static class DapperExtensions
{
    public static Task<IEnumerable<T>> QueryAsync<T>(this SqlConnection cnn, string sql, CancellationToken cancellationToken, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        => cnn.QueryAsync<T>(new CommandDefinition(sql, param, transaction, commandTimeout, commandType, cancellationToken: cancellationToken));
}