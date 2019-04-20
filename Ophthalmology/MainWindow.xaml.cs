using Ophthalmology.EyeLogics;
using Ophthalmology.PatientLogics;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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


        private void ClearAll()
        {
            EyesGrid.Visibility = Visibility.Visible;
            LPars.Clear();
            LDiags.Clear();
            RPars.Clear();
            RDiags.Clear();

            RightImage.Source = _is;
            RightDiagList.ItemsSource = null;
            RightParsList.ItemsSource = null;
            LeftImage.Source = _is;
            LeftDiagList.ItemsSource = null;
            LeftParsList.ItemsSource = null;
        }


        private void FillSide(bool left)
        {
            List<string> target_pars_list = left ? LPars : RPars;
            List<string> target_diag_list = left ? LDiags : RDiags;
            Image rarget_image = left ? LeftImage : RightImage;
            ListBox target_diag = left ? LeftDiagList : RightDiagList;
            ListBox target_pars = left ? LeftParsList : RightParsList;

            Tuple<List<string[]>, string> art = ConfigLogics.ConfigLogic.Instance.ReadEyeInfo(pat, time, left);
            foreach (string par in art.Item1[0])
            {
                target_pars_list.Add(par);
            }

            foreach (string diag in art.Item1[1])
            {
                target_diag_list.Add(diag);
            }
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(art.Item2);
            bi3.EndInit();
            rarget_image.Source = bi3;

            target_diag.ItemsSource = null;
            target_pars.ItemsSource = null;
            target_diag.ItemsSource = target_diag_list;
            target_pars.ItemsSource = target_pars_list;
            UpdateLayout();
        }


        private void FillPat(Patient _pat, DateTime _time)
        {
            pat = _pat;
            time = _time;

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
        }

        private void FillEyes()
        {
            if (time == DateTime.MinValue)
            {
                return;
            }

            EyesGrid.Visibility = Visibility.Visible;


            if (ConfigLogics.ConfigLogic.Instance.CheckIfEyeExist(pat, time, false))
            {
                FillSide(false);
            }

            if (ConfigLogics.ConfigLogic.Instance.CheckIfEyeExist(pat, time, true))
            {
                FillSide(true);
            }
        }

        private void ShowListButton_Click(object sender, RoutedEventArgs e)
        {
            PatientListWindow win2 = new PatientListWindow();
            if (win2.ShowDialog() != true)
            {
                return;
            }

            FillPat(win2.Patient, win2.Time);

            FillEyes();
        }

        private void ShowConfigButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigLogics.ConfigWindow win2 = new ConfigLogics.ConfigWindow();
            win2.Show();
        }

        private void PreClear()
        {
            EyesGrid.Visibility = Visibility.Hidden;
            DateGrid.Visibility = Visibility.Hidden;
            DateTextBlock.Text = "Дата осмотра";
        }

        private void PatientRightButton_Click(object sender, RoutedEventArgs e)
        {
            var tpat = ConfigLogics.ConfigLogic.Instance.GetPatient(pat, true);
            if (tpat == null)
                return;
            pat = tpat;
            PreClear();
            time = pat.Dates.Count < 1 ? DateTime.MinValue : Convert.ToDateTime(pat.Dates[0]);
            FillPat(pat, time);
            FillEyes();
        }

        private void PatientLeftButton_Click(object sender, RoutedEventArgs e)
        {
            var tpat = ConfigLogics.ConfigLogic.Instance.GetPatient(pat, false);
            if (tpat == null)
                return;
            pat = tpat;
            PreClear();
            time = pat.Dates.Count < 1 ? DateTime.MinValue : Convert.ToDateTime(pat.Dates[0]);

            FillPat(pat, time);
            FillEyes();
        }

        private void DateNext_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DatePrev_Click(object sender, RoutedEventArgs e)
        {

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

            ClearAll();
            // Сброс

            DateTextBlock.Text = time.ToShortDateString();
            EyesGrid.Visibility = Visibility.Visible;
        }


        private void EyeClick(bool left)
        {
            EyeWindow w = new EyeWindow(left);
            if (w.ShowDialog() != true)
            {
                return;
            }
            LPars.Clear();
            LDiags.Clear();
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
