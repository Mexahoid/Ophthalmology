using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophthalmology.PatientLogics
{
    public class Patient
    {
        public string Name { get; set; }
        public ObservableCollection<DateTime> Dates { get; set; }
    }
}
