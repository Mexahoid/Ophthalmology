using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
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

        private List<string> LPars, LDiags, RPars, RDiags;
        public MainWindow()
        {
            InitializeComponent();
            LPars = new List<string>();
            RPars = new List<string>();
            LDiags = new List<string>();
            RDiags = new List<string>();

            LeftDiagList.ItemsSource = LDiags;
            RightDiagList.ItemsSource = RDiags;
            LeftParsList.ItemsSource = LPars;
            RightParsList.ItemsSource = RPars;
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
            EyesGrid.Visibility = Visibility.Visible;
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
            foreach (string wParameter in w.Parameters)
            {
                LPars.Add(wParameter);
            }
            foreach (string wDiagnosi in w.Diagnosis)
            {
                LDiags.Add(wDiagnosi);
            }
            var path = w.ImagePath;

            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(path);
            bi3.EndInit();
            LeftImage.Source = bi3;

        }

        private void RightEyeButton_Click(object sender, RoutedEventArgs e)
        {
            var w = new EyeWindow(false);
            if (w.ShowDialog() != true)
                return;
            foreach (string wParameter in w.Parameters)
            {
                RPars.Add(wParameter);
            }
            foreach (string wDiagnosi in w.Diagnosis)
            {
                RDiags.Add(wDiagnosi);
            }
            var path = w.ImagePath;

            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(path);
            bi3.EndInit();
            RightImage.Source = bi3;

        }
    }
}
