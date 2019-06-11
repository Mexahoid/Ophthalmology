using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Emit;
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
using Microsoft.Win32;
using Ophthalmology.ConfigLogics.Classes;
using Ophthalmology.Patients.Classes;

namespace Ophthalmology.EyeLogics
{
    /// <summary>
    /// Логика взаимодействия для EyeWindow.xaml
    /// </summary>
    public partial class EyeWindow : Window
    {
        private readonly List<string> _diagnosisBinding;
        private readonly List<PropObj> _objs;
        private readonly ObservableCollection<string> _commentBinding;
        private readonly List<EllipseData> _ellipses;

        public List<int> RealDiagnosis { get; private set; }

        public List<string> Parameters { get; }

        public string NewImagePath { get; private set; }

        public string UsedImagePath { get; private set; }


        private bool _isPlacing = false;
        private bool _isDragging = false;
        private Point _lastPos;
        private Ellipse _currentFigure;
        private bool _programSelect;

        public class PropObj
        {
            public string Property { get; set; }
            public int Value { get; set; }
        }

        public class EllipseData
        {
            public Ellipse Figure { get; set; }

            public string Text { get; set; }
        }
        


        public EyeWindow(bool isLeft, Patient pat, DateTime time)
        {
            Parameters = new List<string>();

            var t = ConfigLogic.Instance.ReadEyeInfo(pat, time, isLeft);
            _ellipses = new List<EllipseData>();
            _commentBinding = new ObservableCollection<string>();
            InitializeComponent();
            _diagnosisBinding = new List<string>();
            _objs = new List<PropObj>();
            Title = isLeft ? "Левый глаз" : "Правый глаз";
            DataContext = this;
            DiagList.ItemsSource = _diagnosisBinding;

            foreach (string instanceParameter in ConfigLogic.Instance.Parameters)
            {
                PropObj po = new PropObj {Property = instanceParameter};
                _objs.Add(po);
            }

            if (t != null)
            {
                EyeParser ep = new EyeParser(t);
                ep.FillParams(_objs);
                ep.FillEllipseDatas(_ellipses, EyeCanvas, DeleteComment);
                foreach (EllipseData ellipseData in _ellipses)
                {
                    _commentBinding.Add(ellipseData.Text);
                }
                RealDiagnosis = ep.DiagsValues;
                UpdateDiag();
                EyeImage.Source = ep.GetImage();
                UsedImagePath = ep.ImagePath;
                NewImagePath = ep.ImagePath;
                OkButton.IsEnabled = true;
                DrawBtn.IsEnabled = true;
            }
            else
            {
                RealDiagnosis = new List<int>();
                for (int i = 0; i < 16; i++)
                {
                    RealDiagnosis.Add(0);
                }
            }

            ParametersDataGrid.ItemsSource = _objs;
            PointsList.ItemsSource = _commentBinding;
        }

        public List<int> GetParamValues()
        {
            var vals = new List<int>();
            foreach (PropObj propObj in _objs)
            {
                vals.Add(propObj.Value);
            }
            return vals;
        }

        private void UpdateDiag()
        {
            var texts = DiagnosiTextHolder.Instance.DiagsItself;
            _diagnosisBinding.Clear();
            for (int i = 0; i < texts.Count; i++)
            {
                if (RealDiagnosis[i] == 0)
                    continue;
                _diagnosisBinding.Add($"{texts[i]}: {RealDiagnosis[i]}");
            }

            DiagList.ItemsSource = null;
            DiagList.ItemsSource = _diagnosisBinding;
        }

        public Tuple<double[], double[], string[]> GetComments()
        {
            double[] xses = new double[_ellipses.Count];
            double[] yses = new double[_ellipses.Count];
            string[] texts = new string[_ellipses.Count];


            for (int i = 0; i < _ellipses.Count; i++)
            {
                EllipseData ed = _ellipses[i];
                Point relativePoint = ed.Figure.TransformToAncestor(EyeCanvas)
                    .Transform(new Point(0, 0));
                xses[i] = relativePoint.X;
                yses[i] = relativePoint.Y;
                texts[i] = ed.Text;
            }

            return Tuple.Create(xses, yses, texts);
        }

        private void DiagnosisButton_Click(object sender, RoutedEventArgs e)
        {
            EfronWindow ew = new EfronWindow(RealDiagnosis);
            if (ew.ShowDialog() != true)
            {
                return;
            }

            RealDiagnosis = ew.Diagnosis;

            UpdateDiag();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != true)
                return;
            NewImagePath = ofd.FileName;
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(NewImagePath);
            bi3.EndInit();
            EyeImage.Source = bi3;

            OkButton.IsEnabled = true;
            DrawBtn.IsEnabled = true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (PropObj t in _objs)
            {
                Parameters.Add(t.Property);
            }
            DialogResult = true;
            UsedImagePath = NewImagePath;
            Close();
        }


