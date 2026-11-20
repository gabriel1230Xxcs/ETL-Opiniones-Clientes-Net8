using ETLOpinionesClientes.Configuration;
using ETLOpinionesClientes.Extractors;
using ETLOpinionesClientes.Services;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<EtLSettings>(builder.Configuration.GetSection("ETL"));
builder.Services.AddHttpClient<ApiExtractor>();
builder.Services.AddSingleton<CsvExtractor>();
builder.Services.AddSingleton<DatabaseExtractor>();
builder.Services.AddSingleton<StagingLoader>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();
