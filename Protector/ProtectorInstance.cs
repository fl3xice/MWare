using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualBasic;

namespace Protector
{
    public static class ProtectorInstance
    {
        public static void Main(string[] args)
        {
            if (args[0] == "-decrypt")
            {
                if (args.Length < 4) return;
                Decrypt(args);
            }
            else
            {
                if (args.Length < 3) return;
                Crypt(args);
            }
        }

        private static void Decrypt(IReadOnlyList<string> args)
        {
            var archive = args[1];
            var destinationPath = args[2];
            var keysFile = args[3];
            
            Extensions.PrintText("Decrypt: \n", ConsoleColor.Green);
            
            if (!File.Exists(archive))
            {
                Extensions.PrintText("You need to enter the archive in the first argument", ConsoleColor.Red);
                return;
            }
            
            var cryptor = new Cryptor(archive, destinationPath, keysFile);
            
            Cryptor.Decrypt(cryptor, archive, destinationPath, keysFile);
        }

        private static void Crypt(IReadOnlyList<string> args)
        {
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