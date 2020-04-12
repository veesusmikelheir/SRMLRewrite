using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SRML
{
    public class FileSystemDomain : IModLoadingDomain
    {
        List<IModFileSystem> systems;
        public IEnumerable<IModFileSystem> ModFiles => systems;

        public IEnumerable<string> AllFiles => Directory.EnumerateFiles(CoreDirectory, "*.*", SearchOption.AllDirectories);

        string CoreDirectory;
        public FileSystemDomain(string directory)
        {
            systems = new List<IModFileSystem>();
            CoreDirectory = directory;

            GenerateModSystems();
        }


        void GenerateModSystems()
        {
            foreach (var file in Directory.EnumerateFiles(CoreDirectory, "*.dll", SearchOption.TopDirectoryOnly))
            {
                systems.Add(new BasicModFileSystem() { file});
            }
            foreach(var dir in Directory.EnumerateDirectories(CoreDirectory))
            {
                var system = new BasicModFileSystem();
                systems.Add(system);
                foreach(var file in Directory.EnumerateFiles(dir, "*.dll", SearchOption.AllDirectories))
                {
                    system.Add(file);
                }
            }
        }
        class BasicModFileSystem : List<string>, IModFileSystem
        {
            public IEnumerable<string> ModFiles => this;
        }
    }
}
