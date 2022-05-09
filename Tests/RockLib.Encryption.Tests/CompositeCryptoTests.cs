using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace RockLib.Encryption.Tests;

public static class CompositeCryptoTests
{
    [Fact]
    public static void ConstructorThrowsWhenCryptoListIsNull()
    {
        var action = () => new CompositeCrypto(null!);

        action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null.*Parameter*cryptos*");
    }

    [Fact]
    public static void GetEncryptorSucceedsWithExistingICrypto()
    {
        var fooCrypto = CreateICrypto("foo");
        var barCrypto = CreateICrypto("bar");

        var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

        var fooEncryptor = compositeCrypto.GetEncryptor("foo");
        var barEncryptor = compositeCrypto.GetEncryptor("bar");
        
        fooEncryptor.Should().NotBeNull();
        barEncryptor.Should().NotBeNull();
        fooEncryptor.Should().NotBeSameAs(barEncryptor);
    }

    [Fact]
    public static void GetEncryptorThrowsWhenICryptoDoesntExist()
    {
        var fooCrypto = CreateICrypto("foo");
        var barCrypto = CreateICrypto("bar");

        var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

        Action action = () => compositeCrypto.GetEncryptor("baz");
        action.Should().Throw<KeyNotFoundException>()
            .WithMessage("Unable to locate implementation of ICrypto that can locate a credential using credentialName: baz");
    }

    [Fact]
    public static void GetDecryptorSucceedsWithExistingICrypto()
    {
        var fooCrypto = CreateICrypto("foo");
        var barCrypto = CreateICrypto("bar");

        var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

        var fooEncryptor = compositeCrypto.GetDecryptor("foo");
        var barEncryptor = compositeCrypto.GetDecryptor("bar");

        fooEncryptor.Should().NotBeNull();
        barEncryptor.Should().NotBeNull();
        fooEncryptor.Should().NotBeSameAs(barEncryptor);
    }

    [Fact]
    public static void GetDecryptorThrowsWhenICryptoDoesntExist()
    {
        var fooCrypto = CreateICrypto("foo");
        var barCrypto = CreateICrypto("bar");

        var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

        Action action = () => compositeCrypto.GetDecryptor("baz");
        action.Should().Throw<KeyNotFoundException>()
            .WithMessage("Unable to locate implementation of ICrypto that can locate a credential using credentialName: baz");
    }

    [Fact]
    public static void CanEncryptReturnsTrueForExistingCrypto()
    {
        var fooCrypto = CreateICrypto("foo");
        var barCrypto = CreateICrypto("bar");

        var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

        compositeCrypto.CanEncrypt("foo").Should().BeTrue();
        compositeCrypto.CanEncrypt("bar").Should().BeTrue();
    }

    [Fact]
    public static void CanEncryptReturnsFalseForNonExistingCrypto()
    {
        var fooCrypto = CreateICrypto("foo");
        var barCrypto = CreateICrypto("bar");

        var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

        compositeCrypto.CanEncrypt("baz").Should().BeFalse();
    }

    [Fact]
    public static void CanDecryptReturnsTrueForExistingCrypto()
    {
        var fooCrypto = CreateICrypto("foo");
        var barCrypto = CreateICrypto("bar");

        var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

        compositeCrypto.CanDecrypt("foo").Should().BeTrue();
        compositeCrypto.CanDecrypt("bar").Should().BeTrue();
    }

    [Fact]
    public static void CanDecryptReturnsFalseForNonExistingCrypto()
    {
        var fooCrypto = CreateICrypto("foo");
        var barCrypto = CreateICrypto("bar");

        var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

        compositeCrypto.CanDecrypt("baz").Should().BeFalse();
    }

    [Fact]
    public static void EncryptSucceedsWithExistingICrypto()
    {
        var fooCrypto = CreateICrypto("foo");
        var barCrypto = CreateICrypto("bar");

        var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

        compositeCrypto.Encrypt("stuff", "foo").Should().Be("EncryptedString : foo");
        compositeCrypto.Encrypt(Array.Empty<byte>(), "foo").Should().BeEquivalentTo(Encoding.UTF8.GetBytes("foo"));
        compositeCrypto.Encrypt("stuff", "bar").Should().Be("EncryptedString : bar");
        compositeCrypto.Encrypt(Array.Empty<byte>(), "bar").Should().BeEquivalentTo(Encoding.UTF8.GetBytes("bar"));
    }

    [Fact]
    public static void EncryptThrowsWhenICryptoDoesntExist()
    {
        var fooCrypto = CreateICrypto("foo");
        var barCrypto = CreateICrypto("bar");

        var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

        var action = () => compositeCrypto.Encrypt("stuff", "baz");
        action.Should().Throw<KeyNotFoundException>()
            .WithMessage("Unable to locate implementation of ICrypto that can locate a credential using credentialName: baz");
    }

    [Fact]
    public static void DecryptSucceedsWithExistingICrypto()
    {
        var fooCrypto = CreateICrypto("foo");
        var barCrypto = CreateICrypto("bar");

        var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

        compositeCrypto.Decrypt("stuff", "foo").Should().Be("DecryptedString : foo");
        compositeCrypto.Decrypt(Array.Empty<byte>(), "foo").Should().BeEquivalentTo(Encoding.UTF8.GetBytes("foo"));
        compositeCrypto.Decrypt("stuff", "bar").Should().Be("DecryptedString : bar");
        compositeCrypto.Decrypt(Array.Empty<byte>(), "bar").Should().BeEquivalentTo(Encoding.UTF8.GetBytes("bar"));
    }

    [Fact]
    public static void DecryptThrowsWhenICryptoDoesntExist()
    {
        ICrypto fooCrypto = CreateICrypto("foo");
        ICrypto barCrypto = CreateICrypto("bar");

        var compositeCrypto = new CompositeCrypto(new List<ICrypto> { fooCrypto, barCrypto });

        Action action = () => compositeCrypto.Decrypt("stuff", "baz");
        action.Should().Throw<KeyNotFoundException>()
            .WithMessage("Unable to locate implementation of ICrypto that can locate a credential using credentialName: baz");
    }

    private static ICrypto CreateICrypto(string cryptoId)
    {
        var cryptoMock = new Mock<ICrypto>();

        cryptoMock.Setup(f => f.CanEncrypt(cryptoId)).Returns(true);
        cryptoMock.Setup(f => f.CanEncrypt(It.Is<string>(s => s != cryptoId))).Returns(false);
        cryptoMock.Setup(f => f.Encrypt(It.IsAny<string>(), It.IsAny<string>())).Returns($"EncryptedString : {cryptoId}");
        cryptoMock.Setup(f => f.Encrypt(It.IsAny<byte[]>(), It.IsAny<string>())).Returns(Encoding.UTF8.GetBytes(cryptoId));
        cryptoMock.Setup(f => f.GetEncryptor(cryptoId)).Returns(new Mock<IEncryptor>().Object);

        cryptoMock.Setup(f => f.CanDecrypt(cryptoId)).Returns(true);
        cryptoMock.Setup(f => f.CanDecrypt(It.Is<string>(s => s != cryptoId))).Returns(false);
        cryptoMock.Setup(f => f.Decrypt(It.IsAny<string>(), It.IsAny<string>())).Returns($"DecryptedString : {cryptoId}");
        cryptoMock.Setup(f => f.Decrypt(It.IsAny<byte[]>(), It.IsAny<string>())).Returns(Encoding.UTF8.GetBytes(cryptoId));
        cryptoMock.Setup(f => f.GetDecryptor(cryptoId)).Returns(new Mock<IDecryptor>().Object);

        return cryptoMock.Object;
    }
}
