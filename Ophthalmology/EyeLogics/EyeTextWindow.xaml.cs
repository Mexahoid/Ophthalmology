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
    /// Логика взаимодействия для EyeTextWindow.xaml
    /// </summary>
    public partial class EyeTextWindow : Window
    {
        private int _fontSize = 20;
        private BitmapImage _img;
        private Color _textColor = Colors.Black;
        public EyeTextWindow(List<string> paramsList, string imagePath)
        {
            InitializeComponent();
            ParamsList.ItemsSource = paramsList;
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(imagePath);
            bi3.EndInit();
            _img = bi3;
            int width = bi3.PixelWidth;
            int height = bi3.PixelHeight;
            
            int marg = 10;
            // 2 отступа сверху-снизу и 40 на отступ строки состояния
            height += marg + marg + 40;
            MinHeight = height;
            if (Height < MinHeight)
                Height = MinHeight;
            MaxHeight = MinHeight;
            width += marg + 5 + 10;
            // +300, т.к. дефолтно 700 под картинку и 300 под логику
            MinWidth = width + 300;
            if (Width < MinWidth)
                Width = MinWidth;
            MaxWidth = MinWidth;
            BackImg.Source = _img;
        }


        private void UpdateFont()
        {
            TxtNum.Text = _fontSize.ToString();
            ExampleTxb.FontSize = _fontSize;
            ExampleTxb.Foreground = new SolidColorBrush(_textColor);
        }

        private void NudUp_Click(object sender, RoutedEventArgs e)
        {
            _fontSize++;
            UpdateFont();
        }

        private void NudDown_Click(object sender, RoutedEventArgs e)
        {
            if (_fontSize > 1)
                _fontSize--;
            UpdateFont();
        }


        private void SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (!cp.SelectedColor.HasValue)
                return;
            _textColor = cp.SelectedColor.Value;
            UpdateFont();
        }

        private void PlaceTextClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {

            DialogResult = true;
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
