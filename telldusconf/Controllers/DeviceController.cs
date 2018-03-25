using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using telldusconf.Models;
using telldusconf.Parsing;

namespace telldusconf.Controllers
{
    [Produces("application/json")]
    [Route("api/Device")]
    public class DeviceController : Controller
    {
        [SwaggerOperation("GetAllDevices")]
        [HttpGet(Name = "GetStores")]
        public List<Device> GetAllDevices()
        {
            var c = GetConfig();
            return c.Devices;
        }

        private static ConfigFile GetConfig()
        {
            var p = new Parser("./telldus.conf");
            return p.Parse();
        }

        private static void StoreConfig(ConfigFile c)
        {
            var p = new ConfigWriter("./telldus-new.conf");
            p.Write(c);
        }

        [SwaggerOperation("GetDeviceByName")]
        [HttpGet("{Name}")]
        public List<Device> GetDeviceByName(string Name)
        {
            var c = GetConfig();

            var devices = c.Devices.Where(d => d.Name == Name);

            if (devices.Count() > 0)
            {
                return devices.ToList();
            }

            throw new Exception("No device found");
        }

        [SwaggerOperation("GetDeviceById")]
        [HttpGet("Id/{Id}")]
        public Device GetDeviceById(int Id)
        {
            var c = GetConfig();

            var device = c.Devices.FirstOrDefault(d => d.Id == Id);
            return device;

        }

        [SwaggerOperation("AddDevice")]
        [HttpPost]
        public IActionResult AddDevice([FromBody] Device device)
        {
            if (device != null &&
                IsNotNullOrEmpty(device.Name) &&
                IsNotNullOrEmpty(device.Protocol) &&
                IsNotNullOrEmpty(device.Model))
            {
                var c = GetConfig();

                if (c.Devices.Any(d => d.Id == device.Id))
                {
                    return BadRequest("Id already exists");

                }
                if (c.Devices.Any(d => d.Name == device.Name))
                {
                    return BadRequest("Name already exists");
                }

                c.Devices.Add(device);
            }

            return Ok(string.Format("Device {0} added", device.Name));

        }


        //Replace with string extension
        private bool IsNotNullOrEmpty(string str)
        {
            return !string.IsNullOrEmpty(str);
        }
    }
}