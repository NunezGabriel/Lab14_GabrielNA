namespace Lab13_GabrielNA.Application.Dtos;

public class ProductoVendidoDto
{
    public int ProductoId { get; set; }
    public string Producto { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int UnidadesVendidas { get; set; }
    public decimal Ingresos { get; set; }
}
