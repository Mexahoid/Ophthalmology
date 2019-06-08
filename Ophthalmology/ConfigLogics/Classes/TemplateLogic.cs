using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Ophthalmology.ConfigLogics.Serialization;
using Ophthalmology.Properties;

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

        private readonly Dictionary<string, string> _aliases;

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
            _aliases = new Dictionary<string, string>();

            LoadTemplateList();
        }

        public List<string[]> GetAliases()
        {
            List<string[]> aliases = new List<string[]>();

            var keys = _aliases.Keys;

            foreach (string key in keys)
            {
                aliases.Add(new []{key, _aliases[key]});
            }

            return aliases;
        }

        public void InitParams(IEnumerable<string> paramnames)
        {
            List<string> names = new List<string>
            {
                Resources.B,
                Resources.Cd,
                Resources.Ci,
                Resources.Cn,
                Resources.Co,
                Resources.Cr,
                Resources.Cs,
                Resources.Cst,
                Resources.Cu,
                Resources.Eb,
                Resources.Em,
                Resources.Ep,
                Resources.Lr,
                Resources.Mgd,
                Resources.Pc,
                Resources.Slk
            };
            
            _aliases.Clear();
            int number = 1;
            foreach (string paramname in paramnames)
            {
                
                _aliases.Add($"$_{number++}", paramname);
            }

            foreach (string name in names)
            {
                _aliases.Add($"$_{number++}", name);
            }
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

            _sl.WriteTemplatesList(new List<string[]>
            {
                _names.ToArray(),
                _paths.ToArray()
            }, _file);
        }

        public void AddTemplate(string name, TextRange tr)
        {
            if (_names.Contains(name))
                return;
            string path = _root + "\\" + name + ".rtf";
            ReSave(path, tr);

            _names.Add(name);
            _paths.Add(path);

            _sl.WriteTemplatesList(new List<string[]>
            {
                _names.ToArray(),
                _paths.ToArray()
            }, _file);
        }

        public void EditTemplate(string name, TextRange tr)
        {
            string path = _root + "\\" + name + ".rtf";
            ReSave(path, tr);
        }

        public void SaveReport(string reportpath, string templatename, List<string[]>[] params_diags)
        {
            FlowDocument workDoc = new FlowDocument();
            TextRange tr = new TextRange(workDoc.ContentStart, workDoc.ContentEnd);
            LoadTemplate(templatename, tr);
            var a = workDoc.Blocks;

            var eyes = new[]
            {
                Tuple.Create(params_diags[0], params_diags[1]),
                Tuple.Create(params_diags[2], params_diags[3])
            };

            foreach (Block block in a)
            {
                var b = block as Paragraph;
                var iss = b.Inlines;

                foreach (Inline inline in iss)
                {
                    var ttt = inline.TextDecorations;
                }

                var tt = iss.FirstInline;
            }

            int il = tr.Text.Length;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < il; i++)
            {
                if (tr.Text[i] != '$')
                {
                    if (tr.Text[i] == '\r')
                    {
                        sb.Append(Environment.NewLine);
                        i++;
                    }
                    else
                        sb.Append(tr.Text[i]);
                    continue;
                }
                    
                int len = 1;
                while (tr.Text[i + len] != ' ' && tr.Text[i + len] != '\r')
                    len++;

                var tag = tr.Text.Substring(i, len);

                var tag_parts = tag.Split('-');
                int pos = int.Parse(tag_parts[1][0].ToString()) - 1;
                var alias = _aliases[tag_parts[0]];
                if (pos > 1)
                    continue;
                var eye = eyes[pos];
                foreach (var t in eye.Item1)
                {
                    if (t[0] != alias.Trim())
                        continue;
                    sb.Append(t[1].Trim());
                    break;
                }
                foreach (var t in eye.Item2)
                {
                    if (t[0] != alias.Trim())
                        continue;
                    sb.Append(t[1].Trim());
                    break;
                }

                i += len - 1;
            }

            tr.Text = sb.ToString();
            il = tr.Text.Length;



            ReSave(reportpath, tr);
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
