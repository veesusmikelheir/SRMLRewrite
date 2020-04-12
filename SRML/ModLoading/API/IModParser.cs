using SRML.ModLoading.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.ModLoading.API
{
    public interface IModParser
    {
        /// <summary>
        /// Parses a mod file system to try and file the mods information, whereever it may be
        /// </summary>
        /// <param name="loadInfo">Mod files</param>
        /// <param name="modInfo">Mod Information</param>
        /// <returns>Whether or not the mod was parsed successfully</returns>
        bool TryParse(IModFileSystem loadInfo, out IModInfo modInfo);
    }
}
