using SRML.ModLoading.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.ModLoading.Core.Integrity
{
    public class AggregateIntegrityChecker : IModIntegrityChecker
    {
        List<IModIntegrityChecker> DeferredCheckers = new List<IModIntegrityChecker>();

        public AggregateIntegrityChecker(params IModIntegrityChecker[] checkers)
        {
            DeferredCheckers.AddRange(checkers);
        }

        public void CheckForValidity(IEnumerable<IModInfo> informations)
        {
            var infos = informations.ToList();
            foreach(var v in DeferredCheckers)
            {
                v.CheckForValidity(infos);
            }
        }
    }
}
