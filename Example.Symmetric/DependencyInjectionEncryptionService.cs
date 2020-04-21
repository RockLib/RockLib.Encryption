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
    public class DependencyInjectionEncryptionService : IHostedService
    {
        private readonly ICrypto _crypto;
        private IReadOnlyCollection<Credential> _credentialOptions;
        private readonly Thread _executionThread;

        public DependencyInjectionEncryptionService(ICrypto crypto)
        {
            _crypto = crypto;
            _credentialOptions = ((InMemoryCredentialRepository)((SymmetricCrypto)_crypto).CredentialRepository).Credentials;
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
            Console.WriteLine("Starting Dependency Injection Encryption Service...");
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
                        encrypted = _crypto.Encrypt(input, encryptMethod);
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
                        decrypted = _crypto.Decrypt(encrypted, decryptMethod);
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
