namespace Lab13_GabrielNA.Application.Dtos;

public class PedidoReporteDto
{
    public int PedidoId { get; set; }
    public DateTime Fecha { get; set; }
    public int ClienteId { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Productos { get; set; }
    public int Unidades { get; set; }
    public decimal Total { get; set; }
}
