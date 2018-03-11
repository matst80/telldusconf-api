using System.Linq;
using Microsoft.AspNetCore.Mvc;
using telldusconf.Models;
using telldusconf.Parsing;

namespace telldusconf.Controllers
{
    [Produces("application/json")]
    [Route("api/Device")]
    public class DeviceController : Controller
    {
        [HttpGet]
        public IActionResult GetAllDevices()
        {
            var c = GetConfig();

            return Ok(c.Devices);
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

        [HttpGet("{Name}")]
        public IActionResult GetDeviceByName(string Name)
        {
            var c = GetConfig();

            var devices = c.Devices.Where(d => d.Name == Name);

            if (devices.Count() > 0)
            {
                return Ok(devices);
            }

            return Ok("Device not found. Did you mean to search by Id api/Device/Id/{id}");
        }

        [HttpGet("Id/{Id}")]
        public IActionResult GetDeviceById(int Id)
        {
            var c = GetConfig();

            var devices = c.Devices.Where(d => d.Id == Id);

            if (devices.Count() > 0)
            {
                return Ok(devices);
            }

            return Ok("Device not found. Did you mean to search by Name api/Device/{Name}");
        }

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