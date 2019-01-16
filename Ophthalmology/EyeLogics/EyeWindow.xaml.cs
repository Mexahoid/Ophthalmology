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

namespace Ophthalmology.EyeLogics
{
    /// <summary>
    /// Логика взаимодействия для EyeWindow.xaml
    /// </summary>
    public partial class EyeWindow : Window
    {
        private List<string> diagnosis;
        private List<PropObj> objs;

        public List<string> Parameters { get; private set; }
        public List<string> Diagnosis { get; private set; }
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

            InitializeComponent();
            diagnosis = new List<string>();
            objs = new List<PropObj>();
            Title = isLeft ? "Левый глаз" : "Правый глаз";
            DataContext = this;
            DiagList.ItemsSource = diagnosis;

            foreach (string instanceParameter in ConfigLogics.ConfigLogic.Instance.Parameters)
            {
                PropObj po = new PropObj {Property = instanceParameter};
                objs.Add(po);
            }

            ParametersDataGrid.ItemsSource = objs;
        }

        private void DiagnosisButton_Click(object sender, RoutedEventArgs e)
        {
            diagnosis.Clear();

            Random rnd = new Random();

            diagnosis.Add($"Симптом 1, степень: {rnd.Next(0, 10)}");
            diagnosis.Add($"Симптом 2, степень: {rnd.Next(0, 10)}");
            diagnosis.Add($"Симптом 3, степень: {rnd.Next(0, 10)}");
            diagnosis.Add($"Симптом 4, степень: {rnd.Next(0, 10)}");
            diagnosis.Add($"Симптом 5, степень: {rnd.Next(0, 10)}");

            DiagList.ItemsSource = null;
            DiagList.ItemsSource = diagnosis;
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                ImagePath = ofd.FileName;
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri(ImagePath);
                bi3.EndInit();
                Image.Source = bi3;

                OkButton.IsEnabled = true;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (PropObj t in objs)
            {
                Parameters.Add(t.Property + ": " + t.Value);
            }
            foreach (string diag in diagnosis)
            {
                Diagnosis.Add(diag);
            }
            DialogResult = true;

            Close();
        }
    }
}
