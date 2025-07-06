using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FocalFilter
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DoneBlockingWindow : Window
    {
        public DoneBlockingWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void buttonBlockAgain_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow win = new MainWindow();
            win.Show();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

    }
}
