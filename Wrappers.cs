using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public ConsoleKeyInfo ReadKey() => Console.ReadKey();
        public string ReadLine() => Console.ReadLine();
        public void WriteLine(string message) => Console.WriteLine(message);
        public void Write(string message) => Console.Write(message);
        public void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);
        public int WindowWidth => Console.WindowWidth;
    }

    public class AnsiConsoleWrapper : IAnsiConsoleWrapper
    {
        public string Prompt(SelectionPrompt<string> prompt) => AnsiConsole.Prompt(prompt);
        public List<string> MultiPrompt(MultiSelectionPrompt<string> prompt) => AnsiConsole.Prompt(prompt);
        public void Write(Table table) => AnsiConsole.Write(table);
    }
}
