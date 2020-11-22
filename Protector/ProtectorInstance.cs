using System;
using System.IO;
using System.Text;
using Microsoft.VisualBasic;

namespace Protector
{
    public static class ProtectorInstance
    {
        public static void Main(string[] args)
        {
            
            if (args.Length < 3) return;

            var sourceFolder = args[0];
            var destinationPath = args[1];
            var keysFile = args[2];
            
            if (!Directory.Exists(destinationPath))
            {
                Extensions.PrintText("You need to enter the destination path to the folder in the second argument", ConsoleColor.Red);
                return;
            }

            var cryptor = new Cryptor(sourceFolder, destinationPath, keysFile);

            cryptor.Crypt();

        }
    }
}