using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using MyGameEngine;

namespace GameWithConsoleInterface
{
    internal class ConsoleInterface
    {
        private static readonly SortedDictionary<string, Army> armys = new SortedDictionary<string, Army>();
        private static readonly SortedDictionary<string, BattleUnitsStack> stacks = new SortedDictionary<string, BattleUnitsStack>();

        private static readonly Unit[] units =
        {
            new Abaddon(), new Axe(), new Dark_Seer(), new Doom(), new Drow_Ranger(), new Huskar(), new Juggernaut(), new Keeper_of_the_Light(), new Lich(),
            new Line_meleee_creep(), new Omniknight(), new Sniper(), new Sven(), new Tower()
        };

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

        public void Start()
        {
            var hConsole = DllImports.GetStdHandle(-11);
            DllImports.SetConsoleDisplayMode(hConsole, 1, out var x);
            round = 0;
//            delta = 0;
            players = 0;
            success = false;
            game = false;
            GAME = true;
            menu = true;
            while (!success || players < 2)
            {
                Console.Out.WriteLine("Введите количество игроков");
                var a = Console.ReadLine();
                success = Int32.TryParse(a, out players);
                if (success && players > 1)
                    for (var i = 1; i <= players; i++)
                        armys.Add(i.ToString(), new Army(i.ToString()));
                else
                    Console.WriteLine("\nНеправильный формат ввода\n");
            }

            MainMenu();
        }

        private void MainMenu()
        {
            Console.WriteLine();
            PrintRound("ПОДГОТОВКА");
            while (menu)
            {
                Console.WriteLine("\n1 - вывести все юниты    2 - создать стек    3 - вывести все стеки    4 - вывести все армии    5 - добавить стек в армию    " +
                                  "6 - удалить стек из армии    7 - список способностей    8 - список временных модификаторов    " + "9 - шкала инициативы    10 - начать    " +
                                  "0 - выход");
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
                        curScale = new Scale(stacks);
                        Console.WriteLine();
                        PrintInitiative3(curScale);
                        Console.WriteLine();
                        break;
                    case "10":
                        menu = false;
                        break;
                    default:
                        Console.WriteLine("Такой команды нет");
                        break;
                }
            }

            if (GetAliveArmy(out var z) == 0)
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
            PrintRound("РАУНД " + (++round));
            nextScale = new Scale(stacks);
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

                curScale = new Scale(stacks);
                playerStack = curScale.GetStack();
                Console.WriteLine($"\nХод {++step}. Очередь игрока {playerStack.Army.Name}. Стек {playerStack.Name}\n");
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
            {
                Environment.Exit(0);
            }

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
            foreach (var i in units)
            {
                PrintCenter("ЮНИТ " + i.Id);
                Console.WriteLine();
                PrintUnit(i);
                Console.WriteLine("\n\n");
            }

            Console.WriteLine();
        }

        private void PrintUnit(Unit unit)
        {
            Console.Write(
                "Имя - {0}, тип - {7}, здоровье - {1}, атака - {2}, защита - {3}, разброс урона - {4}-{5}, инициатива - {6}, способности: ",
                unit.Name, unit.Hitpoints, unit.Attack, unit.Defence, unit.Damage.Item1(), unit.Damage.Item2(),
                unit.Initiative, unit.Type == Types.Hero ? "герой" : unit.Type == Types.Creep ? "крип" : "строение");
            foreach (var i in unit.SelfAbilities)
                Console.Write(i + " ");
            Console.WriteLine();
        }

        private void CreateStack()
        {
            Console.WriteLine("Введите название стека");
            var r = Console.ReadLine();
            if (r != null && !stacks.ContainsKey(r))
            {
                Console.WriteLine("Введите тип юнита");
                var t = Console.ReadLine();
                success = Enum.TryParse<Units>(t, out var type);
                if (success && Enum.IsDefined(typeof(Units), type))
                {
                    success = false;
                    while (!success)
                    {
                        Console.WriteLine("Введите количество юнитов");
                        t = Console.ReadLine();
                        success = Int32.TryParse(t, out var count);
                        if (success && count > 0 && count < 1000000)
                        {
                            stacks.Add(r, new BattleUnitsStack(units[(int) type], count, r));
                            Console.WriteLine("\nСтек успешно создан");
                        }
                        else
                            Console.WriteLine("\nНеправильный формат ввода\n");
                    }
                }
                else
                    Console.WriteLine("\nЮнита с таким типом не существует");
            }
            else
                Console.WriteLine("\nСтек с таким названием уже существует");
        }

