using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Config
{
    public static class ConfigManager
    {
        static List<IConfigGenerator> ConfigProviders = new List<IConfigGenerator>();
        public static void RegisterConfigGenerator(IConfigGenerator generator)
        {
            ConfigProviders.Add(generator);
        }



        public static bool TryGenerateConfigFile(object obj, out ConfigFile file)
        {
            file = null;
            foreach (var v in ConfigProviders)
            {
                if (v.IsCompatible(obj))
                {
                    file = v.GenerateConfigFile(obj);
                    return true;
                }
            }
            return false;
        }
    }
}
