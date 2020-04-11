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
        /// The main assembly where the mods entry point is located]
        /// </summary>
        Assembly MainAssembly { get; }

        /// <summary>
        /// Load this mod
        /// </summary>
        void Load();
    }
}
