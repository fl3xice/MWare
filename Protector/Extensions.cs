using System;

namespace Protector
{
    public static class Extensions
    {

        public static void PrintText(string text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text);
        }
        
        public static void PrintText(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            var beforeForegroundColor = Console.ForegroundColor;
            var beforeBackgroundColor = Console.BackgroundColor;
            
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            
            Console.Write(text);
            
            Console.ForegroundColor = beforeForegroundColor;
            Console.BackgroundColor = beforeBackgroundColor;
        }
    }
}