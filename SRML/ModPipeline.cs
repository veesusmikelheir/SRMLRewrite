using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML
{
    class ModPipeline : IModContainer
    {
        List<IMod> _mods;

        public IEnumerable<IMod> Mods => _mods;

        public ModPipeline(IModLocator modLocator, IModParser modParser, IModLoader modLoader, IModIntegrityChecker integrityChecker,IAssemblyResolver resolver)
        {
            ModLocator = modLocator;
            ModParser = modParser;
            ModLoader = modLoader;
            IntegrityChecker = integrityChecker;
            Resolver = resolver;
            _mods = new List<IMod>();
        }

        IModLocator ModLocator;
        IModParser ModParser;
        IModLoader ModLoader;
        IModIntegrityChecker IntegrityChecker;
        IAssemblyResolver Resolver;


        public void InitializeMods(string directory)
        {
            var modDomain = ModLocator.LocateMods(directory);
            using (var dummy = Resolver.Initialize(modDomain))
            {
                foreach (var files in modDomain.ModFiles)
                {
                    ModParser.TryParse(files, out var info);
                    _mods.Add(ModLoader.Load(info, files));
                    IntegrityChecker.CheckForValidity(_mods);
                }
            }

            
        }
    }
}
