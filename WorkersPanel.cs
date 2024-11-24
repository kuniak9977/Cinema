using Cinema.Models;
using Spectre.Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
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
                .AddChoices("Dodaj nowego pracownika", "Dodaj podwładnego do pracownika","Modyfikuj rekord pracownika", "Przeglądaj bazę", "Powrót"));

            switch (action)
            {
                case "Dodaj nowego pracownika":
                    AddEmployeeToDatabase(_employers);
                    break;
                case "Dodaj podwładnego do pracownika":
                    AddWorkerToEmployee(_employers);
                    break;
                case "Modyfikuj rekord pracownika":
                    ModifyEmployee(_employers);
                    break;
                case "Przeglądaj bazę":
                    EmployeeDatabaseReview(_employers);
                    break;
                case "Powrót":
                    return true;
            }


            return false;
        }

        void EmployeeDatabaseReview(List<Employee> _list)
        {
            ClearConsolePart(13,40);
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
            Console.WriteLine("Wciśnij dowolny przycisk, aby wrócić...");
            Console.ReadLine();
            ClearConsolePart(13, 30);
        }

        void ModifyEmployee(List<Employee> _list)
        {
            ClearConsolePart(12, Console.WindowHeight);

            // Wybierz pracownika do modyfikacji
            Employee emp = ChooseEmploye(_list, "Wybierz pracownika do modyfikacji w bazie:");
            int idx = _list.IndexOf(emp);

            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();

            grid.AddRow(new Markup($"[purple]Stare dane[/]"));
            grid.Centered();
            grid.Expand();

            grid.AddRow($"Imię: {emp.Name}");
            grid.AddRow($"Nazwisko: {emp.Surname}");
            grid.AddRow($"Stanowisko: {emp.Role}");
            grid.AddRow($"Prywatny kod: {emp.EmployeePrivateCode}");

            AnsiConsole.Write(grid);

            Console.WriteLine("Wstaw x jeśli nie chcesz zmieniać");
            Console.CursorVisible = true;

            // Pobierz nowe dane lub zachowaj stare, jeśli wpisano 'x'
            Console.Write("Nowe imię: ");
            string newName = Console.ReadLine()?.Trim();
            if (!string.Equals(newName, "x", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(newName))
            {
                emp.Name = newName;
            }

            Console.Write("Nowe nazwisko: ");
            string newSurname = Console.ReadLine()?.Trim();
            if (!string.Equals(newSurname, "x", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(newSurname))
            {
                emp.Surname = newSurname;
            }

            Console.Write("Nowe stanowisko (podaj numer z listy dostępnych ról): ");
            string newRoleInput = Console.ReadLine()?.Trim();
            if (!string.Equals(newRoleInput, "x", StringComparison.OrdinalIgnoreCase) && int.TryParse(newRoleInput, out int newRoleValue))
            {
                if (Enum.IsDefined(typeof(Employee.Occupation), newRoleValue))
                {
                    emp.Role = (Employee.Occupation)newRoleValue;
                }
                else
                {
                    Console.WriteLine("Niepoprawna rola. Pomijam zmianę.");
                }
            }

            Console.Write("Nowy prywatny kod: ");
            string newCodeInput = Console.ReadLine()?.Trim();
            if (!string.Equals(newCodeInput, "x", StringComparison.OrdinalIgnoreCase) && short.TryParse(newCodeInput, out short newCode))
            {
                emp.SetEncryptedCode(newCode);
            }

            // Zastąp zmodyfikowanego pracownika w liście
            _list[idx] = emp;

            Console.WriteLine("Dane zostały zmodyfikowane.");
        }
        void AddWorkerToEmployee(List<Employee> _list)
        {
            Employee emp = ChooseEmploye(_list, "Wybierz pracownika do którego dodasz podwładnych:");
            if (emp == null)
                return;
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
            string name, surname;
            int code;
            do
                Console.WriteLine("Podaj imię pracownika:");
            while ((name = Console.ReadLine()) == string.Empty);
            do
                Console.WriteLine("Podaj nazwisko pracownika:");
            while ((surname = Console.ReadLine()) == string.Empty);
            do
            {
                Console.WriteLine("Stwórz 4-cyfrowy indywidualy kod pracownika");
                code = short.Parse(Console.ReadLine());
            } while (!(code.ToString().Length == 4));
            
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

            Employee employee = new Employee(name, surname, (short)code, r);
            _list.Add(employee);
            Console.WriteLine("Dodano poprawnie pracownika.");
            Console.ReadLine();
            ClearConsolePart(13, Console.WindowHeight);
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

        public void ClearConsolePart(int _oldY, int _newY)
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
