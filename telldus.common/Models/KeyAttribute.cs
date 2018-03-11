using System;

namespace telldusconf.Models
{
    public class KeyAttribute : Attribute
    {
        private string _key;

        public string Key { get { return _key; } }

        public KeyAttribute(string key)
        {
            _key = key;
        }
    }

    public class ObjectKeyAttribute : KeyAttribute
    {
        public ObjectKeyAttribute(string key) : base(key)
        {
        }
    }

    public class ListKeyAttribute : KeyAttribute
    {
        public ListKeyAttribute(string key) : base(key)
        {
        }
    }
}