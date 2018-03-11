using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using telldusconf.Models;

namespace telldusconf.Parsing
{
    public class Parser
    {
        private string _filePath;

        public Parser(string filename)
        {
            _filePath = filename;
            var fileStream = new FileStream(_filePath, FileMode.Open);
            Parse(fileStream);
        }

        public ConfigFile Parse(FileStream fileStream)
        {
            var ret = new ConfigFile();
            using (StreamReader reader = new StreamReader(fileStream))
            {
                ret = ParseObject<ConfigFile>(reader);
            }
            return ret;
        }

        private T ParseObject<T>(StreamReader reader)
        {
            var ret = Activator.CreateInstance<T>();
            var type = typeof(T);
            var prps = type.GetProperties();
            var keyDict = new Dictionary<string, PropertyInfo>();
            foreach (var prp in prps)
            {
                var key = prp.GetCustomAttributes(true).OfType<KeyAttribute>().FirstOrDefault();
                if (key != null)
                {
                    keyDict.Add(key.Key, prp);
                }
            }
            return ret;
        }
    }
}
