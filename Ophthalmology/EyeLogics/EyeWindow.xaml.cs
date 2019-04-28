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

namespace Ophthalmology.EyeLogics
{
    /// <summary>
    /// Логика взаимодействия для EyeWindow.xaml
    /// </summary>
    public partial class EyeWindow : Window
    {
        private readonly List<string> _diagnosisBinding;
        private readonly List<PropObj> _objs;

        private readonly List<int> _realDiagnosis;

        public List<string> Parameters { get; }
        public List<string> Diagnosis { get; }

        public string ImagePath { get; private set; }


        private class PropObj
        {
            public string Property { get; set; }
            public int Value { get; set; }
        }

        public EyeWindow(bool isLeft)
        {
            Parameters = new List<string>();
            Diagnosis = new List<string>();
            _realDiagnosis = new List<int>();

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

            ParametersDataGrid.ItemsSource = _objs;
        }

        private void DiagnosisButton_Click(object sender, RoutedEventArgs e)
        {
            _diagnosisBinding.Clear();

            EfronWindow ew = new EfronWindow(null);
            if (ew.ShowDialog() != true)
            {
                return;
            }

            var diagnosi = ew.Diagnosis;

            /*Random rnd = new Random();

            _diagnosis.Add($"Симптом 1, степень: {rnd.Next(0, 10)}");
            _diagnosis.Add($"Симптом 2, степень: {rnd.Next(0, 10)}");
            _diagnosis.Add($"Симптом 3, степень: {rnd.Next(0, 10)}");
            _diagnosis.Add($"Симптом 4, степень: {rnd.Next(0, 10)}");
            _diagnosis.Add($"Симптом 5, степень: {rnd.Next(0, 10)}");*/

            var texts = diagnosi.Item1;
            var diags = diagnosi.Item2;

            for (int i = 0; i < diagnosi.Item1.Count; i++)
            {
                if (diags[i] == 0)
                    continue;
                _diagnosisBinding.Add($"{texts[i]}: {diags[i]}");
            }

            DiagList.ItemsSource = null;
            DiagList.ItemsSource = _diagnosisBinding;
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != true)
                return;
            ImagePath = ofd.FileName;
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(ImagePath);
            bi3.EndInit();
            Image.Source = bi3;

            OkButton.IsEnabled = true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (PropObj t in _objs)
            {
                Parameters.Add(t.Property + ": " + t.Value);
            }
            foreach (string diag in _diagnosisBinding)
            {
                Diagnosis.Add(diag);
            }
            DialogResult = true;

            Close();
        }
    }
}
