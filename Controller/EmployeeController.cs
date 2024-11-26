using System.Collections.Generic;
using Cinema.Models;
using Cinema.Views;
using Spectre.Console;

namespace Cinema.Controllers
{
    public class EmployeeController
    {
        private List<Employee> employees;
        private IEmployeeView view;

        public EmployeeController(List<Employee> employees, IEmployeeView view)
        {
            this.employees = employees;
            this.view = view;
        }

        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                string action = view.PromptAction();
                switch (action)
                {
                    case "Dodaj nowego pracownika":
                        AddEmployee();
                        break;
                    case "Dodaj podwładnego do pracownika":
                        AssignSubordinates();
                        break;
                    case "Modyfikuj rekord pracownika":
                        ModifyEmployee();
                        break;
                    case "Przeglądaj bazę":
                        ViewDatabase();
                        break;
                    case "Powrót":
                        exit = true;
                        break;
                }
            }
        }

        public void AddEmployee()
        {
            Employee newEmployee = view.CollectEmployeeData();
            employees.Add(newEmployee);
            SortEmployeesByRole();
            Console.WriteLine("Dodano nowego pracownika.");
        }

        public void AssignSubordinates()
        {
            // Wybierz pracownika, do którego będą przypisani podwładni
            int managerId = view.ChooseEmployee(employees, "Wybierz pracownika, do którego chcesz przypisać podwładnych:");
            var manager = employees.FirstOrDefault(e => e.Id == managerId);

            if (manager == null)
            {
                Console.WriteLine("Nie wybrano pracownika.");
                return;
            }

            // Filtruj pracowników o jeden poziom niżej w hierarchii
            var potentialSubordinates = employees
                .Where(e => e.Role == (Employee.Occupation)((int)manager.Role + 1) && !manager.SubordinateIds.Contains(e.Id))
                .ToList();

            if (!potentialSubordinates.Any())
            {
                Console.WriteLine("Brak dostępnych pracowników o odpowiednim poziomie do przypisania.");
                return;
            }

            // Przygotuj listę opcji do wyboru w MultiSelectionPrompt
            var choices = potentialSubordinates
                .Select(e => $"{e.Id}: {e.Name} {e.Surname} - {e.Role}")
                .ToList();

            // Wyświetl MultiSelectionPrompt
            var selected = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title($"Wybierz podwładnych dla {manager.Name} {manager.Surname}:")
                    .InstructionsText("[grey](Użyj spacji do wyboru, Enter do potwierdzenia)[/]")
                    .AddChoices(choices));

            // Zmapuj wybory na ID i przypisz podwładnych
            foreach (var choice in selected)
            {
                int selectedId = int.Parse(choice.Split(':')[0]); // Parsujemy ID z wybranego tekstu
                manager.SubordinateIds.Add(selectedId);
            }

            Console.WriteLine($"Podwładni zostali przypisani dla {manager.Name} {manager.Surname}.");
        }


        public void SortEmployeesByRole()
        {
            employees.Sort((e1, e2) => e1.Role.CompareTo(e2.Role));
        }

        public void ModifyEmployee()
        {
            int employeeId = view.ChooseEmployee(employees, "Wybierz pracownika do modyfikacji:");
            var employee = employees.Find(e => e.Id == employeeId);

            if (employee != null)
            {
                Console.WriteLine("Podaj nowe imię (pozostaw puste, aby zachować stare):");
                string newName = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    employee.Name = newName;
                }

                Console.WriteLine("Podaj nowe nazwisko (pozostaw puste, aby zachować stare):");
                string newSurname = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(newSurname))
                {
                    employee.Surname = newSurname;
                }

                Console.WriteLine("Podaj nowe stanowisko (0-Dyrektor, 1-Kierownik, itd.):");
                if (int.TryParse(Console.ReadLine(), out int newRole))
                {
                    employee.Role = (Employee.Occupation)newRole;
                }

                Console.WriteLine("Dane zostały zmodyfikowane.");
            }
        }

        private void ViewDatabase()
        {
            view.DisplayEmployees(employees);
        }
    }
}
