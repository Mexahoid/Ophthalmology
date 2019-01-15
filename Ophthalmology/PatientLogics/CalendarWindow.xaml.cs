using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ophthalmology.PatientLogics
{
    /// <summary>
    /// Логика взаимодействия для CalendarWindow.xaml
    /// </summary>
    public partial class CalendarWindow : Window
    {
        public DateTime Date { get; private set; }
        public CalendarWindow()
        {
            InitializeComponent();
            Date = DateTime.Today;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Date = Calendar.SelectedDate.GetValueOrDefault();
        }
    }
}
