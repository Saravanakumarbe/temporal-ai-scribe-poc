using TemporalAiScribe.Contracts;

namespace TemporalAiScribe.Activities;

public sealed class ValidateActivity
{
    public async Task<bool> ExecuteAsync(StartScribeRequest request)
    {
        Console.WriteLine($"Validate activity started for {request.AudioUrl}");

        if (string.IsNullOrWhiteSpace(request.AudioUrl))
        {
            throw new InvalidOperationException("Audio missing");
        }

        await Task.Delay(1000);

        Console.WriteLine($"Validate activity completed for {request.AudioUrl}");
        return true;
    }
}
