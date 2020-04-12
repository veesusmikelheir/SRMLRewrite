using System.Collections.Generic;

namespace SRML.ModLoading.API
{
    public interface ILoadOrderCalculator
    {
        IEnumerable<IMod> CalculateLoadOrder(IEnumerable<IMod> mods);
    }
}