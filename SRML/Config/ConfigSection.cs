using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Config
{
    public class ConfigSection : Dictionary<string,IConfigValue>
    {
        public void PullFrom(KeyDataCollection collection)
        {
            foreach(var key in collection)
            {
                if (!TryGetValue(key.KeyName, out var value)) continue;
                value.SetStringValue(key.Value);
            }
        }

        public void PushTo(KeyDataCollection collection)
        {
            foreach(var key in this)
            {
                if (key.Value == null) continue;
                collection[key.Key] = key.Value.GetStringValue();
            }
        }

        public new IConfigValue this[string index]
        {
            get => base[index];
            set
            {
                base[index] = value;
                if (value == null) Remove(index);
            }
        }
    }
}
