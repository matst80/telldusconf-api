﻿namespace Telldusconf.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Telldusconf.Models;
    using Telldusconf.Parsing;

    [Route("api/Config")]
    public class ConfigController : Controller
    {
        [SwaggerOperation("GetConfig")]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ConfigFile), 200)]
        public ConfigFile GetConfig()
        {
            var c = ParseConfig();
            return c;
        }

        [SwaggerOperation("GetConfigFile")]
        [Produces("text/plain")]
        [HttpGet("Raw")]
        public string GetConfigFile()
        {
            string data = string.Empty;

            using (var fileStream = new FileStream("./telldus.conf", FileMode.Open))
            {
                using (var reader = new StreamReader(fileStream))
                {
                    data = reader.ReadToEnd();
                }
            }

            return data;
        }

        [SwaggerOperation("StoreConfig")]
        [HttpPost]
        public IActionResult StoreConfigFunc([FromBody] ConfigFile config)
        {
            if (config != null)
            {
                StoreConfig(config);

                return this.Ok(string.Format("Config for user {0} added", config.User));
            }

            return this.NotFound();
        }

        private static ConfigFile ParseConfig()
        {
            var p = new Parser("./telldus.conf");
            return p.Parse();
        }

        private static void StoreConfig(ConfigFile c)
        {
            var p = new ConfigWriter("./telldus.conf");
            p.Write(c);
        }
    }
}
