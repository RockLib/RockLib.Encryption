using Moq;
using NUnit.Framework;
using RockLib.Encryption;
using RockLib.Encryption.Async;
using System.Text;
using System.Threading;

namespace RockLib.Decryption.Tests.Async
{
    [TestFixture]
    public class SynchronousAsyncDecryptorTests
    {
        private Mock<IDecryptor> _decryptorMock;

        private void Setup(string cryptoId)
        {
            _decryptorMock = new Mock<IDecryptor>();

            _decryptorMock.Setup(f => f.Decrypt(It.IsAny<string>())).Returns($"DecryptedString : {cryptoId}");
            _decryptorMock.Setup(f => f.Decrypt(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes(cryptoId));
        }


        [Test]
        public void DecryptAsync_string_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncDecryptor = new SynchronousAsyncDecryptor(_decryptorMock.Object);

            var decryptTask = asyncDecryptor.DecryptAsync("stuff", default(CancellationToken));

            Assert.That(decryptTask.IsCompleted, Is.True);
        }

        [Test]
        public void DecryptAsync_string_ReturnsTheResultReturnedByCryptoDecrypt()
        {
            Setup("foo");

            var asyncDecryptor = new SynchronousAsyncDecryptor(_decryptorMock.Object);

            var decrypted = asyncDecryptor.DecryptAsync("stuff", default(CancellationToken)).Result;

            Assert.That(decrypted, Is.EqualTo("DecryptedString : foo"));
        }

        [Test]
        public void DecryptAsync_bytearray_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncDecryptor = new SynchronousAsyncDecryptor(_decryptorMock.Object);

            var decryptTask = asyncDecryptor.DecryptAsync(new byte[0], default(CancellationToken));

            Assert.That(decryptTask.IsCompleted, Is.True);
        }

        [Test]
        public void DecryptAsync_bytearray_ReturnsTheResultReturnedByCryptoDecrypt()
        {
            Setup("foo");

            var asyncDecryptor = new SynchronousAsyncDecryptor(_decryptorMock.Object);

            var decrypted = asyncDecryptor.DecryptAsync(new byte[0], default(CancellationToken)).Result;

            Assert.That(decrypted, Is.EqualTo(Encoding.UTF8.GetBytes("foo")));
        }
    }
}
