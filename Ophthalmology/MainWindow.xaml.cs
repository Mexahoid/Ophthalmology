using Ophthalmology.EyeLogics;
using Ophthalmology.PatientLogics;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ophthalmology
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Patient pat;
        private DateTime time;
        private ImageSource _is;

        private List<string> LPars, LDiags, RPars, RDiags;
        public MainWindow()
        {
            InitializeComponent();
            LPars = new List<string>();
            RPars = new List<string>();
            LDiags = new List<string>();
            RDiags = new List<string>();
            _is = LeftImage.Source;

            LeftDiagList.ItemsSource = LDiags;
            RightDiagList.ItemsSource = RDiags;
            LeftParsList.ItemsSource = LPars;
            RightParsList.ItemsSource = RPars;
            ShowListButton.IsEnabled = ConfigLogics.ConfigLogic.Instance.IsConfigPresent;
        }

        private void ShowListButton_Click(object sender, RoutedEventArgs e)
        {
            PatientListWindow win2 = new PatientLogics.PatientListWindow();
            if (win2.ShowDialog() != true)
            {
                return;
            }

            pat = win2.Patient;
            time = win2.Time;
            PatientNameTextBlock.Text = pat.Name;
            if (time == DateTime.MinValue && pat.Dates.Count != 0)
            {
                DateTextBlock.Text = pat.Dates[0].ToShortDateString();
            }
            else
            if (time != DateTime.MinValue)
            {
                DateTextBlock.Text = time.ToShortDateString();
            }

            DateGrid.Visibility = Visibility.Visible;
            PatientLeftButton.IsEnabled = true;
            PatientRightButton.IsEnabled = true;

            if (time == DateTime.MinValue)
            {
                return;
            }

            EyesGrid.Visibility = Visibility.Visible;
            LPars.Clear();
            LDiags.Clear();
            RPars.Clear();
            RDiags.Clear();

            if (ConfigLogics.ConfigLogic.Instance.CheckIfEyeExist(pat, time, true))
            {
                Tuple<List<string[]>, string> art = ConfigLogics.ConfigLogic.Instance.ReadEyeInfo(pat, time, true);
                foreach (string par in art.Item1[0])
                {
                    LPars.Add(par);
                }

                foreach (string diag in art.Item1[1])
                {
                    LDiags.Add(diag);
                }

                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri(art.Item2);
                bi3.EndInit();
                LeftImage.Source = bi3;
                LeftDiagList.ItemsSource = null;
                LeftParsList.ItemsSource = null;

                LeftDiagList.ItemsSource = LDiags;
                LeftParsList.ItemsSource = LPars;
                UpdateLayout();
            }
            else
            {
                LeftImage.Source = _is;
                LeftDiagList.ItemsSource = null;
                LeftParsList.ItemsSource = null;

            }

            if (ConfigLogics.ConfigLogic.Instance.CheckIfEyeExist(pat, time, false))
            {
                Tuple<List<string[]>, string> art = ConfigLogics.ConfigLogic.Instance.ReadEyeInfo(pat, time, false);
                foreach (string par in art.Item1[0])
                {
                    RPars.Add(par);
                }

                foreach (string diag in art.Item1[1])
                {
                    RDiags.Add(diag);
                }

                BitmapImage bi4 = new BitmapImage();
                bi4.BeginInit();
                bi4.UriSource = new Uri(art.Item2);
                bi4.EndInit();
                RightImage.Source = bi4;
                RightDiagList.ItemsSource = null;
                RightParsList.ItemsSource = null;
                RightDiagList.ItemsSource = RDiags;
                RightParsList.ItemsSource = RPars;
                UpdateLayout();
            }
            else
            {
                RightImage.Source = _is;
                RightDiagList.ItemsSource = null;
                RightParsList.ItemsSource = null;
            }
        }

        private void ShowConfigButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigLogics.ConfigWindow win2 = new ConfigLogics.ConfigWindow();
            win2.Show();
        }

        private void NewDateButton_Click(object sender, RoutedEventArgs e)
        {
            CalendarWindow w = new CalendarWindow();
            if (w.ShowDialog() != true)
            {
                return;
            }

            time = w.Date;
            ConfigLogics.ConfigLogic.Instance.AddDate(pat, time);
            DateTextBlock.Text = time.ToShortDateString();
            EyesGrid.Visibility = Visibility.Visible;
        }

        private void LeftEyeButton_Click(object sender, RoutedEventArgs e)
        {
            EyeWindow w = new EyeWindow(true);
            if (w.ShowDialog() != true)
            {
                return;
            }
            LPars.Clear();
            LDiags.Clear();

            foreach (string wParameter in w.Parameters)
            {
                LPars.Add(wParameter);
            }
            foreach (string wDiagnosi in w.Diagnosis)
            {
                LDiags.Add(wDiagnosi);
            }
            LeftDiagList.ItemsSource = null;
            LeftParsList.ItemsSource = null;
            LeftDiagList.ItemsSource = LDiags;
            LeftParsList.ItemsSource = LPars;
            string path = w.ImagePath;

            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(path);
            bi3.EndInit();
            LeftImage.Source = bi3;
            ConfigLogics.ConfigLogic.Instance.AddEye(true, pat, time, path, LPars, LDiags);
            UpdateLayout();

        }

        private void RightEyeButton_Click(object sender, RoutedEventArgs e)
        {
            EyeWindow w = new EyeWindow(false);
            if (w.ShowDialog() != true)
            {
                return;
            }

            RPars.Clear();
            RDiags.Clear();

            foreach (string wParameter in w.Parameters)
            {
                RPars.Add(wParameter);
            }
            foreach (string wDiagnosi in w.Diagnosis)
            {
                RDiags.Add(wDiagnosi);
            }
            RightDiagList.ItemsSource = null;
            RightParsList.ItemsSource = null;
            RightDiagList.ItemsSource = RDiags;
            RightParsList.ItemsSource = RPars;
            string path = w.ImagePath;

            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(path);
            bi3.EndInit();
            RightImage.Source = bi3;
            ConfigLogics.ConfigLogic.Instance.AddEye(false, pat, time, path, RPars, RDiags);
            UpdateLayout();

        }
    }
}
