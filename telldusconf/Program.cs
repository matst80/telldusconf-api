using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using telldusconf.Parsing;

namespace telldusconf
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var p = new Parser("./telldus.conf");
            var w = new ConfigWriter("./copy.conf");
            var config = p.Parse();
            w.Write(config);
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
