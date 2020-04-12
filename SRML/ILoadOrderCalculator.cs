using System.Collections.Generic;

namespace SRML
{
    public interface ILoadOrderCalculator
    {
        IEnumerable<IMod> CalculateLoadOrder(IEnumerable<IMod> mods);
    }
}