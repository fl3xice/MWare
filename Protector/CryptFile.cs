using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Protector
{
    public static class CryptFile
    {
        
        public static ResultCryptFile EncryptFile(string sourceFile)
        {
            if (!File.Exists(sourceFile)) throw new Exception("Source file does don't exists");
            
            var metaFile = new FileInfo(sourceFile);
            var bytes = new byte[metaFile.Length];
            
            using var sourceStream = File.Open(sourceFile, FileMode.Open);
            
            sourceStream.Read(bytes, 0, bytes.Length);
            
            using var aes = new AesCryptoServiceProvider
            {
                Mode = CipherMode.CBC
            };
            
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(Encoding.UTF8.GetString(bytes));
            }
            
            var encrypted = msEncrypt.ToArray();

            return new 
                ResultCryptFile
                (
                    Convert.ToBase64String(aes.Key), 
                    Convert.ToBase64String(encrypted),
                    aes.IV
                );
        }

        public static ResultDecryptFile DecryptFile(string sourceFile, string key, byte[] IV)
        {
            
            if (!File.Exists(sourceFile)) throw new Exception("Source file does don't exists");
            
            var metaFile = new FileInfo(sourceFile);
            var bytes = new byte[metaFile.Length];
            
            using var sourceStream = File.Open(sourceFile, FileMode.Open);
            
            sourceStream.Read(bytes, 0, bytes.Length);

            var bytesForDecrypt = Convert.FromBase64String(Encoding.UTF8.GetString(bytes));
            
            using var aes = new AesCryptoServiceProvider
            {
                Key = Convert.FromBase64String(key),
                IV = IV,
                Mode = CipherMode.CBC
            };
            
            var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            string data;
            
            using var msDecrypt = new MemoryStream(bytesForDecrypt);
            using var csDecrypt = new CryptoStream(msDecrypt, decrypt, CryptoStreamMode.Read);
            using (var srDecrypt = new StreamReader(csDecrypt))
            {
                data = srDecrypt.ReadToEnd();
            }

            return new ResultDecryptFile
                (
                    data
                );
        }
    }

    public class ResultCryptFile
    {
        public readonly string Key;
        public readonly string Result;
        public readonly byte[] Iv;
        
        public ResultCryptFile(string key, string result, byte[] iv)
        {
            this.Key = key;
            this.Result = result;
            this.Iv = iv;
        }
    }

    public class ResultDecryptFile
    {
        public readonly string Data;

        public ResultDecryptFile(string data)
        {
            this.Data = data;
        }
    }
}