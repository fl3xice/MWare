using System;
using System.IO;
using System.Text;

namespace Protector
{
    public static class ProtectorInstance
    {
        public static void Main(string[] args)
        {
            
            if (args.Length < 3) return;

            var sourceFile = args[0];
            var destinationFile = args[1];
            var keysFile = args[2]; // file for save all keys for all files
            
            var resultCryptFile = CryptFile.EncryptFile(sourceFile);

            using (var destinationStream = File.Open(destinationFile, FileMode.OpenOrCreate))
            {
                destinationStream.Write(Encoding.UTF8.GetBytes(resultCryptFile.Result));
            }

            Extensions.PrintText(resultCryptFile.Key + "\n", ConsoleColor.Red);
            Extensions.PrintText(resultCryptFile.Result + "\n", ConsoleColor.Blue);

            var resultDecryptFile = CryptFile.DecryptFile(destinationFile, resultCryptFile.Key, resultCryptFile.Iv);
            
            Extensions.PrintText(resultDecryptFile.Data + "\n", ConsoleColor.Yellow);
        }
    }
}