using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RockLib.Encryption.Symmetric.DependencyInjection;
using System.Threading.Tasks;

namespace Example.DependencyInjection
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices)
                .RunConsoleAsync();
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSymmetricCrypto()
               .AddCredential("MyFirstCredential", "1J9Og/OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s=");

            services.AddHostedService<EncryptionService>();
        }
    }
}
