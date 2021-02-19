using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RockLib.Encryption.Symmetric;
using RockLib.Encryption.Symmetric.DependencyInjection;
using System;

namespace Example.Symmetric
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);

            Console.WriteLine("Select the service to run:");
            Console.WriteLine($"1) {nameof(DependencyInjectionEncryptionService)}");
            Console.WriteLine($"2) {nameof(StaticCryptoEncryptionService)}");

            while (true)
            {
                Console.Write(">");

                switch (Console.ReadLine())
                {
                    case "1":
                        return hostBuilder.ConfigureServices(services =>
                        {
                            // Configuring a symmetric crypto programmatically
                            services.AddSymmetricCrypto()
                                .AddCredential("Aes", "1J9Og/OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s=")
                                .AddCredential("DES", "2LQliivTtNo=", SymmetricAlgorithm.DES, 8)
                                .AddCredential("RC2", "v6/CJsBIAK4rKs0hJR+JoA==", SymmetricAlgorithm.RC2, 8)
                                .AddCredential("Rijndael", "CQSImVlbvJMZcnrkzT3/ouW1klt6STljrDjRiBzIsSk=", SymmetricAlgorithm.Rijndael)
                                .AddCredential("TripleDES", "qHIqXosjnkzkUM9yp2aE+J+aey9w53+B", SymmetricAlgorithm.TripleDES, 8);

                            services.AddHostedService<DependencyInjectionEncryptionService>();
                        });
                    case "2":
                        return hostBuilder.ConfigureServices(services =>
                        {
                            // Symmetric crypto will be loaded and configured from configuration
                            services.AddHostedService<StaticCryptoEncryptionService>();
                        });
                    default:
                        Console.WriteLine(" Invalid input. Valid values are '1' and '2'.");
                        break;
                }
            }
        }
    }
}
