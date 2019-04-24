using System;
using System.Collections.ObjectModel;

namespace Ophthalmology.Patients.Classes
{
    public class Patient
    {
        public string Name { get; set; }
        public ObservableCollection<DateTime> Dates { get; set; }
    }
}
