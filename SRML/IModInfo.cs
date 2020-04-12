using System;

namespace SRML
{
    public interface IModInfo
    {
        
        /// <summary>
        /// Unique ID for the given mod
        /// </summary>
        string ID { get; }
        /// <summary>
        /// The version of the mod
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// SRML Version this mod was built for
        /// </summary>
        Version APIVersion { get; }

        /// <summary>
        /// Metadata that is not essential for the mods function, but useful to have
        /// </summary>
        IModMetaData MetaData { get; set; }


        /// <summary>
        /// List of Depencies for this mod
        /// </summary>
        IModDependency[] Dependencies { get; }

        /// <summary>
        /// Information regarding the location of this mod in the load order
        /// </summary>
        IModLoadOrder LoadOrder { get; }
    }
}