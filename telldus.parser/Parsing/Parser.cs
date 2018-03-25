namespace Telldusconf.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Telldusconf.Models;

    public class Parser
    {
        private string filePath;

        public Parser(string filename)
        {
            this.filePath = filename;
        }

        public ConfigFile Parse()
        {
            var fileStream = new FileStream(this.filePath, FileMode.Open);
            return this.Parse(fileStream);
        }

        public ConfigFile Parse(FileStream fileStream)
        {
            var ret = new ConfigFile();
            using (StreamReader reader = new StreamReader(fileStream))
            {
                ret = this.ParseObject(typeof(ConfigFile), reader) as ConfigFile;
            }

            return ret;
        }
        
        internal static Dictionary<string, PropertyInfo> GetProperties(PropertyInfo[] prps)
        {
            var keyDict = new Dictionary<string, PropertyInfo>();
            foreach (var prp in prps)
            {
                var key = prp.GetCustomAttributes(true).OfType<KeyAttribute>().FirstOrDefault();
                if (key != null)
                {
                    keyDict.Add(key.Key, prp);
                }
            }

            return keyDict;
        }

        private object ParseObject(Type type, StreamReader reader)
        {
            var ret = Activator.CreateInstance(type);
            var keyDict = GetProperties(type.GetProperties());
            bool ok = true;
            while (ok)
            {
                var line = reader.ReadLine();
                if (line == null || line.Contains('}') || reader.EndOfStream)
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
                        this.PopulateValue(ret, prp, reader, val);
                    }

                    var value = kv[1];
                }
                else if (line.Contains('{'))
                {
                    var key = line.Trim().Split(' ').FirstOrDefault().Trim();
                    if (keyDict.ContainsKey(key))
                    {
                        var prp = keyDict[key];
                        this.PopulateValue(ret, prp, reader);
                    }
                }
            }

            return ret;
        }

        private void PopulateValue<T>(T ret, PropertyInfo prp, StreamReader reader, string val = "")
        {
            if (prp.PropertyType == typeof(string))
            {
                if (!string.IsNullOrEmpty(val))
                {
                    prp.SetValue(ret, val.Trim().Replace("\"", string.Empty));
                }
            }
            else if (prp.PropertyType == typeof(int))
            {
                prp.SetValue(ret, Convert.ToInt32(val));
            }
            else
            {
                var obj = prp.GetValue(ret);
                if (obj == null)
                {
                    obj = Activator.CreateInstance(prp.PropertyType);
                }

                if (obj is System.Collections.IList lst)
                {
                    var rowType = obj.GetType().GenericTypeArguments.FirstOrDefault();
                    if (rowType != null)
                    {
                        var newRow = this.ParseObject(rowType, reader);
                        lst.Add(newRow);
                    }
                }
                else
                {
                    obj = this.ParseObject(prp.PropertyType, reader);
                }

                prp.SetValue(ret, obj);
            }
        }
    }
}
