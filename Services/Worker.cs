using ETLOpinionesClientes.Configuration;
using ETLOpinionesClientes.Extractors;
using ETLOpinionesClientes.Services;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace ETLOpinionesClientes;

public sealed class Worker(
    ILogger<Worker> logger,
    CsvExtractor csv,
    DatabaseExtractor database,
    ApiExtractor api,
    StagingLoader loader,
    IOptions<EtLSettings> options) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var settings = options.Value;
        var timer = Stopwatch.StartNew();
        try
        {
            var tasks = new List<Task<List<Models.Opinion>>>
            {
                csv.ExtractAsync(settings.CsvPath, stoppingToken)
            };
            if (!string.IsNullOrWhiteSpace(settings.SourceConnectionString))
                tasks.Add(database.ExtractAsync(settings.SourceConnectionString, settings.SourceQuery, stoppingToken));
            if (!string.IsNullOrWhiteSpace(settings.ApiUrl))
                tasks.Add(api.ExtractAsync(settings.ApiUrl, stoppingToken));

            var results = await Task.WhenAll(tasks);
            var all = results.SelectMany(x => x).ToList();
            logger.LogInformation("Extracción completada. Registros: {Count}", all.Count);

            if (string.IsNullOrWhiteSpace(settings.StagingConnectionString))
                throw new InvalidOperationException("Configure ETL:StagingConnectionString antes de ejecutar.");
            await loader.LoadAsync(all, settings.StagingConnectionString, stoppingToken);
            timer.Stop();
            logger.LogInformation("Proceso finalizado en {Elapsed} ms", timer.ElapsedMilliseconds);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            logger.LogWarning("Proceso cancelado.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error durante el proceso de extracción ETL.");
            throw;
        }
    }
}
