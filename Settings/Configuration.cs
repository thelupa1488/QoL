using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QoL.Settings
{
    public class Config
    {
        public bool LogInformation = true;
    }

    public static class Configuration
    {
        public static Config _Configuration { get; set; }

        public static void CheckExistence()
        {
            if (!Directory.Exists("QoL")) Directory.CreateDirectory("QoL");
            if (!Directory.Exists("QoL\\Logs")) Directory.CreateDirectory("QoL\\Logs");
            if (!File.Exists("QoL\\Configuration.json")) File.WriteAllText("QoL\\Configuration.json", JsonConvert.SerializeObject(new Config()));
        }

        public static void LoadConfiguration()
        {
            CheckExistence();
            _Configuration = JsonConvert.DeserializeObject<Config>(File.ReadAllText("QoL\\Configuration.json"));
        }

        public static void SaveConfiguration() => File.WriteAllText("QoL\\Configuration.json", JsonConvert.SerializeObject(_Configuration));
    }
}
