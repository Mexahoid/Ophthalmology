using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using Ophthalmology.ConfigLogics.Serialization;
using Ophthalmology.Patients.Classes;
using Ophthalmology.Properties;

namespace Ophthalmology.ConfigLogics.Classes
{
    internal class ConfigLogic
    {
        private static ConfigLogic _instance;
        public static ConfigLogic Instance => _instance ?? (_instance = new ConfigLogic());

        public bool RootFolderTyped { get; set; }
        public bool ParametersTyped { get; set; }
        public bool IsAdding { get; set; }
        public bool IsConfigPresent { get; set; }
        private readonly string _path = Directory.GetCurrentDirectory() + "\\config.json";

        public string[] Parameters { get; private set; }
        private string _root;

        private SerializerLogic _sl;
        private DeserializerLogic _dl;
        private PatientLogic _pl;
        private DateLogic _dtl;
        private EyeLogic _el;
        private TemplateLogic _tl;

        private ConfigLogic()
        {
            IsConfigPresent = Directory.GetFiles(Directory.GetCurrentDirectory()).Contains(_path);
            _dl = new DeserializerLogic();
            if (IsConfigPresent)
            {
                (var pars, string root) = _dl.ReadConfig(_path);
                Parameters = pars;
                _root = root;
                _sl = new SerializerLogic(_root);
                _tl = new TemplateLogic(_root, _sl, _dl);
                _tl.InitParams(Parameters);
                _pl = new PatientLogic(_sl, _root, _dl);
                _dtl = new DateLogic(_sl, _root, _dl);
                _el = new EyeLogic(_sl, _root, _dl);
            }
            IsAdding = true;
        }

        public void SaveReport(string reportpath, string templatename, List<string[]>[] params_diags, List<string>[] texts)
        {
            _tl.SaveReport(reportpath, templatename, params_diags, texts);
        }

        public List<string[]> GetTemplateAliases()
        {
            return _tl.GetAliases();
        }

        public void AddTemplate(string name, TextRange tr)
        {
            _tl.AddTemplate(name, tr);
        }

        public void DeleteTemplate(string name)
        {
            _tl.DeleteTemplate(name);
        }

        public void LoadTemplate(string name, TextRange tr)
        {
            _tl.LoadTemplate(name, tr);
        }

        public List<string> GetTemplateNames()
        {
            return _tl.GetNames();
        }

        public void EditTemplate(string oldname, string newname, TextRange tr)
        {
            if(oldname == newname)
                _tl.EditTemplate(oldname, tr);
            else
                _tl.EditTemplate(oldname, newname, tr);
        }

        public Tuple<string[], string> GetParametersAndRoot()
        {
            return new Tuple<string[], string>(Parameters, _root);
        }

        public void SaveLastPatient(Patient p, DateTime d)
        {
            _pl.SaveLastPatient(p, d);
        }

        public (Patient, DateTime) LoadLastPatient()
        {
            return _pl.LoadLastPatient(GetPatients().ToList());
        }

        public void CreateConfig(string[] parameters, string rootFolder)
        {
            Parameters = parameters;
            _root = rootFolder;
            ConfigJson js = new ConfigJson
            {
                Parameters = parameters,
                RootFolder = rootFolder
            };

            _dl = new DeserializerLogic(_root);
            _sl = new SerializerLogic(_root);
            _pl = new PatientLogic(_sl, _root, _dl);
            _dtl = new DateLogic(_sl, _root, _dl);
            _el = new EyeLogic(_sl, _root, _dl);
            _sl.SaveConfig(js, IsConfigPresent);
        }

        public Tuple<string[], int[], int[], string, double[], double[], string[]> ReadEyeInfo(Patient pat, DateTime date, bool isLeft)
        {
            return _el.LoadEyeInfo(isLeft, pat, date);
        }

        public void AddPatient(string name)
        {
            _pl.AddPatient(name);
        }

        public void DeletePatient(int pos)
        {
            _pl.DeletePatient(pos);
        }

        public void DeleteDate(Patient pat, DateTime date)
        {
            _dtl.DeleteDate(pat, date);
        }

        public void AddDate(Patient pat, DateTime date)
        {
            _dtl.AddDate(pat, date);
        }

        public void AddEye(bool isLeft, Patient pat, DateTime date, string newPath, Tuple<string[], int[], int[], string, double[], double[], string[]> args)
        {
            _el.AddEye(isLeft, pat, date, newPath, args);
        }

        public Patient GetPatient(Patient input, bool next)
        {
            return _pl.GetPatient(input, next);
        }

        public ObservableCollection<Patient> GetPatients()
        {
            return _pl.GetPatients();
        }

        public bool CheckIfEyeExist(Patient pat, DateTime date, bool isLeft)
        {
            return _el.CheckIfEyeExist(pat, date, isLeft);
        }

    }
}
