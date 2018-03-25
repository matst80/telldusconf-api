// ConfigFile.cs
namespace Telldusconf.Models
{
    using System;
    using System.Collections.Generic;

    public class ConfigFile
    {
        public ConfigFile()
        {
        }

        [Key("user")]
        public string User { get; set; }

        [ListKey("device")]
        public List<Device> Devices { get; set; }

        [Key("controller")]
        public TelldusController Controller { get; set; }
    }
}