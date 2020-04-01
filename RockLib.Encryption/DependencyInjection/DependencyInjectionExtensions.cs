#if !NET451
using Microsoft.Extensions.DependencyInjection;
using RockLib.Encryption.Async;
using System;

namespace RockLib.Encryption.DependencyInjection
{
    /// <summary>
    /// Defines extension methods related to dependency injection and encryption.
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="Crypto.Current"/> to the service collection. <see cref="Crypto.Current"/> is
        /// created using configuration or set directly by using <see cref="Crypto.SetCurrent(ICrypto)"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The same <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddCrypto(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSingleton(_ => Crypto.Current);
            services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<ICrypto>().AsAsync());
            return services;
        }

        /// <summary>
        /// Adds the specified <see cref="ICrypto"/> to the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="crypto">The <see cref="ICrypto"/> to add to the service collection.</param>
        /// <returns>The same <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddCrypto(this IServiceCollection services, ICrypto crypto)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (crypto == null)
                throw new ArgumentNullException(nameof(crypto));

            services.AddSingleton(crypto);
            services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<ICrypto>().AsAsync());
            return services;
        }

        /// <summary>
        /// Adds the specified <see cref="ICrypto"/> to the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="cryptoFactory">A func that returns an <see cref="ICrypto"/> when given an <see cref="IServiceProvider"/>.</param>
        /// <returns>The same <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddCrypto(this IServiceCollection services, Func<IServiceProvider, ICrypto> cryptoFactory)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (cryptoFactory == null)
                throw new ArgumentNullException(nameof(cryptoFactory));

            services.AddSingleton(cryptoFactory);
            services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<ICrypto>().AsAsync());
            return services;
        }
    }
}
#endif