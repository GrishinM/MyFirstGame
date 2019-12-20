using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using MyGameEngine;

namespace GameWithConsoleInterface
{
    internal class ConsoleInterface
    {
        private static Dictionary<string, Army> armys = new Dictionary<string, Army>();
        private static Dictionary<string, BattleUnitsStack> stacks = new Dictionary<string, BattleUnitsStack>();
        private static Dictionary<String, Unit> units = new Dictionary<string, Unit>();

        private BattleUnitsStack playerStack;

        private int round;

//        private int delta;
        private int players;
        private bool GAME;
        private bool game;
        private bool menu;
        private bool success;

        private int step;

//        private int player;
        private Scale curScale;
        private Scale nextScale;
        private readonly XmlFormatter xml;
        private const string directory = @"C:\2kurs1sem\OOP\Saves";

        public ConsoleInterface()
        {
            xml = new XmlFormatter();
            units = Loader.GetUnits(@"C:\2kurs1sem\OOP", "Units.db");
        }

        public void Start()
        {
            var hConsole = DllImports.GetStdHandle(-11);
            DllImports.SetConsoleDisplayMode(hConsole, 1, out _);
            round = 0;
//            delta = 0;
            players = 0;
            success = false;
            game = false;
            GAME = true;
            menu = true;
            Console.WriteLine("1 - новая игра    2 - загрузить игру");
            switch (Console.ReadLine())
            {
                case "1":
                    NewGame();
                    break;
                case "2":
                    LoadGame();
                    break;
                default:
                    Console.WriteLine("Такой команды нет");
                    break;
            }
        }

        private void NewGame()
        {
            success = false;
            while (!success || players < 2)
            {
                Console.Out.WriteLine("\nВведите количество игроков");
                var a = Console.ReadLine();
                success = Int32.TryParse(a, out players);
                if (!success || players < 2)
                {
                    Console.WriteLine("\nНеправильный формат ввода\n");
                    continue;
                }

                for (var i = 1; i <= players; i++)
                    armys.Add(i.ToString(), new Army(i.ToString()));
            }

            MainMenu();
        }

        private void LoadGame()
        {
            Console.WriteLine("\nВведите название файла");
            var a = Console.ReadLine();
            if (!File.Exists(a + ".xml"))
            {
                Console.WriteLine("\nТакого файла не существует\n");
                Start();
            }

            (armys, stacks) = xml.Open(directory, a, units);
            MainMenu();
        }

        private void SaveGame()
        {
            Console.WriteLine("\nВведите название файла");
            var a = Console.ReadLine();
            xml.Save(directory, a, armys.Values);
            Console.WriteLine("\nИгра успешно сохранена");
            MainMenu();
        }

        private void MainMenu()
        {
            Console.WriteLine();
            PrintRound("ПОДГОТОВКА");
            while (menu)
            {
                Console.WriteLine("\n1 - вывести все юниты    2 - создать стек    3 - вывести все стеки    4 - вывести все армии    5 - добавить стек в армию    " +
                                  "6 - удалить стек из армии    7 - список способностей    8 - список временных модификаторов    9 - шкала инициативы    10 - начать    " +
                                  "11 - сохранить игру    0 - выход");
                var a = Console.ReadLine();
                switch (a)
                {
                    case "0":
                        GAME = false;
                        menu = false;
                        break;
                    case "1":
                        PrintUnits();
                        break;
                    case "2":
                        CreateStack();
                        break;
                    case "3":
                        PrintStacks();
                        break;
                    case "4":
                        PrintArmys();
                        break;
                    case "5":
                        AddStackToArmy();
                        break;
                    case "6":
                        DeleteStackFromArmy();
                        break;
                    case "7":
                        PrintAbilities();
                        break;
                    case "8":
                        PrintTempMods();
                        break;
//                    case "9":
//                        ChangeInitiative();
//                        break;
                    case "9":
                        curScale = new Scale(stacks.Values);
                        Console.WriteLine();
                        PrintInitiative1(curScale);
                        Console.WriteLine();
                        break;
                    case "10":
                        menu = false;
                        break;
                    case "11":
                        SaveGame();
                        break;
                    default:
                        Console.WriteLine("Такой команды нет");
                        break;
                }
            }

            if (GetAliveArmy(out _) == 0)
            {
                Console.WriteLine("\nНИЧЬЯ");
                Console.ReadLine();
                GAME = false;
            }

            IsWinner();
            if (!GAME)
                Environment.Exit(0);

            StartRound();
        }

