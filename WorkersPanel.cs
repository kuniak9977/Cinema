using Cinema.Models;
using Spectre.Console;
using System;
using System.Collections;
using System.Collections.Generic;
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

            string name = string.Empty;

            Tree root = new Tree("ROOT");
            Employee emp = _employers.First();

            //root = DFSWritingOutWorkers(emp, root);
            /*
            foreach (Employee e in _employers)
            {  
                name = e.Name + e.Surname;
                if (e.Role == Employee.Occupation.Menedżer)
                    root = new Tree(new Markup($"[seagreen1_1]{e.Name}[/]"));
            }*/

            AnsiConsole.Write(root);




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

                    break;
                case "Podgląd hierarchii":

                    break;
                case "Powrót":
                    return true;
            }


            return false;
        }

        Tree DFSWritingOutWorkers(Employee _employee, Tree _tree)
        {
            if (_tree == null)
                _tree = new Tree($"{_employee.Name} " + $"{_employee.Surname}");

            List<Employee> el = _employee.ListOfEmployees;
            if (el == null)
                return _tree;
            foreach (Employee e in el)
            {
                _tree = DFSWritingOutWorkers(e, _tree);
            }

            if (_employee == null)
                return _tree;
            return _tree;
        }

        void AddWorkerToEmployee(List<Employee> _list)
        {
            Employee[] workers = _list.ToArray();
            string[] strings = new string[workers.Length];
            for (int i = 0; i < _list.Count; i++)
            {
                strings[i] = workers[i].ToString();
            }
            int y = Console.CursorTop;
            string choosenWorker = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Wybierz pracownika do którego dodasz podwładnego:")
                .AddChoices(strings));
            ClearConsolepart(y, y + 20);

            var multiSelection = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                .Title($"Przydzielasz pracowników do [red]{choosenWorker}[/]")
                .PageSize(10)
                .InstructionsText(
                    "Spacja - wybierz pracownika" + 
                    "Enter - zatwierdź wybór")
                .AddChoices(strings.SkipWhile(s => s.StartsWith(choosenWorker))));

            Employee toAddEmployees = _list.Find(s => s.NormalizedName == choosenWorker);
            List<Employee> listOfAddingEmp = new List<Employee>();

            foreach (string s in multiSelection)
            {
                s.ToUpper();
                Employee e = _list.Find(x => x.NormalizedName == s);
                toAddEmployees.AddToList(e);
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
                    r = 0;
                    break;
                case "Dyrektor":
                    r = 1;
                    break;
                case "Kierownik":
                    r = 2;
                    break;
                case "Menedżer":
                    r = 3;
                    break;
                case "Specjalista":
                    r = 4;
                    break;
                case "Pracownik":
                    r = 5;
                    break;
                default: r = 0; break;
            }

            Employee employee = new Employee(name, surname, code, r);
            _list.Add(employee);
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
