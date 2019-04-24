using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Ophthalmology.ConfigLogics.Serialization;
using Ophthalmology.Patients.Classes;

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

        private readonly SerializerLogic _sl;
        private readonly DeserializerLogic _dl;
        private readonly PatientLogic _pl;
        private readonly DateLogic _dtl;
        private readonly EyeLogic _el;

        private ConfigLogic()
        {
            IsConfigPresent = Directory.GetFiles(Directory.GetCurrentDirectory()).Contains(_path);
            _dl = new DeserializerLogic();
            if (IsConfigPresent)
            {
                (var pars, string root) = _dl.ReadConfig(_path);
                Parameters = pars;
                _root = root;
            }

            _sl = new SerializerLogic(_root);
            _pl = new PatientLogic(_sl, _root, _dl);
            _dtl = new DateLogic(_sl, _root, _dl);
            _el = new EyeLogic(_sl, _root, _dl);

            IsAdding = true;
        }

        public Tuple<string[], string> GetParametersAndRoot()
        {
            return new Tuple<string[], string>(Parameters, _root);
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
            _sl.SaveConfig(js, IsConfigPresent);
        }

        public Tuple<List<string[]>, string> ReadEyeInfo(Patient pat, DateTime date, bool isLeft)
        {
            return _dl.ReadEyeInfo(pat, date, isLeft);
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

        public void AddEye(bool isLeft, Patient pat, DateTime date, string path, List<string> pars, List<string> diags)
        {
            _el.AddEye(isLeft, pat, date, path, pars, diags);
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
