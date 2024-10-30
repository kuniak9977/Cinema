using Cinema.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    internal class RoomPanelAdm
    {
        public RoomPanelAdm() { }

        public void RoomPanel(Database _database)
        {
            Console.SetCursorPosition(0, 13);
            ClearConsolepart(13, 30);
            Console.WriteLine("Zarządzanie salami. Najpierw wybierasz sale, a następnie czynność.");

            List<Room> list = _database.RoomList;
            string[] rooms = new string[list.Count];
            int i = 0;
            foreach (Room room in list)
            {
                rooms[i++] = room.Name;
            }

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Wybierz sale:")
                .AddChoices(rooms));

            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Wybierz działanie")
                .AddChoices(new[] { "Przegląd sali", "Zaplanuj film", "Zobacz historię filmów" }));

            switch (action)
            {
                case "Przegląd sali":
                    RoomReview(_database, selection);
                    break;
                case "Zaplanuj film":
                    break;
            }
        }

        void RoomReview(Database _database, string _selection)
        {
            Console.SetCursorPosition(0, 13);

            List<Room> list = _database.RoomList;

            int col = 22;

            Grid roomgrid = new Grid();
            roomgrid.Expand();
            roomgrid.Centered();
            roomgrid.Width(Console.WindowWidth);

            Markup taken = new Markup("{[yellow]X[/]}");
            Markup free = new Markup("{ }");
            Markup broken = new Markup("{[Red]B[/]}");

            Room room = list.Find(r => r.Name == _selection);

            string[] gridColumsHeader = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX", "" };
            roomgrid.AddColumns(22);
            roomgrid.AddRow(gridColumsHeader);
            roomgrid.Alignment(Justify.Center);

            int quantity = room.ChairsQuantity;
            int checkedSeats = 0;

            for (int i = 0; i < quantity; i++)
            {
                Markup[] insertRow = new Markup[col];
                insertRow[0] = new Markup("");
                insertRow[21] = new Markup("");
                for (int j = 1; j < col - 1; j++)
                {
                    if (room.States[checkedSeats] == Room.State.Free)
                    {
                        insertRow[j] = free;
                    }
                    else if (room.States[checkedSeats] == Room.State.Taken)
                    {
                        insertRow[j] = taken;
                    }
                    else
                    {
                        insertRow[j] = broken;
                    }
                    checkedSeats++;
                    quantity--;
                }

                roomgrid.AddRow(insertRow);

            }

            AnsiConsole.Write(roomgrid);
        }

        void ClearConsolepart(int _oldY, int _newY)
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
