# GestionEscuela
Actividad Zinclas

El Sistema de Gestión es una aplicación completa desarrollada en .NET 8 que permite administrar cursos, alumnos y asistencias. La aplicación sigue una arquitectura de tres capas bien diferenciadas: una API backend (GestionApi) que expone endpoints REST, una librería cliente (GestionApiClient) que actúa como puente de comunicación, y una aplicación de consola (GestionConsole) que sirve como interfaz de usuario. El sistema utiliza Entity Framework Core para la persistencia de datos en SQL Server, implementa DTOs para la transferencia segura de información, y sigue principios de arquitectura limpia con separación de responsabilidades mediante servicios y controladores.

Tecnologías y Librerías Utilizadas
El proyecto está construido sobre .NET 8 (Long Term Support). Para el acceso a datos se utilizó Entity Framework Core 8.0, que permite trabajar con la base de datos de manera orientada a objetos mediante LINQ, junto con el proveedor Microsoft.EntityFrameworkCore.SqlServer para la conexión con SQL Server. Las migraciones de base de datos se gestionan con Microsoft.EntityFrameworkCore.Design y Microsoft.EntityFrameworkCore.Tools, que facilitan la creación y actualización del esquema de la base de datos. Para la serialización de datos en formato JSON, tanto la API como el cliente utilizan System.Text.Json, el serializador nativo de .NET. En la capa de API se incorporó Swagger (Swashbuckle.AspNetCore) para la documentación interactiva de los endpoints. La aplicación de consola utiliza únicamente las bibliotecas estándar de .NET sin dependencias adicionales.

Funcionalidades del Sistema
Gestión de Cursos
El sistema permite crear nuevos cursos especificando año, división y especialidad. Permite listar todos los cursos registrados en formato tabular mostrando ID, año, división y especialidad. También posibilita eliminar cursos existentes mediante su identificador único, con confirmación previa para evitar eliminaciones accidentales.

Gestión de Alumnos
Para la administración de alumnos, el sistema permite agregar estudiantes a un curso específico, requiriendo nombre, apellido y email. Antes de agregar un alumno, verifica que el curso seleccionado exista y que el email no esté duplicado en el sistema. Permite listar todos los alumnos pertenecientes a un curso particular, mostrando sus datos completos. También implementa la eliminación de alumnos con confirmación previa.

Gestión de Asistencias
Permite registrar la asistencia de un estudiante marcándolo como presente o ausente, y aplica una regla de negocio fundamental: cada estudiante solo puede tener una asistencia por día en un curso determinado. Si se intenta registrar nuevamente al mismo alumno en el mismo día, el sistema actualiza el registro existente en lugar de crear uno duplicado. El sistema permite visualizar todas las asistencias de un curso en el día actual o en una fecha específica. Además, ofrece un resumen estadístico que muestra el total de alumnos del curso, la cantidad de presentes, la cantidad de ausentes y los porcentajes correspondientes.

La base de datos se compone de tres tablas principales. La tabla Cursos almacena los datos básicos de cada curso: identificador único (GUID), año, división y especialidad. La tabla Alumnos contiene los datos personales de los estudiantes: identificador, nombre, apellido, email, fecha de registro, y una clave foránea CursoId que establece la relación con la tabla Cursos. La tabla Asistencias registra el control de presencia con los campos: identificador, fecha (solo la fecha sin hora), estado (presente o ausente), fecha y hora de registro, y las claves foráneas AlumnoId y CursoId. Se implementó un índice único compuesto sobre AlumnoId, CursoId y Fecha para garantizar que no existan duplicados de asistencia por día. La relación entre Alumnos y Cursos es de muchos a uno, mientras que Asistencias se relaciona con ambas tablas manteniendo la integridad referencial.

Pasos para Ejecutar la Aplicación
Primero debe configurarse la cadena de conexión a la base de datos. En el proyecto GestionApi, dentro del archivo appsettings.json, debe verificarse o modificarse la sección ConnectionStrings. Para usar SQL Server LocalDB, la cadena por defecto funciona sin cambios. Si se utiliza otra instancia de SQL Server, debe ajustarse el nombre del servidor correspondientemente.

Luego deben aplicarse las migraciones para crear la base de datos. Esto puede hacerse desde la Consola del Administrador de Paquetes en Visual Studio, seleccionando GestionApi como proyecto por defecto y ejecutando el comando Update-Database. Alternativamente, desde la terminal en la carpeta del proyecto GestionApi puede ejecutarse dotnet ef database update. Esto creará la base de datos con todas las tablas y relaciones necesarias.

Antes de ejecutar la aplicación, debe verificarse la URL base de la API. Cuando se ejecuta GestionApi, Visual Studio muestra una URL en la barra de direcciones, típicamente https://localhost:7098 o un puerto similar. Esta URL debe copiarse y reemplazarse en el archivo Program.cs del proyecto GestionConsole, en la variable apiBaseUrl.

Finalmente, deben ejecutarse ambos proyectos. Primero se inicia GestionApi. La API quedará corriendo en segundo plano. Luego, se selecciona GestionConsole.

Uso de la Aplicación
Al iniciar GestionConsole, se presenta un menú principal con cuatro opciones: Gestión de Cursos, Gestión de Alumnos, Gestión de Asistencias y Salir.

En el menú de Cursos, se pueden listar los cursos existentes, crear nuevos cursos ingresando año, división y especialidad, o eliminar cursos existentes seleccionándolos por su identificador. 

En el menú de Alumnos, primero debe seleccionarse un curso existente. Luego pueden listarse los alumnos de ese curso, agregar nuevos alumnos proporcionando nombre, apellido y email, o eliminar alumnos existentes.

En el menú de Asistencias, se dispone de varias opciones. Para registrar asistencia, se selecciona un curso, luego un alumno de ese curso, y se elige entre presente o ausente. El sistema permite ver las asistencias del día actual o de una fecha específica, mostrando una lista con todos los alumnos que ya tienen registro. También se pueden consultar resúmenes estadísticos que incluyen total de alumnos, cantidad y porcentaje de presentes y ausentes. Si se registra nuevamente a un alumno en el mismo día, el sistema actualiza su estado anterior en lugar de crear un duplicado, lo que permite corregir asistencias mal cargadas.