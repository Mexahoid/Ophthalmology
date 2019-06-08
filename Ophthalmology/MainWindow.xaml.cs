using Ophthalmology.EyeLogics;
using Ophthalmology.PatientLogics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

            try
            {
                LoadLastPatient();
            }
            catch
            {
                // пропущено
            }
        }


        private void LoadLastPatient()
        {
            var t = ConfigLogic.Instance.LoadLastPatient();
            pat = t.Item1;
            time = t.Item2;
            PreClear();
            FillPat(pat, time);
            FillEyes();
        }


        private void FillSide(bool left, Tuple<string, Tuple<string[], int[], int[], string>> inputargs = null)
        {
            List<string> target_pars_list = left ? LPars : RPars;
            target_pars_list.Clear();
            List<string> target_diag_list = left ? LDiags : RDiags;
            target_diag_list.Clear();
            Image target_image = left ? LeftImage : RightImage;
            ListBox target_diag = left ? LeftDiagList : RightDiagList;
            ListBox target_pars = left ? LeftParsList : RightParsList;


            var art = inputargs == null ? ConfigLogic.Instance.ReadEyeInfo(pat, time, left) : inputargs.Item2;
            target_pars_list.AddRange(art.Item1.Select((t, i) => $"{t}: {art.Item2[i]}"));

            var diagTexts = DiagnosiTextHolder.Instance.DiagsItself;

            var tt = art.Item3;
            for (int i = 0; i < tt.Length; i++)
            {
                if (tt[i] > 0)
                {
                    target_diag_list.Add($"{diagTexts[i]}: {art.Item3[i]}");
                }
            }

            string path = inputargs == null ? art.Item4 : inputargs.Item1;
            if(File.Exists(path))
            {
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri(path);
                bi3.EndInit();
                target_image.Source = bi3;
            }


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
            if (win2.ShowDialog() != true)
            {
                return;
            }
            ShowListButton.IsEnabled = ConfigLogic.Instance.IsConfigPresent;
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

        private void PatientRightButton_Click(object sender, RoutedEventArgs e) => ChangePatient(true);

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

        private void PatientLeftButton_Click(object sender, RoutedEventArgs e) => ChangePatient(false);

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

        private void DateNext_Click(object sender, RoutedEventArgs e) => ChangeDate(true);

        private void DatePrev_Click(object sender, RoutedEventArgs e) => ChangeDate(false);

        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow w = new ReportWindow();
            if (w.ShowDialog() != true)
            {
                return;
            }



            List<string[]> diags_right = (from object o in RightDiagList.ItemsSource select o.ToString().Split(':')).ToList();
            List<string[]> pars_right = (from object o in RightParsList.ItemsSource select o.ToString().Split(':')).ToList();
            List<string[]> diags_left = (from object o in LeftDiagList.ItemsSource select o.ToString().Split(':')).ToList();
            List<string[]> pars_left = (from object o in LeftParsList.ItemsSource select o.ToString().Split(':')).ToList();

            var ps = new[]
            {
                pars_left,
                diags_left,
                pars_right,
                diags_right
            };



            ConfigLogic.Instance.SaveReport(w.ReportPath, w.TemplateName, ps);
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(pat != null)
                ConfigLogic.Instance.SaveLastPatient(pat, time);
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
            EyeWindow w = new EyeWindow(left, pat, time);
            if (w.ShowDialog() != true)
            {
                return;
            }
            var t = Tuple.Create(
                w.Parameters.ToArray(),
                w.GetParamValues().ToArray(),
                w.RealDiagnosis.ToArray(),
                w.UsedImagePath);
            FillSide(left, Tuple.Create(w.NewImagePath, t));

            ConfigLogic.Instance.AddEye(left, pat, time, w.NewImagePath, t);
            //FillEyes();
            UpdateLayout();

        }
    }
}
