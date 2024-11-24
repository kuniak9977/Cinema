using System.Collections.Generic;
using Cinema.Models;
using Cinema.Views;

namespace Cinema.Controllers
{
    public class EmployeeController
    {
        private List<Employee> employees;
        private EmployeeView view;

        public EmployeeController(List<Employee> employees, EmployeeView view)
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
                        AddSubordinateToEmployee();
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

        private void AddEmployee()
        {
            Employee newEmployee = view.CollectEmployeeData();
            employees.Add(newEmployee);
            SortEmployeesByRole();
            Console.WriteLine("Dodano nowego pracownika.");
        }

        private void AddSubordinateToEmployee()
        {
            int managerId = view.ChooseEmployee(employees, "Wybierz pracownika do którego chcesz dodać podwładnych:");
            int subordinateId = view.ChooseEmployee(employees, "Wybierz podwładnego:");
            var manager = employees.Find(e => e.Id == managerId);
            if (manager != null)
            {
                manager.AddSubordinate(subordinateId);
                Console.WriteLine("Dodano podwładnego.");
            }
        }
        public void SortEmployeesByRole()
        {
            employees.Sort((e1, e2) => e1.Role.CompareTo(e2.Role));
        }

        private void ModifyEmployee()
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
