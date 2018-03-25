namespace Telldusconf.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Telldusconf.Models;
    using Telldusconf.Parsing;

    [Produces("application/json")]
    [Route("api/Device")]
    public class DeviceController : Controller
    {
        [SwaggerOperation("GetAllDevices")]
        [HttpGet(Name = "GetStores")]
        public IActionResult GetAllDevices()
        {
            var c = GetConfig();

            return this.Ok(c.Devices);
        }

        [SwaggerOperation("GetDeviceByName")]
        [HttpGet("{Name}")]
        public IActionResult GetDeviceByName(string name)
        {
            var c = GetConfig();

            var devices = c.Devices.Where(d => d.Name == name);

            if (devices.Count() > 0)
            {
                return this.Ok(devices);
            }

            return this.Ok("Device not found. Did you mean to search by Id api/Device/Id/{id}");
        }

        [SwaggerOperation("GetDeviceById")]
        [HttpGet("Id/{id}")]
        public IActionResult GetDeviceById(int id)
        {
            var c = GetConfig();

            var devices = c.Devices.Where(d => d.Id == id);

            if (devices.Count() > 0)
            {
                return this.Ok(devices);
            }

            return this.Ok("Device not found. Did you mean to search by Name api/Device/{Name}");
        }

        [SwaggerOperation("AddDevice")]
        [HttpPost]
        public IActionResult AddDevice([FromBody] Device device)
        {
            if (device != null &&
                this.IsNotNullOrEmpty(device.Name) &&
                this.IsNotNullOrEmpty(device.Protocol) &&
                this.IsNotNullOrEmpty(device.Model))
            {
                var c = GetConfig();

                if (c.Devices.Any(d => d.Id == device.Id))
                {
                    return this.BadRequest("Id already exists");
                }

                if (c.Devices.Any(d => d.Name == device.Name))
                {
                    return this.BadRequest("Name already exists");
                }

                c.Devices.Add(device);
            }

            return this.Ok(string.Format("Device {0} added", device.Name));
        }
        
        private static ConfigFile GetConfig()
        {
            var p = new Parser("./telldus.conf");
            return p.Parse();
        }

        private static void StoreConfig(ConfigFile c)
        {
            var p = new ConfigWriter("./telldus.conf");
            p.Write(c);
        }

        // Replace with string extension
        private bool IsNotNullOrEmpty(string str)
        {
            return !string.IsNullOrEmpty(str);
        }
    }
}