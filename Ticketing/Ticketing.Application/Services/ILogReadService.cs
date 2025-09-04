using Ticketing.Application.Features.Log.Dtos;

namespace Ticketing.Application.Services;

public interface ILogReadService
{
    Task<List<LogDto>> GetLatestLogsAsync(int take = 50);
}
