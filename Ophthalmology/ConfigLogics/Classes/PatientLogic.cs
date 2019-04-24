using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophthalmology.ConfigLogics.Serialization;
using Ophthalmology.Patients.Classes;

namespace Ophthalmology.ConfigLogics.Classes
{
    class PatientLogic
    {
        private readonly SerializerLogic _sl;
        private readonly DeserializerLogic _dl;
        private readonly string _root;

        public PatientLogic(SerializerLogic sl, string root, DeserializerLogic dl)
        {
            _sl = sl;
            _root = root;
            _dl = dl;
        }

        private void SavePatient(List<string[]> dates, List<string[]> namesPaths, string path)
        {
            Directory.CreateDirectory(_root + "\\" + path);
            _sl.WriteDatesList(dates, path);
            _sl.WritePatientsList(namesPaths);
        }

        public void AddPatient(string name)
        {
            var curr = _dl.ReadPatientsList();
            int num = curr[0].Length;
            string path = $"{num + 1}. {name}";
            var names = curr[0];
            var paths = curr[1];
            Array.Resize(ref names, num + 1);
            Array.Resize(ref paths, num + 1);
            names[num] = name;
            paths[num] = path;

            var dates = new List<string[]>
            {
                new string[0],
                new string[0]
            };
            var namesPaths = new List<string[]>
            {
                names,
                paths
            };
            SavePatient(dates, namesPaths, path);
        }

        public ObservableCollection<Patient> GetPatients()
        {
            var pats = new ObservableCollection<Patient>();

            var tp = _dl.ReadPatientsList();

            for (int i = 0; i < tp[0].Length; i++)
            {
                Patient pat = new Patient
                {
                    Name = tp[0][i]
                };

                var td = _dl.ReadDatesList(tp[1][i]);
                pat.Dates = new ObservableCollection<DateTime>();
                for (int j = 0; j < td[0].Length; j++)
                {
                    pat.Dates.Add(DateTime.Parse(td[0][j]));
                }
                pats.Add(pat);
            }
            return pats;
        }

        public Patient GetPatient(Patient input, bool next)
        {
            var p = GetPatients();

            int ind;

            for (ind = 0; ind < p.Count; ind++)
            {
                if (p[ind].Name == input.Name)
                    break;
            }

            if (next)
            {
                return ind == p.Count - 1 ? null : p[ind + 1];
            }
            return ind == 0 ? null : p[ind - 1];
        }

        public void DeletePatient(int pos)
        {
            // 0 - Names, 1 - FolderPaths
            var curr = _dl.ReadPatientsList();

            // todo: переименованеи папок
            // Да зачем? Будем считать, что это id'шники

            var c = curr[0];
            var p = curr[1];

            // Delete [pos] directory
            try
            {
                Directory.Delete(_root + "\\" + p[pos], true);
            }
            catch
            {
                // Подавлено
            }
            var cl = c.ToList();
            cl.RemoveAt(pos);
            c = cl.ToArray();
            cl = p.ToList();
            cl.RemoveAt(pos);
            p = cl.ToArray();
            // For remains rename to N-1
            int i;

            for (i = pos; i < p.Length; i++)
            {
                int num = int.Parse(p[i].Split('.')[0]) - 1;
                string old = p[i];
                p[i] = $"{num}. " + c[i];
                try
                {
                    Directory.Move(_root + "\\" + old, _root + "\\" + p[i]);
                }
                catch
                {
                    // Подавлено
                }

            }

            _sl.WritePatientsList(new List<string[]>
            {
                c,
                p
            });
        }
    }
}
