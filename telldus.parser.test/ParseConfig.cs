using FluentAssertions;
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

        [Fact]
        public void AssertNewFileIsSameAsOldFile()
        {
            var p = new Parser("ConfigFilesForTest/telldus.conf");
            var config = p.Parse();

            var w = new ConfigWriter("ConfigFilesForTest/test.conf");
            w.Write(config);

            var p2 = new Parser("ConfigFilesForTest/test.conf");
            var config2 = p2.Parse();

            Assert.Equal(config.Devices.Count, config2.Devices.Count);
        }
    }
}
