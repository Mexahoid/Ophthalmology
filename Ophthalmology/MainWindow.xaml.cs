using System;
using System.Windows;
using Ophthalmology.EyeLogics;
using Ophthalmology.PatientLogics;

namespace Ophthalmology
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Patient pat;
        private DateTime time;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowListButton_Click(object sender, RoutedEventArgs e)
        {
            var win2 = new PatientLogics.PatientListWindow();
            if (win2.ShowDialog() != true)
                return;
            pat = win2.Patient;
            time = win2.Time;
            PatientNameTextBlock.Text = pat.Name;
            if (time == DateTime.MinValue && pat.Dates.Count != 0)
                DateTextBlock.Text = pat.Dates[0].ToShortDateString();
            else
            if(time != DateTime.MinValue)
                DateTextBlock.Text = time.ToShortDateString();
            DateGrid.Visibility = Visibility.Visible;
            PatientLeftButton.IsEnabled = true;
            PatientRightButton.IsEnabled = true;
        }

        private void ShowConfigButton_Click(object sender, RoutedEventArgs e)
        {
            var win2 = new ConfigLogics.ConfigWindow();
            win2.Show();
        }

        private void NewDateButton_Click(object sender, RoutedEventArgs e)
        {
            var w = new CalendarWindow();
            if (w.ShowDialog() != true)
                return;
            time = w.Date;
            ConfigLogics.ConfigLogic.Instance.AddDate(pat, time);
            DateTextBlock.Text = time.ToShortDateString();
        }

        private void LeftEyeButton_Click(object sender, RoutedEventArgs e)
        {
            var w = new EyeWindow(true);
            if (w.ShowDialog() != true)
                return;

        }

        private void RightEyeButton_Click(object sender, RoutedEventArgs e)
        {
            var w = new EyeWindow(false);
        }
    }
}
