using MediatR;
using Ticketing.Application.Features.Log.Dtos;
using Ticketing.Application.Services;

namespace Ticketing.Application.Features.LogRead.Queries.GetLatestLogs;

public sealed record GetLatestLogsQuery(int Take = 5) : IRequest<List<LogDto>>;

internal sealed class GetLatestLogsQueryHandler(
    ILogReadService logReadService) : IRequestHandler<GetLatestLogsQuery, List<LogDto>>
{
    public async Task<List<LogDto>> Handle(GetLatestLogsQuery request, CancellationToken cancellationToken)
    {
        return await logReadService.GetLatestLogsAsync(request.Take);
    }
}