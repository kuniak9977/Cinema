using Cinema.Models;
using Spectre.Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cinema.Models.Employee;
using static Cinema.RoomPanelAdm;

namespace Cinema
{
    public class WorkersPanel
    {
        public WorkersPanel() { }
        public bool WorkersReview(List<Employee> _employers)
        {
            SortByRole(_employers);

            string action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Akcje do wybrania w tym panelu")
                .AddChoices("Dodaj nowego pracownika", "Dodaj podwładnego do pracownika", "Przeglądaj bazę", "Podgląd hierarchii", "Powrót"));

            switch (action)
            {
                case "Dodaj nowego pracownika":
                    AddEmployeeToDatabase(_employers);
                    break;
                case "Dodaj podwładnego do pracownika":
                    AddWorkerToEmployee(_employers);
                    break;
                case "Przeglądaj bazę":
                    EmployeeDatabaseReview(_employers);
                    break;
                case "Podgląd hierarchii":

                    break;
                case "Powrót":
                    return true;
            }


            return false;
        }

        void EmployeeDatabaseReview(List<Employee> _list)
        {
            ClearConsolepart(13,40);
            var table = new Table();
            table.Title("Lista zatrudnionych pracowników");
            table.Border(TableBorder.Markdown);
            table.Expand();
            table.Centered();

            TableColumn[] columns = { new TableColumn(new Markup($"[yellow]Imię[/]")), new TableColumn(new Markup($"[yellow]Nazwisko[/]")), new TableColumn(new Markup($"[yellow]Stanowisko[/]")) };
            table.AddColumns(columns);

            foreach (Employee e in _list)
            {
                table.AddRow($"{e.Name}", $"{e.Surname}", $"{e.Role}");
            }

            AnsiConsole.Write(table);
        }

        void AddWorkerToEmployee(List<Employee> _list)
        {
            Employee emp = ChooseEmploye(_list, "Wybierz pracownika do którego dodasz podwładnych:");
            Occupation oc = emp.Role;
            List<Employee> newList = new List<Employee>();
            foreach (Employee e in _list)
            {
                if (e == emp)
                    continue;
                if (oc == (e.Role - 1))
                    newList.Add(e);
            }

            string[] strings = EmployeToStringArray(newList);
            string empName = emp.ToString();

            var multiSelection = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                .Title($"Przydzielasz pracowników do [red]{empName}[/]")
                .PageSize(10)
                .InstructionsText(
                    "Spacja - wybierz pracownika" + 
                    "Enter - zatwierdź wybór")
                .AddChoices(strings.SkipWhile(s => s.StartsWith(empName))));

            if (multiSelection.Contains("Powrót"))
                return;

            List<Employee> listOfAddingEmp = new List<Employee>();

            foreach (string s in multiSelection)
            {
                string tmp = s.ToUpper();
                string text = tmp.Split('-')[0].Trim();
                Employee e = _list.Find(x => x.NormalizedName == text);
                emp.AddToList(e);
            }

        }

        void AddEmployeeToDatabase(List<Employee> _list)
        {
            Console.WriteLine("Podaj imię pracownika:");
            string name = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko pracownika:");
            string surname = Console.ReadLine();
            Console.WriteLine("Stwórz 4-cyfrowy indywidualy kod pracownika");
            short code =short.Parse(Console.ReadLine());
            string role = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Wybierz stanowisko:")
                .AddChoices("Pracownik", "Specjalista", "Menedżer", "Kierownik", "Dyrektor", "Nieprzydzielony"));
            int r;
            switch (role)
            {
                case "Nieprzydzielony":
                    r = 5;
                    break;
                case "Dyrektor":
                    r = 0;
                    break;
                case "Kierownik":
                    r = 1;
                    break;
                case "Menedżer":
                    r = 2;
                    break;
                case "Specjalista":
                    r = 3;
                    break;
                case "Pracownik":
                    r = 4;
                    break;
                default: r = 0; break;
            }

            Employee employee = new Employee(name, surname, code, r);
            _list.Add(employee);
        }

        Employee ChooseEmploye(List<Employee> _list, string _promptTitle)
        {
            string[] strings = EmployeToStringArray(_list);
            string choosenWorker = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title($"{_promptTitle}")
                .AddChoices(strings));

            choosenWorker = choosenWorker.ToUpper();
            string text = choosenWorker.Split('-')[0].Trim();

            Employee emp = _list.Find(s => s.NormalizedName == text);
            return emp;
        }

        string[] EmployeToStringArray(List<Employee> _list)
        {
            string[] strings = new string[_list.Count + 1];
            int i = 0;
            foreach (Employee emp in _list)
            {
                strings[i++] = emp.ToString();
            }
            strings[_list.Count] = "Powrót";
            return strings;
        }

        public void ClearConsolepart(int _oldY, int _newY)
        {
            Console.SetCursorPosition(0, _oldY);
            int width = Console.WindowWidth;
            for (int i = _oldY; i <= _newY; i++)
            {
                Console.Write(new string(' ', width));
            }
            Console.SetCursorPosition(0, _oldY);
        }
    }
}
