using SRML.ModLoading.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SRML.ModLoading.Core
{
    class BasicAssemblyResolver : IAssemblyResolver, IDisposable
    {
        IModLoadingDomain domain;

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= Resolve;
        }

        public IDisposable Initialize(IModLoadingDomain domain)
        {
            this.domain = domain;
            AppDomain.CurrentDomain.AssemblyResolve += Resolve;
            return this;
        }

        private System.Reflection.Assembly Resolve(object sender, ResolveEventArgs args)
        {
            foreach(var file in domain.AllFiles)
            {
                if (Path.GetExtension(file) != "dll") continue;
                if (AssemblyName.ReferenceMatchesDefinition(new AssemblyName(args.Name), AssemblyName.GetAssemblyName(file))) return Assembly.LoadFrom(file);
            }
            return null;
        }
    }
}
