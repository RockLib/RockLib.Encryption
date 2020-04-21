using Microsoft.Extensions.Hosting;
using RockLib.Encryption;
using RockLib.Encryption.Symmetric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Example.DependencyInjection
{
    public class StaticCryptoEncryptionService : IHostedService
    {
        private IReadOnlyCollection<Credential> _credentialOptions;
        private readonly Thread _executionThread;

        public StaticCryptoEncryptionService()
        {
            _credentialOptions = ((InMemoryCredentialRepository)((SymmetricCrypto)Crypto.Current).CredentialRepository).Credentials;
            _executionThread = new Thread(RunEncryptionPrompt);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executionThread.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        private void RunEncryptionPrompt()
        {
            Thread.Sleep(500);
            Console.WriteLine();
            Console.WriteLine("Starting Static Crypto.Current Encryption Service...");
            Console.WriteLine();

            while (true)
            {
                try
                {
                    Console.Write("Please enter the text you would like to encrypt: ");

                    var input = Console.ReadLine();

                    var options = string.Join(", ", _credentialOptions.Select(co => co.Name));
                    Console.WriteLine($"Please choose a method to encrypt the text ({options}):");
                    var encryptMethod = Console.ReadLine();

                    string encrypted;
                    try
                    {
                        encrypted = Crypto.Current.Encrypt(input, encryptMethod);
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
                        decrypted = Crypto.Current.Decrypt(encrypted, decryptMethod);
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
