using SRML.ModLoading.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.ModLoading.Core
{
    public class LoadOrderCalculator : ILoadOrderCalculator
    {
        public IEnumerable<IMod> CalculateLoadOrder(IEnumerable<IMod> mods)
        {
            var list = mods.ToList();

            var realLoadsAfters = new Dictionary<IMod, HashSet<IMod>>();
            HashSet<IMod> GetForMod(IMod mod)
            {
                if(!realLoadsAfters.TryGetValue(mod, out var l))
                {
                    l = new HashSet<IMod>();
                    realLoadsAfters[mod] = l;
                }
                return l;
            }

            // Create a dictionary of mods that each mod wants to load after
            foreach(var mod in list)
            {
                // add this mods loads afters, easy enough
                var loads = GetForMod(mod);
                foreach(var v in mod.Info.LoadOrder.LoadsAfter)
                {
                    var m = list.FirstOrDefault(x => x.Info.ID == v);
                    if (m == null) continue;
                    loads.Add(m);
                }

                // add this mod to the loads after of every mod it loads before
                foreach(var v in mod.Info.LoadOrder.LoadsBefore)
                {
                    var m = list.FirstOrDefault(x => x.Info.ID == v);
                    if (m == null) continue;
                    GetForMod(m).Add(mod);
                }
            }


            List<IMod> pushedModsSoFar = new List<IMod>();

            HashSet<IMod> currentlyLoadingMods = new HashSet<IMod>();

            void PushModToList(IMod mod)
            {
                if (pushedModsSoFar.Contains(mod)) return;
                currentlyLoadingMods.Add(mod);
                // since we want to load after these mods, we make sure they're pushed to the list of mods before us
                foreach(var v in realLoadsAfters[mod])
                {
                    // CIRCULAR LOAD ORDER FOUND!
                    if (currentlyLoadingMods.Contains(v)) throw new Exception($"Circular load order detected with mod '{v.Info.ID}'");
                    // load the load afters before us
                    PushModToList(v);
                
                }
                // we managed to load our load afters before us! now we can load
                pushedModsSoFar.Add(mod);
                // we loaded so remove from currently loading
                currentlyLoadingMods.Remove(mod);
            }

            foreach(var v in list)
            {
                PushModToList(v);
            }

            return pushedModsSoFar;
        }
    }
}
