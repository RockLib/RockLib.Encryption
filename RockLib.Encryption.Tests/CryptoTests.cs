using System;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using RockLib.Configuration;
using RockLib.Encryption.Symmetric;
using RockLib.Immutable;
using RockLib.Encryption.Async;
using System.Threading.Tasks;

namespace RockLib.Encryption.Tests
{
    [TestFixture]
    public class CryptoTests
    {
        [Test]
        public void CurrentReturnsSameAsSetCurrent()
        {
            ResetCrypto();

            var crypto = new Mock<ICrypto>().Object;

            Crypto.SetCurrent(crypto);

            Crypto.Current.Should().BeSameAs(crypto);
        }

        [Test]
        public void MissingConfigThrowsWhenUsingDefaultCrypto()
        {
            ResetConfig();

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("NoFactories.json");
            Config.SetRoot(configBuilder.Build());

            ResetCrypto();

            Crypto.SetCurrent(null);
            Action action = () => { var current = Crypto.Current; };

            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("No crypto implementations found in config.  See the Readme.md file for details on how to setup the configuration.");
        }

        [Test]
        public void SingleFactoryCreatesSpecificCrypto()
        {
            ResetConfig();

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("SingleFactory.json");
            Config.SetRoot(configBuilder.Build());

            ResetCrypto();

            Crypto.SetCurrent(null);

            Crypto.Current.Should().BeAssignableTo<SymmetricCrypto>();
        }

        [Test]
        public void MultipleFactoriesCreatesCompositeCrypto()
        {
            ResetConfig();

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("MultiFactory.json");
            Config.SetRoot(configBuilder.Build());

            ResetCrypto();

            Crypto.SetCurrent(null);

            Crypto.Current.Should().BeAssignableTo<CompositeCrypto>();
        }

        [Test]
        public void GetEncryptorCallsCryptoGetEncryptor()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);
            
            var identifier = new object();

            Crypto.GetEncryptor();
            Crypto.GetEncryptor(identifier);

