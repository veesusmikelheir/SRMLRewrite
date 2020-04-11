using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML
{
    interface IAssemblyResolver
    {
        IDisposable Initialize(IModLoadingDomain domain);
    }
}
