using FluentAssertions;
using Moq;
using RockLib.Encryption;
using RockLib.Encryption.Async;
using System.Text;
using System.Threading;
using Xunit;

namespace RockLib.Decryption.Tests.Async
{
    public class SynchronousAsyncDecryptorTests
    {
        private Mock<IDecryptor> _decryptorMock;

        private void Setup(string cryptoId)
        {
            _decryptorMock = new Mock<IDecryptor>();

            _decryptorMock.Setup(f => f.Decrypt(It.IsAny<string>())).Returns($"DecryptedString : {cryptoId}");
            _decryptorMock.Setup(f => f.Decrypt(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes(cryptoId));
        }


        [Fact]
        public void DecryptAsync_string_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncDecryptor = new SynchronousAsyncDecryptor(_decryptorMock.Object);

            var decryptTask = asyncDecryptor.DecryptAsync("stuff", default(CancellationToken));

            decryptTask.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public void DecryptAsync_string_ReturnsTheResultReturnedByCryptoDecrypt()
        {
            Setup("foo");

            var asyncDecryptor = new SynchronousAsyncDecryptor(_decryptorMock.Object);

            var decrypted = asyncDecryptor.DecryptAsync("stuff", default(CancellationToken)).Result;

            decrypted.Should().Be("DecryptedString : foo");
        }

        [Fact]
        public void DecryptAsync_bytearray_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncDecryptor = new SynchronousAsyncDecryptor(_decryptorMock.Object);

            var decryptTask = asyncDecryptor.DecryptAsync(new byte[0], default(CancellationToken));

            decryptTask.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public void DecryptAsync_bytearray_ReturnsTheResultReturnedByCryptoDecrypt()
        {
            Setup("foo");

            var asyncDecryptor = new SynchronousAsyncDecryptor(_decryptorMock.Object);

            var decrypted = asyncDecryptor.DecryptAsync(new byte[0], default(CancellationToken)).Result;

            decrypted.Should().BeEquivalentTo(Encoding.UTF8.GetBytes("foo"));
        }
    }
}