        private void PrintStacks()
        {
            Console.WriteLine();
            foreach (var i in stacks)
            {
                PrintCenter("СТЕК " + i.Key);
                Console.WriteLine();
                PrintStack(i.Value);
                Console.WriteLine("\n\n");
            }

            Console.WriteLine();
        }

        private void PrintStack(BattleUnitsStack stack)
        {
            Console.Write("Юнит - {0}, количество - {1}, здоровье последнего - {2}, инициатива - {3}, временные моды: ",
                stack.Unit.Id, stack.CurrentCount, stack.CurrentHealth, stack.Initiative);
            foreach (var i in stack.Mods)
                Console.Write(i.Key + " (осталось: {0})    ", i.Value == -1 ? "бесконечно" : i.Value.ToString());
            Console.WriteLine();
        }

        private void PrintArmys()
        {
            Console.WriteLine();
            foreach (var i in armys)
            {
                PrintCenter("АРМИЯ " + i.Key);
                Console.WriteLine();
                PrintArmy(i.Value);
                Console.WriteLine("\n\n");
            }

            Console.WriteLine();
        }

        private void PrintArmy(Army army)
        {
            var s = 0;
            foreach (var i in army.SelfStacks.Values)
                Console.WriteLine("{0}) Стек {1}, юнит - {2}, количество - {3}, здоровье последнего - {4}, инициатива - {5}",
                    ++s, i.Name, i.Unit.Id, i.CurrentCount, i.CurrentHealth, i.Initiative);
        }

        private void PrintInitiative1(Scale scale)
        {
            foreach (var stack in scale.Stacks.Values())
                Console.Write("--> {0} ({1}) ", stack.Name, stack.Initiative);
        }

        private void PrintInitiative2(Scale scale)
        {
            var a = true;
            foreach (var stack in scale.Stacks.Values())
            {
                if (a)
                {
                    Console.Write("{0}", stack.Name);
                    a = false;
                }
                else
                {
                    Console.Write(" --> {0}", stack.Name);
                }
            }
        }

        private void PrintInitiative3(Scale scale)
        {
            var a = true;
            foreach (var stack in scale.Stacks.Values())
            {
                if (a)
                {
                    Console.Write("{0} ({1})", stack.Name, stack.Initiative);
                    a = false;
                }
                else
                {
                    Console.Write(" --> {0} ({1})", stack.Name, stack.Initiative);
                }
            }
        }

        private void AddStackToArmy()
        {
            Console.WriteLine("Введите название армии");
            var t = Console.ReadLine();
            if (t != null && armys.ContainsKey(t))
            {
                Console.WriteLine("Введите название стека");
                var r = Console.ReadLine();
                if (r != null && stacks.ContainsKey(r))
                {
                    if (stacks[r].Army == null)
                    {
                        if (armys[t].Add(stacks[r], r))
                        {
                            stacks[r].Army = armys[t];
                            Console.WriteLine("\nСтек успешно добавлен");
                        }
                        else
                            Console.WriteLine("Превышено максимальное количество стеков в армии");
                    }
                    else
                        Console.WriteLine("\nСтек уже в " + (stacks[r].Army == armys[t] ? "этой" : "другой") + " армии");
                }
                else
                    Console.WriteLine("\nСтека с таким названием не существует");
            }
            else
                Console.WriteLine("\nАрмии с таким названием не существует");
        }

        private void DeleteStackFromArmy()
        {
            Console.WriteLine("Введите название армии");
            var t = Console.ReadLine();
            if (t != null && armys.ContainsKey(t))
            {
                Console.WriteLine();
                PrintArmy(armys[t]);
                Console.WriteLine("\nВведите название стека");
                var r = Console.ReadLine();
                Console.WriteLine(armys[t].Remove(r) ? "\nСтек успешно удален из армии" : "\nВ этой армии нет стека с таким названием");
            }
            else
                Console.WriteLine("\nАрмии с таким названием не существует");
        }

        private void PrintAbilities()
        {
            PrintCenter("СПОСОБНОСТИ");
            Console.WriteLine();
            var lst = Enum.GetValues(typeof(Abilities));
            foreach (var i in lst)
                Console.WriteLine(i + " - " + GetDescription(i) + " (" +
                                  (BattleUnitsStack.GetIsActive(i) ? "активная" : "пассивная") + ")");
            Console.WriteLine("\n" + (new String('=', Console.WindowWidth)) + "\n\n");
        }

