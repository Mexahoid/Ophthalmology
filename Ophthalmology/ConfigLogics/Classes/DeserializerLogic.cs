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

        public Tuple<List<string[]>, string> ReadEyeInfo(Patient pat, DateTime date, bool isLeft)
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

            EyeJson ej = JsonConvert.DeserializeObject<EyeJson>(ReadData(_root + '\\' + patientPath + "\\" + date.ToShortDateString() + "\\" + (isLeft ? "Левый глаз\\info.json" : "Правый глаз\\info.json")));

            List<string[]> arList = new List<string[]> { ej.Params, ej.Diags };
            return Tuple.Create(arList, ej.Path);
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
