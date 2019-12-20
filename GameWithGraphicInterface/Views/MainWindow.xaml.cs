using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MyGameEngine;

namespace GameWithGraphicInterface.Views
{
    public partial class MainWindow
    {
//        private readonly string directory = Directory.GetCurrentDirectory();
        private const string directory = @"C:\2kurs1sem\OOP\Saves";

        public MainWindow()
        {
            InitializeComponent();
            var applicationViewModel = new ApplicationViewModel();
            DataContext = applicationViewModel;
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            MainMenuGrid.Visibility = Visibility.Hidden;
            MainGrid.Visibility = Visibility.Visible;
        }

        private void EndGame(object sender, RoutedEventArgs e)
        {
            MainMenuGrid.Visibility = Visibility.Visible;
            MainGrid.Visibility = Visibility.Hidden;
        }

        private void ExitGame(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowAddArmyDialog(object sender, RoutedEventArgs e)
        {
            ArmyNameBox.Text = "";
            MainGrid.IsEnabled = false;
            AddArmyDialog.Visibility = Visibility.Visible;
            ArmyNameBox.Focus();
        }

        private void HideAddArmyDialog(object sender, RoutedEventArgs e)
        {
            AddArmyButton.CommandParameter = new Army(ArmyNameBox.Text);
            MainGrid.IsEnabled = true;
            AddArmyDialog.Visibility = Visibility.Hidden;
        }

//        private Button b;

        private void ShowAddStackDialog(object sender, RoutedEventArgs e)
        {
//            b = (Button) sender;
            AddStackButton.DataContext = (ArmyViewModel) ((Button) sender).CommandParameter;
            StackNameBox.Text = "";
            StackCountBox.Text = "1";
            MainGrid.IsEnabled = false;
            AddStackDialog.Visibility = Visibility.Visible;
            StackNameBox.Focus();
        }

        private void HideAddStackDialog(object sender, RoutedEventArgs e)
        {
//            if (((ArmyViewModel) b.CommandParameter).Army.Count == 5)
//            {
//                b.Visibility = Visibility.Hidden;
//            }

//            AddStackButton.CommandParameter = new BattleUnitsStack((Unit) UnitsList.SelectedItem, Convert.ToInt32(StackCountBox.Text), StackNameBox.Text);
            AddStackButton.CommandParameter=(UnitsList.SelectedItem, Convert.ToInt32(StackCountBox.Text), StackNameBox.Text);
            AddStackDialog.Visibility = Visibility.Hidden;
            MainGrid.IsEnabled = true;
        }

        private void ShowUnits(object sender, RoutedEventArgs e)
        {
            UnitsGrid.Visibility = Visibility.Visible;
            MainGrid.Visibility = Visibility.Hidden;
        }

        private void HideUnits(object sender, RoutedEventArgs e)
        {
            UnitsGrid.Visibility = Visibility.Hidden;
            MainGrid.Visibility = Visibility.Visible;
        }

        private void ShowLoadGameDialog(object sender, RoutedEventArgs e)
        {
            MainMenuGrid.Visibility = Visibility.Hidden;
            LoadGameDialog.Visibility = Visibility.Visible;
            var xmlFiles = Directory.EnumerateFiles(directory, "*.mygame");
            var files = xmlFiles.Select(xmlFile => xmlFile.Substring(directory.Length + 1)).Select(file => file.Remove(file.Length - 7)).ToList();
            FilesBox.ItemsSource = files;
        }

        private void HideLoadGameDialog(object sender, RoutedEventArgs e)
        {
            LoadGameDialog.Visibility = Visibility.Hidden;
            MainMenuGrid.Visibility = Visibility.Visible;
        }

        private void LoadGame(object sender, RoutedEventArgs e)
        {
            LoadGameDialog.Visibility = Visibility.Hidden;
            MainGrid.Visibility = Visibility.Visible;
        }

        private void DeleteStackButton_OnClick(object sender, RoutedEventArgs e)
        {
//            var btn = (Button) sender;
//            var stack = (BattleUnitsStack) btn.CommandParameter;
//            b.Visibility = Visibility.Visible;
        }

        private void ShowSaveGameDialog(object sender, RoutedEventArgs e)
        {
            MainGrid.IsEnabled = false;
            GameNameBox.Text = "";
            SaveGameDialog.Visibility = Visibility.Visible;
            GameNameBox.Focus();
        }

        private void HideSaveGameDialog(object sender, RoutedEventArgs e)
        {
            SaveGameDialog.Visibility = Visibility.Hidden;
            MainGrid.IsEnabled = true;
        }

        private void DeleteFile(object sender, RoutedEventArgs e)
        {
            File.Delete(directory + @"\" + FilesBox.SelectedItem + ".mygame");
            var xmlFiles = Directory.EnumerateFiles(directory, "*.mygame");
            var files = xmlFiles.Select(xmlFile => xmlFile.Substring(directory.Length + 1)).Select(file => file.Remove(file.Length - 7)).ToList();
            FilesBox.ItemsSource = files;
        }

        private void Fight(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Hidden;
            MainGame.Visibility = Visibility.Visible;
        }
    }
}