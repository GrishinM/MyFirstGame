using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MyGameEngine;

namespace GameWithGraphicInterface
{
    internal class GraphicInterface : Interface
    {
        private MainWindow window;

        public override void Start()
        {
            window = (MainWindow) Application.Current.MainWindow;
            var pan = new StackPanel() {VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center};
            var t = new TextBlock() {Text = "Введите количество игроков"};
            var y = new TextBox();
            y.Name = "box1";
            var but = new Button() {Content = "Ок", MaxHeight = 20, MaxWidth = 50};
            but.Click += but_Click;
            pan.Children.Add(t);
            pan.Children.Add(y);
            pan.Children.Add(but);
            window.grid.Children.Add(pan);

            void but_Click(object sender, RoutedEventArgs e)
            {
                success = Int32.TryParse(y.Text, out players);
                if (success && players > 1)
                {
                    for (var i = 1; i <= players; i++)
                        armys.Add(i.ToString(), new Army());
                    MainMenu();
                }
                else
                {
                    MessageBox.Show("Неправильный формат ввода");
                    y.Text = "";
                }
            }
        }

        protected override void AddStackToArmy()
        {
            throw new NotImplementedException();
        }

        protected override bool Attack(BattleUnitsStack attacker)
        {
            throw new NotImplementedException();
        }

        protected override bool Cast(BattleUnitsStack caster)
        {
            throw new NotImplementedException();
        }

        protected override void ChangeInitiative()
        {
            throw new NotImplementedException();
        }

        protected override void CreateStack()
        {
            throw new NotImplementedException();
        }

        protected override void DeleteStackFromArmy()
        {
            throw new NotImplementedException();
        }

        protected override void IsWinner()
        {
            throw new NotImplementedException();
        }

        protected override void MainMenu()
        {
            Clear();
            window.grid.ColumnDefinitions.Add(new ColumnDefinition());
            window.grid.ColumnDefinitions.Add(new ColumnDefinition());
            window.grid.RowDefinitions.Add(new RowDefinition());
            window.grid.RowDefinitions.Add(new RowDefinition());
            window.grid.RowDefinitions.Add(new RowDefinition());
            window.grid.RowDefinitions.Add(new RowDefinition());
            window.grid.RowDefinitions.Add(new RowDefinition());
            window.grid.RowDefinitions.Add(new RowDefinition());
            window.grid.RowDefinitions.Add(new RowDefinition());
            var txt = new TextBlock() {Text = "Подгототвка", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 20};
            var but2 = new Button() {Content = "Юниты", FontSize = 15};
            var but3 = new Button() {Content = "Стеки", FontSize = 15};
            var but4 = new Button() {Content = "Создать стек", FontSize = 15};
            var but5 = new Button() {Content = "Армии", FontSize = 15};
            var but6 = new Button() {Content = "Добавить стек в армию", FontSize = 15};
            var but7 = new Button() {Content = "Удалить стек из армии", FontSize = 15};
            var but8 = new Button() {Content = "Способности", FontSize = 15};
            var but9 = new Button() {Content = "Временные модификаторы", FontSize = 15};
            var but10 = new Button() {Content = "Изменить инициативу", FontSize = 15};
            var but11 = new Button() {Content = "Сражение", FontSize = 15};
            var but12 = new Button() {Content = "Выход", FontSize = 15};
            txt.SetValue(Grid.RowProperty, 0);
            txt.SetValue(Grid.ColumnSpanProperty, 2);
            window.grid.Children.Add(txt);
            but2.SetValue(Grid.RowProperty, 1);
            but2.SetValue(Grid.ColumnProperty, 0);
            window.grid.Children.Add(but2);
            but3.SetValue(Grid.RowProperty, 1);
            but3.SetValue(Grid.ColumnProperty, 1);
            window.grid.Children.Add(but3);
            but4.SetValue(Grid.RowProperty, 2);
            but4.SetValue(Grid.ColumnProperty, 0);
            window.grid.Children.Add(but4);
            but5.SetValue(Grid.RowProperty, 2);
            but5.SetValue(Grid.ColumnProperty, 1);
            window.grid.Children.Add(but5);
            but6.SetValue(Grid.RowProperty, 3);
            but6.SetValue(Grid.ColumnProperty, 0);
            window.grid.Children.Add(but6);
            but7.SetValue(Grid.RowProperty, 3);
            but7.SetValue(Grid.ColumnProperty, 1);
            window.grid.Children.Add(but7);
            but8.SetValue(Grid.RowProperty, 4);
            but8.SetValue(Grid.ColumnProperty, 0);
            window.grid.Children.Add(but8);
            but9.SetValue(Grid.RowProperty, 4);
            but9.SetValue(Grid.ColumnProperty, 1);
            window.grid.Children.Add(but9);
            but10.SetValue(Grid.RowProperty, 5);
            but10.SetValue(Grid.ColumnProperty, 0);
            window.grid.Children.Add(but10);
            but11.SetValue(Grid.RowProperty, 5);
            but11.SetValue(Grid.ColumnProperty, 1);
            window.grid.Children.Add(but11);
            but12.SetValue(Grid.RowProperty, 6);
            but12.SetValue(Grid.ColumnSpanProperty, 2);
            window.grid.Children.Add(but12);
            but2.Click += but2_Click;
//            but3.Click += but3_Click;
//            but4.Click += but4_Click;
//            but5.Click += but5_Click;
//            but6.Click += but6_Click;
//            but7.Click += but7_Click;
//            but8.Click += but8_Click;
//            but9.Click += but9_Click;
//            but10.Click += but10_Click;
//            but11.Click += but11_Click;
            but12.Click += but12_Click;
        }

        protected override void Pass(BattleUnitsStack stack)
        {
            throw new NotImplementedException();
        }

        protected override void PrintAbilities()
        {
            throw new NotImplementedException();
        }

        protected override void PrintArmy(Army army)
        {
            throw new NotImplementedException();
        }

        protected override void PrintArmys()
        {
            throw new NotImplementedException();
        }

        protected override void PrintRound(string round)
        {
            throw new NotImplementedException();
        }

        protected override void PrintStack(BattleUnitsStack stack)
        {
            throw new NotImplementedException();
        }

        protected override void PrintStacks()
        {
            throw new NotImplementedException();
        }

        protected override void PrintTempMods()
        {
            throw new NotImplementedException();
        }

        protected override void PrintUnit(Unit unit)
        {
            throw new NotImplementedException();
        }

        protected override void PrintUnits()
        {
            Clear();
            var b=new Button(){Content = "Назад", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Bottom};
            b.Click += back_Click;
            b.SetValue(Grid.RowProperty, 6);
            b.SetValue(Grid.ColumnSpanProperty, 2);
            window.grid.Children.Add(b);
        }

        protected override void StartRound()
        {
            throw new NotImplementedException();
        }
        private void but2_Click(object sender, RoutedEventArgs e)
        {
            PrintUnits();
        }
        private static void but12_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            MainMenu();
        }

        private void Clear()
        {
            window.grid.Children.Clear();
            window.grid.ColumnDefinitions.Clear();
            window.grid.RowDefinitions.Clear();
        }
    }
}