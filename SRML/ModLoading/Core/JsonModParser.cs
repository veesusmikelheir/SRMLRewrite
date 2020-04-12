using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using SRML.ModLoading.API;

namespace SRML.ModLoading.Core
{
    public abstract class JsonModParser : IModParser
    {
        public bool TryParse(IModFileSystem loadInfo, out IModInfo modInfo)
        {
            modInfo = null;
            var modinfo = ParseJSON(GetJSONInput(loadInfo));
            if (modinfo == null) return false;
            return true;
        }

        public abstract string GetJSONInput(IModFileSystem loadInfo);

        public static IModInfo ParseJSON(string json)
        {
            if (json == null) throw new ArgumentNullException("json");
            if (json.Length == 0) throw new ArgumentException("json");
            return JsonInfoMod.FromJSON(json);
        }
        [JsonObject(MemberSerialization.Fields)]
        internal class JsonInfoMod : IModInfo, IModMetaData, IModLoadOrder
        {

            [JsonProperty]string id;
            [JsonProperty] string name;
            [JsonProperty] string description;
            [JsonProperty] string author;
            [JsonProperty] string version;
            [JsonProperty] string api_version;
            [JsonProperty] JsonModDependency[] dependencies;
            [JsonProperty] string[] loads_before;
            [JsonProperty] string[] loads_after;
            public string Name => name;

            public string Description => description;

            public string Author => author;

            public string ID => id;

            public Version Version { get; private set; }

            public Version APIVersion { get; private set; }

            public IModMetaData MetaData => this;

            public IModDependency[] Dependencies => dependencies;
            public IModLoadOrder LoadOrder => this;

            public string[] LoadsBefore => loads_before;

            public string[] LoadsAfter => loads_after;

            public static JsonInfoMod FromJSON(string json)
            {
                if (json == null) throw new ArgumentNullException("json");
                if (json.Length == 0) throw new ArgumentOutOfRangeException("json");
                try
                {
                    var proto = JsonConvert.DeserializeObject<JsonInfoMod>(json);

                    return proto;
                }
                catch(TargetInvocationException e)
                {
                    throw e.InnerException;
                }
            }


            [OnDeserialized]
            void OnDeserialize(StreamingContext context)
            {
                if (id == null || id.Length == 0) throw new ArgumentNullException("id");
                if (name == null || name.Length == 0) throw new ArgumentNullException("name");
                if (version == null || version.Length == 0) throw new ArgumentNullException("version");

                MakeNotNull(ref author);
                MakeNotNull(ref description);
                MakeNotNull(ref loads_before, new string[0]);
                MakeNotNull(ref loads_after, new string[0]);
                MakeNotNull(ref dependencies, new JsonModDependency[0]);
                MakeNotNull(ref api_version, Assembly.GetExecutingAssembly().GetName().Version.ToString());
                foreach (var dependency in dependencies)
                {
                    if (dependency.ID == null || dependency.ID.Length == 0) throw new ArgumentNullException("dependencies");
                    
                }
                

                Version = Version.Parse(version);
                APIVersion = Version.Parse(api_version);
            }
            void MakeNotNull<T>(ref T toCheck, T obj) where T : class
            {
                toCheck = toCheck ?? obj;
            }

            void MakeNotNull(ref string toCheck)=>MakeNotNull(ref toCheck,"");

            [JsonObject(MemberSerialization.Fields)]
            class JsonModDependency : IModDependency
            {
                string id;
                string version;
                public string ID => id;

                public VersionRequirement VersionRequirement { get; set; }
                [OnDeserialized]
                internal void OnDeserialize(StreamingContext context)
                {
                    VersionRequirement = VersionRequirement.Parse(version);
                }
            }

        }
    }
}
