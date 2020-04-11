using System.Collections.Generic;

namespace SRML
{
    internal interface ILoadOrderCalculator
    {
        IEnumerable<IMod> CalculateLoadOrder(IEnumerable<IMod> mods);
    }
}