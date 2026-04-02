namespace GestionConsole.Helpers
{
    public static class ConsoleHelper
    {
        public static void WriteTitle(string title)
        {
            Console.Clear();
            Console.WriteLine("=".PadRight(50, '='));
            Console.WriteLine($"  {title}");
            Console.WriteLine("=".PadRight(50, '='));
            Console.WriteLine();
        }

        public static void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ {message}");
            Console.ResetColor();
        }

        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"✗ {message}");
            Console.ResetColor();
        }

        public static void WriteInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void PressAnyKey()
        {
            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        public static string ReadString(string prompt)
        {
            Console.Write($"{prompt}: ");
            return Console.ReadLine() ?? string.Empty;
        }

        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int result))
                {
                    return result;
                }
                WriteError("Por favor, ingrese un número válido.");
            }
        }

        public static bool ReadBool(string prompt)
        {
            Console.Write($"{prompt} (s/n): ");
            var input = Console.ReadLine()?.ToLower();
            return input == "s" || input == "si" || input == "y" || input == "yes";
        }
    }
}