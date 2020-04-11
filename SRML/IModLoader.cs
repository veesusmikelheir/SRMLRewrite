using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML
{
    interface IModLoader
    {
        bool TryLoad(IModInfo info, IModFileSystem modfiles, out IMod mod);
    }
}
