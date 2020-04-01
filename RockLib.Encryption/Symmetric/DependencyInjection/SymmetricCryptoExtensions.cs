#if !NET451
using Microsoft.Extensions.DependencyInjection;
using System;

namespace RockLib.Encryption.Symmetric.DependencyInjection
{
    /// <summary>
    /// Defines extension methods for depency injection with <see cref="SymmetricCrypto"/>.
    /// </summary>
    public static class SymmetricCryptoExtensions
    {
        private const ServiceLifetime _defaultLifetime = ServiceLifetime.Singleton;

        /// <summary>
        /// Adds a <see cref="SymmetricCrypto"/> to the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="lifetime">The <see cref="ServiceLifetime"/>.</param>
        /// <returns></returns>
        public static SymmetricCryptoBuilder AddSymmetricCrypto(this IServiceCollection services, ServiceLifetime lifetime = _defaultLifetime)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var builder = new SymmetricCryptoBuilder();
            services.Add(new ServiceDescriptor(typeof(ICrypto), builder.Build, lifetime));
            return builder;
        }
    }
}
#endif