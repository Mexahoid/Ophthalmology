using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Ophthalmology.ConfigLogics.Classes;
using Ophthalmology.Patients.Classes;

namespace Ophthalmology.EyeLogics
{
    /// <summary>
    /// Логика взаимодействия для EyeWindow.xaml
    /// </summary>
    public partial class EyeWindow : Window
    {
        private readonly List<string> _diagnosisBinding;
        private readonly List<PropObj> _objs;

        public List<int> RealDiagnosis { get; private set; }

        public List<string> Parameters { get; }

        public string NewImagePath { get; private set; }

        public string UsedImagePath { get; private set; }


        public class PropObj
        {
            public string Property { get; set; }
            public int Value { get; set; }
        }

        public EyeWindow(bool isLeft, Patient pat, DateTime time)
        {
            Parameters = new List<string>();

            var t = ConfigLogic.Instance.ReadEyeInfo(pat, time, isLeft);

            InitializeComponent();
            _diagnosisBinding = new List<string>();
            _objs = new List<PropObj>();
            Title = isLeft ? "Левый глаз" : "Правый глаз";
            DataContext = this;
            DiagList.ItemsSource = _diagnosisBinding;

            foreach (string instanceParameter in ConfigLogic.Instance.Parameters)
            {
                PropObj po = new PropObj {Property = instanceParameter};
                _objs.Add(po);
            }

            if (t != null)
            {
                EyeParser ep = new EyeParser(t);
                ep.FillParams(_objs);
                RealDiagnosis = ep.DiagsValues;
                UpdateDiag();
                Image.Source = ep.GetImage();
                UsedImagePath = ep.ImagePath;
                NewImagePath = ep.ImagePath;
                OkButton.IsEnabled = true;
                DrawBtn.IsEnabled = true;
            }
            else
            {
                RealDiagnosis = new List<int>();
                for (int i = 0; i < 16; i++)
                {
                    RealDiagnosis.Add(0);
                }
            }

            ParametersDataGrid.ItemsSource = _objs;
        }

        public List<int> GetParamValues()
        {
            var vals = new List<int>();
            foreach (PropObj propObj in _objs)
            {
                vals.Add(propObj.Value);
            }
            return vals;
        }

        private void UpdateDiag()
        {
            var texts = DiagnosiTextHolder.Instance.DiagsItself;
            _diagnosisBinding.Clear();
            for (int i = 0; i < texts.Count; i++)
            {
                if (RealDiagnosis[i] == 0)
                    continue;
                _diagnosisBinding.Add($"{texts[i]}: {RealDiagnosis[i]}");
            }

            DiagList.ItemsSource = null;
            DiagList.ItemsSource = _diagnosisBinding;
        }

        private void DiagnosisButton_Click(object sender, RoutedEventArgs e)
        {
            EfronWindow ew = new EfronWindow(RealDiagnosis);
            if (ew.ShowDialog() != true)
            {
                return;
            }

            RealDiagnosis = ew.Diagnosis;

            UpdateDiag();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != true)
                return;
            NewImagePath = ofd.FileName;
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(NewImagePath);
            bi3.EndInit();
            Image.Source = bi3;

            OkButton.IsEnabled = true;
            DrawBtn.IsEnabled = true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (PropObj t in _objs)
            {
                Parameters.Add(t.Property);
            }
            DialogResult = true;
            UsedImagePath = NewImagePath;
            Close();
        }


        private void DrawTextButtonClick(object sender, RoutedEventArgs e)
        {
            List<string> paramsList = new List<string>();

            foreach (PropObj propObj in _objs)
            {
                paramsList.Add($"{propObj.Property}: {propObj.Value}");
            }

            foreach (string s in _diagnosisBinding)
            {
                paramsList.Add(s);
            }

            EyeTextWindow w = new EyeTextWindow(paramsList, NewImagePath);
            if (w.ShowDialog() != true)
            {
                return;
            }
        }
    }
}
