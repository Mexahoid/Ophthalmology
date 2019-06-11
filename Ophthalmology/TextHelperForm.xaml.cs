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

namespace Ophthalmology
{
    /// <summary>
    /// Логика взаимодействия для TextHelperForm.xaml
    /// </summary>
    public partial class TextHelperForm : Window
    {
        public string Text { get; set; }
        public TextHelperForm()
        {
            InitializeComponent();
            TextHolder.Focus();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Text = TextHolder.Text;
            DialogResult = true;
            Close();
        }
    }
}
