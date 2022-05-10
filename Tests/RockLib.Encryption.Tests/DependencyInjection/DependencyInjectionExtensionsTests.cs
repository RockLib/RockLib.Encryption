using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RockLib.Encryption.DependencyInjection;
using System;
using Xunit;

namespace RockLib.Encryption.Tests.DependencyInjection;

public static class DependencyInjectionExtensionsTests
{
    [Fact(DisplayName = "AddCrypto Extension 1 throws when the services parameter is null")]
    public static void AddCryptoExtension1SadPath()
    {
        IServiceCollection services = null!;

        Action act = () => services.AddCrypto();

        act.Should().ThrowExactly<ArgumentNullException>().Which.Message.Should().Contain("services");
    }

    [Fact(DisplayName = "AddCrypto Extension 2 adds the crypto parameter to the service collection")]
    public static void AddCryptoExtension2HappyPath()
    {
        var mockCrypto = new Mock<ICrypto>();

        var services = new ServiceCollection();

        services.AddCrypto(mockCrypto.Object);

        var provider = services.BuildServiceProvider();

        var crypto = provider.GetRequiredService<ICrypto>();

        crypto.Should().BeSameAs(mockCrypto.Object);
    }

    [Fact(DisplayName = "AddCrypto Extension 2 throws when the services parameter is null")]
    public static void AddCryptoExtension2SadPath1()
    {
        var mockCrypto = new Mock<ICrypto>();

        IServiceCollection services = null!;

        Action act = () => services.AddCrypto(mockCrypto.Object);

        act.Should().ThrowExactly<ArgumentNullException>().Which.Message.Should().Contain("services");
    }

    [Fact(DisplayName = "AddCrypto Extension 2 throws when the crypto parameter is null")]
    public static void AddCryptoExtension2SadPath2()
    {
        ICrypto crypto = null!;

        var services = new ServiceCollection();

        Action act = () => services.AddCrypto(crypto);

        act.Should().ThrowExactly<ArgumentNullException>().Which.Message.Should().Contain("crypto");
    }

    [Fact(DisplayName = "AddCrypto Extension 3 adds the ICrypto returned by the cryptoFactory parameter to the service collection")]
    public static void AddCryptoExtension3HappyPath()
    {
        var mockCrypto = new Mock<ICrypto>();
        Func<IServiceProvider, ICrypto> cryptoFactory = sp => mockCrypto.Object;

        var services = new ServiceCollection();

        services.AddCrypto(cryptoFactory);

        var provider = services.BuildServiceProvider();

        var crypto = provider.GetRequiredService<ICrypto>();

        crypto.Should().BeSameAs(mockCrypto.Object);
    }

    [Fact(DisplayName = "AddCrypto Extension 3 throws when the services parameter is null")]
    public static void AddCryptoExtension3SadPath1()
    {
        var mockCrypto = new Mock<ICrypto>();
        Func<IServiceProvider, ICrypto> cryptoFactory = provider => mockCrypto.Object;

        IServiceCollection services = null!;

        Action act = () => services.AddCrypto(cryptoFactory);

        act.Should().ThrowExactly<ArgumentNullException>().Which.Message.Should().Contain("services");
    }

    [Fact(DisplayName = "AddCrypto Extension 3 throws when the cryptoFactory parameter is null")]
    public static void AddCryptoExtension3SadPath2()
    {
        Func<IServiceProvider, ICrypto> cryptoFactory = null!;

        var services = new ServiceCollection();

        Action act = () => services.AddCrypto(cryptoFactory);

        act.Should().ThrowExactly<ArgumentNullException>().Which.Message.Should().Contain("cryptoFactory");
    }
}
