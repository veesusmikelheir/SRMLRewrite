using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML
{
    class JsonModParser : IModParser
    {
        public bool TryParse(IModFileSystem loadInfo, out IModInfo modInfo)
        {
            modInfo = null;
            var modinfo = loadInfo.ModFiles.FirstOrDefault(x => x.EndsWith("modinfo.json"));
            if (modinfo == null) return false;

            // do some kind of loading behaviour
            return true;
        }
    }
}
