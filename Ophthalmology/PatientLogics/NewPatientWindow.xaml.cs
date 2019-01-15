using System;
using System.Collections.Generic;
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

namespace Ophthalmology.PatientLogics
{
    /// <summary>
    /// Логика взаимодействия для NewPatientWindow.xaml
    /// </summary>
    public partial class NewPatientWindow : Window
    {
        public List<string> Fields { get; }
        public NewPatientWindow()
        {
            InitializeComponent();
            Fields = new List<string>();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Fields.Add(SurnameTextBox.Text);
            Fields.Add(FirstNameTextBox.Text);
            Fields.Add(SecondNameTextBox.Text);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
