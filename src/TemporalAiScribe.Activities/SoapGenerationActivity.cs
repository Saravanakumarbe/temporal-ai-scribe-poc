using TemporalAiScribe.Contracts;

namespace TemporalAiScribe.Activities;

public sealed class SoapGenerationActivity
{
    public Task<SoapNote> ExecuteAsync(TranscriptResult transcript)
    {
        Console.WriteLine("SOAP generation activity started");

        var soapNote = new SoapNote
        {
            Subjective = $"Patient reports: {transcript.Transcript}",
            Objective = "No acute distress. Vital signs stable.",
            Assessment = "Mild respiratory symptoms without fever.",
            Plan = "Continue observation and follow-up as needed."
        };

        Console.WriteLine("SOAP generation activity completed");
        return Task.FromResult(soapNote);
    }
}
