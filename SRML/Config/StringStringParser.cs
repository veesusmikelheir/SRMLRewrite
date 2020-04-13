using SRML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Config
{
    /// <summary>
    /// A custom parser specifically for strings, to make sure they are properly quoted 
    /// </summary>
    public class StringStringParser : IStringParser
    {
        
        public object FromString(string str)
        {
            return str.Trim().Trim('\"');
        }

        public string ToString(object value)
        {
            if (!ReflectionUtil.IsCompatible(value, typeof(string))) throw new ArgumentException();
            return '"' + value.ToString() + '"';
        }
    }
}
