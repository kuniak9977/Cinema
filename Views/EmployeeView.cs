using System;
using System.Collections.Generic;
using Cinema.Models;
using Spectre.Console;

namespace Cinema.Views
{
    public class EmployeeView
    {
        public string PromptAction()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Akcje do wybrania w tym panelu")
                    .AddChoices("Dodaj nowego pracownika", "Dodaj podwładnego do pracownika", "Modyfikuj rekord pracownika", "Przeglądaj bazę", "Powrót"));
        }

        public void DisplayEmployees(List<Employee> employees)
        {
            ClearConsolePart(13, 40);
            var table = new Table()
                .Title("Lista zatrudnionych pracowników")
                .Border(TableBorder.Markdown)
                .Expand()
                .Centered();

            table.AddColumns(
                new TableColumn("ID").Centered(),
                new TableColumn("Imię").Centered(),
                new TableColumn("Nazwisko").Centered(),
                new TableColumn("Stanowisko").Centered());

            foreach (var e in employees)
            {
                table.AddRow($"{e.Id}", $"{e.Name}", $"{e.Surname}", $"{e.Role}");
            }

            AnsiConsole.Write(table);
            Console.WriteLine("Wciśnij dowolny przycisk, aby wrócić...");
            Console.ReadLine();
            ClearConsolePart(13, 30);
        }

        public Employee CollectEmployeeData()
        {
            Console.WriteLine("Podaj imię pracownika:");
            string name = Console.ReadLine()?.Trim();

            Console.WriteLine("Podaj nazwisko pracownika:");
            string surname = Console.ReadLine()?.Trim();

            short code;
            while (true)
            {
                Console.WriteLine("Podaj 4-cyfrowy kod pracownika (np. 1234):");
                string input = Console.ReadLine()?.Trim();

                // Sprawdzenie czy input jest liczbą i ma dokładnie 4 cyfry
                if (short.TryParse(input, out code) && input.Length == 4)
                {
                    break;
                }

                Console.WriteLine("Kod musi być liczbą składającą się z dokładnie 4 cyfr. Spróbuj ponownie.");
            }

            int role;
            while (true)
            {
                Console.WriteLine("Podaj numer stanowiska (0-Dyrektor, 1-Kierownik, 2-Menedżer, 3-Specjalista, 4-Pracownik, 5-Nieprzydzielony):");
                string input = Console.ReadLine()?.Trim();

                // Sprawdzenie czy role jest liczbą oraz czy mieści się w dozwolonym zakresie
                if (int.TryParse(input, out role) && role >= 0 && role <= 5)
                {
                    break;
                }

                Console.WriteLine("Numer stanowiska musi być liczbą od 0 do 5. Spróbuj ponownie.");
            }

            return new Employee(name, surname, code, role);
        }


        public int ChooseEmployee(List<Employee> employees, string promptTitle)
        {
            var choices = new List<string>();
            foreach (var e in employees)
            {
                choices.Add($"{e.Id}: {e.Name} {e.Surname} - {e.Role}");
            }

            string selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(promptTitle)
                    .AddChoices(choices));
            return int.Parse(selected.Split(':')[0]);
        }

        public void ClearConsolePart(int oldY, int newY)
        {
            Console.SetCursorPosition(0, oldY);
            int width = Console.WindowWidth;
            for (int i = oldY; i <= newY; i++)
            {
                Console.Write(new string(' ', width));
            }
            Console.SetCursorPosition(0, oldY);
        }
    }
}
