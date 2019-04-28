using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ophthalmology.ConfigLogics.Serialization;
using Ophthalmology.Patients.Classes;

namespace Ophthalmology.ConfigLogics.Classes
{
    class DeserializerLogic
    {
        private string _root;

        private string ReadData(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }

        public DeserializerLogic(string root)
        {
            _root = root;
        }

        public DeserializerLogic()
        {
            
        }

        public Tuple<string[], string> ReadConfig(string path)
        {
            string json = ReadData(path);
            ConfigJson cfg = JsonConvert.DeserializeObject<ConfigJson>(json);
            _root = cfg.RootFolder;
            return Tuple.Create(cfg.Parameters, _root);
        }

        public List<string[]> ReadPatientsList()
        {
            PatientsJson pj = JsonConvert.DeserializeObject<PatientsJson>(ReadData(_root + "\\patientlist.json"));
            return new List<string[]>
            {
                pj.PatientNames,
                pj.PatientFolderPaths
            };
        }

        public Tuple<string[], int[], int[], string> ReadEyeInfo(Patient pat, DateTime date, bool isLeft)
        {
            var pats = ReadPatientsList();
            string patientPath = null;
            for (int i = 0; i < pats[0].Length; i++)
            {
                if (pats[0][i] != pat.Name)
                    continue;
                patientPath = pats[1][i];
                break;
            }

            string path = _root + '\\' + patientPath + "\\" + date.ToShortDateString() + "\\" +
                          (isLeft ? "Левый глаз\\info.json" : "Правый глаз\\info.json");
            if (!File.Exists(path))
                return null;
            
            EyeJson ej = JsonConvert.DeserializeObject<EyeJson>(ReadData(path));

            return Tuple.Create(ej.Params, ej.ParamsValues, ej.Diags, ej.Path);
        }

        public List<string[]> ReadDatesList(string patientPath)
        {
            DatesJson pj = JsonConvert.DeserializeObject<DatesJson>(ReadData(_root + '\\' + patientPath + "\\datelist.json"));
            return new List<string[]>
            {
                pj.DateStrings,
                pj.DateFolderPaths
            };
        }
    }
}
