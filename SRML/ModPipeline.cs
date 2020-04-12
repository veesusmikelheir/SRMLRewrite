using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML
{
    public class ModPipeline : IModContainer
    {
        List<IMod> _mods;

        public IEnumerable<IMod> Mods => _mods;

        public ModPipeline(IModLocator modLocator, IModParser modParser, IModLoader modLoader, IModIntegrityChecker integrityChecker,IAssemblyResolver resolver,ILoadOrderCalculator orderer)
        {
            ModLocator = modLocator;
            ModParser = modParser;
            ModLoader = modLoader;
            IntegrityChecker = integrityChecker;
            Resolver = resolver;
            OrderCalculator = orderer;
            _mods = new List<IMod>();
        }

        IModLocator ModLocator;
        IModParser ModParser;
        IModLoader ModLoader;
        IModIntegrityChecker IntegrityChecker;
        IAssemblyResolver Resolver;
        ILoadOrderCalculator OrderCalculator; 
        
        public void InitializeMods(string directory)
        {
            var modDomain = ModLocator.LocateMods(directory);
            var parsedMods = modDomain.ModFiles.Select(x => new { FileSystem = x, Info = ModParser.TryParse(x, out var info) ? info : throw new Exception("Could not parse one of the mods")});
            IntegrityChecker.CheckForValidity(parsedMods.Select(x => x.Info));
            using (var dummy = Resolver.Initialize(modDomain))
            {
                foreach (var parsedMod in parsedMods)
                {
                    if (ModLoader.TryLoad(parsedMod.Info,parsedMod.FileSystem,modDomain,out var mod))
                    {
                        _mods.Add(mod);

                    }
                    else
                    {
                        throw new Exception($"Could not load mod {parsedMod.Info.ID}");
                    }
                }
            }
            _mods = OrderCalculator.CalculateLoadOrder(_mods).ToList();
        }
    }
}
