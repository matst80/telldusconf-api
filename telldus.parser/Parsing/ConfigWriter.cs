using System;
using System.Collections;
using System.IO;
using telldusconf.Parsing;

namespace telldus.parser.Parsing
{
    public static class ConfigWriter
    {
        public static void Write(object data, string filename)
        {
            using (var logWriter = File.CreateText(filename))
            {
                Write(data, logWriter);
                logWriter.Flush();
            }
        }

        public static void Write(object data, StreamWriter writer, string prefix = "")
        {
            var type = data.GetType();
            var keyDict = Parser.GetProperties(type.GetProperties());
            foreach (var kv in keyDict)
            {
                var toWrite = kv.Value.GetValue(data);
                if (toWrite is string || toWrite is int)
                {
                    writer.Write($"{prefix}{kv.Key} = \"{toWrite}\"{Environment.NewLine}");
                }
                else if (toWrite is IList lst)
                {
                    foreach(var obj in lst) {
                        writer.Write($"{prefix}{kv.Key} {{{Environment.NewLine}");
                        Write(obj, writer, prefix + "  ");
                        writer.Write($"{prefix}}}{Environment.NewLine}");
                    }
                }
                else {
                    if (toWrite != null)
                    {
                        writer.Write($"{prefix}{kv.Key} {{{Environment.NewLine}");
                        Write(toWrite, writer, prefix + "  ");
                        writer.Write($"{prefix}}}{Environment.NewLine}");
                    }
                }
            }
        }
    }
}
