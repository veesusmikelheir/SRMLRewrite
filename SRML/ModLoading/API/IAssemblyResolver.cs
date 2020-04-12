using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.ModLoading.API
{
    public interface IAssemblyResolver
    {
        IDisposable Initialize(IModLoadingDomain domain);
    }
}
