using System;
using System.Text;

namespace CrossMail.Utils
{
    class ProcessUtils
    {
        public static string GenerateRandomText(int length) {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++) {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

        public static void ConsoleLog(string text)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(text);
            Console.ResetColor();
        }

    }

}