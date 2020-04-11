using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SRML
{
    public interface IMod
    {
        /// <summary>
        /// Information about this mod
        /// </summary>
        IModInfo Info { get; }


        /// <summary>
        /// Load this mod
        /// </summary>
        void Load();

        /// <summary>
        /// Setup things that need to be setup before load
        /// </summary>
        void PreLoad();
    }
}
