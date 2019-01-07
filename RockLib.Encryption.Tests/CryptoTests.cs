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
using System.Threading;

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
            
            var credentialName = "foo";

            Crypto.GetEncryptor();
            Crypto.GetEncryptor(credentialName);

            cryptoMock.Verify(cm => cm.GetEncryptor(null));
            cryptoMock.Verify(cm => cm.GetEncryptor(It.Is<string>(o => o == credentialName)));
        }

        [Test]
        public void GetDecryptorCallsCryptoGetDecryptor()
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

        [Test]
        public void EncryptByStringCallsCryptoEncryptByString()
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

        [Test]
        public void EncryptByByteArrayCallsCryptoEncryptByByteArray()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var byteArrayToEncrypt = new byte[0];
            var credentialName = "foo";

            Crypto.Encrypt(byteArrayToEncrypt);
            Crypto.Encrypt(byteArrayToEncrypt, credentialName);

            cryptoMock.Verify(cm => cm.Encrypt(It.Is<byte[]>(s => s == byteArrayToEncrypt), null));
            cryptoMock.Verify(cm => cm.Encrypt(It.Is<byte[]>(s => s == byteArrayToEncrypt), It.Is<string>(o => o == credentialName)));
        }

        [Test]
        public void DecryptByStringCallsCryptoDecryptByString()
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

        [Test]
        public void DecryptByByteArrayCallsCryptoDecryptByByteArray()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var byteArrayToDecrypt = new byte[0];
            var credentialName = "foo";

            Crypto.Decrypt(byteArrayToDecrypt);
            Crypto.Decrypt(byteArrayToDecrypt, credentialName);

            cryptoMock.Verify(cm => cm.Decrypt(It.Is<byte[]>(s => s == byteArrayToDecrypt), null));
            cryptoMock.Verify(cm => cm.Decrypt(It.Is<byte[]>(s => s == byteArrayToDecrypt), It.Is<string>(o => o == credentialName)));
        }

        [Test]
        public void GetEncryptorAsyncGivenCurrentIsIAsyncCryptoCallsCryptoGetEncryptorAsync()
        {
            var cryptoMock = new Mock<AbstractAsyncCrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);
            
            var credentialName = "foo";

            Crypto.GetAsyncEncryptor();
            Crypto.GetAsyncEncryptor(credentialName);

            cryptoMock.Verify(cm => cm.GetAsyncEncryptor(null));
            cryptoMock.Verify(cm => cm.GetAsyncEncryptor(It.Is<string>(o => o == credentialName)));
        }

        [Test]
        public void GetEncryptorAsyncGivenCurrentIsNotIAsyncCryptoCallsCryptoGetEncryptor()
        {
            var cryptoMock = new Mock<ICrypto>();
            cryptoMock.Setup(cm => cm.GetEncryptor(It.IsAny<string>())).Returns(new Mock<IEncryptor>().Object);

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var credentialName = "foo";

            Crypto.GetAsyncEncryptor();
            Crypto.GetAsyncEncryptor(credentialName);

            cryptoMock.Verify(cm => cm.GetEncryptor(null));
            cryptoMock.Verify(cm => cm.GetEncryptor(It.Is<string>(o => o == credentialName)));
        }

        [Test]
        public void GetDecryptorAsyncGivenCurrentIsIAsyncCryptoCallsCryptoGetDecryptorAsync()
        {
            var cryptoMock = new Mock<AbstractAsyncCrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var credentialName = "foo";

            Crypto.GetAsyncDecryptor();
            Crypto.GetAsyncDecryptor(credentialName);

            cryptoMock.Verify(cm => cm.GetAsyncDecryptor(null));
            cryptoMock.Verify(cm => cm.GetAsyncDecryptor(It.Is<string>(o => o == credentialName)));
        }

        [Test]
        public void GetDecryptorAsyncGivenCurrentIsNotIAsyncCryptoCallsCryptoGetDecryptor()
        {
            var cryptoMock = new Mock<ICrypto>();
            cryptoMock.Setup(cm => cm.GetDecryptor(It.IsAny<string>())).Returns(new Mock<IDecryptor>().Object);

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var credentialName = "foo";

            Crypto.GetAsyncDecryptor();
            Crypto.GetAsyncDecryptor(credentialName);

            cryptoMock.Verify(cm => cm.GetDecryptor(null));
            cryptoMock.Verify(cm => cm.GetDecryptor(It.Is<string>(o => o == credentialName)));
        }

        [Test]
        public void EncryptAsyncByStringGivenCurrentIsIAsyncCryptoCallsCryptoEncryptAsyncByString()
        {
            var cryptoMock = new Mock<AbstractAsyncCrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var stringToEncrypt = "Something to encrypt";
            var credentialName = "foo";

            Crypto.EncryptAsync(stringToEncrypt).Wait();
            Crypto.EncryptAsync(stringToEncrypt, credentialName).Wait();

            cryptoMock.Verify(cm => cm.EncryptAsync(It.Is<string>(s => s == stringToEncrypt), null, It.IsAny<CancellationToken>()));
            cryptoMock.Verify(cm => cm.EncryptAsync(It.Is<string>(s => s == stringToEncrypt), It.Is<string>(o => o == credentialName), It.IsAny<CancellationToken>()));
        }

        [Test]
        public void EncryptAsyncByStringGivenCurrentIsNotIAsyncCryptoCallsCryptoEncryptByString()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var stringToEncrypt = "Something to encrypt";
            var credentialName = "foo";

            Crypto.EncryptAsync(stringToEncrypt).Wait();
            Crypto.EncryptAsync(stringToEncrypt, credentialName).Wait();

            cryptoMock.Verify(cm => cm.Encrypt(It.Is<string>(s => s == stringToEncrypt), null));
            cryptoMock.Verify(cm => cm.Encrypt(It.Is<string>(s => s == stringToEncrypt), It.Is<string>(o => o == credentialName)));
        }

        [Test]
        public void EncryptAsyncByByteArrayGivenCurrentIsIAsyncCryptoCallsCryptoEncryptAsyncByByteArray()
        {
            var cryptoMock = new Mock<AbstractAsyncCrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var byteArrayToEncrypt = new byte[0];
            var credentialName = "foo";

            Crypto.EncryptAsync(byteArrayToEncrypt);
            Crypto.EncryptAsync(byteArrayToEncrypt, credentialName);

            cryptoMock.Verify(cm => cm.EncryptAsync(It.Is<byte[]>(s => s == byteArrayToEncrypt), null, It.IsAny<CancellationToken>()));
            cryptoMock.Verify(cm => cm.EncryptAsync(It.Is<byte[]>(s => s == byteArrayToEncrypt), It.Is<string>(o => o == credentialName), It.IsAny<CancellationToken>()));
        }

        [Test]
        public void EncryptAsyncByByteArrayGivenCurrentIsNotIAsyncCryptoCallsCryptoEncryptByByteArray()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var byteArrayToEncrypt = new byte[0];
            var credentialName = "foo";

            Crypto.EncryptAsync(byteArrayToEncrypt);
            Crypto.EncryptAsync(byteArrayToEncrypt, credentialName);

            cryptoMock.Verify(cm => cm.Encrypt(It.Is<byte[]>(s => s == byteArrayToEncrypt), null));
            cryptoMock.Verify(cm => cm.Encrypt(It.Is<byte[]>(s => s == byteArrayToEncrypt), It.Is<string>(o => o == credentialName)));
        }

        [Test]
        public void DecryptAsyncByStringGivenCurrentIsIAsyncCryptoCallsCryptoDecryptAsyncByString()
        {
            var cryptoMock = new Mock<AbstractAsyncCrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var stringToDecrypt = "Something to encrypt";
            var credentialName = "foo";

            Crypto.DecryptAsync(stringToDecrypt).Wait();
            Crypto.DecryptAsync(stringToDecrypt, credentialName).Wait();

            cryptoMock.Verify(cm => cm.DecryptAsync(It.Is<string>(s => s == stringToDecrypt), null, It.IsAny<CancellationToken>()));
            cryptoMock.Verify(cm => cm.DecryptAsync(It.Is<string>(s => s == stringToDecrypt), It.Is<string>(o => o == credentialName), It.IsAny<CancellationToken>()));
        }

        [Test]
        public void DecryptAsyncByStringGivenCurrentIsNotIAsyncCryptoCallsCryptoDecryptByString()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var stringToDecrypt = "Something to encrypt";
            var credentialName = "foo";

            Crypto.DecryptAsync(stringToDecrypt).Wait();
            Crypto.DecryptAsync(stringToDecrypt, credentialName).Wait();

            cryptoMock.Verify(cm => cm.Decrypt(It.Is<string>(s => s == stringToDecrypt), null));
            cryptoMock.Verify(cm => cm.Decrypt(It.Is<string>(s => s == stringToDecrypt), It.Is<string>(o => o == credentialName)));
        }

        [Test]
        public void DecryptAsyncByByteArrayGivenCurrentIsIAsyncCryptoCallsCryptoDecryptAsyncByByteArray()
        {
            var cryptoMock = new Mock<AbstractAsyncCrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var byteArrayToDecrypt = new byte[0];
            var credentialName = "foo";

            Crypto.DecryptAsync(byteArrayToDecrypt).Wait();
            Crypto.DecryptAsync(byteArrayToDecrypt, credentialName).Wait();

            cryptoMock.Verify(cm => cm.DecryptAsync(It.Is<byte[]>(s => s == byteArrayToDecrypt), null, It.IsAny<CancellationToken>()));
            cryptoMock.Verify(cm => cm.DecryptAsync(It.Is<byte[]>(s => s == byteArrayToDecrypt), It.Is<string>(o => o == credentialName), It.IsAny<CancellationToken>()));
        }

        [Test]
        public void DecryptAsyncByByteArrayGivenCurrentIsNotIAsyncCryptoCallsCryptoDecryptByByteArray()
        {
            var cryptoMock = new Mock<ICrypto>();

            ResetCrypto();
            Crypto.SetCurrent(cryptoMock.Object);

            var byteArrayToDecrypt = new byte[0];
            var credentialName = "foo";

            Crypto.DecryptAsync(byteArrayToDecrypt).Wait();
            Crypto.DecryptAsync(byteArrayToDecrypt, credentialName).Wait();

            cryptoMock.Verify(cm => cm.Decrypt(It.Is<byte[]>(s => s == byteArrayToDecrypt), null));
            cryptoMock.Verify(cm => cm.Decrypt(It.Is<byte[]>(s => s == byteArrayToDecrypt), It.Is<string>(o => o == credentialName)));
        }

        private static void ResetConfig()
        {
            var rootField = typeof(Config).GetField("_root", BindingFlags.NonPublic | BindingFlags.Static);
            var root = (Semimutable<IConfiguration>)rootField.GetValue(null);
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
            public abstract bool CanDecrypt(string credentialName);
            public abstract bool CanEncrypt(string credentialName);
            public abstract string Decrypt(string cipherText, string credentialName);
            public abstract byte[] Decrypt(byte[] cipherText, string credentialName);
            public abstract Task<string> DecryptAsync(string cipherText, string credentialName, CancellationToken cancellationToken);
            public abstract Task<byte[]> DecryptAsync(byte[] cipherText, string credentialName, CancellationToken cancellationToken);
            public abstract string Encrypt(string plainText, string credentialName);
            public abstract byte[] Encrypt(byte[] plainText, string credentialName);
            public abstract Task<string> EncryptAsync(string plainText, string credentialName, CancellationToken cancellationToken);
            public abstract Task<byte[]> EncryptAsync(byte[] plainText, string credentialName, CancellationToken cancellationToken);
            public abstract IDecryptor GetDecryptor(string credentialName);
            public abstract IAsyncDecryptor GetAsyncDecryptor(string credentialName);
            public abstract IEncryptor GetEncryptor(string credentialName);
            public abstract IAsyncEncryptor GetAsyncEncryptor(string credentialName);
        }
    }
}
