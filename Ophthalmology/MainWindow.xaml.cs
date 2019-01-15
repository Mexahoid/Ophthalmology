using System;
using System.Windows;
using Ophthalmology.PatientLogics;

namespace Ophthalmology
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowListButton_Click(object sender, RoutedEventArgs e)
        {
            var win2 = new PatientLogics.PatientListWindow();
            if (win2.ShowDialog() != true)
                return;
            win2.Show();
            Patient pat = win2.Patient;
            DateTime time = win2.Time;
        }

        private void ShowConfigButton_Click(object sender, RoutedEventArgs e)
        {
            var win2 = new ConfigLogics.ConfigWindow();
            win2.Show();
        }
    }
}