        private void StartRound()
        {
            step = 0;
            Console.WriteLine();
            PrintRound("РАУНД " + ++round);
            nextScale = new Scale(stacks.Values);
            while (GAME && IsTurn())
            {
                IsWinner();
//                player = (player + 1) % players;
//                var playerArmy = (player + 1).ToString();
//                while (!armys[playerArmy].CanTurn())
//                {
//                    player = (player + 1) % players;
//                    playerArmy = (player + 1).ToString();
//                }
//
//                var playerStack = armys[playerArmy].GetStack();

                curScale = new Scale(stacks.Values);
                playerStack = curScale.GetStack();
                Console.WriteLine($"\nХод {++step}. Очередь игрока {playerStack.Army.Name}. Стек {playerStack.Name}");
                PrintStack(playerStack);
                game = true;
                while (game)
                {
                    Console.WriteLine(
                        "\nВыберите действие\n\n1 - атаковать стек    2 - использовать способность    3 - оборона    4 - ожидание    5 - шкала инициативы    0 - выход");
                    var a = Console.ReadLine();
                    Console.WriteLine();
                    switch (a)
                    {
                        case "0":
                            game = false;
                            GAME = false;
                            break;
                        case "1":
                            game = !Attack(playerStack);
                            break;
                        case "2":
                            game = !Cast(playerStack);
                            break;
                        case "3":
                            game = false;
                            playerStack.Defend();
                            break;
                        case "4":
                            try
                            {
                                curScale.ch(playerStack);
                                game = false;
                            }
                            catch (MyException e)
                            {
                                Console.WriteLine(e.Message);
                            }

                            break;
                        case "5":
                            PrintInitiative1(curScale);
                            Console.Write("| ");
                            PrintInitiative2(nextScale);
                            Console.WriteLine();
                            break;
                        default:
                            Console.WriteLine("Такой команды нет");
                            break;
                    }

                    Console.WriteLine("\n\n");
                }
            }

            if (!GAME)
                Environment.Exit(0);

            foreach (var stack in stacks.Values)
            {
                stack.Initiative = stack.Unit.Initiative;
            }

            IsWinner();
            NextRound();
            menu = true;
            MainMenu();
        }

        private void PrintRound(string s)
        {
            PrintCenter("");
            PrintCenter(s);
            PrintCenter("");
        }

        private void PrintUnits()
        {
            Console.WriteLine();
            foreach (var unit in units.Values)
            {
                PrintCenter("ЮНИТ " + unit.Id);
                PrintUnit(unit);
            }

            Console.WriteLine();
        }

        private void PrintUnit(Unit unit)
        {
            Console.WriteLine(
                $"\nИмя - {unit.Name}, тип - {unit.Type}, здоровье - {unit.Hitpoints}, атака - {unit.Attack}, защита - {unit.Defence}, " +
                $"разброс урона - {unit.Damage.Min}-{unit.Damage.Max}, инициатива - {unit.Initiative}, способности: {String.Join(", ", unit.Abilities)}\n\n\n\n");
        }

        private void CreateStack()
        {
            Console.WriteLine("\nВведите название стека");
            var r = Console.ReadLine();
            if (r == null || stacks.ContainsKey(r))
            {
                Console.WriteLine("\nСтек с таким названием уже существует");
                return;
            }

            Console.WriteLine("\nВведите тип юнита");
            var type = Console.ReadLine();
            if (type==null || !units.ContainsKey(type))
            {
                Console.WriteLine("\nЮнита с таким типом не существует");
                return;
            }

            success = false;
            while (!success)
            {
                Console.WriteLine("\nВведите количество юнитов");
                var t = Console.ReadLine();
                success = Int32.TryParse(t, out var count);
                if (!success || count <= 0 || count >= 1000000)
                {
                    Console.WriteLine("\nНеправильный формат ввода\n");
                    return;
                }

                stacks.Add(r, new BattleUnitsStack(units[type], count, r));
                Console.WriteLine("\nСтек успешно создан");
            }
        }

