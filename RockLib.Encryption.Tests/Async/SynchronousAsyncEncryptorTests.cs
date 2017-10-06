using Moq;
using NUnit.Framework;
using RockLib.Encryption.Async;
using System.Text;
using System.Threading;

namespace RockLib.Encryption.Tests.Async
{
    [TestFixture]
    public class SynchronousAsyncEncryptorTests
    {
        private Mock<IEncryptor> _encryptorMock;

        private void Setup(string cryptoId)
        {
            _encryptorMock = new Mock<IEncryptor>();

            _encryptorMock.Setup(f => f.Encrypt(It.IsAny<string>())).Returns($"EncryptedString : {cryptoId}");
            _encryptorMock.Setup(f => f.Encrypt(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes(cryptoId));
        }


        [Test]
        public void EncryptAsync_string_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncEncryptor = new SynchronousAsyncEncryptor(_encryptorMock.Object);

            var encryptTask = asyncEncryptor.EncryptAsync("stuff", default(CancellationToken));

            Assert.That(encryptTask.IsCompleted, Is.True);
        }

        [Test]
        public void EncryptAsync_string_ReturnsTheResultReturnedByCryptoEncrypt()
        {
            Setup("foo");

            var asyncEncryptor = new SynchronousAsyncEncryptor(_encryptorMock.Object);

            var encrypted = asyncEncryptor.EncryptAsync("stuff", default(CancellationToken)).Result;

            Assert.That(encrypted, Is.EqualTo("EncryptedString : foo"));
        }

        [Test]
        public void EncryptAsync_bytearray_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncEncryptor = new SynchronousAsyncEncryptor(_encryptorMock.Object);

            var encryptTask = asyncEncryptor.EncryptAsync(new byte[0], default(CancellationToken));

            Assert.That(encryptTask.IsCompleted, Is.True);
        }

        [Test]
        public void EncryptAsync_bytearray_ReturnsTheResultReturnedByCryptoEncrypt()
        {
            Setup("foo");

            var asyncEncryptor = new SynchronousAsyncEncryptor(_encryptorMock.Object);

            var encrypted = asyncEncryptor.EncryptAsync(new byte[0], default(CancellationToken)).Result;

            Assert.That(encrypted, Is.EqualTo(Encoding.UTF8.GetBytes("foo")));
        }
    }
}
