using Temporalio.Client;
using TemporalAiScribe.Activities;
using TemporalAiScribe.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(sp =>
{
    var targetHost = builder.Configuration["Temporal:Host"] ?? "localhost:7233";
    return TemporalClient.ConnectAsync(new()
    {
        Namespace = builder.Configuration["Temporal:Namespace"] ?? "default",
        TargetHost = targetHost
    }).GetAwaiter().GetResult();
});

builder.Services.AddSingleton<ValidateActivity>();
builder.Services.AddHostedService<WorkerService>();

var host = builder.Build();
host.Run();
