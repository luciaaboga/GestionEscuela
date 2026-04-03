using ApiClient = GestionApiClient.GestionApiClient;
using GestionApiClient.Exceptions;
using GestionApiClient.Models.Dtos;
using GestionConsole.Helpers;

namespace GestionConsole.Menus
{
    public class AsistenciaMenu
    {
        private readonly ApiClient _client;

        public AsistenciaMenu(ApiClient client)
        {
            _client = client;
        }

        public async Task ShowAsync()
        {
            bool exit = false;
            while (!exit)
            {
                ConsoleHelper.WriteTitle("GESTIÓN DE ASISTENCIAS");
                Console.WriteLine("1. Registrar asistencia (Presente/Ausente)");
                Console.WriteLine("2. Ver asistencias de hoy");
                Console.WriteLine("3. Ver resumen de asistencias de hoy");
                Console.WriteLine("4. Ver asistencias por fecha específica");
                Console.WriteLine("5. Ver resumen por fecha específica");
                Console.WriteLine("6. Volver al menú principal");
                Console.WriteLine();

                var option = ConsoleHelper.ReadInt("Seleccione una opción");

                switch (option)
                {
                    case 1:
                        await RegistrarAsistenciaAsync();
                        break;
                    case 2:
                        await VerAsistenciasHoyAsync();
                        break;
                    case 3:
                        await VerResumenHoyAsync();
                        break;
                    case 4:
                        await VerAsistenciasPorFechaAsync();
                        break;
                    case 5:
                        await VerResumenPorFechaAsync();
                        break;
                    case 6:
                        exit = true;
                        break;
                    default:
                        ConsoleHelper.WriteError("Opción no válida");
                        ConsoleHelper.PressAnyKey();
                        break;
                }
            }
        }

