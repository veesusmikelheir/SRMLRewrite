using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static SRMLInjector.Utils;

namespace SRMLInjector
{
    public static class Config
    {
        public const string CONFIG_FILE = "manifest.cfg";
        public static Dictionary<string, ExtractArea> FilesToExtract = new Dictionary<string, ExtractArea>();
        public static MethodLocator EntryMethod;
        public static MethodLocator PatchMethod;
        public static void Load()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var configText = "";
            using(var reader = new StreamReader(assembly.GetManifestResourceStream($"SRMLInjector.{CONFIG_FILE}")))
            {
                configText = reader.ReadToEnd();
            }
            foreach (var l in configText.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var line = l.Trim();
                if (line.StartsWith("//")) continue;

                var args = line.Split('=');
                switch (args[0])
                {
                    case "EntryMethod":
                        EntryMethod = Utils.ExtractMethodInformation(args[1]);
                        break;
                    case "PatchMethod":
                        PatchMethod = Utils.ExtractMethodInformation(args[1]);
                        break;
                    default:
                        FilesToExtract[args[0]] = (ExtractArea)Enum.Parse(typeof(ExtractArea), args[1]);
                        break;
                }
            }
        }

        public enum ExtractArea
        {
            Libs,
            Managed
        }
    }
}
