using ApiClient = GestionApiClient.GestionApiClient;
using GestionApiClient.Exceptions;
using GestionApiClient.Models.Dtos;
using GestionConsole.Helpers;

namespace GestionConsole.Menus
{
    public class AlumnoMenu
    {
        private readonly ApiClient _client;

        public AlumnoMenu(ApiClient client)
        {
            _client = client;
        }

        public async Task ShowAsync()
        {
            bool exit = false;
            while (!exit)
            {
                ConsoleHelper.WriteTitle("GESTIÓN DE ALUMNOS");
                Console.WriteLine("1. Listar alumnos de un curso");
                Console.WriteLine("2. Agregar alumno a un curso");
                Console.WriteLine("3. Eliminar alumno");
                Console.WriteLine("4. Volver al menú principal");
                Console.WriteLine();

                var option = ConsoleHelper.ReadInt("Seleccione una opción");

                switch (option)
                {
                    case 1:
                        await ListAlumnosByCursoAsync();
                        break;
                    case 2:
                        await AddAlumnoToCursoAsync();
                        break;
                    case 3:
                        await DeleteAlumnoAsync();
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

        private async Task ListAlumnosByCursoAsync()
        {
            try
            {
                ConsoleHelper.WriteTitle("LISTAR ALUMNOS POR CURSO");

                var cursos = await _client.GetAllCursosAsync();

                if (!cursos.Any())
                {
                    ConsoleHelper.WriteInfo("No hay cursos registrados. Primero debe crear un curso.");
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
                var cursoIdStr = ConsoleHelper.ReadString("Ingrese el ID del curso");

                if (!Guid.TryParse(cursoIdStr, out Guid cursoId))
                {
                    ConsoleHelper.WriteError("ID no válido");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                var alumnos = await _client.GetAlumnosByCursoIdAsync(cursoId);

                Console.WriteLine();

                if (!alumnos.Any())
                {
                    ConsoleHelper.WriteInfo("No hay alumnos registrados en este curso.");
                }
                else
                {
                    Console.WriteLine($"{"ID",-36} {"Apellido",-15} {"Nombre",-15} {"Email",-30}");
                    Console.WriteLine(new string('-', 100));

                    foreach (var alumno in alumnos)
                    {
                        Console.WriteLine($"{alumno.Id,-36} {alumno.Apellido,-15} {alumno.Nombre,-15} {alumno.Email,-30}");
                    }
                }

                ConsoleHelper.PressAnyKey();
            }
            catch (ApiException ex)
            {
                ConsoleHelper.WriteError($"Error al obtener alumnos: {ex.Message}");
                ConsoleHelper.PressAnyKey();
            }
        }

        private async Task AddAlumnoToCursoAsync()
        {
            try
            {
                ConsoleHelper.WriteTitle("AGREGAR ALUMNO A CURSO");

                var cursos = await _client.GetAllCursosAsync();

                if (!cursos.Any())
                {
                    ConsoleHelper.WriteInfo("No hay cursos registrados. Primero debe crear un curso.");
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
                var cursoIdStr = ConsoleHelper.ReadString("Ingrese el ID del curso");

                if (!Guid.TryParse(cursoIdStr, out Guid cursoId))
                {
                    ConsoleHelper.WriteError("ID no válido");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                Console.WriteLine();
                ConsoleHelper.WriteInfo("Datos del alumno:");
                Console.WriteLine();

                var nuevoAlumno = new AddAlumnoToCursoDto
                {
                    Nombre = ConsoleHelper.ReadString("Nombre"),
                    Apellido = ConsoleHelper.ReadString("Apellido"),
                    Email = ConsoleHelper.ReadString("Email")
                };

                Console.WriteLine();
                ConsoleHelper.WriteInfo($"Agregando alumno: {nuevoAlumno.Apellido}, {nuevoAlumno.Nombre}");

                var alumnoCreado = await _client.AddAlumnoToCursoAsync(cursoId, nuevoAlumno);

                ConsoleHelper.WriteSuccess($"Alumno agregado exitosamente!");
                ConsoleHelper.WriteInfo($"ID: {alumnoCreado.Id}");
                ConsoleHelper.WriteInfo($"Curso: {alumnoCreado.CursoNombre}");

                ConsoleHelper.PressAnyKey();
            }
            catch (ApiException ex)
            {
                ConsoleHelper.WriteError($"Error al agregar alumno: {ex.Message}");
                ConsoleHelper.PressAnyKey();
            }
        }

        private async Task DeleteAlumnoAsync()
        {
            try
            {
                ConsoleHelper.WriteTitle("ELIMINAR ALUMNO");

                var cursos = await _client.GetAllCursosAsync();

                if (!cursos.Any())
                {
                    ConsoleHelper.WriteInfo("No hay cursos registrados.");
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
                var cursoIdStr = ConsoleHelper.ReadString("Ingrese el ID del curso para ver sus alumnos");

                if (!Guid.TryParse(cursoIdStr, out Guid cursoId))
                {
                    ConsoleHelper.WriteError("ID no válido");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                var alumnos = await _client.GetAlumnosByCursoIdAsync(cursoId);

                if (!alumnos.Any())
                {
                    ConsoleHelper.WriteInfo("No hay alumnos en este curso.");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("Alumnos del curso:");
                Console.WriteLine();

                foreach (var alumno in alumnos)
                {
                    Console.WriteLine($"[{alumno.Id}] - {alumno.Apellido}, {alumno.Nombre} - {alumno.Email}");
                }

                Console.WriteLine();
                var alumnoIdStr = ConsoleHelper.ReadString("Ingrese el ID del alumno a eliminar");

                if (!Guid.TryParse(alumnoIdStr, out Guid alumnoId))
                {
                    ConsoleHelper.WriteError("ID no válido");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                var confirm = ConsoleHelper.ReadBool($"¿Está seguro que desea eliminar este alumno?");

                if (!confirm)
                {
                    ConsoleHelper.WriteInfo("Eliminación cancelada.");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                var result = await _client.DeleteAlumnoAsync(alumnoId);

                if (result)
                {
                    ConsoleHelper.WriteSuccess("Alumno eliminado exitosamente!");
                }
                else
                {
                    ConsoleHelper.WriteError("No se encontró el alumno especificado.");
                }

                ConsoleHelper.PressAnyKey();
            }
            catch (ApiException ex)
            {
                ConsoleHelper.WriteError($"Error al eliminar alumno: {ex.Message}");
                ConsoleHelper.PressAnyKey();
            }
        }
    }
}