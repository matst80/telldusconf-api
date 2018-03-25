namespace Telldusconf.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Device
    {
        [Key("id")]
        public int Id { get; set; }

        [Key("name")]
        public string Name { get; set; }

        [Key("controller")]
        public int Controller { get; set; }

        [Key("protocol")]
        public string Protocol { get; set; }

        [Key("model")]
        public string Model { get; set; }

        [Key("parameters")]
        public DeviceParameter Parameters { get; set; }
    }
}
