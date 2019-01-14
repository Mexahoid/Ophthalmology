using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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


        private ConfigLogic()
        {
            IsConfigPresent = Directory.GetFiles(Directory.GetCurrentDirectory()).Contains(Directory.GetCurrentDirectory() + "\\config.json");
            IsAdding = true;
        }

        public Tuple<string[], string> ReadConfig()
        {
            var path = Directory.GetCurrentDirectory() + "\\config.json";
            using (StreamReader sr = new StreamReader(path))
            {
                path = sr.ReadToEnd();
            }
            ConfigJson cfg = JsonConvert.DeserializeObject<ConfigJson>(path);
            return new Tuple<string[], string>(cfg.Parameters, cfg.RootFolder);
        }

        public void CreateConfig(string[] parameters, string rootFolder)
        {
            var path = Directory.GetCurrentDirectory() + "\\config.json";
            ConfigJson js = new ConfigJson
            {
                Parameters = parameters,
                RootFolder = rootFolder
            };
            var json = JsonConvert.SerializeObject(js);

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(json);
            }
        }
    }
}
