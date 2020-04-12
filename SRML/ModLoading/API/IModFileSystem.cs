using System.Collections.Generic;

namespace SRML.ModLoading.API
{
    public interface IModFileSystem
    {
        IEnumerable<string> ModFiles { get; }
    }
}