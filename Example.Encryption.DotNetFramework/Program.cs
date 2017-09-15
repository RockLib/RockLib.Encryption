using System;
using RockLib.Encryption;

namespace Example.Encryption.DotNetFramework
{
    public class Program
    {
        static void Main(string[] args)
        {
            var originalValue = "This is some string that we want to encrypt";

            Console.WriteLine($"Original Value: {originalValue}");

            var encryptedValue = Crypto.Encrypt(originalValue);

            Console.WriteLine($"Encrypted Value: {encryptedValue}");

            var decryptedValue = Crypto.Decrypt(encryptedValue);

            Console.WriteLine($"Decrypted Value: {decryptedValue}");

            Console.ReadLine();
        }
    }
}
