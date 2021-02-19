using FluentAssertions;
using Moq;
using RockLib.Encryption.Async;
using System.Text;
using System.Threading;
using Xunit;

namespace RockLib.Encryption.Tests.Async
{
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

        [Fact]
        public void Crypto_IsTheSameInstancePassedToTheConstructor()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            asyncCrypto.Crypto.Should().BeSameAs(_cryptoMock.Object);
        }

        [Fact]
        public void EncryptAsync_string_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encryptTask = asyncCrypto.EncryptAsync("stuff", "foo", default(CancellationToken));

            encryptTask.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public void EncryptAsync_string_ReturnsTheResultReturnedByCryptoEncrypt()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encrypted = asyncCrypto.EncryptAsync("stuff", "foo", default(CancellationToken)).Result;

            encrypted.Should().Be("EncryptedString : foo");
        }

        [Fact]
        public void EncryptAsync_bytearray_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encryptTask = asyncCrypto.EncryptAsync(new byte[0], "foo", default(CancellationToken));

            encryptTask.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public void EncryptAsync_bytearray_ReturnsTheResultReturnedByCryptoEncrypt()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encrypted = asyncCrypto.EncryptAsync(new byte[0], "foo", default(CancellationToken)).Result;

            encrypted.Should().BeEquivalentTo(Encoding.UTF8.GetBytes("foo"));
        }

        [Fact]
        public void DecryptAsync_string_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encryptTask = asyncCrypto.DecryptAsync("stuff", "foo", default(CancellationToken));

            encryptTask.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public void DecryptAsync_string_ReturnsTheResultReturnedByCryptoEncrypt()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encrypted = asyncCrypto.DecryptAsync("stuff", "foo", default(CancellationToken)).Result;

            encrypted.Should().Be("DecryptedString : foo");
        }

        [Fact]
        public void DecryptAsync_bytearray_ReturnsACompletedTask()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encryptTask = asyncCrypto.DecryptAsync(new byte[0], "foo", default(CancellationToken));

            encryptTask.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public void DecryptAsync_bytearray_ReturnsTheResultReturnedByCryptoEncrypt()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encrypted = asyncCrypto.DecryptAsync(new byte[0], "foo", default(CancellationToken)).Result;

            encrypted.Should().BeEquivalentTo(Encoding.UTF8.GetBytes("foo"));
        }

        [Fact]
        public void GetEncryptorAsync_ReturnsASynchronousAsyncEncryptor()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encryptor = asyncCrypto.GetAsyncEncryptor("foo");

            encryptor.Should().BeOfType<SynchronousAsyncEncryptor>();
        }

        [Fact]
        public void GetEncryptorAsync_ReturnsASynchronousAsyncEncryptorWhoseEncryptorIsTheOneReturnedByACallToTheCryptoGetEncryptorMethod()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var encryptor = (SynchronousAsyncEncryptor)asyncCrypto.GetAsyncEncryptor("foo");

            encryptor.Encryptor.Should().BeSameAs(_encryptor);
        }

        [Fact]
        public void GetDecryptorAsync_ReturnsASynchronousAsyncDecryptor()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var decryptor = asyncCrypto.GetAsyncDecryptor("foo");

            decryptor.Should().BeOfType<SynchronousAsyncDecryptor>();
        }

        [Fact]
        public void GetDecryptorAsync_ReturnsASynchronousAsyncDecryptorWhoseDecryptorIsTheOneReturnedByACallToTheCryptoGetDecryptorMethod()
        {
            Setup("foo");

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            var decryptor = (SynchronousAsyncDecryptor)asyncCrypto.GetAsyncDecryptor("foo");

            decryptor.Decryptor.Should().BeSameAs(_decryptor);
        }

        [Fact]
        public void CanEncrypt_ReturnsTheSameThingAsACallToCryptoCanEncrypt()
        {
            Setup("foo");

            // Assumptions
            _cryptoMock.Object.CanEncrypt("foo").Should().BeTrue();
            _cryptoMock.Object.CanEncrypt("bar").Should().BeFalse();

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            asyncCrypto.CanEncrypt("foo").Should().BeTrue();
            asyncCrypto.CanEncrypt("bar").Should().BeFalse();
        }

        [Fact]
        public void CanDecrypt_ReturnsTheSameThingAsACallToCryptoCanDecrypt()
        {
            Setup("foo");

            // Assumptions
            _cryptoMock.Object.CanDecrypt("foo").Should().BeTrue();
            _cryptoMock.Object.CanDecrypt("bar").Should().BeFalse();

            var asyncCrypto = new SynchronousAsyncCrypto(_cryptoMock.Object);

            asyncCrypto.CanDecrypt("foo").Should().BeTrue();
            asyncCrypto.CanDecrypt("bar").Should().BeFalse();
        }
    }
}
