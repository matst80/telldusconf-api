using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using telldusconf.Models;
using telldusconf.Parsing;

namespace telldusconf.Controllers
{

    [Produces("application/json")]
    [Route("api/Config")]
    public class ConfigController : Controller
    {
        [HttpGet]
        public IActionResult GetConfig()
        {
            var c = ParseConfig();

            return Ok(c);
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
        
        [HttpPost]
        public IActionResult AddDevice([FromBody] ConfigFile config)
        {
            StoreConfig(config);

            return Ok(string.Format("Config for user {0} added", config.User));

        }

        //Replace with string extension
        private bool IsNotNullOrEmpty(string str)
        {
            return !string.IsNullOrEmpty(str);
        }
    }
}
