using Temporalio.Client;
using Temporalio.Worker;
using TemporalAiScribe.Activities;
using TemporalAiScribe.Workflows;

namespace TemporalAiScribe.Worker;

public sealed class WorkerService : BackgroundService
{
    private readonly TemporalClient _client;
    private readonly ValidateActivity _validateActivity;

    public WorkerService(TemporalClient client, ValidateActivity validateActivity)
    {
        _client = client;
        _validateActivity = validateActivity;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Temporal worker started.");

        using var worker = new TemporalWorker(
            _client,
            new TemporalWorkerOptions("scribe-task-queue")
                .AddWorkflow<ScribeWorkflow>()
                .AddActivity((ValidateActivity activity) => activity.ExecuteAsync)
                .AddActivity((StorageActivity activity) => activity.ExecuteAsync)
                .AddActivity((DeepgramActivity activity) => activity.ExecuteAsync)
                .AddActivity((TranscriptCleanupActivity activity) => activity.ExecuteAsync)
                .AddActivity((SoapGenerationActivity activity) => activity.ExecuteAsync)
                .AddActivity((PersistActivity activity) => activity.ExecuteAsync));

        await worker.ExecuteAsync(stoppingToken);
    }
}
