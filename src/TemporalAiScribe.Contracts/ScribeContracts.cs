namespace TemporalAiScribe.Contracts;

public sealed class StartScribeRequest
{
    public string AudioUrl { get; set; } = string.Empty;
}

public sealed class ScribeResult
{
    public string WorkflowId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Transcript { get; set; }
    public SoapNote? SoapNote { get; set; }
}

public sealed class WorkflowProgress
{
    public string WorkflowId { get; init; } = string.Empty;
    public string CurrentStep { get; init; } = string.Empty;
    public int Percent { get; init; }
    public string Status { get; init; } = string.Empty;
}

public sealed class StorageResult
{
    public string FileId { get; set; } = string.Empty;
    public string StorageUrl { get; set; } = string.Empty;
}

public sealed class TranscriptResult
{
    public string Transcript { get; init; } = string.Empty;
    public string Language { get; init; } = "en";
    public int DurationSeconds { get; init; }
    public string Model { get; init; } = string.Empty;
}

public sealed class SoapNote
{
    public string Subjective { get; init; } = string.Empty;
    public string Objective { get; init; } = string.Empty;
    public string Assessment { get; init; } = string.Empty;
    public string Plan { get; init; } = string.Empty;
}
