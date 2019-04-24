using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophthalmology.Patients.Classes;

namespace Ophthalmology.ConfigLogics.Classes
{
    class EyeLogic
    {
        private readonly SerializerLogic _sl;
        private readonly DeserializerLogic _dl;
        private readonly string _root;
        public EyeLogic(SerializerLogic sl, string root, DeserializerLogic dl)
        {
            _sl = sl;
            _root = root;
            _dl = dl;
        }

        public void AddEye(bool isLeft, Patient pat, DateTime date, string path, List<string> pars, List<string> diags)
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
            string datePath = null;

            for (int i = 0; i < dates[0].Length; i++)
            {
                if (dates[0][i] != date.ToShortDateString())
                    continue;
                datePath = dates[1][i];
                break;
            }

            string eye = isLeft ? "Левый глаз" : "Правый глаз";

            string rp = _root + "\\" + paths + "\\" + datePath + "\\" + eye;

            try
            {
                File.Copy(path, rp + "\\image.jpg");
            }
            catch
            {
                // Подавлено
            }

            _sl.WriteEyeInfo(pars, diags, rp);
        }

        public bool CheckIfEyeExist(Patient pat, DateTime date, bool isLeft)
        {
            var pats = _dl.ReadPatientsList();
            string patientPath = null;
            for (int i = 0; i < pats[0].Length; i++)
            {
                if (pats[0][i] != pat.Name)
                    continue;
                patientPath = pats[1][i];
                break;
            }

            return File.Exists(_root + '\\' + patientPath + "\\" + date.ToShortDateString() + "\\" +
                               (isLeft ? "Левый глаз\\info.json" : "Правый глаз\\info.json"));
        }
    }
}
