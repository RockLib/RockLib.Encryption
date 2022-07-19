using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RockLib.Encryption;
using RockLib.Encryption.Symmetric.DependencyInjection;
using System;

namespace Example.Symmetric;

#pragma warning disable CA1303 // Do not pass literals as localized parameters
class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        var hostBuilder = Host.CreateDefaultBuilder(args);

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
                            .AddCredential("Aes", "1J9Og/OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="));
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
#pragma warning restore CA1303 // Do not pass literals as localized parameters