        private void PrintStacks()
        {
            Console.WriteLine();
            foreach (var stack in stacks.Values)
            {
                PrintCenter("СТЕК " + stack.Name);
                PrintStack(stack);
                Console.WriteLine("\n\n");
            }

            Console.WriteLine();
        }

        private void PrintStack(BattleUnitsStack stack)
        {
            Console.WriteLine(
                $"\nЮнит - {stack.Unit.Id}, количество - {stack.CurrentCount}, здоровье последнего - {stack.CurrentHealth}, инициатива - {stack.Initiative}, временные моды: {String.Join(", ", stack.Mods.Select(mod => String.Format($"{mod.Key} (осталось: {(mod.Value == -1 ? "бесконечно" : mod.Value.ToString())})")))}");
        }

        private void PrintArmys()
        {
            Console.WriteLine();
            foreach (var army in armys.Values)
            {
                PrintCenter("АРМИЯ " + army.Name);
                PrintArmy(army);
                Console.WriteLine("\n\n");
            }

            Console.WriteLine();
        }

        private void PrintArmy(Army army)
        {
            var s = 0;
            Console.WriteLine(String.Join("\n",
                army.Stacks.Select(stack =>
                    String.Format(
                        $"\n{++s}) Стек {stack.Name}, юнит - {stack.Unit.Id}, количество - {stack.CurrentCount}, здоровье последнего - {stack.CurrentHealth}, " +
                        $"инициатива - {stack.Initiative}"))));
        }

        private void PrintInitiative1(Scale scale)
        {
            Console.Write(String.Join("--> ", scale.Stacks.Select(stack => String.Format($"{stack.Name} ({stack.Initiative}) "))));
        }

        private void PrintInitiative2(Scale scale)
        {
            Console.WriteLine(String.Join(" --> ", scale.Stacks.Select(stack => stack.Name)));
        }

        private void AddStackToArmy()
        {
            Console.WriteLine("\nВведите название армии");
            var t = Console.ReadLine();
            if (t == null || !armys.ContainsKey(t))
            {
                Console.WriteLine("\nАрмии с таким названием не существует");
                return;
            }

            if (armys[t].Count == 6)
            {
                Console.WriteLine("\nВ этой армии уже максимальное количество стеков");
                return;
            }

            Console.WriteLine("\nВведите название стека");
            var r = Console.ReadLine();
            if (r == null || !stacks.ContainsKey(r))
            {
                Console.WriteLine("\nСтека с таким названием не существует");
                return;
            }

            if (stacks[r].Army != null)
            {
                Console.WriteLine("\nСтек уже в " + (stacks[r].Army == armys[t] ? "этой" : "другой") + " армии");
                return;
            }

            armys[t].Add(stacks[r]);
            Console.WriteLine("\nСтек успешно добавлен");
        }

        private void DeleteStackFromArmy()
        {
            Console.WriteLine("\nВведите название армии");
            var t = Console.ReadLine();
            if (t == null || !armys.ContainsKey(t))
            {
                Console.WriteLine("\nАрмии с таким названием не существует");
                return;
            }

            PrintArmy(armys[t]);
            Console.WriteLine("\nВведите название стека");
            var r = Console.ReadLine();
            Console.WriteLine(armys[t].Remove(r) ? "\nСтек успешно удален из армии" : "\nВ этой армии нет стека с таким названием");
        }

        private void PrintAbilities()
        {
            PrintCenter("СПОСОБНОСТИ");
            Console.WriteLine(
                $"\n{String.Join("\n", Enum.GetValues(typeof(Abilities)).OfType<Abilities>().Select(ability => String.Format($"{ability} - {GetDescription(ability)} ({(BattleUnitsStack.GetIsActive(ability) ? "активная" : "пассивная")})")))}\n\n{new String('=', Console.WindowWidth)}\n\n");
        }

