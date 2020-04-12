using SRML.ModLoading.API;
using SRML.ModLoading.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.ModLoading
{
    public class FileSystemModLocator : IModLocator
    {
        public IModLoadingDomain LocateMods(string coredirectory)
        {
            return new FileSystemDomain(coredirectory);
        }
    }
}
