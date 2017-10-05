using System;
using RockLib.Encryption;
using System.Threading.Tasks;

namespace Example.Encryption.DotNetCore
{
    public class Program
    {
        static void Main(string[] args)
        {
            var originalValue = "This is some string that we want to encrypt";
            Example(originalValue);
            Console.WriteLine();
            ExampleAsync(originalValue).Wait();
            Console.ReadLine();
        }

        private static void Example(string plaintext)
        {
            Console.WriteLine($"Synchronous - Original: {plaintext}");

            var encryptedValue = Crypto.Encrypt(plaintext);

            Console.WriteLine($"Synchronous - Encrypted: {encryptedValue}");

            var decryptedValue = Crypto.Decrypt(encryptedValue);

            Console.WriteLine($"Synchronous - Decrypted: {decryptedValue}");
        }

        private static async Task ExampleAsync(string plaintext)
        {
            Console.WriteLine($"Asynchronous - Original: {plaintext}");

            var encryptedValue = await Crypto.EncryptAsync(plaintext);

            Console.WriteLine($"Asynchronous - Encrypted: {encryptedValue}");

            var decryptedValue = await Crypto.DecryptAsync(encryptedValue);

            Console.WriteLine($"Asynchronous - Decrypted: {decryptedValue}");
        }
    }
}
