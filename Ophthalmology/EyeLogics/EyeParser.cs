using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Ophthalmology.EyeLogics
{
    class EyeParser
    {
        private readonly List<string> _params;
        private readonly List<int> _paramsValues;
        public List<int> DiagsValues { get; }
        public string ImagePath { get; }

        public EyeParser(Tuple<string[], int[], int[], string, double[], double[], string[]> tuple)
        {
            _params = tuple.Item1.ToList();
            _paramsValues = tuple.Item2.ToList();
            DiagsValues = tuple.Item3.ToList();
            ImagePath = tuple.Item4;
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
