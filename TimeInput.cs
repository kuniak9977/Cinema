using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class TimeInput
    {
        FigletText[] H1 = { new FigletText("0"), new FigletText("1"), new FigletText("2") };
        FigletText[] H2 = { new FigletText("0"), new FigletText("1"), new FigletText("2"), new FigletText("3"), new FigletText("4"), new FigletText("5"), new FigletText("6")
        , new FigletText("7"), new FigletText("8"), new FigletText("9")};
        FigletText[] M1 = { new FigletText("0"), new FigletText("1"), new FigletText("2"), new FigletText("3"), new FigletText("4"), new FigletText("5") };
        FigletText[] M2 = { new FigletText("0"), new FigletText("1"), new FigletText("2"), new FigletText("3"), new FigletText("4"), new FigletText("5"), new FigletText("6")
        , new FigletText("7"), new FigletText("8"), new FigletText("9")};
        FigletText point = new FigletText(":");

        short[] maxValue = { 2, 9, 5, 9 };

        static Style cursor = new Style(Color.Lime, Color.Black);
        static Style ordinary = new Style(Color.White, Color.Black);

        Text empty = new Text("           ");
        Text plus = new Text("+++++", ordinary);
        Text plusC = new Text("+++++", cursor);
        Text minus = new Text("-----", ordinary);
        Text minusC = new Text("-----", cursor);

        public TimeInput() { }

        public void DrawTimeInputPanel()
        {
            int width = Console.WindowWidth;
            int heightPos = Console.CursorTop;
            short option = 0;
            short[] options = { 0, 0, 0, 0 };
            ConsoleKeyInfo cki;

            while (true)
            {
                Console.SetCursorPosition(0, heightPos);
                var grid = new Grid();
                for (int i = 0; i < 7; i++)
                {
                    grid.AddColumn();
                }
                switch (option)
                {
                    case 0:
                        grid.AddRow(empty, plusC.RightJustified(), plus, empty, plus.RightJustified(), plus.LeftJustified(), empty);
                        grid.AddRow(empty, H1[options[0]].RightJustified(), H2[options[1]], point, M1[options[2]].RightJustified(), M2[options[3]].LeftJustified(), empty);
                        grid.AddRow(empty, minusC.RightJustified(), minus, empty, minus.Centered(), minus.LeftJustified(), empty);
                        break;
                    case 1:
                        grid.AddRow(empty, plus.RightJustified(), plusC, empty, plus.RightJustified(), plus.LeftJustified(), empty);
                        grid.AddRow(empty, H1[options[0]].RightJustified(), H2[options[1]], point, M1[options[2]].RightJustified(), M2[options[3]].LeftJustified(), empty);
                        grid.AddRow(empty, minus.RightJustified(), minusC, empty, minus.Centered(), minus.LeftJustified(), empty);
                        break;
                    case 2:
                        grid.AddRow(empty, plus.RightJustified(), plus, empty, plusC.RightJustified(), plus.LeftJustified(), empty);
                        grid.AddRow(empty, H1[options[0]].RightJustified(), H2[options[1]], point, M1[options[2]].RightJustified(), M2[options[3]].LeftJustified(), empty);
                        grid.AddRow(empty, minus.RightJustified(), minus, empty, minusC.Centered(), minus.LeftJustified(), empty);
                        break;
                    case 3:
                        grid.AddRow(empty, plus.RightJustified(), plus, empty, plus.RightJustified(), plusC.LeftJustified(), empty);
                        grid.AddRow(empty, H1[options[0]].RightJustified(), H2[options[1]], point, M1[options[2]].RightJustified(), M2[options[3]].LeftJustified(), empty);
                        grid.AddRow(empty, minus.RightJustified(), minus, empty, minus.Centered(), minusC.LeftJustified(), empty);
                        break;
                }
                grid.Width = width;
                grid.Expand();

                AnsiConsole.Write(grid);

                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.RightArrow)
                    option = (short)((option + 1) % options.Length);
                if (cki.Key == ConsoleKey.LeftArrow)
                    option = (short)((option - 1 + options.Length) % options.Length);

                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                        options[option] = ChangeValue(options[option], true);
                        if (options[option] > maxValue[option])
                            options[option] = 0;
                        break;
                    case ConsoleKey.DownArrow:
                        options[option] = ChangeValue(options[option], false);
                        if (options[option] < 0)
                            options[option] = maxValue[option];
                        break;
                    default:
                        break;
                }

            }
            
        }

        short ChangeValue(short _value, bool _inc)
        {
            if (_inc)
                return ++_value;
            return --_value;
        }
    }
}
