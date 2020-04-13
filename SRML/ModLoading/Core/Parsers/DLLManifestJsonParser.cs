using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SRML.ModLoading.API;

namespace SRML.ModLoading.Core.Parsers
{
    
    public class DLLManifestJsonParser : JsonModParser
    {

        public override string GetJSONInput(IModFileSystem loadInfo)
        {
            foreach(var v in loadInfo.ModFiles)
            {

                if (Path.GetExtension(v) != ".dll") continue;
                Assembly loadedAssembly = Assembly.LoadFrom(v);

                foreach (var resource in loadedAssembly.GetManifestResourceNames())
                {
                    if (!resource.EndsWith("modinfo.json")) continue;
                    using (var reader = new StreamReader(loadedAssembly.GetManifestResourceStream(resource)))
                    {
                        var read = reader.ReadToEnd();
                        return read;
                    }
                }
                    
            }
            return null;
        }

    }
}
