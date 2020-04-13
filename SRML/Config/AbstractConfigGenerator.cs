using SRML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Config
{
    public abstract class AbstractConfigGenerator<T> : IConfigGenerator<T>
    {
        public abstract ConfigFile GenerateConfigFile(T t);

        public ConfigFile GenerateConfigFile(object obj)
        {
            if (!IsCompatible(obj)) throw new ArgumentException("obj");
            return GenerateConfigFile((T)obj);
        }

        public bool IsCompatible(object obj)
        {
            return ReflectionUtil.IsCompatible(obj, typeof(T));
        }
    }
}
