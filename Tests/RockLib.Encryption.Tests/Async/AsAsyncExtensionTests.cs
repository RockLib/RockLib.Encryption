using FluentAssertions;
using Moq;
using RockLib.Encryption.Async;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RockLib.Encryption.Tests.Async
{
    public class AsAsyncExtensionTests
    {
        [Fact]
        public void AsAsync_GivenAnObjectThatImplementsIAsyncCryptoTheSameObjectIsReturned()
        {
            var crypto = new TestCrypto();

            var asyncCrypto = crypto.AsAsync();

            asyncCrypto.Should().BeSameAs(crypto);
        }

        [Fact]
        public void AsAsync_GivenAnObjectThatDoesNotImplementsIAsyncCryptoASynchronousAsyncCryptoIsReturned()
        {
            var crypto = new Mock<ICrypto>().Object;

            var asyncCrypto = crypto.AsAsync();

            asyncCrypto.Should().BeOfType<SynchronousAsyncCrypto>();
        }

        [Fact]
        public void AsAsync_GivenAnObjectThatDoesNotImplementsIAsyncCryptoTheSynchronousAsyncCryptoUsesTheOriginalICrypto()
        {
            var crypto = new Mock<ICrypto>().Object;

            var asyncCrypto = (SynchronousAsyncCrypto)crypto.AsAsync();

            asyncCrypto.Crypto.Should().BeSameAs(crypto);
        }

        [Fact]
        public void AsAsync_MultipleCallsWithTheSameObjectThatDoesNotImplementIAsyncCryptoReturnTheSameObjectEachTime()
        {
            var crypto = new Mock<ICrypto>().Object;

            var asyncCrypto1 = crypto.AsAsync();
            var asyncCrypto2 = crypto.AsAsync();

            asyncCrypto1.Should().BeSameAs(asyncCrypto2);
        }

        private class TestCrypto : ICrypto, IAsyncCrypto
        {
            public bool CanDecrypt(string credentialName) => throw new NotImplementedException();
            public bool CanEncrypt(string credentialName) => throw new NotImplementedException();
            public string Decrypt(string cipherText, string credentialName) => throw new NotImplementedException();
            public byte[] Decrypt(byte[] cipherText, string credentialName) => throw new NotImplementedException();
            public Task<string> DecryptAsync(string cipherText, string credentialName, CancellationToken cancellationToken) => throw new NotImplementedException();
            public Task<byte[]> DecryptAsync(byte[] cipherText, string credentialName, CancellationToken cancellationToken) => throw new NotImplementedException();
            public string Encrypt(string plainText, string credentialName) => throw new NotImplementedException();
            public byte[] Encrypt(byte[] plainText, string credentialName) => throw new NotImplementedException();
            public Task<string> EncryptAsync(string plainText, string credentialName, CancellationToken cancellationToken) => throw new NotImplementedException();
            public Task<byte[]> EncryptAsync(byte[] plainText, string credentialName, CancellationToken cancellationToken) => throw new NotImplementedException();
            public IDecryptor GetDecryptor(string credentialName) => throw new NotImplementedException();
            public IAsyncDecryptor GetAsyncDecryptor(string credentialName) => throw new NotImplementedException();
            public IEncryptor GetEncryptor(string credentialName) => throw new NotImplementedException();
            public IAsyncEncryptor GetAsyncEncryptor(string credentialName) => throw new NotImplementedException();
        }
    }
}
