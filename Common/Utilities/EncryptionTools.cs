using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace plannerBackEnd.Common.Utilities
{
    public class EncryptionTools
    {
        // ---------------------------------------------------------------------------------------------
        public static string EncryptString(string key, string plainText)
        {
            byte[] initializationVector = new byte[16];
            byte[] returnArray;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = initializationVector;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        returnArray = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(returnArray);
        }

        // ---------------------------------------------------------------------------------------------
        public static string DecryptString(string key, string cipherText)
        {
            byte[] initializationVector = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = initializationVector;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
