using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML
{
    public class FileSystemModLocator : IModLocator
    {
        public IModLoadingDomain LocateMods(string coredirectory)
        {
            return new FileSystemDomain(coredirectory);
        }
    }
}
