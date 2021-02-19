using FluentAssertions;
using Moq;
using RockLib.Encryption.Async;
using System.Text;
using System.Threading;
using Xunit;

namespace RockLib.Encryption.Tests.Async
{
    public class SynchronousAsyncEncryptorTests
    {
        private Mock<IEncryptor> _encryptorMock;

        private void Setup(string cryptoId)
        {
            _encryptorMock = new Mock<IEncryptor>();

            _encryptorMock.Setup(f => f.Encrypt(It.IsAny<string>())).Returns($"EncryptedString : {cryptoId}");
            _encryptorMock.Setup(f => f.Encrypt(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes(cryptoId));
        }


        [Fact]
        public void EncryptAsync_string_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncEncryptor = new SynchronousAsyncEncryptor(_encryptorMock.Object);

            var encryptTask = asyncEncryptor.EncryptAsync("stuff", default(CancellationToken));

            encryptTask.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public void EncryptAsync_string_ReturnsTheResultReturnedByCryptoEncrypt()
        {
            Setup("foo");

            var asyncEncryptor = new SynchronousAsyncEncryptor(_encryptorMock.Object);

            var encrypted = asyncEncryptor.EncryptAsync("stuff", default(CancellationToken)).Result;

            encrypted.Should().Be("EncryptedString : foo");
        }

        [Fact]
        public void EncryptAsync_bytearray_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncEncryptor = new SynchronousAsyncEncryptor(_encryptorMock.Object);

            var encryptTask = asyncEncryptor.EncryptAsync(new byte[0], default(CancellationToken));

            encryptTask.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public void EncryptAsync_bytearray_ReturnsTheResultReturnedByCryptoEncrypt()
        {
            Setup("foo");

            var asyncEncryptor = new SynchronousAsyncEncryptor(_encryptorMock.Object);

            var encrypted = asyncEncryptor.EncryptAsync(new byte[0], default(CancellationToken)).Result;

            encrypted.Should().BeEquivalentTo(Encoding.UTF8.GetBytes("foo"));
        }
    }
}
