using ApiClient = GestionApiClient.GestionApiClient;

using GestionApiClient;
using GestionApiClient.Exceptions;
using GestionApiClient.Models.Dtos;
using GestionConsole.Helpers;

namespace GestionConsole.Menus
{
    public class CursoMenu
    {
        private readonly ApiClient _client; 

        public CursoMenu(ApiClient client)  
        {
            _client = client;
        }
        public async Task ShowAsync()
        {
            bool exit = false;
            while (!exit)
            {
                ConsoleHelper.WriteTitle("GESTIÓN DE CURSOS");
                Console.WriteLine("1. Listar todos los cursos");
                Console.WriteLine("2. Crear un nuevo curso");
                Console.WriteLine("3. Eliminar un curso");
                Console.WriteLine("4. Volver al menú principal");
                Console.WriteLine();

                var option = ConsoleHelper.ReadInt("Seleccione una opción");

                switch (option)
                {
                    case 1:
                        await ListAllCursosAsync();
                        break;
                    case 2:
                        await CreateCursoAsync();
                        break;
                    case 3:
                        await DeleteCursoAsync();
                        break;
                    case 4:
                        exit = true;
                        break;
                    default:
                        ConsoleHelper.WriteError("Opción no válida");
                        ConsoleHelper.PressAnyKey();
                        break;
                }
            }
        }

        private async Task ListAllCursosAsync()
        {
            try
            {
                ConsoleHelper.WriteTitle("LISTADO DE CURSOS");

                var cursos = await _client.GetAllCursosAsync();

                if (!cursos.Any())
                {
                    ConsoleHelper.WriteInfo("No hay cursos registrados.");
                }
                else
                {
                    Console.WriteLine($"{"ID",-36} {"Año",-6} {"División",-10} {"Especialidad",-20}");
                    Console.WriteLine(new string('-', 75));

                    foreach (var curso in cursos)
                    {
                        Console.WriteLine($"{curso.Id,-36} {curso.Anio,-6} {curso.Division,-10} {curso.Especialidad,-20}");
                    }
                }

                ConsoleHelper.PressAnyKey();
            }
            catch (ApiException ex)
            {
                ConsoleHelper.WriteError($"Error al obtener cursos: {ex.Message}");
                ConsoleHelper.PressAnyKey();
            }
        }

        private async Task CreateCursoAsync()
        {
            try
            {
                ConsoleHelper.WriteTitle("CREAR NUEVO CURSO");

                var nuevoCurso = new AddCursoDto
                {
                    Anio = ConsoleHelper.ReadString("Año (ej: 1ro, 2do, 3ro)"),
                    Division = ConsoleHelper.ReadString("División (ej: A, B, C)"),
                    Especialidad = ConsoleHelper.ReadString("Especialidad (ej: Informática, Electrónica)")
                };

                Console.WriteLine();
                ConsoleHelper.WriteInfo($"Creando curso: {nuevoCurso.Anio}° {nuevoCurso.Division} - {nuevoCurso.Especialidad}");

                var cursoCreado = await _client.CreateCursoAsync(nuevoCurso);

                ConsoleHelper.WriteSuccess($"Curso creado exitosamente!");
                ConsoleHelper.WriteInfo($"ID: {cursoCreado.Id}");

                ConsoleHelper.PressAnyKey();
            }
            catch (ApiException ex)
            {
                ConsoleHelper.WriteError($"Error al crear curso: {ex.Message}");
                ConsoleHelper.PressAnyKey();
            }
        }

        private async Task DeleteCursoAsync()
        {
            try
            {
                ConsoleHelper.WriteTitle("ELIMINAR CURSO");

                var cursos = await _client.GetAllCursosAsync();

                if (!cursos.Any())
                {
                    ConsoleHelper.WriteInfo("No hay cursos para eliminar.");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                Console.WriteLine("Cursos disponibles:");
                Console.WriteLine();

                foreach (var curso in cursos)
                {
                    Console.WriteLine($"[{curso.Id}] - {curso.Anio}° {curso.Division} - {curso.Especialidad}");
                }

                Console.WriteLine();
                var idStr = ConsoleHelper.ReadString("Ingrese el ID del curso a eliminar");

                if (!Guid.TryParse(idStr, out Guid id))
                {
                    ConsoleHelper.WriteError("ID no válido");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                var confirm = ConsoleHelper.ReadBool($"¿Está seguro que desea eliminar el curso?");

                if (!confirm)
                {
                    ConsoleHelper.WriteInfo("Eliminación cancelada.");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                var result = await _client.DeleteCursoAsync(id);

                if (result)
                {
                    ConsoleHelper.WriteSuccess("Curso eliminado exitosamente!");
                }
                else
                {
                    ConsoleHelper.WriteError("No se encontró el curso especificado.");
                }

                ConsoleHelper.PressAnyKey();
            }
            catch (ApiException ex)
            {
                ConsoleHelper.WriteError($"Error al eliminar curso: {ex.Message}");
                ConsoleHelper.PressAnyKey();
            }
        }
    }
}