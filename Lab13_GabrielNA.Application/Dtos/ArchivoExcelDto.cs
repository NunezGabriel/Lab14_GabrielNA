namespace Lab13_GabrielNA.Application.Dtos;

public class ArchivoExcelDto
{
    public const string TipoContenido = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public byte[] Contenido { get; set; } = Array.Empty<byte>();
    public string NombreArchivo { get; set; } = string.Empty;
}
