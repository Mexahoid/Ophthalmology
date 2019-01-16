using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ophthalmology.PatientLogics;

namespace Ophthalmology.ConfigLogics
{
    internal class ConfigLogic
    {
        private static ConfigLogic _instance;
        public static ConfigLogic Instance => _instance ?? (_instance = new ConfigLogic());

        public bool RootFolderTyped { get; set; }
        public bool ParametersTyped { get; set; }
        public bool IsAdding { get; set; }
        public bool IsConfigPresent { get; set; }
        private string _path = Directory.GetCurrentDirectory() + "\\config.json";

        public string[] Parameters { get; private set; }
        public string RootFolder { get; private set; }

        private ConfigLogic()
        {
            IsConfigPresent = Directory.GetFiles(Directory.GetCurrentDirectory()).Contains(_path);
            if (IsConfigPresent)
                ReadConfig();
            IsAdding = true;
        }

        public Tuple<string[], string> ReadConfig()
        {
            string json = ReadData(_path);
            ConfigJson cfg = JsonConvert.DeserializeObject<ConfigJson>(json);
            Parameters = cfg.Parameters;
            RootFolder = cfg.RootFolder;
            return new Tuple<string[], string>(cfg.Parameters, cfg.RootFolder);
        }

        private string ReadData(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }

        private void Serialize(object obj, string path)
        {
            var json = JsonConvert.SerializeObject(obj);
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(json);
            }
        }

        public void CreateConfig(string[] parameters, string rootFolder)
        {
            Parameters = parameters;
            RootFolder = rootFolder;
            ConfigJson js = new ConfigJson
            {
                Parameters = parameters,
                RootFolder = rootFolder
            };
            Serialize(js, Directory.GetCurrentDirectory() + "\\config.json");

            if (!IsConfigPresent || Directory.GetFiles(RootFolder).Length < 1)
            {
                WritePatientsList(new List<string[]>
                {
                    new string[0],
                    new string[0]
                });
            }
        }


        public void AddPatient(string name)
        {
            var curr = ReadPatientsList();
            int num = curr[0].Length;
            string path = $"{num + 1}. {name}";
            var names = curr[0];
            var paths = curr[1];
            Array.Resize(ref names, num + 1);
            Array.Resize(ref paths, num + 1);
            names[num] = name;
            paths[num] = path;
            Directory.CreateDirectory(RootFolder + "\\" + path);
            WriteDatesList(new List<string[]>
            {
                new string[0],
                new string[0]
            }, path);
            WritePatientsList(new List<string[]>
            {
                names,
                paths
            });
        }

        public void AddDate(Patient pat, DateTime date)
        {
            var pats = ReadPatientsList();
            string paths = null;
            for (int i = 0; i < pats[0].Length; i++)
            {
                if (pats[0][i] != pat.Name)
                    continue;
                paths = pats[1][i];
                break;
            }

            var dates = ReadDatesList(paths);
            var datesArr = dates[0];
            var datesPaths = dates[1];

            Array.Resize(ref datesArr, datesArr.Length + 1);
            Array.Resize(ref datesPaths, datesArr.Length);

            datesArr[datesArr.Length - 1] = date.ToShortDateString();
            datesPaths[datesPaths.Length - 1] = date.ToShortDateString();
            Directory.CreateDirectory(RootFolder + "\\" + paths + "\\" + datesPaths[datesPaths.Length - 1]);

            WriteDatesList(new List<string[]>
            {
                datesArr,
                datesPaths
            }, paths);

        }

        public List<string[]> ReadPatientsList()
        {
            PatientsJson pj = JsonConvert.DeserializeObject<PatientsJson>(ReadData(RootFolder + "\\patientlist.json"));
            return new List<string[]>
            {
                pj.PatientNames,
                pj.PatientFolderPaths
            };
        }

        public void WritePatientsList(List<string[]> fields)
        {
            PatientsJson pj = new PatientsJson
            {
                PatientNames = fields[0],
                PatientFolderPaths = fields[1]
            };
            Serialize(pj, RootFolder + "\\patientlist.json");
        }


        public ObservableCollection<Patient> GetPatients()
        {
            var pats = new ObservableCollection<Patient>();

            var tp = ReadPatientsList();

            for (int i = 0; i < tp[0].Length; i++)
            {
                Patient pat = new Patient
                {
                    Name = tp[0][i]
                };

                var td = ReadDatesList(tp[1][i]);
                pat.Dates = new ObservableCollection<DateTime>();
                for (int j = 0; j < td[0].Length; j++)
                {
                    pat.Dates.Add(DateTime.Parse(td[0][j]));
                }
                pats.Add(pat);
            }
            return pats;
        }

        public List<string[]> ReadDatesList(string patientPath)
        {
            DatesJson pj = JsonConvert.DeserializeObject<DatesJson>(ReadData(RootFolder + '\\' + patientPath + "\\datelist.json"));
            return new List<string[]>
            {
                pj.DateStrings,
                pj.DateFolderPaths
            };
        }

        public void WriteDatesList(List<string[]> fields, string patientPath)
        {
            DatesJson dj = new DatesJson
            {
                DateStrings = fields[0],
                DateFolderPaths = fields[1]
            };
            Serialize(dj, RootFolder + '\\' + patientPath + "\\datelist.json");
        }
    }
}
