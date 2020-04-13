using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static SRMLInjector.Config;

namespace SRMLInjector
{
    class Program
    {
        public const string MANAGED_PATH = "SlimeRancher_Data/Managed";
        public const string LIB_PATH = "SRML/Libs";
        const string MANIFEST_RESOURCE_PATH = "SRMLInjectory.Files.";
        static int Main(string[] args)
        {

            Directory.SetCurrentDirectory(args[0]);
            Config.Load();
            
            switch (args[1].ToLower())
            {
                case "uninstall":
                    ClearFiles();
                    Patcher.Init(true);
                    Patcher.UnPatch();
                    break;
                case "install":
                    CopyFiles();
                    Patcher.Init(false);
                    Patcher.Patch();
                    break;
                default:
                    return 1;
            }
            


            
            //CopyFiles();

            Patcher.Save();

            return 0;
        }

        static string ValidateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return directory;
        }
        static void ClearFiles()
        {
            foreach(var file in Directory.EnumerateFiles(GetLocation(ExtractArea.Libs)))
            {
                File.Delete(file);
            }
            foreach(var file in Config.FilesToExtract)
            {
                if(file.Value == ExtractArea.Managed)
                {
                    File.Delete(Path.Combine(GetLocation(ExtractArea.Managed),file.Key));
                }
            }
        }
        static void CopyFiles()
        {
            ClearFiles();
            var ourAssembly = Assembly.GetExecutingAssembly();
            foreach (var file in ourAssembly.GetManifestResourceNames())
            {
                if (file.Length <= MANIFEST_RESOURCE_PATH.Length) continue;
                var name = file.Remove(0, MANIFEST_RESOURCE_PATH.Length-1);
                if (Config.FilesToExtract.TryGetValue(name,out var location))
                {
                    var endGoal = ValidateDirectory(GetLocation(location));
                    var path = Path.Combine(endGoal, name);

                    if (File.Exists(path)) File.Delete(path);
                    using(var stream = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        using (var manifestStream = ourAssembly.GetManifestResourceStream(file))
                        {
                            manifestStream.CopyTo(stream);
                        }
                    }
                }
            }
        }
        static string GetLocation(ExtractArea area)
        {
            switch (area)
            {
                case ExtractArea.Libs:
                    return LIB_PATH;
                case ExtractArea.Managed:
                    return MANAGED_PATH;
            }
            throw new Exception();
        }
        
    }
}
