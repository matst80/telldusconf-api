using System;
using telldusconf.Parsing;
using Xunit;

namespace telldus.parser.test
{
    public class ParseConfig
    {
        [Fact]
        public void AssertCommonConfigHas36Devices()
        {
            var p = new Parser("ConfigFilesForTest/telldus.conf");
            var config = p.Parse();

            Assert.Equal(36, config.Devices.Count);
        }

        [Fact]
        public void AssertDevicesHasProperties()
        {
            var p = new Parser("ConfigFilesForTest/telldus.conf");
            var config = p.Parse();


            var device = config.Devices[0];
            Assert.True(device.Parameters != null);
        }
    }
}
