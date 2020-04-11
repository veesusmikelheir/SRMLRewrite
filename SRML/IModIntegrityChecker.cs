using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML
{
    interface IModIntegrityChecker
    {
        /// <summary>
        /// Makes sure that the given list of mods is valid, otherwise throws an exception
        /// </summary>
        /// <param name="informations"></param>
        void CheckForValidity(IEnumerable<IModInfo> informations);
    }
}
