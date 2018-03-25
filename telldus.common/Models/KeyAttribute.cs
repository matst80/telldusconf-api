namespace Telldusconf.Models
{
    using System;

    public class KeyAttribute : Attribute
    {
        private string key;

        public KeyAttribute(string key)
        {
            this.key = key;
        }

        public string Key
        {
            get
            {
                return this.key;
            }
        }
    }
}