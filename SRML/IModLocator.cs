﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML
{
    interface IModLocator
    {
        /// <summary>
        /// Get a mod loading domain for a given directory
        /// </summary>
        /// <param name="coredirectory"></param>
        /// <returns></returns>
        IModLoadingDomain LocateMods(string coredirectory);
    }
}
