using RockLib.Encryption;
using RockLib.Encryption.Symmetric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Example.Symmetric
{
    public static class Common
    {
        public static void EncryptionPrompt(ICrypto crypto, IReadOnlyCollection<Credential> credentials, string serviceName)
        {
            Thread.Sleep(500);
            Console.WriteLine();
            Console.WriteLine($"Starting {serviceName}...");
            Console.WriteLine();

            while (true)
            {
                try
                {
                    Console.Write("Please enter the text you would like to encrypt: ");

                    var input = Console.ReadLine();

                    var options = string.Join(", ", credentials.Select(co => co.Name));
                    Console.WriteLine($"Please choose a method to encrypt the text ({options}):");
                    var encryptMethod = Console.ReadLine();

                    string encrypted;
                    try
                    {
                        encrypted = crypto.Encrypt(input, encryptMethod);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Unable to encrypt using '{encryptMethod}'. Error: {ex.Message}");
                        continue;
                    }

                    Console.WriteLine("Encryption successful: " + encrypted);
                    Console.WriteLine();

                    Console.WriteLine($"Please choose a method to decrypt the text ({options}):");
                    var decryptMethod = Console.ReadLine();
                    string decrypted;
                    try
                    {
                        decrypted = crypto.Decrypt(encrypted, decryptMethod);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Unable to decrypt using '{decryptMethod}'. Error: {ex.Message}");
                        continue;
                    }

                    Console.WriteLine("Decryption successful: " + decrypted);
                }
                finally
                {
                    Console.WriteLine("Restarting...");
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
        }
    }
}
