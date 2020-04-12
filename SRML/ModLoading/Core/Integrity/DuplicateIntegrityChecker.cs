using SRML.ModLoading.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.ModLoading.Core.Integrity
{
    public class DuplicateIntegrityChecker : IModIntegrityChecker
    {
        HashSet<string> set = new HashSet<string>();
        public void CheckForValidity(IEnumerable<IModInfo> informations)
        {
            set.Clear();
            foreach(var info in informations)
            {
                if (!set.Add(info.ID))
                {
                    throw new Exception($"Duplicate mod id '{info.ID}'");
                }
            }
        }
    }
}
