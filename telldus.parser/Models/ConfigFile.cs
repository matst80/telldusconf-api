using System;
namespace telldusconf.Models
{
    public class ConfigFile
    {
        [Key("user")]
        public string User { get; set; }

        public ConfigFile()
        {
        }
    }
}
