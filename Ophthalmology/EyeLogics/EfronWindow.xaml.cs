using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Newtonsoft.Json.Serialization;

namespace Ophthalmology.EyeLogics
{
    /// <summary>
    /// Логика взаимодействия для EfronWindow.xaml
    /// </summary>
    public partial class EfronWindow : Window
    {
        private readonly EfronLogic _el;
        private List<string> _diagStrings;
        private bool  _systemCheckedChange = true;
        private bool _showNulls;
        private readonly List<RadioButton> _rbs;

        public List<int> Diagnosis { get; private set; }

        public EfronWindow(List<int> curr)
        {
            _el = new EfronLogic(curr);
            InitializeComponent();
            _rbs = new List<RadioButton>
            {
                RB0,
                RB1,
                RB2,
                RB3,
                RB4
            };
            _diagStrings = new List<string>();
            FillDiags();
            //SetImage();
        }

        private void FillDiags()
        {
            if (_el == null)
                return;
            _diagStrings = _el.GetDiagnosis(_showNulls);
            if (DiagList == null)
                return;
            DiagList.ItemsSource = null;
            DiagList.ItemsSource = _diagStrings;
        }

        private void OnCheckedChanged(object sender, RoutedEventArgs e)
        {
            if (_systemCheckedChange)
            {
                _systemCheckedChange = false;
                return;
            }
            _el.CurrentStage = int.Parse((sender as RadioButton)?.Tag.ToString() ?? throw new InvalidOperationException());
            FillDiags();
        }

        private void DiagLeftButton_Click(object sender, RoutedEventArgs e)
        {
            _el.SwitchStage(false);
            Update();
        }

        private void SetImage()
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(_el.GetLink());
            bi3.EndInit();

            if ((Selector.SelectedItem as TabItem)?.Content is Image a)
                a.Source = bi3;
            else
                BIm.Source = bi3;
            
        }

        private void DiagRightButton_Click(object sender, RoutedEventArgs e)
        {
            _el.SwitchStage(true);
            Update();
        }

        private void Update()
        {
            DiagTB.Text = _el.GetCurrentStageText();
            int cs = _el.CurrentStage;

            if (_rbs[cs].IsChecked != null && !(bool)_rbs[cs].IsChecked)
            {
                _systemCheckedChange = true;
                _rbs[cs].IsChecked = true;
            }
            SetImage();
        }

        private void Selector_OnSelected(object sender, RoutedEventArgs e)
        {
            if (!(Selector.SelectedItem is TabItem t))
                return;
            _el.CurrentDiag = t.Header.ToString();
            Update();
        }

        private void OnShowChanged(object sender, RoutedEventArgs e)
        {
            if (ShowRB == null)
                return;
            _showNulls = ShowRB.IsChecked != null && (bool) ShowRB.IsChecked;
            FillDiags();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            Diagnosis = _el.GetDiagnosisTuple();
            DialogResult = true;
            Close();
        }
    }
}
