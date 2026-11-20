using ETLOpinionesClientes.Models;
using Microsoft.Data.SqlClient;

namespace ETLOpinionesClientes.Extractors;

public sealed class DatabaseExtractor
{
    public async Task<List<Opinion>> ExtractAsync(string connectionString, string query, CancellationToken ct)
    {
        var result = new List<Opinion>();
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(ct);
        await using var command = new SqlCommand(query, connection) { CommandTimeout = 60 };
        await using var reader = await command.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            result.Add(new Opinion {
                Fuente = "SQL", IdOrigen = Convert.ToString(reader["Id"]) ?? Guid.NewGuid().ToString(),
                Cliente = Convert.ToString(reader["Cliente"]) ?? "",
                Producto = Convert.ToString(reader["Producto"]) ?? "",
                Calificacion = reader["Calificacion"] == DBNull.Value ? null : Convert.ToInt32(reader["Calificacion"]),
                Comentario = Convert.ToString(reader["Comentario"]) ?? "",
                Fecha = reader["Fecha"] == DBNull.Value ? null : Convert.ToDateTime(reader["Fecha"])
            });
        }
        return result;
    }
}
