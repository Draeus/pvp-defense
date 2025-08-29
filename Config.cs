using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TShockAPI;

namespace PvpDefense
{
    public class Config
    {
        public double defenseEffectiveness = 0.5;

        public bool critToggle = false;

        public void Write()
        {
            File.WriteAllText(PvpDefense.path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public static Config Read() {
            
            if (!File.Exists(PvpDefense.path))
                return new Config();
            try { 
                return JsonConvert.DeserializeObject<Config>(File.ReadAllText(PvpDefense.path)) ?? new Config();
            }
            catch {
                // If file is corrupted or unreadable, return default config
                return new Config();
            }
        }

    }
}
