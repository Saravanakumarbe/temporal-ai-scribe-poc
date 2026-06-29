using TemporalAiScribe.Contracts;

namespace TemporalAiScribe.Activities;

public sealed class StorageActivity
{
    public Task<StorageResult> ExecuteAsync(StartScribeRequest request)
    {
        Console.WriteLine($"Storage activity started for {request.AudioUrl}");

        var result = new StorageResult
        {
            FileId = Guid.NewGuid().ToString("N"),
            StorageUrl = $"/storage/{Guid.NewGuid():N}.mp3"
        };

        Console.WriteLine($"Storage activity completed for {request.AudioUrl}");
        return Task.FromResult(result);
    }
}
