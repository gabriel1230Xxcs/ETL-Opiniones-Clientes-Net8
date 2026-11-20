using CsvHelper;
using CsvHelper.Configuration;
using ETLOpinionesClientes.Models;
using System.Globalization;

namespace ETLOpinionesClientes.Extractors;

public sealed class CsvExtractor
{
    public async Task<List<Opinion>> ExtractAsync(string path, CancellationToken ct)
    {
        if (!File.Exists(path)) throw new FileNotFoundException("No existe el archivo CSV", path);
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        { HeaderValidated = null, MissingFieldFound = null, BadDataFound = null });
        var rows = new List<Opinion>();
        await foreach (var row in csv.GetRecordsAsync<CsvRow>(ct))
        {
            rows.Add(new Opinion {
                Fuente = "CSV", IdOrigen = row.Id ?? Guid.NewGuid().ToString(),
                Cliente = row.Cliente ?? "No especificado", Producto = row.Producto ?? "No especificado",
                Calificacion = int.TryParse(row.Calificacion, out var score) ? score : null,
                Comentario = row.Comentario ?? "",
                Fecha = DateTime.TryParse(row.Fecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : null
            });
        }
        return rows;
    }

    private sealed class CsvRow
    {
        public string? Id { get; set; }
        public string? Cliente { get; set; }
        public string? Producto { get; set; }
        public string? Calificacion { get; set; }
        public string? Comentario { get; set; }
        public string? Fecha { get; set; }
    }
}
