using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using OpCodes = Mono.Cecil.Cil.OpCodes;

namespace SRMLInjector
{
    public static class Utils
    {
        public static MethodLocator ExtractMethodInformation(string input)
        {
            var str = input.Split(' ');
            var assembly = new AssemblyName(str[0]);
            var type = str[1];
            var method = str[2];
            var remaining = str.Length - 3;
            var parameters = new string[remaining];
            Array.Copy(str, 3, parameters, 0, remaining);
            return new MethodLocator(assembly, type, method, parameters);
        }

        public struct MethodLocator
        {
            public AssemblyName Name;
            public string Type;
            public string Method;
            public string[] Parameters;

            public override string ToString()
            {
                return $"[{Name}:{Type}.{Method}({String.Join(",", Parameters)})]";
            }

            public MethodLocator(AssemblyName name, string type, string method, string[] parameters)
            {
                Name = name;
                Type = type;
                Method = method;
                Parameters = parameters;
            }

            public MethodDefinition Resolve(string coreDirectory, out string assemblyPath)
            {
                foreach (var file in Directory.EnumerateFiles(coreDirectory, "*.dll"))
                {
                    if (!AssemblyName.ReferenceMatchesDefinition(Name, AssemblyName.GetAssemblyName(file))) continue;
                    var resolver = new DefaultAssemblyResolver();
                    resolver.AddSearchDirectory(coreDirectory);
                    AssemblyDefinition def = AssemblyDefinition.ReadAssembly(file,new ReaderParameters()
                    {
                        AssemblyResolver=resolver
                    });
                    var t = this;
                    try
                    {
                        assemblyPath = file;
                        return def.MainModule.GetType(Type).Methods.First(x => x.Name == t.Method && x.Parameters.Select(y => y.ParameterType.Name).SequenceEqual(t.Parameters));
                    }
                    catch(Exception e)
                    {
                        throw new Exception($"Could not locate method at {this}", e);
                    }
                    
                }
                throw new FileNotFoundException("Could not find assembly " + Name);
            }
        }

       

        

    }
}
