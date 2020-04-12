using SRML.ModLoading.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SRML.ModLoading.Core
{
    public class ModInfo : IModInfo, IModMetaData, IModLoadOrder
    {
        public string ID { get; private set; }

        public Version Version { get; private set; }

        public Version APIVersion {get; private set; }

        public IModMetaData MetaData => this;

        public IModDependency[] Dependencies { get; private set; } = new IModDependency[0];

        public IModLoadOrder LoadOrder => this;

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string Author { get; private set; }

        public string[] LoadsBefore { get; private set; } = new string[0];

        public string[] LoadsAfter { get; private set; } = new string[0];

        public static IModInfo CombineInfos(IModInfo a, IModInfo b)
        {
            return new ModInfo()
            {
                ID = a.ID ?? b.ID,
                Version = a.Version ?? b.Version,
                Name = a.MetaData.Name ?? b.MetaData.Name,
                Author = a.MetaData.Author ?? b.MetaData.Author ?? "",
                APIVersion = a.APIVersion ?? b.APIVersion ?? Assembly.GetExecutingAssembly().GetName().Version,
                Dependencies = (a.Dependencies ?? new IModDependency[0]).Concat(b.Dependencies ?? new IModDependency[0]).Distinct().ToArray(),
                LoadsBefore = (a.LoadOrder.LoadsBefore ?? new string[0]).Concat(b.LoadOrder.LoadsBefore ?? new string[0]).Distinct().ToArray(),
                LoadsAfter = (a.LoadOrder.LoadsAfter ?? new string[0]).Concat(b.LoadOrder.LoadsAfter ?? new string[0]).Distinct().ToArray()
            };
        }
    }
}
