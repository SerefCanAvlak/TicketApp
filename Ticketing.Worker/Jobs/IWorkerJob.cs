namespace Ticketing.Worker.Jobs;

internal interface IWorkerJob
{
    string Name { get; }
    Task ExecuteAsync(CancellationToken cancellationToken);
}
