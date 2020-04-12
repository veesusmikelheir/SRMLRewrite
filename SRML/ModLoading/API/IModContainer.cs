using SRML.ModLoading.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.ModLoading.API
{
    public interface IModContainer
    {
        /// <summary>
        /// All mods present and loaded in this <see cref="IModContainer"/>
        /// </summary>
        IEnumerable<IMod> Mods { get; }
    }
}
