using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using Ticketing.Application.Features.Log.Dtos;
using Ticketing.Application.Services;

namespace Ticketing.Infrastructure.Services;

public class LogReadService(IConfiguration configuration) : ILogReadService
{
    private readonly string _connectionString = configuration.GetConnectionString("SqlServer")
    ?? throw new InvalidOperationException("Connection string 'SqlServer' is not configured.");



    public async Task<List<LogDto>> GetLatestLogsAsync(int take = 50)
    {
        using var conn = new SqlConnection(_connectionString);

        var query = @"
            SELECT TOP (@take) 
                Id,
                LogDate,
                Thread,
                LogLevel,
                Logger,
                LogMessage,
                Exception
            FROM Log
            ORDER BY LogDate DESC";

        var logs = await conn.QueryAsync<LogDto>(query, new { take });
        return logs.ToList();
    }
}
