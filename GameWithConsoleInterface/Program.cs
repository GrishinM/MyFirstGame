using System;

namespace GameWithConsoleInterface
{
    internal static class Program
    {
        private static void Main()
        {
            var game = new ConsoleInterface();
            game.Start();
        }
    }
}