        private void PrintTempMods()
        {
            PrintCenter("МОДИФИКАТОРЫ");
            Console.WriteLine(
                $"\n{String.Join("\n", Enum.GetValues(typeof(TempMods)).OfType<TempMods>().Select(mod => String.Format($"{mod} - {GetDescription(mod)}")))}\n\n{new String('=', Console.WindowWidth)}\n\n");
        }

        private bool Attack(BattleUnitsStack attacker)
        {
            Console.WriteLine("Введите название защищающегося стека");
            var t = Console.ReadLine();
            if (t == null || !stacks.ContainsKey(t))
            {
                Console.WriteLine("\nСтека с таким названием не существует");
                return false;
            }

            if (t == attacker.Name)
            {
                Console.WriteLine("\nДва одинаковых стека");
                return false;
            }

            try
            {
                var (d1, d2) = attacker.Fight(stacks[t]);
                Console.WriteLine(
                    $"\nРезультат:\nСтек {attacker.Name} нанес стеку {stacks[t].Name} {d1} единиц урона\nСтек {stacks[t].Name}: количество - {stacks[t].CurrentCount}, " +
                    $"здоровье последнего - {stacks[t].CurrentHealth}\n");
                if (d2 != -1)
                    Console.WriteLine(
                        $"Контратака:\nСтек {stacks[t].Name} нанес стеку {attacker.Name} {d2} единиц урона\nСтек {attacker.Name}: количество - {attacker.CurrentCount}, " +
                        $"здоровье последнего - {attacker.CurrentHealth}\n");
                return true;
            }
            catch (MyException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private bool Cast(BattleUnitsStack caster)
        {
            var abilities = caster.Unit.Abilities.Where(ab => BattleUnitsStack.GetIsActive(ab)).ToList();

            if (abilities.Count == 0)
            {
                Console.WriteLine("У юнита нет активных способностей");
                return false;
            }

            Console.Write("Выберите способность\n\nСпособности: ");
            Console.WriteLine(String.Join(", ", abilities));
            Console.WriteLine();
            var t = Console.ReadLine();
            success = Enum.TryParse<Abilities>(t, out var ability);
            if (!success || !caster.Unit.Abilities.Contains(ability))
            {
                Console.WriteLine("\nУ стека нет такой способности");
                return false;
            }

            Console.WriteLine("\nВведите название стека, на который хотите применить способность");
            t = Console.ReadLine();
            if (t == null || !stacks.ContainsKey(t))
            {
                Console.WriteLine("\nСтека с таким названием не существует");
                return false;
            }

            try
            {
                caster.Cast(ability, stacks[t]);
                return true;
            }
            catch (MyException e)
            {
                Console.WriteLine("\n" + e.Message);
                return false;
            }
        }

        private static void PrintCenter(string s, char c = '=')
        {
            var n = Console.WindowWidth - s.Length;
            var l = Convert.ToInt32(n / 2);
            Console.Write(new String(c, l) + s + new String(c, n - l));
        }

        private void IsWinner()
        {
            if (GetAliveArmy(out var winner) != 1)
                return;
            Console.WriteLine($"\nПОБЕДИЛ ИГРОК {winner}");
            Console.ReadLine();
            Environment.Exit(0);
        }

        private static string GetDescription(object enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()?
                .GetCustomAttribute<Attr>()?
                .Description;
        }

        private static int GetAliveArmy(out string a)
        {
            var c = 0;
            a = null;
            foreach (var army in armys.Values.Where(army => army.IsAlive()))
            {
                a = army.Name;
                c++;
            }

            return c;
        }

        private static bool IsTurn()
        {
            return stacks.Values.Any(stack => stack.CanTurn && stack.IsAlive() && stack.Army != null);
        }

        private static void NextRound()
        {
            foreach (var stack in stacks.Values)
                stack.NextRound();
        }

        internal static class DllImports
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct COORD
            {
                private readonly short X;
                private readonly short Y;
            }

            [DllImport("kernel32.dll")]
            public static extern IntPtr GetStdHandle(int handle);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleDisplayMode(
                IntPtr ConsoleOutput
                , uint Flags
                , out COORD NewScreenBufferDimensions
            );
        }
    }
}