        private void DrawTextButtonClick(object sender, RoutedEventArgs e)
        {
            List<string> paramsList = new List<string>();

            foreach (PropObj propObj in _objs)
            {
                paramsList.Add($"{propObj.Property}: {propObj.Value}");
            }

            foreach (string s in _diagnosisBinding)
            {
                paramsList.Add(s);
            }

            EyeTextWindow w = new EyeTextWindow(paramsList, NewImagePath);
            if (w.ShowDialog() != true)
            {
                return;
            }
        }

        private void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            _isPlacing = true;
            AddComment.IsEnabled = false;
        }


        private void DeleteComment(EllipseData ed)
        {
            if (_ellipses.Contains(ed))
            {
                _ellipses.Remove(ed);
                EyeCanvas.Children.Remove(ed.Figure);
            }

            if (!_commentBinding.Contains(ed.Text))
                return;
            _programSelect = true;
            _commentBinding.Remove(ed.Text);
            _programSelect = false;
        }

        private void ImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_isPlacing)
                return;
            _lastPos = Mouse.GetPosition(EyeCanvas);
            TextHelperForm t = new TextHelperForm();
            if (t.ShowDialog() != true)
            {
                _isPlacing = false;
                return;
            }

            string text = t.Text;

            Ellipse ell = new Ellipse
            {
                Width = 20,
                Height = 20,
                Stroke = Brushes.Orange,
                Fill = Brushes.Transparent,
                StrokeThickness = 4
            };

            EllipseData ed = new EllipseData
            {
                Text = text,
                Figure = ell
            };
            ell.MouseRightButtonDown += (ssender, args) => DeleteComment(ed);
            _ellipses.Add(ed);

            Canvas.SetLeft(ell, _lastPos.X - 5);
            Canvas.SetTop(ell, _lastPos.Y - 5);
            EyeCanvas.Children.Add(ell);
            _commentBinding.Add(text);
            _isPlacing = false;
            AddComment.IsEnabled = true;
        }

        private void CanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            _isDragging = false;
            _isPlacing = false;
            AddComment.IsEnabled = true;
        }

        private void ImageMove(object sender, MouseEventArgs e)
        {
            
            if (_isDragging)
            {
                var pos = Mouse.GetPosition(EyeCanvas);
                double dx = pos.X - _lastPos.X;
                double dy = pos.Y - _lastPos.Y;
                Point relativePoint = _currentFigure.TransformToAncestor(EyeCanvas)
                    .Transform(new Point(0, 0));
                Canvas.SetLeft(_currentFigure, relativePoint.X + dx);
                Canvas.SetTop(_currentFigure, relativePoint.Y + dy);
                _lastPos = pos;
            }
            else
            {
                bool found = false;
                _lastPos = Mouse.GetPosition(EyeCanvas);
                _programSelect = true;
                PointsList.SelectedItem = null;
                _programSelect = false;
                foreach (EllipseData ellipseData in _ellipses)
                {
                    Point relativePoint = ellipseData.Figure.TransformToAncestor(EyeCanvas)
                        .Transform(new Point(0, 0));
                    // Переменная для небольшой области вокруг кружка
                    int delta = 5;
                    if (!(_lastPos.X >= relativePoint.X - delta) ||
                        !(_lastPos.X <= relativePoint.X + ellipseData.Figure.Width + delta) ||
                        !(_lastPos.Y >= relativePoint.Y - delta) ||
                        !(_lastPos.Y <= relativePoint.Y + ellipseData.Figure.Height + delta))
                    {
                        SelectEllipseCond(ellipseData, false);
                        continue;
                    }

                    if (found)
                        continue;
                    SelectEllipseCond(ellipseData, true);
                    // Выделить в списке
                    foreach (var pos in PointsList.Items)
                    {
                        if (pos.ToString() != ellipseData.Text)
                            continue;
                        _programSelect = true;
                        PointsList.SelectedItem = pos;
                        _programSelect = false;
                        break;
                    }
                    _currentFigure = ellipseData.Figure;

                    found = true;
                }
            }
        }

        private void SelectEllipseCond(EllipseData ellipseData, bool select)
        {
            Point relativePoint = ellipseData.Figure.TransformToAncestor(EyeCanvas)
                .Transform(new Point(0, 0));
            // Если выделение, то эллипс в 2 раза больше и красный
            int dims = select ? 40 : 20;
            int offset = select ? -10 : 10;
            Brush cl = select ? Brushes.Red : Brushes.Orange;
            ellipseData.Figure.Width = dims;
            ellipseData.Figure.Height = dims;
            if (ellipseData.Figure.Stroke != cl)
            {
                Canvas.SetLeft(ellipseData.Figure, relativePoint.X + offset);
                Canvas.SetTop(ellipseData.Figure, relativePoint.Y + offset);
            }
            ellipseData.Figure.Stroke = cl;
        }

        private void PointsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_programSelect)
                return;

            string text = PointsList.SelectedItem.ToString();
            foreach (EllipseData ellipseData in _ellipses)
            {
                SelectEllipseCond(ellipseData, text == ellipseData.Text);
            }
        }

        private void CanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_currentFigure == null)
                return;
            _isDragging = true;
            Mouse.OverrideCursor = Cursors.Hand;
        }
    }
}
