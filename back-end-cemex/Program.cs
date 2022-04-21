using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using back_end_cemex;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["AzureStorage:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["AzureStorage:queue"], preferMsi: true);
});

var app = builder.Build();
var logger = app.Services.GetService(typeof(ILogger<Startup>)) as ILogger<Startup>;

startup.Configure(app, app.Environment, logger);


app.Run();
