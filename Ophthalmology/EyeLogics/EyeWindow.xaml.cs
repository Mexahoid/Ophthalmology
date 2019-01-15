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

namespace Ophthalmology.EyeLogics
{
    /// <summary>
    /// Логика взаимодействия для EyeWindow.xaml
    /// </summary>
    public partial class EyeWindow : Window
    {
        private List<string> diagnosis;
        private List<PropObj> objs;

        private class PropObj
        {
            public string Property { get; set; }
            public int Value { get; set; }
        }

        public EyeWindow(bool isLeft)
        {
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
            diagnosis.Add("Первый диываыва");
            diagnosis.Add("выафываыв диываыва");
            diagnosis.Add("впаывапывап диываыва");
            diagnosis.Add("негк567ке диываыва");
            diagnosis.Add("нкенгенг диываыва");
        }
    }
}
