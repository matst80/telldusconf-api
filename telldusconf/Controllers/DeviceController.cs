using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using telldusconf.Models;

namespace telldusconf.Controllers
{
    [Produces("application/json")]
    [Route("api/Device")]
    public class DeviceController : Controller
    {
        [HttpGet("{Name}")]
        public IActionResult GetDeviceByName(string Name)
        {
            return Ok("Device not found. Did you mean to search by Id api/Device/Id/{id}");
        }

        [HttpGet("Id/{Id}")]
        public IActionResult GetDeviceById(string Id)
        {
            return Ok("Device not found. Did you mean to search by Name api/Device/{Name}");
        }

        [HttpPost("{deviceJson}")]
        public IActionResult AddDevice(string deviceJson)
        {
            return Ok();
        }
    }
}