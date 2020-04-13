using SRML.Config.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static SRML.Config.Reflection.ReflectionConfigGenerator;

namespace SRML.Config.Reflection
{
    public delegate void OnValueChangedEvent(FieldInfo field, IConfigValue value, object oldValue);
    public class ReflectionConfigGenerator : AbstractConfigGenerator<GeneratorConfig>
    {
        public override ConfigFile GenerateConfigFile(GeneratorConfig t)
        {
            var type = t.Type;
            var instance = t.Instance;
            var file = new ConfigFile();
            var global = file["GLOBAL"] = new ConfigSection();
            
            foreach (var field in type.GetFields())
            {
                var element = GetFieldValue(field,instance);
                global[element.GetDisplayName()] = element;
            }
            foreach(var subType in type.GetNestedTypes())
            {
                var section = file[subType.Name] = new ConfigSection();
                foreach(var field in subType.GetFields(BindingFlags.Static | BindingFlags.Public))
                {
                    var element = GetFieldValue(field, null);
                    section[element.GetDisplayName()] = element;
                }
            }
            return file;
        }

        static FieldBackedConfigValue GetFieldValue(FieldInfo info, object instance)
        {
            try
            {
                var element = new FieldBackedConfigValue(info, instance);
                return element;
            }
            catch (Exception e)
            {
                throw new Exception($"Error while trying to generate config for field '{info}'", e);
                
            }
        }



        public struct GeneratorConfig
        {
            /// <summary>
            /// The instance of the type to generate the config for (make null for static types)
            /// </summary>
            public object Instance;
            /// <summary>
            /// Decorated type to generate a config file out of
            /// </summary>
            public Type Type;
        }
    }
}
