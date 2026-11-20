using ETLOpinionesClientes.Models;
using System.Net.Http.Json;

namespace ETLOpinionesClientes.Extractors;

public sealed class ApiExtractor(HttpClient httpClient)
{
    public async Task<List<Opinion>> ExtractAsync(string url, CancellationToken ct)
    {
        using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, ct);
        response.EnsureSuccessStatusCode();
        var items = await response.Content.ReadFromJsonAsync<List<ApiComment>>(cancellationToken: ct) ?? [];
        return items.Select(x => new Opinion {
            Fuente = "API", IdOrigen = x.Id.ToString(), Cliente = x.Email ?? "No especificado",
            Producto = "Comentario web", Calificacion = null, Comentario = x.Body ?? "", Fecha = null
        }).ToList();
    }
    private sealed class ApiComment { public int Id { get; set; } public string? Email { get; set; } public string? Body { get; set; } }
}
