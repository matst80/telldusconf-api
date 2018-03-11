using System;
using System.Collections.Generic;

namespace telldusconf.Models
{
    public class ConfigFile
    {
        [Key("user")]
        public string User { get; set; }

        [ListKey("device")]
        public List<Device> Devices { get; set; }

        [Key("controller")]
        public TelldusController Controller { get; set; }

        public ConfigFile()
        {
        }
    }
}
