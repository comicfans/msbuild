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
                //do not support src file name with namespace inside
                List<String> flags=new List<String>(cmd.Split(' '));

                List<String> srcs = new List<String>();

                //back search all source file 

                int i = flags.Count- 1;
                for(; i >= 0; --i)
                {
                    String flag = flags[i];
                    if (!System.IO.File.Exists(System.IO.Path.Combine(dir, flag)))
                    {
                        break;
                    }

                    //this is src file
                    srcs.Add(flag);
                    flags.RemoveAt(i);
                }

                //consider if last file name is a define command
                if (flags.Last().Equals(@"\D"))
                {
                    String last = srcs.Last();
                    srcs.RemoveAt(srcs.Count - 1);
                    flags.Add(last);
                }


                String finalFlags = "--driver-mode=cl " + String.Join(" ", flags);

                foreach (String f in srcs)
                {
                    _items.Add(new Item
                    {
                        directory = dir,
                        command = finalFlags,
                        file = f
                    });
                }
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
