using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SRML.ModLoading.API;

namespace SRML.ModLoading.Core.Parsers
{
    public class ModInfoJsonParser : JsonModParser
    {
        public override string GetJSONInput(IModFileSystem loadInfo)
        {
            
            foreach(var v in loadInfo.ModFiles)
            {
                if (Path.GetFileName(v)==("modinfo.json")) return File.ReadAllText(v);
            }
            return null;
        }
    }
}
