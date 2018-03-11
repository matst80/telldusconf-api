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
        }

        public ConfigFile Parse()
        {
            var fileStream = new FileStream(_filePath, FileMode.Open);
            return Parse(fileStream);
        }

        public ConfigFile Parse(FileStream fileStream)
        {
            var ret = new ConfigFile();
            using (StreamReader reader = new StreamReader(fileStream))
            {
                ret = ParseObject(typeof(ConfigFile), reader) as ConfigFile;
            }
            return ret;
        }

        private object ParseObject(Type type, StreamReader reader)
        {

            //var type = typeof(T);
            var ret = Activator.CreateInstance(type);
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
            bool ok = true;
            while (ok)
            {
                var line = reader.ReadLine();
                if (line.Contains('}') || reader.EndOfStream)
                {
                    ok = false;
                }
                else if (line.Contains("="))
                {
                    var kv = line.Split('=').Select(d => d.Trim()).ToArray();
                    var key = kv[0];
                    if (keyDict.ContainsKey(key))
                    {
                        var prp = keyDict[key];
                        var val = kv[1];
                        PopulateValue(ret, prp, reader, val);
                    }
                    var value = kv[1];

                }
                else if (line.Contains('{'))
                {
                    var key = line.Split(' ').FirstOrDefault().Trim();
                    if (keyDict.ContainsKey(key))
                    {
                        var prp = keyDict[key];
                        PopulateValue(ret, prp, reader);
                    }
                }
            }
            return ret;
        }

        private void PopulateValue<T>(T ret, PropertyInfo prp, StreamReader reader, string val = "")
        {
            if (prp.PropertyType == typeof(string))
            {
                prp.SetValue(ret, val.Trim().Replace("\"",""));
            }
            else if (prp.PropertyType == typeof(int))
            {
                prp.SetValue(ret, Convert.ToInt32(val));
            }
            else
            {
                var obj = prp.GetValue(ret);
                if (obj == null)
                    obj = Activator.CreateInstance(prp.PropertyType);
                if (obj is System.Collections.IList lst)
                {
                    var rowType = obj.GetType().GenericTypeArguments.FirstOrDefault();
                    if (rowType != null)
                    {
                        var newRow = ParseObject(rowType, reader);
                        lst.Add(newRow);
                    }
                }
                else {
                    obj = ParseObject(prp.PropertyType, reader);
                }
                prp.SetValue(ret, obj);
            }
        }
    }
}
