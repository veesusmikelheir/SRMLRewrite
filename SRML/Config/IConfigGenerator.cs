using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Config
{
    public interface IConfigGenerator<T> : IConfigGenerator
    {
        ConfigFile GenerateConfigFile(T t);
    }

    public interface IConfigGenerator
    {
        ConfigFile GenerateConfigFile(object obj);
        bool IsCompatible(object obj);
    }
}
