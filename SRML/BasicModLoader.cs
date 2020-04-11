using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML
{
    class BasicModLoader : IModLoader
    {
        public bool TryLoad(IModInfo info, IModFileSystem modfiles, IModLoadingDomain loadingdomain, out IMod mod)
        {
            mod = null;
            
        }
    }
}
