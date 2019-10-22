using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using static GameWithGraphicInterface.App;

namespace GameWithGraphicInterface
{
    public partial class MainWindow : Window 
    {
        public MainWindow(Window oldWindow)
        {
            InitializeComponent();
            var i = 1;
            var s = VisualTreeHelper.GetDescendantBounds(oldWindow).Right;
            var width = s / (players > 10 ? 10 : players);
            foreach (var army in armys)
            {
                //var g=new Grid(){MinWidth = 100};
                var a = new Border() {BorderThickness = new Thickness(1, 0, 1, 0), BorderBrush = Brushes.Black, Width = width, Child = new UniformGrid() {Rows = 7, Columns = 1}};
                StackPanel.Children.Add(a);
                //StackPanel.Children.Add(g);
            }
        }

        public void but1(object sender, RoutedEventArgs e)
        {
            
        }
        
        public void but2(object sender, RoutedEventArgs e)
        {
            Image.Visibility = Visibility.Visible;
            Viewer.Visibility = Visibility.Visible;
            Button1.Visibility = Visibility.Visible;
            Button2.Visibility = Visibility.Collapsed;
        }
    }
}