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

        public EyeParser(Tuple<string[], int[], int[], string> tuple)
        {
            (var item1, var item2, var item3, string item4) = tuple;
            _params = item1.ToList();
            _paramsValues = item2.ToList();
            DiagsValues = item3.ToList();
            ImagePath = item4;
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
