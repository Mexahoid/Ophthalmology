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
    class SerializerLogic
    {
        private readonly string _root;
        public SerializerLogic(string root)
        {
            _root = root;
        }

        public void SaveConfig(ConfigJson js, bool isPresent)
        {
            Serialize(js, Directory.GetCurrentDirectory() + "\\config.json");

            if (!isPresent || Directory.GetFiles(_root).Length < 1)
            {
                WritePatientsList(new List<string[]>
                {
                    new string[0],
                    new string[0]
                });
            }
        }

        public void WriteTemplatesList(List<string[]> fields, string path)
        {
            TemplateJson tj = new TemplateJson
            {
                Names = fields[0],
                Paths = fields[1]
            };
            Serialize(tj, path);
        }

        public void WritePatientsList(List<string[]> fields)
        {
            PatientsJson pj = new PatientsJson
            {
                PatientNames = fields[0],
                PatientFolderPaths = fields[1]
            };
            Serialize(pj, _root + "\\patientlist.json");
        }


        public void WriteDatesList(List<string[]> fields, string patientPath)
        {
            DatesJson dj = new DatesJson
            {
                DateStrings = fields[0],
                DateFolderPaths = fields[1]
            };
            Serialize(dj, _root + '\\' + patientPath + "\\datelist.json");
        }

        public void WriteLastPatient(string name, string date)
        {
            LastPatientJson lj = new LastPatientJson
            {
                LastPatient = new[] 
                {
                    name,
                    date
                }
            };
            Serialize(lj, _root + "\\lastpatient.json");
        }

        public void WriteEyeInfo(Tuple<string[], int[], int[], string, double[], double[], string[]> args)
        {
            EyeJson ej = new EyeJson
            {
                Params = args.Item1,
                ParamsValues = args.Item2,
                Diags = args.Item3,
                Path = args.Item4 + "\\image.jpg",
                Xses = args.Item5,
                Yses = args.Item6,
                Texts = args.Item7
            };
            Serialize(ej, args.Item4 + "\\info.json");
        }

        private void Serialize(object obj, string path)
        {
            var json = JsonConvert.SerializeObject(obj);
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(json);
            }
        }
    }
}
