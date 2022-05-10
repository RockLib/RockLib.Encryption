using System;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using RockLib.Configuration;
using RockLib.Encryption.Symmetric;
using RockLib.Immutable;
using Xunit;

namespace RockLib.Encryption.Tests;

public static class CryptoTests
{
    [Fact]
    public static void CurrentReturnsSameAsSetCurrent()
    {
        lock (CryptoReset.Locker)
        {
            ResetCrypto();

            var crypto = new Mock<ICrypto>().Object;

            Crypto.SetCurrent(crypto);

            Crypto.Current.Should().BeSameAs(crypto); 
        }
    }

    [Fact]
    public static void MissingConfigThrowsWhenUsingDefaultCrypto()
    {
        lock (CryptoReset.Locker)
        {
            ResetConfig();

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("NoFactories.json");
            Config.SetRoot(configBuilder.Build());

            ResetCrypto();

            Crypto.SetCurrent(null!);
            var action = () => { var current = Crypto.Current; };

            action.Should().Throw<InvalidOperationException>()
                .WithMessage("No crypto implementations found in config. See the Readme.md file for details on how to setup the configuration."); 
        }
    }

    [Fact]
    public static void SingleFactoryCreatesSpecificCrypto()
    {
        lock (CryptoReset.Locker)
        {
            ResetConfig();

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("SingleFactory.json");
            Config.SetRoot(configBuilder.Build());

            ResetCrypto();

            Crypto.SetCurrent(null!);

            Crypto.Current.Should().BeAssignableTo<SymmetricCrypto>(); 
        }
    }

    [Fact]
    public static void MultipleFactoriesCreatesCompositeCrypto()
    {
        lock (CryptoReset.Locker)
        {
            ResetConfig();

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("MultiFactory.json");
            Config.SetRoot(configBuilder.Build());

            ResetCrypto();

            Crypto.SetCurrent(null!);

            Crypto.Current.Should().BeAssignableTo<CompositeCrypto>(); 
        }
    }

    [Fact]
    public static void GetEncryptorCallsCryptoGetEncryptor()
    {
        lock (CryptoReset.Locker)
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var credentialName = "foo";

            Crypto.GetEncryptor();
            Crypto.GetEncryptor(credentialName);

            cryptoMock.Verify(cm => cm.GetEncryptor(null));
            cryptoMock.Verify(cm => cm.GetEncryptor(It.Is<string>(o => o == credentialName))); 
        }
    }

    [Fact]
    public static void GetDecryptorCallsCryptoGetDecryptor()
    {
        lock (CryptoReset.Locker)
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var credentialName = "foo";

            Crypto.GetDecryptor();
            Crypto.GetDecryptor(credentialName);

            cryptoMock.Verify(cm => cm.GetDecryptor(null));
            cryptoMock.Verify(cm => cm.GetDecryptor(It.Is<string>(o => o == credentialName))); 
        }
    }

    [Fact]
    public static void EncryptByStringCallsCryptoEncryptByString()
    {
        lock (CryptoReset.Locker)
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var stringToEncrypt = "Something to encrypt";
            var credentialName = "foo";

            Crypto.Encrypt(stringToEncrypt);
            Crypto.Encrypt(stringToEncrypt, credentialName);

            cryptoMock.Verify(cm => cm.Encrypt(It.Is<string>(s => s == stringToEncrypt), null));
            cryptoMock.Verify(cm => cm.Encrypt(It.Is<string>(s => s == stringToEncrypt), It.Is<string>(o => o == credentialName))); 
        }
    }

    [Fact]
    public static void EncryptByByteArrayCallsCryptoEncryptByByteArray()
    {
        lock (CryptoReset.Locker)
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var byteArrayToEncrypt = Array.Empty<byte>();
            var credentialName = "foo";

            Crypto.Encrypt(byteArrayToEncrypt);
            Crypto.Encrypt(byteArrayToEncrypt, credentialName);

            cryptoMock.Verify(cm => cm.Encrypt(It.Is<byte[]>(s => s == byteArrayToEncrypt), null));
            cryptoMock.Verify(cm => cm.Encrypt(It.Is<byte[]>(s => s == byteArrayToEncrypt), It.Is<string>(o => o == credentialName))); 
        }
    }

    [Fact]
    public static void DecryptByStringCallsCryptoDecryptByString()
    {
        lock (CryptoReset.Locker)
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var stringToDecrypt = "Something to encrypt";
            var credentialName = "foo";

            Crypto.Decrypt(stringToDecrypt);
            Crypto.Decrypt(stringToDecrypt, credentialName);

            cryptoMock.Verify(cm => cm.Decrypt(It.Is<string>(s => s == stringToDecrypt), null));
            cryptoMock.Verify(cm => cm.Decrypt(It.Is<string>(s => s == stringToDecrypt), It.Is<string>(o => o == credentialName))); 
        }
    }

    [Fact]
    public static void DecryptByByteArrayCallsCryptoDecryptByByteArray()
    {
        lock (CryptoReset.Locker)
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var byteArrayToDecrypt = Array.Empty<byte>();
            var credentialName = "foo";

            Crypto.Decrypt(byteArrayToDecrypt);
            Crypto.Decrypt(byteArrayToDecrypt, credentialName);

            cryptoMock.Verify(cm => cm.Decrypt(It.Is<byte[]>(s => s == byteArrayToDecrypt), null));
            cryptoMock.Verify(cm => cm.Decrypt(It.Is<byte[]>(s => s == byteArrayToDecrypt), It.Is<string>(o => o == credentialName))); 
        }
    }

    private static void ResetConfig()
    {
        var rootField = typeof(Config).GetField("_root", BindingFlags.NonPublic | BindingFlags.Static)!;
        var root = (Semimutable<IConfiguration>)rootField.GetValue(null!)!;
        root.GetUnlockValueMethod().Invoke(root, null);
    }

    internal static void ResetCrypto()
    {
        var currentField = typeof(Crypto).GetField("_current", BindingFlags.NonPublic | BindingFlags.Static)!;
        var current = (Semimutable<ICrypto>)currentField.GetValue(null!)!;
        current.GetUnlockValueMethod().Invoke(current, null);
    }
}