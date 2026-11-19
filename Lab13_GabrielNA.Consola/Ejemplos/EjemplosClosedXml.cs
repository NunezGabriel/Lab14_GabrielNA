using ClosedXML.Excel;

namespace Lab13_GabrielNA.Consola.Ejemplos;

public class EjemplosClosedXml
{
    private readonly string _carpeta;

    public EjemplosClosedXml()
    {
        _carpeta = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "ArchivosExcel"));
        Directory.CreateDirectory(_carpeta);
    }

    public void CrearArchivoExcel()
    {
        var ruta = Path.Combine(_carpeta, "archivo.xlsx");

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Hoja1");

            worksheet.Cell(1, 1).Value = "Nombre";
            worksheet.Cell(1, 2).Value = "Edad";
            worksheet.Cell(2, 1).Value = "Juan";
            worksheet.Cell(2, 2).Value = 28;

            workbook.SaveAs(ruta);
        }

        Console.WriteLine($"Archivo creado en: {ruta}");
    }

    public void ModificarArchivoExcel()
    {
        var ruta = Path.Combine(_carpeta, "archivo.xlsx");

        if (!File.Exists(ruta))
        {
            Console.WriteLine("Primero crea el archivo con la opción 1.");
            return;
        }

        using (var workbook = new XLWorkbook(ruta))
        {
            var worksheet = workbook.Worksheet(1);
            var edadAnterior = worksheet.Cell(2, 2).GetValue<int>();

            worksheet.Cell(2, 2).Value = 30;

            workbook.Save();

            Console.WriteLine($"Celda B2 modificada: {edadAnterior} -> {worksheet.Cell(2, 2).GetValue<int>()}");
        }

        Console.WriteLine($"Archivo actualizado: {ruta}");
    }

    public void CrearTablaExcel()
    {
        var ruta = Path.Combine(_carpeta, "tabla.xlsx");

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Datos");

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Nombre";
            worksheet.Cell(1, 3).Value = "Edad";

            worksheet.Cell(2, 1).Value = 1;
            worksheet.Cell(2, 2).Value = "Juan";
            worksheet.Cell(2, 3).Value = 28;

            worksheet.Cell(3, 1).Value = 2;
            worksheet.Cell(3, 2).Value = "María";
            worksheet.Cell(3, 3).Value = 34;

            var range = worksheet.Range("A1:C3");
            range.CreateTable();

            workbook.SaveAs(ruta);
        }

        Console.WriteLine($"Tabla creada en: {ruta}");
    }

    public void AplicarFormatoCelda()
    {
        var ruta = Path.Combine(_carpeta, "archivo_con_estilos.xlsx");

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Estilos");

            var headerRow = worksheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.Cyan;
            headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Nombre";
            worksheet.Cell(1, 3).Value = "Edad";

            worksheet.Cell(2, 1).Value = 1;
            worksheet.Cell(2, 2).Value = "Juan";
            worksheet.Cell(2, 3).Value = 28;

            workbook.SaveAs(ruta);
        }

        Console.WriteLine($"Archivo con estilos creado en: {ruta}");
    }
}
