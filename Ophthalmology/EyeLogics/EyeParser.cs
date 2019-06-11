using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ophthalmology.EyeLogics
{
    class EyeParser
    {
        private readonly List<string> _params;
        private readonly List<int> _paramsValues;
        public List<int> DiagsValues { get; }
        public string ImagePath { get; }

        private double[] _xses, _yses;
        private string[] _texts;

        public EyeParser(Tuple<string[], int[], int[], string, double[], double[], string[]> tuple)
        {
            _params = tuple.Item1.ToList();
            _paramsValues = tuple.Item2.ToList();
            DiagsValues = tuple.Item3.ToList();
            ImagePath = tuple.Item4;
            _xses = tuple.Item5;
            _yses = tuple.Item6;
            _texts = tuple.Item7;
        }

        public void FillParams(IEnumerable<EyeWindow.PropObj> bindingList)
        {
            foreach (EyeWindow.PropObj param in bindingList)
            {
                int ind = _params.IndexOf(param.Property);
                if(ind <= -1)
                    continue;
                param.Value = _paramsValues[ind];
            }
        }



        public void FillEllipseDatas(List<EyeWindow.EllipseData> datas, Canvas c, Action<EyeWindow.EllipseData> deleter)
        {
            c.Children.Clear();
            if (_xses == null)
                return;
            for (int i = 0; i < _xses.Length; i++)
            {
                EyeWindow.EllipseData ed = new EyeWindow.EllipseData
                {
                    Text = _texts[i],
                    Figure = new Ellipse
                    {
                        Width = 20,
                        Height = 20,
                        Stroke = Brushes.Orange,
                        Fill = Brushes.Transparent,
                        StrokeThickness = 4
                    }
                };

                c.Children.Add(ed.Figure);
                Point relativePoint = ed.Figure.TransformToAncestor(c)
                    .Transform(new Point(0, 0));
                Canvas.SetLeft(ed.Figure, relativePoint.X + _xses[i]);
                Canvas.SetTop(ed.Figure, relativePoint.Y + _yses[i]);
                ed.Figure.MouseRightButtonDown += (ssender, args) => deleter(ed);
                datas.Add(ed);
            }
        }


        public BitmapImage GetImage()
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(ImagePath);
            bi3.EndInit();
            return bi3;
        }
    }
}
