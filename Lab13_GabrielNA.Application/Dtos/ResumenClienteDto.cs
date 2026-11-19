namespace Lab13_GabrielNA.Application.Dtos;

public class ResumenClienteDto
{
    public int ClienteId { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public int Pedidos { get; set; }
    public int Unidades { get; set; }
    public decimal Total { get; set; }
}
