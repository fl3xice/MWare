using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Protector
{
    public class Cryptor
    {
        private readonly string _sourceFolder;
        private readonly string _destinationPath;
        private readonly string _keysPath;
        private readonly List<string> _allPaths = new List<string>();
        
        public Cryptor(string sourceFolder, string destinationPath, string keysPath)
        {
            this._sourceFolder = sourceFolder;
            this._destinationPath = destinationPath;
            this._keysPath = keysPath;
        }

        public bool Crypt()
        {
            GetAllFiles(_sourceFolder);

            using var keysFileStream = File.Open(_keysPath, FileMode.OpenOrCreate);

            using var zipToOpen = new FileStream(_destinationPath + "/" + _sourceFolder + ".zip", FileMode.OpenOrCreate);
            using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
            
                for (var i = 0; i < _allPaths.Count; i++)
            {
                var resultCryptFile = CryptFile.EncryptFile(_allPaths[i]);

                var entry = archive.CreateEntry(_allPaths[i],CompressionLevel.NoCompression);

                using var entryWriter = new StreamWriter(entry.Open());

                entryWriter.Write(resultCryptFile.Result);
                
                keysFileStream.Write(
                    Encoding.UTF8.GetBytes($"{resultCryptFile.Key}#{Encoding.UTF8.GetString(resultCryptFile.Iv)}\n"));
                
                ProgressBar(i, _allPaths.Count);
            }
            
            return true;
        }
        
        private static void ProgressBar(int progress, int tot)
        {
            Console.CursorLeft = 0;
            Console.Write("[");
            Console.CursorLeft = 32;
            Console.Write("]");
            Console.CursorLeft = 1;
            var chunk = 30.0f / tot;
            
            var position = 1;
            for (var i = 1; i < chunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }
            
            for (var i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }
            
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(progress.ToString() + " of " + tot.ToString() + "    ");
        }
        
        private void GetAllFiles(string sDir)
        {
            foreach (var f in Directory.GetFiles(sDir))
            {
                this._allPaths.Add(f);
            }
            foreach (var d in Directory.GetDirectories(sDir))
            {
                GetAllFiles(d);
            }
        }
    }
}