        private void PrintTempMods()
        {
            PrintCenter("МОДИФИКАТОРЫ");
            Console.WriteLine();
            var lst = Enum.GetValues(typeof(TempMods));
            foreach (var i in lst)
                Console.WriteLine(i + " - " + GetDescription(i));
            Console.WriteLine("\n" + (new String('=', Console.WindowWidth)) + "\n\n");
        }

//        private void ChangeInitiative()
//        {
//            Console.WriteLine("Введите название стека");
//            var t = Console.ReadLine();
//            if (t != null && stacks.ContainsKey(t))
//            {
//                success = false;
//                while (!success)
//                {
//                    Console.WriteLine("Введите новое значение инициативы");
//                    var r = Console.ReadLine();
//                    success = Single.TryParse(r, out var init);
//                    if (success)
//                    {
//                        stacks[t].Initiative = init;
//                    }
//                    else
//                        Console.WriteLine("\nНеправильный формат ввода\n");
//                }
//            }
//            else
//                Console.WriteLine("\nСтека с таким названием не существует");
//        }

        private bool Attack(BattleUnitsStack attacker)
        {
            Console.WriteLine("Введите название защищающегося стека");
            var t = Console.ReadLine();
            if (t != null && stacks.ContainsKey(t))
            {
                if (t != attacker.Name)
                {
                    try
                    {
                        var (d1, d2) = attacker.Fight(stacks[t]);
                        Console.WriteLine(
                            "\nРезультат:\nСтек {0} нанес стеку {1} {2} единиц урона\nСтек {3}: количество - {4}, здоровье последнего - {5}\n",
                            attacker.Name, stacks[t].Name, d1, stacks[t].Name, stacks[t].CurrentCount, stacks[t].CurrentHealth);
                        if (d2 != -1)
                            Console.WriteLine(
                                "Контратака:\nСтек {0} нанес стеку {1} {2} единиц урона\nСтек {3}: количество - {4}, здоровье последнего - {5}\n",
                                stacks[t].Name, attacker.Name, d2, attacker.Name, attacker.CurrentCount, attacker.CurrentHealth);
                    }
                    catch (MyException e)
                    {
                        Console.WriteLine(e.Message);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Два одинаковых стека");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("\nСтека с таким названием не существует");
                return false;
            }

            return true;
        }

        private bool Cast(BattleUnitsStack caster)
        {
            var q = false;
            var abs = new List<Abilities>();
            foreach (var i in caster.Unit.SelfAbilities)
                if (BattleUnitsStack.GetIsActive(i))
                {
                    q = true;
                    abs.Add(i);
                }

            if (!q)
            {
                Console.WriteLine("У юнита нет активных способностей");
                return false;
            }

            Console.Write("Выберите способность\n\nСпособности: ");
            foreach (var i in abs)
                Console.Write("{0} ", i);
            Console.WriteLine();
            var t = Console.ReadLine();
            success = Enum.TryParse<Abilities>(t, out var ability);
            if (success && caster.Unit.SelfAbilities.Contains(ability))
            {
                Console.WriteLine("\nВведите название стека, на который хотите применить способность");
                t = Console.ReadLine();
                if (t != null && stacks.ContainsKey(t))
                {
                    try
                    {
                        caster.Cast(ability, stacks[t]);
                    }
                    catch (MyException e)
                    {
                        Console.WriteLine("\n" + e.Message);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("\nСтека с таким названием не существует");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("\nУ стека нет такой способности");
                return false;
            }

            return true;
        }

        private static void PrintCenter(string s, char c = '=')
        {
            var n = Console.WindowWidth - s.Length;
            var l = Convert.ToInt32(n / 2);
            Console.Write((new String(c, l)) + s + (new String(c, n - l)));
        }

        private void IsWinner()
        {
            if (GetAliveArmy(out var winner) == 1)
            {
                Console.WriteLine("\nПОБЕДИЛ ИГРОК {0}", winner);
                Console.ReadLine();
                Environment.Exit(0);
            }
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
            foreach (var army in armys)
                if (army.Value.IsAlive())
                {
                    a = army.Key;
                    c++;
                }

            return c;
        }

        private static bool IsTurn()
        {
            foreach (var stack in stacks.Values)
                if (stack.CanTurn && stack.IsAlive() && stack.Army != null)
                    return true;
            return false;
        }

        private static void NextRound()
        {
            foreach (var i in stacks.Values)
                i.NextRound();
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