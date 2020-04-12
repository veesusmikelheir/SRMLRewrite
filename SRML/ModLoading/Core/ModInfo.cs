using SRML.ModLoading.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.ModLoading.Core
{
    public class ModInfo : IModInfo, IModMetaData
    {
        public string ID { get; }

        public Version Version { get; }

        public Version APIVersion {get; }

        public IModMetaData MetaData => this;

        public IModDependency[] Dependencies { get; } = new IModDependency[0];

        public IModLoadOrder LoadOrder { get; }

        public string Name { get;  }

        public string Description { get; }

        public string Author { get; }
    }
}
