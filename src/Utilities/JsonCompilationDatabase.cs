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

                String file = tokens[tokens.Length - 1];


                _items.Add(new Item {
                    directory = dir,
                    command = "--driver-mode=cl "+cmd.Substring(0,cmd.Length-file.Length),
                    file = tokens[tokens.Length - 1]
                });
            }
        }

        public void Save()
        {
            lock (this)
            {
                System.IO.File.WriteAllText("compile_commands.json",
                    JsonConvert.SerializeObject(_items.ToArray(), Formatting.Indented));
            }
        }
    }
}
