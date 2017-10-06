using Moq;
using NUnit.Framework;
using RockLib.Encryption.Async;
using System.Text;

namespace RockLib.Encryption.Tests.Async
{
    [TestFixture]
    public class SynchronousAsyncCryptoTests
    {
        private Mock<ICrypto> _cryptoMock;
        private IEncryptor _encryptor;
        private IDecryptor _decryptor;

        private void Setup(string cryptoId)
        {
            _cryptoMock = new Mock<ICrypto>();
            _encryptor = new Mock<IEncryptor>().Object;
            _decryptor = new Mock<IDecryptor>().Object;

            _cryptoMock.Setup(f => f.CanEncrypt(cryptoId)).Returns(true);
            _cryptoMock.Setup(f => f.CanEncrypt(It.Is<string>(s => s != cryptoId))).Returns(false);
            _cryptoMock.Setup(f => f.Encrypt(It.IsAny<string>(), It.IsAny<string>())).Returns($"EncryptedString : {cryptoId}");
            _cryptoMock.Setup(f => f.Encrypt(It.IsAny<byte[]>(), It.IsAny<string>())).Returns(Encoding.UTF8.GetBytes(cryptoId));
            _cryptoMock.Setup(f => f.GetEncryptor(cryptoId)).Returns(_encryptor);

            _cryptoMock.Setup(f => f.CanDecrypt(cryptoId)).Returns(true);
            _cryptoMock.Setup(f => f.CanDecrypt(It.Is<string>(s => s != cryptoId))).Returns(false);
            _cryptoMock.Setup(f => f.Decrypt(It.IsAny<string>(), It.IsAny<string>())).Returns($"DecryptedString : {cryptoId}");
            _cryptoMock.Setup(f => f.Decrypt(It.IsAny<byte[]>(), It.IsAny<string>())).Returns(Encoding.UTF8.GetBytes(cryptoId));
            _cryptoMock.Setup(f => f.GetDecryptor(cryptoId)).Returns(_decryptor);
        }

        [Test]
        public void Crypto_IsTheSameInstancePassedToTheConstructor()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            Assert.That(asyncCrypto.Crypto, Is.SameAs(_cryptoMock.Object));
        }

        [Test]
        public void EncryptAsync_string_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encryptTask = asyncCrypto.EncryptAsync("stuff", "foo");

            Assert.That(encryptTask.IsCompleted, Is.True);
        }

        [Test]
        public void EncryptAsync_string_ReturnsTheResultReturnedByCryptoEncrypt()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encrypted = asyncCrypto.EncryptAsync("stuff", "foo").Result;

            Assert.That(encrypted, Is.EqualTo("EncryptedString : foo"));
        }

        [Test]
        public void EncryptAsync_bytearray_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encryptTask = asyncCrypto.EncryptAsync(new byte[0], "foo");

            Assert.That(encryptTask.IsCompleted, Is.True);
        }

        [Test]
        public void EncryptAsync_bytearray_ReturnsTheResultReturnedByCryptoEncrypt()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encrypted = asyncCrypto.EncryptAsync(new byte[0], "foo").Result;

            Assert.That(encrypted, Is.EqualTo(Encoding.UTF8.GetBytes("foo")));
        }

        [Test]
        public void DecryptAsync_string_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encryptTask = asyncCrypto.DecryptAsync("stuff", "foo");

            Assert.That(encryptTask.IsCompleted, Is.True);
        }

        [Test]
        public void DecryptAsync_string_ReturnsTheResultReturnedByCryptoEncrypt()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encrypted = asyncCrypto.DecryptAsync("stuff", "foo").Result;

            Assert.That(encrypted, Is.EqualTo("DecryptedString : foo"));
        }

        [Test]
        public void DecryptAsync_bytearray_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encryptTask = asyncCrypto.DecryptAsync(new byte[0], "foo");

            Assert.That(encryptTask.IsCompleted, Is.True);
        }

        [Test]
        public void DecryptAsync_bytearray_ReturnsTheResultReturnedByCryptoEncrypt()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encrypted = asyncCrypto.DecryptAsync(new byte[0], "foo").Result;

            Assert.That(encrypted, Is.EqualTo(Encoding.UTF8.GetBytes("foo")));
        }

        [Test]
        public void GetEncryptorAsync_ReturnsASynchronousAsyncEncryptor()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encryptor = asyncCrypto.GetAsyncEncryptor("foo");

            Assert.That(encryptor, Is.InstanceOf<SynchronousAsyncEncryptor>());
        }

        [Test]
        public void GetEncryptorAsync_ReturnsASynchronousAsyncEncryptorWhoseEncryptorIsTheOneReturnedByACallToTheCryptoGetEncryptorMethod()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encryptor = (SynchronousAsyncEncryptor)asyncCrypto.GetAsyncEncryptor("foo");

            Assert.That(encryptor.Encryptor, Is.SameAs(_encryptor));
        }

        [Test]
        public void GetDecryptorAsync_ReturnsASynchronousAsyncDecryptor()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var decryptor = asyncCrypto.GetAsyncDecryptor("foo");

            Assert.That(decryptor, Is.InstanceOf<SynchronousAsyncDecryptor>());
        }

        [Test]
        public void GetDecryptorAsync_ReturnsASynchronousAsyncDecryptorWhoseDecryptorIsTheOneReturnedByACallToTheCryptoGetDecryptorMethod()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var decryptor = (SynchronousAsyncDecryptor)asyncCrypto.GetAsyncDecryptor("foo");

            Assert.That(decryptor.Decryptor, Is.SameAs(_decryptor));
        }

        [Test]
        public void CanEncrypt_ReturnsTheSameThingAsACallToCryptoCanEncrypt()
        {
            Setup("foo");

            Assume.That(_cryptoMock.Object.CanEncrypt("foo"), Is.True);
            Assume.That(_cryptoMock.Object.CanEncrypt("bar"), Is.False);

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            Assert.That(asyncCrypto.CanEncrypt("foo"), Is.True);
            Assert.That(asyncCrypto.CanEncrypt("bar"), Is.False);
        }

        [Test]
        public void CanDecrypt_ReturnsTheSameThingAsACallToCryptoCanDecrypt()
        {
            Setup("foo");

            Assume.That(_cryptoMock.Object.CanDecrypt("foo"), Is.True);
            Assume.That(_cryptoMock.Object.CanDecrypt("bar"), Is.False);

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            Assert.That(asyncCrypto.CanDecrypt("foo"), Is.True);
            Assert.That(asyncCrypto.CanDecrypt("bar"), Is.False);
        }
    }
}
