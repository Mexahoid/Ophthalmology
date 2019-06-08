using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Ophthalmology.ConfigLogics.Classes;

namespace Ophthalmology
{
    /// <summary>
    /// Логика взаимодействия для ReportForm.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public string ReportPath { get; set; }

        public string TemplateName { get; set; }

        public ReportWindow()
        {
            InitializeComponent();
            TemplatesListbox.ItemsSource = null;
            var _names = new List<string>();

            var tn = ConfigLogic.Instance.GetTemplateNames();
            foreach (string s in tn)
            {
                _names.Add(s);
            }
            TemplatesListbox.ItemsSource = _names;
            UpdateLayout();
        }

        private void SaveReportButton_Click(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "RTF-документ (*.rtf)|*.rtf";
                if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                ReportPath = sfd.FileName;
                DialogResult = true;
            }
        }

        private void TemplatesListbox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (TemplatesListbox.SelectedItems.Count < 1)
                return;
            var si = TemplatesListbox.SelectedItems[0];
            if (si == null)
                return;
            var n = si.ToString();
            TemplateName = n;

            TextRange tr = new TextRange(
                DocumentTemplate.Document.ContentStart, DocumentTemplate.Document.ContentEnd);

            ConfigLogic.Instance.LoadTemplate(n, tr);
        }
    }
}