            cryptoMock.Verify(cm => cm.GetEncryptor(null));
            cryptoMock.Verify(cm => cm.GetEncryptor(It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void GetDecryptorCallsCryptoGetDecryptor()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var identifier = new object();

            Crypto.GetDecryptor();
            Crypto.GetDecryptor(identifier);

            cryptoMock.Verify(cm => cm.GetDecryptor(null));
            cryptoMock.Verify(cm => cm.GetDecryptor(It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void EncryptByStringCallsCryptoEncryptByString()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var stringToEncrypt = "Something to encrypt";
            var identifier = new object();

            Crypto.Encrypt(stringToEncrypt);
            Crypto.Encrypt(stringToEncrypt, identifier);

            cryptoMock.Verify(cm => cm.Encrypt(It.Is<string>(s => s == stringToEncrypt), null));
            cryptoMock.Verify(cm => cm.Encrypt(It.Is<string>(s => s == stringToEncrypt), It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void EncryptByByteArrayCallsCryptoEncryptByByteArray()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var byteArrayToEncrypt = new byte[0];
            var identifier = new object();

            Crypto.Encrypt(byteArrayToEncrypt);
            Crypto.Encrypt(byteArrayToEncrypt, identifier);

            cryptoMock.Verify(cm => cm.Encrypt(It.Is<byte[]>(s => s == byteArrayToEncrypt), null));
            cryptoMock.Verify(cm => cm.Encrypt(It.Is<byte[]>(s => s == byteArrayToEncrypt), It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void DecryptByStringCallsCryptoDecryptByString()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var stringToDecrypt = "Something to encrypt";
            var identifier = new object();

            Crypto.Decrypt(stringToDecrypt);
            Crypto.Decrypt(stringToDecrypt, identifier);

            cryptoMock.Verify(cm => cm.Decrypt(It.Is<string>(s => s == stringToDecrypt), null));
            cryptoMock.Verify(cm => cm.Decrypt(It.Is<string>(s => s == stringToDecrypt), It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void DecryptByByteArrayCallsCryptoDecryptByByteArray()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var byteArrayToDecrypt = new byte[0];
            var identifier = new object();

            Crypto.Decrypt(byteArrayToDecrypt);
            Crypto.Decrypt(byteArrayToDecrypt, identifier);

            cryptoMock.Verify(cm => cm.Decrypt(It.Is<byte[]>(s => s == byteArrayToDecrypt), null));
            cryptoMock.Verify(cm => cm.Decrypt(It.Is<byte[]>(s => s == byteArrayToDecrypt), It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void GetEncryptorAsyncGivenCurrentIsIAsyncCryptoCallsCryptoGetEncryptorAsync()
        {
            var cryptoMock = new Mock<AbstractAsyncCrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);
            
            var identifier = new object();

            Crypto.GetEncryptorAsync().Wait();
            Crypto.GetEncryptorAsync(identifier).Wait();

            cryptoMock.Verify(cm => cm.GetEncryptorAsync(null));
            cryptoMock.Verify(cm => cm.GetEncryptorAsync(It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void GetEncryptorAsyncGivenCurrentIsNotIAsyncCryptoCallsCryptoGetEncryptor()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var identifier = new object();

            Crypto.GetEncryptorAsync().Wait();
            Crypto.GetEncryptorAsync(identifier).Wait();

            cryptoMock.Verify(cm => cm.GetEncryptor(null));
            cryptoMock.Verify(cm => cm.GetEncryptor(It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void GetDecryptorAsyncGivenCurrentIsIAsyncCryptoCallsCryptoGetDecryptorAsync()
        {
            var cryptoMock = new Mock<AbstractAsyncCrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var identifier = new object();

            Crypto.GetDecryptorAsync().Wait();
            Crypto.GetDecryptorAsync(identifier).Wait();

            cryptoMock.Verify(cm => cm.GetDecryptorAsync(null));
            cryptoMock.Verify(cm => cm.GetDecryptorAsync(It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void GetDecryptorAsyncGivenCurrentIsNotIAsyncCryptoCallsCryptoGetDecryptor()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var identifier = new object();

            Crypto.GetDecryptorAsync().Wait();
            Crypto.GetDecryptorAsync(identifier).Wait();

            cryptoMock.Verify(cm => cm.GetDecryptor(null));
            cryptoMock.Verify(cm => cm.GetDecryptor(It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void EncryptAsyncByStringGivenCurrentIsIAsyncCryptoCallsCryptoEncryptAsyncByString()
        {
            var cryptoMock = new Mock<AbstractAsyncCrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var stringToEncrypt = "Something to encrypt";
            var identifier = new object();

            Crypto.EncryptAsync(stringToEncrypt).Wait();
            Crypto.EncryptAsync(stringToEncrypt, identifier).Wait();

            cryptoMock.Verify(cm => cm.EncryptAsync(It.Is<string>(s => s == stringToEncrypt), null));
            cryptoMock.Verify(cm => cm.EncryptAsync(It.Is<string>(s => s == stringToEncrypt), It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void EncryptAsyncByStringGivenCurrentIsNotIAsyncCryptoCallsCryptoEncryptByString()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var stringToEncrypt = "Something to encrypt";
            var identifier = new object();

            Crypto.EncryptAsync(stringToEncrypt).Wait();
            Crypto.EncryptAsync(stringToEncrypt, identifier).Wait();

            cryptoMock.Verify(cm => cm.Encrypt(It.Is<string>(s => s == stringToEncrypt), null));
            cryptoMock.Verify(cm => cm.Encrypt(It.Is<string>(s => s == stringToEncrypt), It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void EncryptAsyncByByteArrayGivenCurrentIsIAsyncCryptoCallsCryptoEncryptAsyncByByteArray()
        {
            var cryptoMock = new Mock<AbstractAsyncCrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var byteArrayToEncrypt = new byte[0];
            var identifier = new object();

            Crypto.EncryptAsync(byteArrayToEncrypt);
            Crypto.EncryptAsync(byteArrayToEncrypt, identifier);

            cryptoMock.Verify(cm => cm.EncryptAsync(It.Is<byte[]>(s => s == byteArrayToEncrypt), null));
            cryptoMock.Verify(cm => cm.EncryptAsync(It.Is<byte[]>(s => s == byteArrayToEncrypt), It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void EncryptAsyncByByteArrayGivenCurrentIsNotIAsyncCryptoCallsCryptoEncryptByByteArray()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var byteArrayToEncrypt = new byte[0];
            var identifier = new object();

            Crypto.EncryptAsync(byteArrayToEncrypt);
            Crypto.EncryptAsync(byteArrayToEncrypt, identifier);

            cryptoMock.Verify(cm => cm.Encrypt(It.Is<byte[]>(s => s == byteArrayToEncrypt), null));
            cryptoMock.Verify(cm => cm.Encrypt(It.Is<byte[]>(s => s == byteArrayToEncrypt), It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void DecryptAsyncByStringGivenCurrentIsIAsyncCryptoCallsCryptoDecryptAsyncByString()
        {
            var cryptoMock = new Mock<AbstractAsyncCrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var stringToDecrypt = "Something to encrypt";
            var identifier = new object();

            Crypto.DecryptAsync(stringToDecrypt).Wait();
            Crypto.DecryptAsync(stringToDecrypt, identifier).Wait();

            cryptoMock.Verify(cm => cm.DecryptAsync(It.Is<string>(s => s == stringToDecrypt), null));
            cryptoMock.Verify(cm => cm.DecryptAsync(It.Is<string>(s => s == stringToDecrypt), It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void DecryptAsyncByStringGivenCurrentIsNotIAsyncCryptoCallsCryptoDecryptByString()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var stringToDecrypt = "Something to encrypt";
            var identifier = new object();

            Crypto.DecryptAsync(stringToDecrypt).Wait();
            Crypto.DecryptAsync(stringToDecrypt, identifier).Wait();

            cryptoMock.Verify(cm => cm.Decrypt(It.Is<string>(s => s == stringToDecrypt), null));
            cryptoMock.Verify(cm => cm.Decrypt(It.Is<string>(s => s == stringToDecrypt), It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void DecryptAsyncByByteArrayGivenCurrentIsIAsyncCryptoCallsCryptoDecryptAsyncByByteArray()
        {
            var cryptoMock = new Mock<AbstractAsyncCrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var byteArrayToDecrypt = new byte[0];
            var identifier = new object();

            Crypto.DecryptAsync(byteArrayToDecrypt).Wait();
            Crypto.DecryptAsync(byteArrayToDecrypt, identifier).Wait();

            cryptoMock.Verify(cm => cm.DecryptAsync(It.Is<byte[]>(s => s == byteArrayToDecrypt), null));
            cryptoMock.Verify(cm => cm.DecryptAsync(It.Is<byte[]>(s => s == byteArrayToDecrypt), It.Is<object>(o => o == identifier)));
        }

        [Test]
        public void DecryptAsyncByByteArrayGivenCurrentIsNotIAsyncCryptoCallsCryptoDecryptByByteArray()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var byteArrayToDecrypt = new byte[0];
            var identifier = new object();

            Crypto.DecryptAsync(byteArrayToDecrypt).Wait();
            Crypto.DecryptAsync(byteArrayToDecrypt, identifier).Wait();

            cryptoMock.Verify(cm => cm.Decrypt(It.Is<byte[]>(s => s == byteArrayToDecrypt), null));
            cryptoMock.Verify(cm => cm.Decrypt(It.Is<byte[]>(s => s == byteArrayToDecrypt), It.Is<object>(o => o == identifier)));
        }

        private static void ResetConfig()
        {
            var rootField = typeof(Config).GetField("_root", BindingFlags.NonPublic | BindingFlags.Static);
            var root = (Semimutable<IConfigurationRoot>)rootField.GetValue(null);
            root.GetUnlockValueMethod().Invoke(root, null);
        }
        private static void ResetCrypto()
        {
            var currentField = typeof(Crypto).GetField("_current", BindingFlags.NonPublic | BindingFlags.Static);
            var current = (Semimutable<ICrypto>)currentField.GetValue(null);
            current.GetUnlockValueMethod().Invoke(current, null);
        }

        public abstract class AbstractAsyncCrypto : ICrypto, IAsyncCrypto
        {
            public abstract bool CanDecrypt(object keyIdentifier);
            public abstract bool CanEncrypt(object keyIdentifier);
            public abstract string Decrypt(string cipherText, object keyIdentifier);
            public abstract byte[] Decrypt(byte[] cipherText, object keyIdentifier);
            public abstract Task<string> DecryptAsync(string cipherText, object keyIdentifier);
            public abstract Task<byte[]> DecryptAsync(byte[] cipherText, object keyIdentifier);
            public abstract string Encrypt(string plainText, object keyIdentifier);
            public abstract byte[] Encrypt(byte[] plainText, object keyIdentifier);
            public abstract Task<string> EncryptAsync(string plainText, object keyIdentifier);
            public abstract Task<byte[]> EncryptAsync(byte[] plainText, object keyIdentifier);
            public abstract IDecryptor GetDecryptor(object keyIdentifier);
            public abstract Task<IAsyncDecryptor> GetDecryptorAsync(object keyIdentifier);
            public abstract IEncryptor GetEncryptor(object keyIdentifier);
            public abstract Task<IAsyncEncryptor> GetEncryptorAsync(object keyIdentifier);
        }
    }
}
