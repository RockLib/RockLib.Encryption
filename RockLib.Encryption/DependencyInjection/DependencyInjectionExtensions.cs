using Microsoft.Extensions.DependencyInjection;
using System;

namespace RockLib.Encryption.DependencyInjection;

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
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton(_ => Crypto.Current);
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
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        if (crypto is null)
        {
            throw new ArgumentNullException(nameof(crypto));
        }

        services.AddSingleton(crypto);
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
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        if (cryptoFactory is null)
        {
            throw new ArgumentNullException(nameof(cryptoFactory));
        }

        services.AddSingleton(cryptoFactory);
        return services;
    }
}