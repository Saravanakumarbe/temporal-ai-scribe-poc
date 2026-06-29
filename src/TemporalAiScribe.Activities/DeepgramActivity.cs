using TemporalAiScribe.Contracts;

namespace TemporalAiScribe.Activities;

public sealed class DeepgramActivity
{
    public Task<TranscriptResult> ExecuteAsync(StorageResult storageResult)
    {
        Console.WriteLine($"Deepgram activity started for {storageResult.FileId}");

        var transcript = new TranscriptResult
        {
            Transcript = "The patient reports mild shortness of breath and fatigue. No fever. Physical exam is unremarkable.",
            Language = "en",
            DurationSeconds = 42,
            Model = "Nova-Medical"
        };

        Console.WriteLine($"Deepgram activity completed for {storageResult.FileId}");
        return Task.FromResult(transcript);
    }
}
