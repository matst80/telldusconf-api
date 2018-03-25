namespace Telldusconf.Parsing
{
    using System;
    using System.Collections;
    using System.IO;
    using Telldusconf.Parsing;

    public class ConfigWriter
    {
        private string filename;

        public ConfigWriter(string filename)
        {
            this.filename = filename;
        }

        public void Write(object data)
        {
            using (var logWriter = File.CreateText(this.filename))
            {
                this.Write(data, logWriter);
                logWriter.Flush();
            }
        }

        public void Write(object data, StreamWriter writer, string prefix = "")
        {
            var type = data.GetType();
            var keyDict = Parser.GetProperties(type.GetProperties());
            foreach (var kv in keyDict)
            {
                var toWrite = kv.Value.GetValue(data);
                if (toWrite is string)
                {
                    writer.Write($"{prefix}{kv.Key} = \"{toWrite}\"{Environment.NewLine}");
                }
                else if (toWrite is int)
                {
                    writer.Write($"{prefix}{kv.Key} = {toWrite}{Environment.NewLine}");
                }
                else if (toWrite is IList lst)
                {
                    foreach (var obj in lst)
                    {
                        writer.Write($"{prefix}{kv.Key} {{{Environment.NewLine}");
                        this.Write(obj, writer, prefix + "  ");
                        writer.Write($"{prefix}}}{Environment.NewLine}");
                    }
                }
                else
                {
                    if (toWrite != null)
                    {
                        writer.Write($"{prefix}{kv.Key} {{{Environment.NewLine}");
                        this.Write(toWrite, writer, prefix + "  ");
                        writer.Write($"{prefix}}}{Environment.NewLine}");
                    }
                }
            }
        }
    }
}
