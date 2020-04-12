using System.Collections.Generic;

namespace SRML
{
    public interface IModFileSystem
    {
        IEnumerable<string> ModFiles { get; }
    }
}