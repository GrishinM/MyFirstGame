using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MyGameEngine;
using static GameWithGraphicInterface.App;

namespace GameWithGraphicInterface
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
            round = 0;
            delta = 0;
            players = 0;
            success = false;
            game = false;
            GAME = true;
            menu = true;
            count.Focus();
        }

        private void but(object sender, RoutedEventArgs e)
        {
            success = Int32.TryParse(count.Text, out players);
            if (success && players > 1)
            {
                for (var i = 1; i <= players; i++)
                    armys.Add(i.ToString(), new Army());
            }
            else
            {
                MessageBox.Show("Неправильный формат ввода");
                count.Text = "";
                return;
            }

            var m = new MainWindow(this);
            m.Show();
            Close();
        }

        private void Count_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
                but(this, new RoutedEventArgs());
        }
    }
}