using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SDB
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
                return;

            switch (args[0].ToLower())
            {
                case "-copy":
                    {
                        List<string> srcs = args.Skip(1).Take(args.Length - 2).ToList();
                        ShellFileOperation.CopyItems(srcs, args[args.Length - 1]);
                        break;
                    }
                case "-rename":
                    {
                        Application.Run(new RenameForm(args.Skip(1).Take(args.Length - 1).ToArray()));
                        break;
                    }
            }
        }

    }
}
