using ClosedXML.Excel;
using Lab13_GabrielNA.Application.Dtos;
using Lab13_GabrielNA.Application.Ports.Reports;

namespace Lab13_GabrielNA.Infrastructure.Adapters.Reports;

public class ClosedXmlReportGenerator : IExcelReportGenerator
{
    private const string FormatoMoneda = "\"S/\" #,##0.00";
    private const string FormatoFecha = "dd/mm/yyyy";
    private const string FormatoPorcentaje = "0.0%";
    private const int FilaEncabezadoTabla = 5;

    private static readonly XLColor ColorPrincipal = XLColor.FromHtml("#1F4E78");
    private static readonly XLColor ColorBorde = XLColor.FromHtml("#BFBFBF");
    private static readonly XLColor ColorAlertaFondo = XLColor.FromHtml("#FCE4E4");
    private static readonly XLColor ColorAlertaTexto = XLColor.FromHtml("#9C0006");

    public byte[] GenerarReporteVentas(IEnumerable<PedidoReporteDto> pedidos, IEnumerable<ResumenClienteDto> resumen, string filtrosAplicados)
    {
        var listaPedidos = pedidos.ToList();
        var listaResumen = resumen.ToList();
        var totalGeneral = listaResumen.Sum(r => r.Total);

        using var workbook = new XLWorkbook();

        var hojaResumen = workbook.Worksheets.Add("Resumen");
        EscribirTitulo(hojaResumen, "Reporte de ventas - Resumen por cliente", filtrosAplicados, 6);

        var tablaResumen = CrearTabla(
            hojaResumen,
            "TablaResumen",
            new[] { "Cliente", "Pedidos", "Unidades", "Total vendido", "Ticket promedio", "% del total" },
            listaResumen.Select(r => new XLCellValue[]
            {
                r.Cliente,
                r.Pedidos,
                r.Unidades,
                r.Total,
                r.Pedidos == 0 ? 0 : Math.Round(r.Total / r.Pedidos, 2, MidpointRounding.AwayFromZero),
                totalGeneral == 0 ? 0 : (double)(r.Total / totalGeneral)
            }).ToList());

        tablaResumen.Field("Cliente").TotalsRowLabel = "TOTAL GENERAL";
        tablaResumen.Field("Pedidos").TotalsRowFunction = XLTotalsRowFunction.Sum;
        tablaResumen.Field("Unidades").TotalsRowFunction = XLTotalsRowFunction.Sum;
        tablaResumen.Field("Total vendido").TotalsRowFunction = XLTotalsRowFunction.Sum;
        tablaResumen.Field("% del total").TotalsRowFunction = XLTotalsRowFunction.Sum;
        AplicarFormato(tablaResumen, 4, FormatoMoneda);
        AplicarFormato(tablaResumen, 5, FormatoMoneda);
        AplicarFormato(tablaResumen, 6, FormatoPorcentaje);
        ResaltarCeros(tablaResumen, 4);
        AjustarHoja(hojaResumen);

        var hojaPedidos = workbook.Worksheets.Add("Pedidos");
        EscribirTitulo(hojaPedidos, "Reporte de ventas - Detalle de pedidos", filtrosAplicados, 7);

        var tablaPedidos = CrearTabla(
            hojaPedidos,
            "TablaPedidos",
            new[] { "N° Pedido", "Fecha", "Cliente", "Email", "Productos", "Unidades", "Total" },
            listaPedidos.Select(p => new XLCellValue[]
            {
                p.PedidoId,
                p.Fecha,
                p.Cliente,
                p.Email,
                p.Productos,
                p.Unidades,
                p.Total
            }).ToList());

        tablaPedidos.Field("N° Pedido").TotalsRowLabel = "TOTAL";
        tablaPedidos.Field("Productos").TotalsRowFunction = XLTotalsRowFunction.Sum;
        tablaPedidos.Field("Unidades").TotalsRowFunction = XLTotalsRowFunction.Sum;
        tablaPedidos.Field("Total").TotalsRowFunction = XLTotalsRowFunction.Sum;
        AplicarFormato(tablaPedidos, 2, FormatoFecha);
        AplicarFormato(tablaPedidos, 7, FormatoMoneda);
        tablaPedidos.DataRange.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        tablaPedidos.DataRange.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        ResaltarCeros(tablaPedidos, 7);
        AjustarHoja(hojaPedidos);

        return Guardar(workbook);
    }

