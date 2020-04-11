using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML
{
    interface IModLoader
    {
        IMod Load(IModInfo info, IModFileSystem modfiles);
    }
}