        private async Task RegistrarAsistenciaAsync()
        {
            try
            {
                ConsoleHelper.WriteTitle("REGISTRAR ASISTENCIA");

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

                if (!alumnos.Any())
                {
                    ConsoleHelper.WriteInfo("No hay alumnos registrados en este curso. Primero debe agregar alumnos.");
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
                var alumnoIdStr = ConsoleHelper.ReadString("Ingrese el ID del alumno");

                if (!Guid.TryParse(alumnoIdStr, out Guid alumnoId))
                {
                    ConsoleHelper.WriteError("ID no válido");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                var alumnoSeleccionado = alumnos.FirstOrDefault(a => a.Id == alumnoId);
                if (alumnoSeleccionado == null)
                {
                    ConsoleHelper.WriteError("El alumno no pertenece al curso seleccionado.");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("Seleccione el estado de asistencia:");
                Console.WriteLine("1. Presente");
                Console.WriteLine("2. Ausente");
                Console.WriteLine();

                var estadoOption = ConsoleHelper.ReadInt("Opción");

                bool presente = estadoOption == 1;
                string estadoTexto = presente ? "Presente" : "Ausente";

                Console.WriteLine();
                ConsoleHelper.WriteInfo($"Registrando asistencia para {alumnoSeleccionado.Apellido}, {alumnoSeleccionado.Nombre}: {estadoTexto}");

                var asistencia = new RegistrarAsistenciaDto
                {
                    AlumnoId = alumnoId,
                    Presente = presente
                };

                var resultado = await _client.RegistrarAsistenciaAsync(cursoId, asistencia);

                ConsoleHelper.WriteSuccess($"Asistencia registrada exitosamente!");
                ConsoleHelper.WriteInfo($"Fecha: {resultado.Fecha:dd/MM/yyyy}");
                ConsoleHelper.WriteInfo($"Estado: {resultado.Estado}");

                ConsoleHelper.PressAnyKey();
            }
            catch (ApiException ex)
            {
                ConsoleHelper.WriteError($"Error al registrar asistencia: {ex.Message}");
                ConsoleHelper.PressAnyKey();
            }
        }

        private async Task VerAsistenciasHoyAsync()
        {
            try
            {
                ConsoleHelper.WriteTitle("ASISTENCIAS DE HOY");

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
                var cursoIdStr = ConsoleHelper.ReadString("Ingrese el ID del curso");

                if (!Guid.TryParse(cursoIdStr, out Guid cursoId))
                {
                    ConsoleHelper.WriteError("ID no válido");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                var asistencias = await _client.GetAsistenciasHoyAsync(cursoId);

                Console.WriteLine();
                ConsoleHelper.WriteInfo($"Fecha: {DateOnly.FromDateTime(DateTime.Today):dd/MM/yyyy}");
                Console.WriteLine();

                if (!asistencias.Any())
                {
                    ConsoleHelper.WriteInfo("No hay asistencias registradas para hoy.");
                }
                else
                {
                    Console.WriteLine($"{"Alumno",-30} {"Estado",-10}");
                    Console.WriteLine(new string('-', 45));

                    foreach (var asistencia in asistencias)
                    {
                        string estadoColor = asistencia.Presente ? "Presente" : "Ausente";
                        Console.WriteLine($"{asistencia.AlumnoApellido}, {asistencia.AlumnoNombre,-25} {estadoColor}");
                    }
                }

                ConsoleHelper.PressAnyKey();
            }
            catch (ApiException ex)
            {
                ConsoleHelper.WriteError($"Error al obtener asistencias: {ex.Message}");
                ConsoleHelper.PressAnyKey();
            }
        }

        private async Task VerResumenHoyAsync()
        {
            try
            {
                ConsoleHelper.WriteTitle("RESUMEN DE ASISTENCIAS DE HOY");

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
                var cursoIdStr = ConsoleHelper.ReadString("Ingrese el ID del curso");

                if (!Guid.TryParse(cursoIdStr, out Guid cursoId))
                {
                    ConsoleHelper.WriteError("ID no válido");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                var resumen = await _client.GetResumenHoyAsync(cursoId);

                Console.WriteLine();
                ConsoleHelper.WriteInfo($"Fecha: {resumen.Fecha:dd/MM/yyyy}");
                Console.WriteLine();
                Console.WriteLine($"Total de alumnos: {resumen.TotalAlumnos}");
                Console.WriteLine($"Presentes: {resumen.Presentes} ({resumen.PorcentajePresentes:F1}%)");
                Console.WriteLine($"Ausentes: {resumen.Ausentes} ({resumen.PorcentajeAusentes:F1}%)");

                ConsoleHelper.PressAnyKey();
            }
            catch (ApiException ex)
            {
                ConsoleHelper.WriteError($"Error al obtener resumen: {ex.Message}");
                ConsoleHelper.PressAnyKey();
            }
        }

        private async Task VerAsistenciasPorFechaAsync()
        {
            try
            {
                ConsoleHelper.WriteTitle("ASISTENCIAS POR FECHA");

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
                var cursoIdStr = ConsoleHelper.ReadString("Ingrese el ID del curso");

                if (!Guid.TryParse(cursoIdStr, out Guid cursoId))
                {
                    ConsoleHelper.WriteError("ID no válido");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                Console.WriteLine();
                var fechaStr = ConsoleHelper.ReadString("Ingrese la fecha (AAAA-MM-DD)");

                if (!DateOnly.TryParse(fechaStr, out DateOnly fecha))
                {
                    ConsoleHelper.WriteError("Fecha no válida. Use el formato AAAA-MM-DD");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                var asistencias = await _client.GetAsistenciasByCursoYFechaAsync(cursoId, fecha);

                Console.WriteLine();
                ConsoleHelper.WriteInfo($"Fecha: {fecha:dd/MM/yyyy}");
                Console.WriteLine();

                if (!asistencias.Any())
                {
                    ConsoleHelper.WriteInfo("No hay asistencias registradas para esta fecha.");
                }
                else
                {
                    Console.WriteLine($"{"Alumno",-30} {"Estado",-10}");
                    Console.WriteLine(new string('-', 45));

                    foreach (var asistencia in asistencias)
                    {
                        Console.WriteLine($"{asistencia.AlumnoApellido}, {asistencia.AlumnoNombre,-25} {(asistencia.Presente ? "Presente" : "Ausente")}");
                    }
                }

                ConsoleHelper.PressAnyKey();
            }
            catch (ApiException ex)
            {
                ConsoleHelper.WriteError($"Error al obtener asistencias: {ex.Message}");
                ConsoleHelper.PressAnyKey();
            }
        }

        private async Task VerResumenPorFechaAsync()
        {
            try
            {
                ConsoleHelper.WriteTitle("RESUMEN DE ASISTENCIAS POR FECHA");

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
                var cursoIdStr = ConsoleHelper.ReadString("Ingrese el ID del curso");

                if (!Guid.TryParse(cursoIdStr, out Guid cursoId))
                {
                    ConsoleHelper.WriteError("ID no válido");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                Console.WriteLine();
                var fechaStr = ConsoleHelper.ReadString("Ingrese la fecha (AAAA-MM-DD)");

                if (!DateOnly.TryParse(fechaStr, out DateOnly fecha))
                {
                    ConsoleHelper.WriteError("Fecha no válida. Use el formato AAAA-MM-DD");
                    ConsoleHelper.PressAnyKey();
                    return;
                }

                var resumen = await _client.GetResumenAsistenciaAsync(cursoId, fecha);

                Console.WriteLine();
                ConsoleHelper.WriteInfo($"Fecha: {resumen.Fecha:dd/MM/yyyy}");
                Console.WriteLine();
                Console.WriteLine($"Total de alumnos: {resumen.TotalAlumnos}");
                Console.WriteLine($"Presentes: {resumen.Presentes} ({resumen.PorcentajePresentes:F1}%)");
                Console.WriteLine($"Ausentes: {resumen.Ausentes} ({resumen.PorcentajeAusentes:F1}%)");

                ConsoleHelper.PressAnyKey();
            }
            catch (ApiException ex)
            {
                ConsoleHelper.WriteError($"Error al obtener resumen: {ex.Message}");
                ConsoleHelper.PressAnyKey();
            }
        }
    }
}