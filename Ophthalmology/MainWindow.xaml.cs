using Ophthalmology.EyeLogics;
using Ophthalmology.PatientLogics;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ophthalmology.ConfigLogics.Classes;
using Ophthalmology.ConfigLogics.Forms;
using Ophthalmology.Patients.Classes;

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
            ShowListButton.IsEnabled = ConfigLogic.Instance.IsConfigPresent;
        }



        private void FillSide(bool left, Tuple<List<string[]>, string> inputargs = null)
        {
            List<string> target_pars_list = left ? LPars : RPars;
            List<string> target_diag_list = left ? LDiags : RDiags;
            Image target_image = left ? LeftImage : RightImage;
            ListBox target_diag = left ? LeftDiagList : RightDiagList;
            ListBox target_pars = left ? LeftParsList : RightParsList;

            var art = inputargs ?? ConfigLogic.Instance.ReadEyeInfo(pat, time, left);

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
            target_image.Source = bi3;

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


            if (ConfigLogic.Instance.CheckIfEyeExist(pat, time, false))
            {
                FillSide(false);
            }

            if (ConfigLogic.Instance.CheckIfEyeExist(pat, time, true))
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
            PreClear();
            FillPat(win2.Patient, win2.Time);
            FillEyes();
        }

        private void ShowConfigButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigWindow win2 = new ConfigWindow();
            win2.Show();
        }

        private void PreClear()
        {
            EyesGrid.Visibility = Visibility.Hidden;
            DateGrid.Visibility = Visibility.Hidden;

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


            DateTextBlock.Text = "Дата осмотра";
        }

        private void PatientRightButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePatient(true);
        }

        private void ChangePatient(bool next)
        {
            Patient tpat = ConfigLogic.Instance.GetPatient(pat, next);
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
            ChangePatient(false);
        }

        private void ChangeDate(bool next)
        {
            if (pat.Dates.Count < 1)
                return;
            int pos;
            for (pos = 0; pos < pat.Dates.Count; pos++)
            {
                if (pat.Dates[pos] == time)
                    break;
            }
            if (next)
                pos++;
            else
                pos--;

            if (pos >= pat.Dates.Count)
                return;

            if (pos < 0)
                return;
            time = Convert.ToDateTime(pat.Dates[pos]);
            PreClear();
            FillPat(pat, time);
            FillEyes();
        }

        private void DateNext_Click(object sender, RoutedEventArgs e)
        {
            ChangeDate(true);
        }

        private void DatePrev_Click(object sender, RoutedEventArgs e)
        {
            ChangeDate(false);
        }

        private void NewDateButton_Click(object sender, RoutedEventArgs e)
        {
            CalendarWindow w = new CalendarWindow();
            if (w.ShowDialog() != true)
            {
                return;
            }

            time = w.Date;
            ConfigLogic.Instance.AddDate(pat, time);

            PreClear();
            // Сброс
            DateGrid.Visibility = Visibility.Visible;
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
            LoadEyeImage(true);
        }

        private void RightEyeButton_Click(object sender, RoutedEventArgs e)
        {
            LoadEyeImage(false);
        }

        private void LoadEyeImage(bool left)
        {
            EyeWindow w = new EyeWindow(left);
            if (w.ShowDialog() != true)
            {
                return;
            }


            FillSide(left, Tuple.Create(new List<string[]>
            {
                w.Parameters.ToArray(),
                w.Diagnosis.ToArray()
            }, w.ImagePath));

            ConfigLogic.Instance.AddEye(left, pat, time, w.ImagePath, w.Parameters, w.Diagnosis);
            UpdateLayout();

        }
    }
}
