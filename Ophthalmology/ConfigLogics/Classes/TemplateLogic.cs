using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Ophthalmology.ConfigLogics.Serialization;

namespace Ophthalmology.ConfigLogics.Classes
{
    class TemplateLogic
    {
        private readonly string _root;
        private readonly string _file;
        private SerializerLogic _sl;
        private DeserializerLogic _dl;
        private readonly List<string> _names;
        private readonly List<string> _paths;

        public TemplateLogic(string root, SerializerLogic sl, DeserializerLogic dl)
        {
            _sl = sl;
            _dl = dl;
            _root = root + "\\Templates";
            _file = _root + "\\templates.json";
            if (!Directory.Exists(_root))
                Directory.CreateDirectory(_root);

            _names = new List<string>();
            _paths = new List<string>();

            LoadTemplateList();
        }

        public List<string> GetNames()
        {
            return _names;
        }

        public void LoadTemplateList()
        {
            _names.Clear();
            _paths.Clear();
            if (!File.Exists(_file))
                return;
            var a = _dl.ReadTemplatesList(_file);
            foreach (string n in a[0])
            {
                _names.Add(n);
            }

            foreach (string p in a[1])
            {
                _paths.Add(p);
            }
        }

        public void DeleteTemplate(string name)
        {
            int ind = _names.IndexOf(name);
            _names.RemoveAt(ind);
            if (File.Exists(_paths[ind]))
                File.Delete(_paths[ind]);
            _paths.RemoveAt(ind);

            _sl.WriteTemplatesList(new List<string[]> { _names.ToArray(), _paths.ToArray() }, _file);
        }

        public void AddTemplate(string name, TextRange tr)
        {
            if (_names.Contains(name))
                return;
            string path = _root + "\\" + name + ".rtf";
            ReSave(path, tr);

            _names.Add(name);
            _paths.Add(path);

            _sl.WriteTemplatesList(new List<string[]> { _names.ToArray(), _paths.ToArray() }, _file);
        }

        public void EditTemplate(string name, TextRange tr)
        {
            string path = _root + "\\" + name + ".rtf";
            ReSave(path, tr);
        }

        public void EditTemplate(string oldname, string newname, TextRange tr)
        {
            int ind = _names.IndexOf(oldname);
            if(File.Exists(_paths[ind]))
                File.Delete(_paths[ind]);
            _names.RemoveAt(ind);
            _paths.RemoveAt(ind);

            _names.Add(newname);
            string path = _root + "\\" + newname + ".rtf";
            _paths.Add(path);
            ReSave(path, tr);
        }

        private void ReSave(string path, TextRange tr)
        {
            using (FileStream fs = File.Create(path))
            {
                tr.Save(fs, DataFormats.Rtf);
            }
        }

        public void LoadTemplate(string name, TextRange tr)
        {
            string path = _paths[_names.IndexOf(name)];
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                tr.Load(fs, DataFormats.Rtf);
            }
        }
    }
}
