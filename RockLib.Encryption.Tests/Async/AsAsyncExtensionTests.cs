using Moq;
using NUnit.Framework;
using RockLib.Encryption.Async;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RockLib.Encryption.Tests.Async
{
    [TestFixture]
    public class AsAsyncExtensionTests
    {
        [Test]
        public void AsAsync_GivenAnObjectThatImplementsIAsyncCryptoTheSameObjectIsReturned()
        {
            var crypto = new TestCrypto();

            var asyncCrypto = crypto.AsAsync();

            Assert.That(asyncCrypto, Is.SameAs(crypto));
        }

        [Test]
        public void AsAsync_GivenAnObjectThatDoesNotImplementsIAsyncCryptoASynchronousAsyncCryptoIsReturned()
        {
            var crypto = new Mock<ICrypto>().Object;

            var asyncCrypto = crypto.AsAsync();

            Assert.That(asyncCrypto, Is.InstanceOf<SynchronousAsyncCrypto>());
        }

        [Test]
        public void AsAsync_GivenAnObjectThatDoesNotImplementsIAsyncCryptoTheSynchronousAsyncCryptoUsesTheOriginalICrypto()
        {
            var crypto = new Mock<ICrypto>().Object;

            var asyncCrypto = (SynchronousAsyncCrypto)crypto.AsAsync();

            Assert.That(asyncCrypto.Crypto, Is.SameAs(crypto));
        }

        [Test]
        public void AsAsync_MultipleCallsWithTheSameObjectThatDoesNotImplementIAsyncCryptoReturnTheSameObjectEachTime()
        {
            var crypto = new Mock<ICrypto>().Object;

            var asyncCrypto1 = crypto.AsAsync();
            var asyncCrypto2 = crypto.AsAsync();

            Assert.That(asyncCrypto1, Is.SameAs(asyncCrypto2));
        }

        private class TestCrypto : ICrypto, IAsyncCrypto
        {
            public bool CanDecrypt(object keyIdentifier) => throw new NotImplementedException();
            public bool CanEncrypt(object keyIdentifier) => throw new NotImplementedException();
            public string Decrypt(string cipherText, object keyIdentifier) => throw new NotImplementedException();
            public byte[] Decrypt(byte[] cipherText, object keyIdentifier) => throw new NotImplementedException();
            public Task<string> DecryptAsync(string cipherText, object keyIdentifier) => throw new NotImplementedException();
            public Task<byte[]> DecryptAsync(byte[] cipherText, object keyIdentifier) => throw new NotImplementedException();
            public string Encrypt(string plainText, object keyIdentifier) => throw new NotImplementedException();
            public byte[] Encrypt(byte[] plainText, object keyIdentifier) => throw new NotImplementedException();
            public Task<string> EncryptAsync(string plainText, object keyIdentifier) => throw new NotImplementedException();
            public Task<byte[]> EncryptAsync(byte[] plainText, object keyIdentifier) => throw new NotImplementedException();
            public IDecryptor GetDecryptor(object keyIdentifier) => throw new NotImplementedException();
            public Task<IAsyncDecryptor> GetDecryptorAsync(object keyIdentifier) => throw new NotImplementedException();
            public IEncryptor GetEncryptor(object keyIdentifier) => throw new NotImplementedException();
            public Task<IAsyncEncryptor> GetEncryptorAsync(object keyIdentifier) => throw new NotImplementedException();
        }
    }
}
