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
using Newtonsoft.Json.Serialization;

namespace Ophthalmology.EyeLogics
{
    /// <summary>
    /// Логика взаимодействия для EfronWindow.xaml
    /// </summary>
    public partial class EfronWindow : Window
    {
        private EfronLogic _el;
        private List<string> _diagStrings;
        private bool  _systemCheckedChange = true;
        private bool _showNulls;

        public EfronWindow(List<int> curr)
        {
            InitializeComponent();
            _el = new EfronLogic(curr);
            _showNulls = true;
            _diagStrings = new List<string>();
            FillDiags();
            //SetImage();
        }

        private void FillDiags()
        {
            if (_el == null)
                return;
            _diagStrings = _el.GetDiagnosis(_showNulls);
            DiagList.ItemsSource = null;
            DiagList.ItemsSource = _diagStrings;
        }

        public Tuple<List<string>, List<int>> GetDiagnosis()
        {
            return _el.GetDiagnosisTuple();
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
            switch (cs)
            {
                case 0:
                    if (RB0.IsChecked != null && (bool) RB0.IsChecked)
                        break;
                    _systemCheckedChange = true;
                    RB0.IsChecked = true;
                    break;
                case 1:
                    if (RB1.IsChecked != null && (bool)RB1.IsChecked)
                        break;
                    _systemCheckedChange = true;
                    RB1.IsChecked = true;
                    break;
                case 2:
                    if (RB2.IsChecked != null && (bool)RB2.IsChecked)
                        break;
                    _systemCheckedChange = true;
                    RB2.IsChecked = true;
                    break;
                case 3:
                    if (RB3.IsChecked != null && (bool)RB3.IsChecked)
                        break;
                    _systemCheckedChange = true;
                    RB3.IsChecked = true;
                    break;
                case 4:
                    if (RB4.IsChecked != null && (bool)RB4.IsChecked)
                        break;
                    _systemCheckedChange = true;
                    RB4.IsChecked = true;
                    break;
                default:
                    throw new Exception("Wtf?");
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
            _showNulls = ShowRB.IsChecked != null && (bool) ShowRB.IsChecked;
            FillDiags();
        }
    }
}
