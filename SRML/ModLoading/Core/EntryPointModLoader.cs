using SRML.ModLoading.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SRML.ModLoading.Core
{
    public class EntryPointModLoader : IModLoader
    {
        public bool TryLoad(IModInfo info, IModFileSystem modfiles, IModLoadingDomain loadingdomain, out IMod mod)
        {
            mod = null;
            foreach(var file in modfiles.ModFiles.Where(x=>Path.GetExtension(x)=="dll"))
            {
                var assembly = Assembly.LoadFrom(file);
                var entryType = assembly.GetTypes().FirstOrDefault(x=>(typeof(IModEntryPoint).IsAssignableFrom(x)));
                if (entryType == null) continue;
                mod = new EntryPointMod(info, (IModEntryPoint)Activator.CreateInstance(entryType));
                return true;
            }
            return false;
        }
    }

    public class EntryPointMod : IMod
    {
        public IModInfo Info { get; private set; }

        IModEntryPoint Entry;
        internal EntryPointMod(IModInfo info, IModEntryPoint point)
        {
            Info = info;
            Entry = point;
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void PreLoad()
        {
            throw new NotImplementedException();
        }
    }
}
