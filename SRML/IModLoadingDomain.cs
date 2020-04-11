using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML
{
    public interface IModLoadingDomain
    {
        /// <summary>
        /// List of mod specific file systems that the domain knows of
        /// </summary>
        IEnumerable<IModFileSystem> ModFiles { get; }

        /// <summary>
        /// Every file in the loading domain
        /// </summary>
        IEnumerable<string> AllFiles { get; }
    }
}
