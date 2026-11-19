using System.Text;
using Lab13_GabrielNA.Consola.Ejemplos;

Console.OutputEncoding = Encoding.UTF8;

var ejemplos = new EjemplosClosedXml();
string? opcion;

do
{
    Console.WriteLine();
    Console.WriteLine("===== LAB 13 - ClosedXML =====");
    Console.WriteLine("1. Crear un archivo Excel");
    Console.WriteLine("2. Leer y modificar el archivo Excel");
    Console.WriteLine("3. Crear una tabla en Excel");
    Console.WriteLine("4. Aplicar formato de celda");
    Console.WriteLine("0. Salir");
    Console.Write("Elige una opción: ");
    opcion = Console.ReadLine()?.Trim();

    switch (opcion)
    {
        case "1":
            ejemplos.CrearArchivoExcel();
            break;
        case "2":
            ejemplos.ModificarArchivoExcel();
            break;
        case "3":
            ejemplos.CrearTablaExcel();
            break;
        case "4":
            ejemplos.AplicarFormatoCelda();
            break;
        case "0":
            Console.WriteLine("Fin del programa.");
            break;
        default:
            Console.WriteLine("Opción no válida.");
            break;
    }
} while (opcion is not null && opcion != "0");
