﻿using System;
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
            var parsedMods = modDomain.ModFiles.Select(x => new { FileSystem = x, Info = ModParser.TryParse(x, out var info) ? info : null });
            IntegrityChecker.CheckForValidity(parsedMods.Select(x => x.Info));
            using (var dummy = Resolver.Initialize(modDomain))
            {
                foreach (var parsedMod in parsedMods)
                {
                    if (ModLoader.TryLoad(parsedMod.Info,parsedMod.FileSystem,out var mod))
                    {
                        _mods.Add(mod);

                    }
                }
            }

            
        }
    }
}
