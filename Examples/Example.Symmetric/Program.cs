using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RockLib.Encryption;
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

            hostBuilder.ConfigureServices(services => services.AddHostedService<EncryptionService>());

            Console.WriteLine("Register ICrypto...");
            Console.WriteLine("1) Programmatically");
            Console.WriteLine("2) With configuration");

            while (true)
            {
                Console.Write(">");

                switch (Console.ReadLine())
                {
                    case "1":
                        // Configuring a symmetric crypto programmatically
                        return hostBuilder.ConfigureServices(services =>
                            services.AddSymmetricCrypto()
                                .AddCredential("Aes", "1J9Og/OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s=")
                                .AddCredential("DES", "2LQliivTtNo=", SymmetricAlgorithm.DES, 8)
                                .AddCredential("RC2", "v6/CJsBIAK4rKs0hJR+JoA==", SymmetricAlgorithm.RC2, 8)
                                .AddCredential("Rijndael", "CQSImVlbvJMZcnrkzT3/ouW1klt6STljrDjRiBzIsSk=", SymmetricAlgorithm.Rijndael)
                                .AddCredential("TripleDES", "qHIqXosjnkzkUM9yp2aE+J+aey9w53+B", SymmetricAlgorithm.TripleDES, 8));
                    case "2":
                        return hostBuilder.ConfigureServices(services =>
                            services.AddSingleton(Crypto.Current));
                    default:
                        Console.WriteLine(" Invalid input. Valid values are '1' and '2'.");
                        break;
                }
            }
        }
    }
}
