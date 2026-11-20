namespace ETLOpinionesClientes.Configuration;

public sealed class EtLSettings
{
    public string CsvPath { get; set; } = "Data/encuestas.csv";
    public string SourceConnectionString { get; set; } = "";
    public string StagingConnectionString { get; set; } = "";
    public string SourceQuery { get; set; } = "SELECT Id, Cliente, Producto, Calificacion, Comentario, Fecha FROM dbo.Resenas";
    public string ApiUrl { get; set; } = "https://jsonplaceholder.typicode.com/comments";
    public int BatchSize { get; set; } = 500;
}
