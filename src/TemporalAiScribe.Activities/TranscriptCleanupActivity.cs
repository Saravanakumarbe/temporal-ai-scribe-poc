using TemporalAiScribe.Contracts;

namespace TemporalAiScribe.Activities;

public sealed class TranscriptCleanupActivity
{
    public Task<TranscriptResult> ExecuteAsync(TranscriptResult transcript)
    {
        Console.WriteLine("Transcript cleanup activity started");

        var cleaned = transcript.Transcript
            .Replace("uh....hmm....you know....okay....", string.Empty)
            .Replace("  ", " ")
            .Trim();

        var cleanedResult = new TranscriptResult
        {
            Transcript = cleaned,
            Language = transcript.Language,
            DurationSeconds = transcript.DurationSeconds,
            Model = transcript.Model
        };

        Console.WriteLine("Transcript cleanup activity completed");
        return Task.FromResult(cleanedResult);
    }
}
