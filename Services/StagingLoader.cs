using ETLOpinionesClientes.Models;
using Microsoft.Data.SqlClient;

namespace ETLOpinionesClientes.Services;

public sealed class StagingLoader(ILogger<StagingLoader> logger)
{
    public async Task LoadAsync(IEnumerable<Opinion> records, string connectionString, CancellationToken ct)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(ct);
        foreach (var item in records)
        {
            const string sql = @"
INSERT INTO dbo.StgOpiniones
(Fuente, IdOrigen, Cliente, Producto, Calificacion, Comentario, FechaExtraccion)
VALUES (@Fuente, @IdOrigen, @Cliente, @Producto, @Calificacion, @Comentario, SYSUTCDATETIME());";
            await using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@Fuente", item.Fuente);
            cmd.Parameters.AddWithValue("@IdOrigen", item.IdOrigen);
            cmd.Parameters.AddWithValue("@Cliente", item.Cliente);
            cmd.Parameters.AddWithValue("@Producto", item.Producto);
            cmd.Parameters.AddWithValue("@Calificacion", (object?)item.Calificacion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Comentario", item.Comentario);
            await cmd.ExecuteNonQueryAsync(ct);
        }
        logger.LogInformation("Se cargaron {Count} registros en staging", records.Count());
    }
}
