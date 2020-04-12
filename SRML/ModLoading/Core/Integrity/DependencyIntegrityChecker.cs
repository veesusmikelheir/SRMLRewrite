using SRML.ModLoading.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.ModLoading.Core.Integrity
{
    public class DependencyIntegrityChecker : IModIntegrityChecker
    {
        HashSet<string> set = new HashSet<string>();
        public void CheckForValidity(IEnumerable<IModInfo> informations)
        {
            
            var infos = informations.ToList();
            foreach (var mod in infos)
            {
                set.Clear();
                foreach (var dependency in mod.Dependencies)
                {
                    if (dependency.ID == mod.ID) continue;
                    if (!set.Add(dependency.ID)) throw new Exception($"Duplicate dependency '{dependency.ID}'");
                    var otherMod = infos.FirstOrDefault(x => x.ID == dependency.ID);
                    if (otherMod == null) throw new Exception($"Cannot locate dependency '{dependency.ID}' for mod '{mod.ID}'");
                    if (!dependency.VersionRequirement.SatisfiedWith(otherMod.Version)) throw new Exception($"Mod {mod.ID} wants version {dependency.VersionRequirement.ToString()} of mod '{otherMod.ID}' but installed version is at {otherMod.Version}");
                }
            }
        }
    }
}
