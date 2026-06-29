using Temporalio.Common;
using Temporalio.Exceptions;
using Temporalio.Workflows;
using TemporalAiScribe.Activities;
using TemporalAiScribe.Contracts;

namespace TemporalAiScribe.Workflows;

[Workflow]
public class ScribeWorkflow
{
    private bool _approved;
    private SoapNote _soap = new();
    private string _status = "Running";

    [WorkflowRun]
    public async Task<ScribeResult> RunAsync(StartScribeRequest request)
    {
        Console.WriteLine($"Workflow started for {request.AudioUrl}");
        UpdateProgress("Validate", 10, "Running");

        var validation = await Workflow.ExecuteActivityAsync(
            (ValidateActivity activity) => activity.ExecuteAsync(request),
            CreateActivityOptions(2));

        UpdateProgress("Storage", 20, "Running");
        var storage = await Workflow.ExecuteActivityAsync(
            (StorageActivity activity) => activity.ExecuteAsync(request),
            CreateActivityOptions(3));

        UpdateProgress("Deepgram", 40, "Running");
        var transcript = await Workflow.ExecuteActivityAsync(
            (DeepgramActivity activity) => activity.ExecuteAsync(storage),
            CreateActivityOptions(5));

        UpdateProgress("Cleanup", 60, "Running");
        var cleanedTranscript = await Workflow.ExecuteActivityAsync(
            (TranscriptCleanupActivity activity) => activity.ExecuteAsync(transcript),
            CreateActivityOptions(3));

        UpdateProgress("Generate SOAP", 80, "Running");
        _soap = await Workflow.ExecuteActivityAsync(
            (SoapGenerationActivity activity) => activity.ExecuteAsync(cleanedTranscript),
            CreateActivityOptions(5));

        UpdateProgress("Waiting Review", 90, "Waiting Review");
        await Workflow.WaitConditionAsync(() => _approved);

        UpdateProgress("Persist", 95, "Running");
        await Workflow.ExecuteActivityAsync(
            (PersistActivity activity) => activity.ExecuteAsync(Workflow.Info.WorkflowId, cleanedTranscript.Transcript, _soap),
            CreateActivityOptions(3));

        _status = "Completed";
        UpdateProgress("Completed", 100, "Completed");

        return new ScribeResult
        {
            WorkflowId = Workflow.Info.WorkflowId,
            Status = _status,
            Transcript = cleanedTranscript.Transcript,
            SoapNote = _soap
        };
    }

    [WorkflowSignal]
    public Task ApproveAsync()
    {
        _approved = true;
        _status = "Approved";
        UpdateProgress("Approved", 95, "Approved");
        return Task.CompletedTask;
    }

    [WorkflowSignal]
    public Task RejectAsync()
    {
        _status = "Rejected";
        UpdateProgress("Rejected", 90, "Rejected");
        throw new ApplicationFailureException("Rejected");
    }

    [WorkflowSignal]
    public Task CancelAsync()
    {
        _status = "Cancelled";
        UpdateProgress("Cancelled", 90, "Cancelled");
        throw new TaskCanceledException("Cancelled");
    }

    [WorkflowQuery]
    public SoapNote GetSoap() => _soap;

    [WorkflowQuery]
    public WorkflowProgress GetProgress() => new()
    {
        WorkflowId = Workflow.Info.WorkflowId,
        CurrentStep = _status,
        Percent = _status == "Completed" ? 100 : _status == "Waiting Review" ? 90 : _status == "Approved" ? 95 : 90,
        Status = _status
    };

    private static void UpdateProgress(string currentStep, int percent, string status)
    {
        // Placeholder hook for future progress publishing.
    }

    private static ActivityOptions CreateActivityOptions(int maxAttempts)
    {
        return new ActivityOptions
        {
            StartToCloseTimeout = TimeSpan.FromMinutes(2),
            RetryPolicy = new RetryPolicy
            {
                MaximumAttempts = maxAttempts,
                BackoffCoefficient = 2,
                InitialInterval = TimeSpan.FromSeconds(2)
            }
        };
    }
}
