using ApiClient = GestionApiClient.GestionApiClient;

using GestionApiClient;
using GestionConsole.Helpers;
using GestionConsole.Menus;

namespace GestionConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Sistema de Gestión - Cursos";

            string apiBaseUrl = "https://localhost:7098";

            var client = new ApiClient(apiBaseUrl);

            bool exit = false;

            while (!exit)
            {
                ConsoleHelper.WriteTitle("SISTEMA DE GESTIÓN");
                Console.WriteLine("1. Gestión de Cursos");
                Console.WriteLine("2. Gestión de Alumnos");
                Console.WriteLine("3. Gestión de Asistencias");
                Console.WriteLine("4. Salir");
                Console.WriteLine();

                var option = ConsoleHelper.ReadInt("Seleccione una opción");

                switch (option)
                {
                    case 1:
                        var cursoMenu = new CursoMenu(client);
                        await cursoMenu.ShowAsync();
                        break;
                    case 2:
                        var alumnoMenu = new AlumnoMenu(client);
                        await alumnoMenu.ShowAsync();
                        break;
                    case 3:
                        var asistenciaMenu = new AsistenciaMenu(client);
                        await asistenciaMenu.ShowAsync();
                        break;
                    case 4:
                        exit = true;
                        ConsoleHelper.WriteSuccess("¡Hasta luego!");
                        break;
                    default:
                        ConsoleHelper.WriteError("Opción no válida");
                        ConsoleHelper.PressAnyKey();
                        break;
                }
            }
        }
    }
}