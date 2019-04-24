using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophthalmology.Patients.Classes;

namespace Ophthalmology.ConfigLogics.Classes
{
    class DateLogic
    {
        private readonly SerializerLogic _sl;
        private readonly DeserializerLogic _dl;
        private readonly string _root;
        public DateLogic(SerializerLogic sl, string root, DeserializerLogic dl)
        {
            _sl = sl;
            _root = root;
            _dl = dl;
        }

        public void DeleteDate(Patient pat, DateTime date)
        {
            // Для дат переименования папок не надо.
            var pats = _dl.ReadPatientsList();
            string paths = null;
            for (int i = 0; i < pats[0].Length; i++)
            {
                if (pats[0][i] != pat.Name)
                    continue;
                paths = pats[1][i];
                break;
            }

            var dates = _dl.ReadDatesList(paths);
            var datesArr = dates[0];
            var datesPaths = dates[1];

            string d = date.ToShortDateString();

            var lp = datesPaths.ToList();
            int pos = lp.IndexOf(d);
            lp.RemoveAt(pos);
            datesPaths = lp.ToArray();

            lp = datesArr.ToList();
            lp.RemoveAt(pos);
            datesArr = lp.ToArray();
            try
            {
                Directory.Delete(_root + "\\" + paths + "\\" + d, true);
            }
            catch
            {
                // Подавлено
            }

            _sl.WriteDatesList(new List<string[]>
            {
                datesArr,
                datesPaths
            }, paths);
        }

        public void AddDate(Patient pat, DateTime date)
        {
            var pats = _dl.ReadPatientsList();
            string paths = null;
            for (int i = 0; i < pats[0].Length; i++)
            {
                if (pats[0][i] != pat.Name)
                    continue;
                paths = pats[1][i];
                break;
            }

            var dates = _dl.ReadDatesList(paths);
            var datesArr = dates[0];
            var datesPaths = dates[1];

            Array.Resize(ref datesArr, datesArr.Length + 1);
            Array.Resize(ref datesPaths, datesArr.Length);

            datesArr[datesArr.Length - 1] = date.ToShortDateString();
            datesPaths[datesPaths.Length - 1] = date.ToShortDateString();
            string path = _root + "\\" + paths + "\\" + datesPaths[datesPaths.Length - 1];

            Directory.CreateDirectory(path);
            Directory.CreateDirectory(path + "\\Левый глаз");
            Directory.CreateDirectory(path + "\\Правый глаз");
            _sl.WriteDatesList(new List<string[]>
            {
                datesArr,
                datesPaths
            }, paths);

        }
    }
}
