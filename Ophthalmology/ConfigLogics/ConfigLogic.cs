using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophthalmology.ConfigLogics
{
    internal class ConfigLogic
    {
        private ConfigLogic _instance;
        public ConfigLogic GetInstance => _instance ?? (_instance = new ConfigLogic());

        private ConfigLogic()
        {
        }

        public bool IsConfigPresent() => Directory.GetFiles(Directory.GetCurrentDirectory()).Contains("config.json");


    }
}
