using System.Windows;

namespace Ophthalmology
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowListButton_Click(object sender, RoutedEventArgs e)
        {
            var win2 = new PatientListWindow();
            win2.Show();
            //Close();
        }

        private void ShowConfigButton_Click(object sender, RoutedEventArgs e)
        {
            var win2 = new ConfigLogics.ConfigWindow();
            win2.Show();
            //Close();
        }
    }
}
