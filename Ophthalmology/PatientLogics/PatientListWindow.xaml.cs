using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Ophthalmology.PatientLogics
{
    /// <summary>
    /// Логика взаимодействия для PatientListWindow.xaml
    /// </summary>
    public partial class PatientListWindow : Window
    {
        private ObservableCollection<Patient> _patients;
        public Patient Patient { get; set; }
        public DateTime Time { get; set; }
        public PatientListWindow()
        {
            InitializeComponent();
            DataContext = this;

            var cfg = ConfigLogics.ConfigLogic.Instance;
            _patients = cfg.GetPatients();


            /*Patients = new ObservableCollection<Patient>
            {
                new Patient
                {
                    Name = "Пациент 1",
                    Dates = new ObservableCollection<DateTime>
                    {
                        DateTime.UtcNow,
                        DateTime.MaxValue,
                        DateTime.Now
                    }
                },
                new Patient
                {
                    Name = "Пациент 2",
                    Dates = new ObservableCollection<DateTime>
                    {
                        DateTime.UtcNow,
                        DateTime.Now
                    }
                },
                new Patient
                {
                Name = "Пациент 3",
                Dates = new ObservableCollection<DateTime>
                {
                    DateTime.MaxValue,
                }
            }
            };*/

            PatientTree.ItemsSource = _patients;
        }

        private ItemsControl GetSelectedTreeViewItemParent(TreeViewItem item)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(item);
            while (!(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as ItemsControl;
        }

        private void OnItemMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DialogResult == true)
                return;
            var s = (sender as TreeViewItem)?.Header;
            
            if (s is DateTime time)
            {
                object p = ((TreeViewItem) GetSelectedTreeViewItemParent((TreeViewItem) sender)).Header;
                Patient = (Patient) p;
                Time = time;
            }
            else
            {
                Patient = (Patient) s;
                if (Patient != null)
                    Time = DateTime.MinValue;
            }

            DialogResult = true;
            Close();
        }

        private void AddPatientButton_Click(object sender, RoutedEventArgs e)
        {
            var w = new NewPatientWindow();
            if (w.ShowDialog() != true)
                return;
            var name = w.Fields;
            ConfigLogics.ConfigLogic.Instance.AddPatient($"{name[0]} {name[1]} {name[2]}");
            _patients.Add(ConfigLogics.ConfigLogic.Instance.GetPatients().Last());
        }
    }
}
