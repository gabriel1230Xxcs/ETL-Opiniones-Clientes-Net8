namespace ETLOpinionesClientes.Models;

public sealed class Opinion
{
    public string Fuente { get; set; } = "";
    public string IdOrigen { get; set; } = "";
    public string Cliente { get; set; } = "";
    public string Producto { get; set; } = "";
    public int? Calificacion { get; set; }
    public string Comentario { get; set; } = "";
    public DateTime? Fecha { get; set; }
}
