using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using telldusconf.Models;
using telldusconf.Parsing;

namespace telldusconf.Controllers
{

    [Produces("application/json")]
    [Route("api/Config")]
    public class ConfigController : Controller
    {
        [SwaggerOperation("GetConfig")]
        [HttpGet]
        public ConfigFile GetConfig()
        {
            var c = ParseConfig();
            return c;
        }

        [Produces("text/html")]
        [SwaggerOperation("GetConfigFile")]
        [HttpGet("Raw")]
        public string GetConfigFile()
        {
            var fileStream = new FileStream("./telldus-new.conf", FileMode.Open);
            var reader = new StreamReader(fileStream);

            var data = reader.ReadToEnd();

            fileStream.Close();
            return data;
        }

        private static ConfigFile ParseConfig()
        {
            var p = new Parser("./telldus.conf");
            return p.Parse();
        }

        private static void StoreConfig(ConfigFile c)
        {
            var p = new ConfigWriter("./telldus-new.conf");
            p.Write(c);
        }

        [SwaggerOperation("StoreConfig")]
        [HttpPost]
        public IActionResult StoreConfigFunc([FromBody] ConfigFile config)
        {
            if (config != null)
            {
                StoreConfig(config);

                return Ok(string.Format("Config for user {0} added", config.User));
            }
            return NotFound();
        }
    }
}
