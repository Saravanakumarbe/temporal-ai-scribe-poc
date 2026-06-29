using Temporalio.Client;
using TemporalAiScribe.Contracts;
using TemporalAiScribe.Workflows;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(sp =>
{
    var targetHost = builder.Configuration["Temporal:Host"] ?? "localhost:7233";
    return TemporalClient.ConnectAsync(new TemporalClientConnectOptions
    {
        Namespace = builder.Configuration["Temporal:Namespace"] ?? "default",
        TargetHost = targetHost
    }).GetAwaiter().GetResult();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapPost("/api/workflows", async (TemporalClient client, StartScribeRequest request) =>
{
    var workflowId = Guid.NewGuid().ToString("N");

    await client.StartWorkflowAsync(
        (ScribeWorkflow workflow) => workflow.RunAsync(request),
        new WorkflowOptions
        {
            Id = workflowId,
            TaskQueue = "scribe-task-queue"
        });

    return Results.Ok(new { workflowId, status = "Started" });
});

app.MapGet("/api/workflows/{id}", async (TemporalClient client, string id) =>
{
    var handle = client.GetWorkflowHandle<ScribeWorkflow>(id);
    var progress = await handle.QueryAsync(workflow => workflow.GetProgress());
    return Results.Ok(progress);
});

app.MapPost("/api/workflows/{id}/approve", async (TemporalClient client, string id) =>
{
    var handle = client.GetWorkflowHandle<ScribeWorkflow>(id);
    await handle.SignalAsync(workflow => workflow.ApproveAsync());
    return Results.Ok(new { workflowId = id, status = "Approved" });
});

app.MapPost("/api/workflows/{id}/reject", async (TemporalClient client, string id) =>
{
    var handle = client.GetWorkflowHandle<ScribeWorkflow>(id);
    await handle.SignalAsync(workflow => workflow.RejectAsync());
    return Results.Ok(new { workflowId = id, status = "Rejected" });
});

app.MapPost("/api/workflows/{id}/cancel", async (TemporalClient client, string id) =>
{
    var handle = client.GetWorkflowHandle<ScribeWorkflow>(id);
    await handle.SignalAsync(workflow => workflow.CancelAsync());
    return Results.Ok(new { workflowId = id, status = "Cancelled" });
});

app.Run();
