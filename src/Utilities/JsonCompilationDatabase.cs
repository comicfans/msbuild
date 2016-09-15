using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Build.Utilities
{
    public class JsonCompilationDatabase
    {
        public static JsonCompilationDatabase Instance = new JsonCompilationDatabase();

        private class Item
        {
            public string directory;
            public string command;
            public string file;
        }

        private List<Item> _items = new List<Item>();

        public void Add(string dir,string cmd)
        {
            lock (this)
            {
                String[] tokens=cmd.Split(' ');

                _items.Add(new Item {
                    directory = dir,
                    command = cmd,
                    file = tokens[tokens.Length - 1]
                });
            }
        }

        public void Save()
        {
            lock (this)
            {

                System.IO.File.WriteAllText("msbuild_compile_commands.json",
                    JsonConvert.SerializeObject(_items.ToArray(), Formatting.Indented));
            }
        }
    }
}
