using Microsoft.Extensions.Hosting;
using RockLib.Encryption;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Example.DependencyInjection
{
    public class EncryptionService : IHostedService
    {
        public EncryptionService(ICrypto myCrypto)
        {
            MyCrypto = myCrypto;
        }

        public ICrypto MyCrypto { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting Encryption Service.");
            Console.Write("Please enter the text you would like to encrypt: ");
            
            var input = Console.ReadLine();
            var encrypted = MyCrypto.Encrypt(input, "MyFirstCredential");
            
            Console.WriteLine("Encryption successful: " + encrypted);
            Console.WriteLine();
            
            Console.WriteLine("Press any key when ready to decrypt.");
            Console.ReadKey();

            var decrypted = MyCrypto.Decrypt(encrypted, "MyFirstCredential");
            Console.WriteLine("Decryption successful: " + decrypted);
            Console.ReadKey();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
