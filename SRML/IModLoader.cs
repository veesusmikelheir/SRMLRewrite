using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML
{
    public interface IModLoader
    {
        /// <summary>
        /// Initializes the mod and returns a complete <see cref="IMod"/> object
        /// </summary>
        /// <param name="info"></param>
        /// <param name="modfiles"></param>
        /// <param name="mod"></param>
        /// <returns></returns>
        bool TryLoad(IModInfo info, IModFileSystem modfiles, IModLoadingDomain loadingdomain, out IMod mod);
    }
}
