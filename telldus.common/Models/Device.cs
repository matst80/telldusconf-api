using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace telldusconf.Models
{
    public class Device
    {

        int Id { get; set; }
        string Name { get; set; }
        int Controller { get; set; }
        string Protocol { get; set; }

        string Model { get; set; }
        DeviceParameter Parameters { get; set; }

    }
}
