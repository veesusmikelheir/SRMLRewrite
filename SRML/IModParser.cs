using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML
{
    interface IModParser
    {
        bool TryParse(IModFileSystem loadInfo, out IModInfo modInfo);
    }
}