    public byte[] GenerarReporteProductos(IEnumerable<ProductoVendidoDto> productos, IEnumerable<DetalleVentaDto> detalle, string filtrosAplicados)
    {
        var listaProductos = productos.ToList();
        var listaDetalle = detalle.ToList();
        var ingresosTotales = listaProductos.Sum(p => p.Ingresos);

        using var workbook = new XLWorkbook();

        var hojaProductos = workbook.Worksheets.Add("Productos");
        EscribirTitulo(hojaProductos, "Reporte de productos - Ranking de ventas", filtrosAplicados, 7);

        var tablaProductos = CrearTabla(
            hojaProductos,
            "TablaProductos",
            new[] { "ID", "Producto", "Descripción", "Precio", "Unidades vendidas", "Ingresos", "% de ingresos" },
            listaProductos.Select(p => new XLCellValue[]
            {
                p.ProductoId,
                p.Producto,
                p.Descripcion ?? "Sin descripción",
                p.Precio,
                p.UnidadesVendidas,
                p.Ingresos,
                ingresosTotales == 0 ? 0 : (double)(p.Ingresos / ingresosTotales)
            }).ToList());

        tablaProductos.Field("ID").TotalsRowLabel = "TOTAL";
        tablaProductos.Field("Unidades vendidas").TotalsRowFunction = XLTotalsRowFunction.Sum;
        tablaProductos.Field("Ingresos").TotalsRowFunction = XLTotalsRowFunction.Sum;
        tablaProductos.Field("% de ingresos").TotalsRowFunction = XLTotalsRowFunction.Sum;
        AplicarFormato(tablaProductos, 4, FormatoMoneda);
        AplicarFormato(tablaProductos, 6, FormatoMoneda);
        AplicarFormato(tablaProductos, 7, FormatoPorcentaje);
        tablaProductos.DataRange.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        tablaProductos.DataRange.Column(6).AddConditionalFormat().DataBar(XLColor.FromHtml("#5B9BD5")).LowestValue().HighestValue();
        ResaltarCeros(tablaProductos, 5);
        AjustarHoja(hojaProductos);

        var hojaDetalle = workbook.Worksheets.Add("Detalle de ventas");
        EscribirTitulo(hojaDetalle, "Reporte de productos - Detalle de ventas", "Líneas de pedido registradas", 7);

        var tablaDetalle = CrearTabla(
            hojaDetalle,
            "TablaDetalle",
            new[] { "N° Pedido", "Fecha", "Cliente", "Producto", "Cantidad", "Precio unitario", "Subtotal" },
            listaDetalle.Select(d => new XLCellValue[]
            {
                d.PedidoId,
                d.Fecha,
                d.Cliente,
                d.Producto,
                d.Cantidad,
                d.PrecioUnitario,
                d.Subtotal
            }).ToList());

        tablaDetalle.Field("N° Pedido").TotalsRowLabel = "TOTAL";
        tablaDetalle.Field("Cantidad").TotalsRowFunction = XLTotalsRowFunction.Sum;
        tablaDetalle.Field("Subtotal").TotalsRowFunction = XLTotalsRowFunction.Sum;
        AplicarFormato(tablaDetalle, 2, FormatoFecha);
        AplicarFormato(tablaDetalle, 6, FormatoMoneda);
        AplicarFormato(tablaDetalle, 7, FormatoMoneda);
        tablaDetalle.DataRange.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        tablaDetalle.DataRange.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        AjustarHoja(hojaDetalle);

        return Guardar(workbook);
    }

    private static void EscribirTitulo(IXLWorksheet hoja, string titulo, string filtrosAplicados, int columnas)
    {
        var rangoTitulo = hoja.Range(1, 1, 1, columnas).Merge();
        rangoTitulo.Value = titulo;
        rangoTitulo.Style.Font.Bold = true;
        rangoTitulo.Style.Font.FontSize = 14;
        rangoTitulo.Style.Font.FontColor = XLColor.White;
        rangoTitulo.Style.Fill.BackgroundColor = ColorPrincipal;
        rangoTitulo.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rangoTitulo.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        hoja.Row(1).Height = 26;

        hoja.Cell(2, 1).Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
        hoja.Cell(3, 1).Value = $"Filtros: {filtrosAplicados}";
        hoja.Range(2, 1, 3, 1).Style.Font.Italic = true;
        hoja.Range(2, 1, 3, 1).Style.Font.FontColor = XLColor.FromHtml("#595959");
    }

    private static IXLTable CrearTabla(IXLWorksheet hoja, string nombre, string[] encabezados, List<XLCellValue[]> filas)
    {
        for (var columna = 0; columna < encabezados.Length; columna++)
            hoja.Cell(FilaEncabezadoTabla, columna + 1).Value = encabezados[columna];

        for (var fila = 0; fila < filas.Count; fila++)
            for (var columna = 0; columna < encabezados.Length; columna++)
                hoja.Cell(FilaEncabezadoTabla + 1 + fila, columna + 1).Value = filas[fila][columna];

        var tabla = hoja.Range(FilaEncabezadoTabla, 1, FilaEncabezadoTabla + filas.Count, encabezados.Length).CreateTable(nombre);
        tabla.Theme = XLTableTheme.TableStyleMedium2;
        tabla.ShowTotalsRow = true;
        tabla.ShowAutoFilter = true;

        tabla.HeadersRow().Style.Font.Bold = true;
        tabla.HeadersRow().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        tabla.TotalsRow().Style.Font.Bold = true;
        tabla.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        tabla.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        tabla.Style.Border.OutsideBorderColor = ColorBorde;
        tabla.Style.Border.InsideBorderColor = ColorBorde;

        return tabla;
    }

    private static void AplicarFormato(IXLTable tabla, int columna, string formato)
    {
        tabla.DataRange.Column(columna).Style.NumberFormat.Format = formato;
        tabla.TotalsRow().Cell(columna).Style.NumberFormat.Format = formato;
    }

    private static void ResaltarCeros(IXLTable tabla, int columna)
    {
        tabla.DataRange.Column(columna).AddConditionalFormat().WhenEquals(0)
            .Fill.SetBackgroundColor(ColorAlertaFondo)
            .Font.SetFontColor(ColorAlertaTexto);
    }

    private static void AjustarHoja(IXLWorksheet hoja)
    {
        hoja.Columns().AdjustToContents(FilaEncabezadoTabla, hoja.LastRowUsed()!.RowNumber());
        foreach (var columna in hoja.ColumnsUsed())
            columna.Width += 3;

        hoja.SheetView.FreezeRows(FilaEncabezadoTabla);
        hoja.PageSetup.PageOrientation = XLPageOrientation.Landscape;
        hoja.PageSetup.FitToPages(1, 0);
    }

    private static byte[] Guardar(XLWorkbook workbook)
    {
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
