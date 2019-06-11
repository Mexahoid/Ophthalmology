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
using Cursors = System.Windows.Input.Cursors;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

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
        private string _currentText;
        private readonly List<TextBlock> _textblocks;
        private TextBlock _currentBlock;
        private bool _isDragging;
        private Point _lastPos;
        public EyeTextWindow(List<string> paramsList, string imagePath)
        {
            InitializeComponent();
            _textblocks = new List<TextBlock>();
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

        private void Text()
        {
            TextBlock textBlock = new TextBlock
            {
                Text = _currentText,
                Foreground = new SolidColorBrush(_textColor),
                FontSize = _fontSize
            };
            textBlock.MouseRightButtonDown += (sender, args) => DeleteTB(textBlock);

            Canvas.SetLeft(textBlock, 0);
            Canvas.SetTop(textBlock, 0);
            MainArea.Children.Add(textBlock);
            _textblocks.Add(textBlock);
        }

        private void DeleteTB(TextBlock tb)
        {
            _textblocks.Remove(tb);
            MainArea.Children.Remove(tb);
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
            if(_currentText != null)
                Text();
            _currentText = null;
            PutBtn.IsEnabled = false;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG картинки (*.png)|*.png|JPEG изображения (*.jpg)|*.jpg";
                sfd.InitialDirectory = _img.UriSource.AbsolutePath;
                if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                Rect bounds = VisualTreeHelper.GetDescendantBounds(MainArea);
                double dpi = 96d;

                RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);

                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(MainArea);
                    dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
                }

                rtb.Render(dv);
                //endcode as PNG
                //var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, (int)rect.Right, (int)rect.Bottom));
                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                pngEncoder.Save(ms);
                ms.Close();
                System.IO.File.WriteAllBytes(sfd.FileName, ms.ToArray());
                DialogResult = true;
                Close();
            }

            

        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ParamsList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _currentText = ((sender as StackPanel)?.Children[0] as TextBlock)?.Text;
            PutBtn.IsEnabled = true;
        }

        private void SelectTextBlock(object sender, MouseButtonEventArgs e)
        {
            _lastPos = Mouse.GetPosition(MainArea);

            foreach (TextBlock textblock in _textblocks)
            {
                Point relativePoint = textblock.TransformToAncestor(MainArea)
                    .Transform(new Point(0, 0));
                Size a = textblock.DesiredSize;

                if (!(_lastPos.X >= relativePoint.X) || !(_lastPos.X <= relativePoint.X + a.Width) ||
                    !(_lastPos.Y >= relativePoint.Y) || !(_lastPos.Y <= relativePoint.Y + a.Height)) continue;
                _currentBlock = textblock;
                _isDragging = true;
                Mouse.OverrideCursor = Cursors.Hand;
                break;
            }
        }

        private void DeselectTextBlock(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            _currentBlock = null;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void MoveTextBlock(object sender, MouseEventArgs e)
        {
            if (!_isDragging)
                return;
            var pos = Mouse.GetPosition(MainArea);
            double dx = pos.X - _lastPos.X;
            double dy = pos.Y - _lastPos.Y;

            Point relativePoint = _currentBlock.TransformToAncestor(MainArea)
                .Transform(new Point(0, 0));

            Canvas.SetLeft(_currentBlock, relativePoint.X + dx);
            Canvas.SetTop(_currentBlock, relativePoint.Y + dy);
            _lastPos = pos;
        }
    }
}
