using FluentAssertions;
using Moq;
using NUnit.Framework;
using XSerializer;

namespace RockLib.Encryption.XSerializer.Tests
{
    [TestFixture]
    public class CryptoEncryptionMechanismTests
    {
        [Test]
        public void CryptoPropertyIsSetFromConstructorParameter()
        {
            var mockCrypto = new Mock<ICrypto>();

            var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

            encryptionMechanism.Crypto.Should().BeSameAs(mockCrypto.Object);
        }

        [Test]
        public void EncryptCallsCryptoGetEncryptorWhenSerializationStateIsEmpty()
        {
            var mockCrypto = new Mock<ICrypto>();
            var mockEncryptor = new Mock<IEncryptor>();

            mockCrypto.Setup(c => c.GetEncryptor(It.IsAny<object>())).Returns(() => mockEncryptor.Object);

            var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

            var keyIdentifier = new object();
            var serializationState = new SerializationState();

            encryptionMechanism.Encrypt("foo", keyIdentifier, serializationState);

            mockCrypto.Verify(c => c.GetEncryptor(It.Is<object>(obj => obj == keyIdentifier)), Times.Once());
        }

        [Test]
        public void GetEncryptorIsNotCalledWhenSerializationStateIsNotEmpty()
        {
            var mockCrypto = new Mock<ICrypto>();
            var mockEncryptor = new Mock<IEncryptor>();

            mockCrypto.Setup(c => c.GetEncryptor(It.IsAny<object>())).Returns(() => mockEncryptor.Object);

            var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

            var keyIdentifier = new object();
            var serializationState = new SerializationState();

            // Force the mock encryptor to be cached in the serialization state.
            serializationState.Get(() => mockEncryptor.Object);

            encryptionMechanism.Encrypt("foo", keyIdentifier, serializationState);

            mockCrypto.Verify(c => c.GetEncryptor(It.Is<object>(obj => obj == keyIdentifier)), Times.Never());
        }

        [Test]
        public void TheCachedEncryptorReturnsTheReturnValue()
        {
            var mockCrypto = new Mock<ICrypto>();
            var mockEncryptor = new Mock<IEncryptor>();

            mockCrypto.Setup(c => c.GetEncryptor(It.IsAny<object>())).Returns(() => mockEncryptor.Object);
            mockEncryptor.Setup(e => e.Encrypt(It.IsAny<string>())).Returns("bar");

            var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

            var keyIdentifier = new object();
            var serializationState = new SerializationState();

            // Force the mock encryptor to be cached in the serialization state.
            serializationState.Get(() => mockEncryptor.Object);

            var encrypted = encryptionMechanism.Encrypt("foo", keyIdentifier, serializationState);

            encrypted.Should().Be("bar");
        }

        [Test]
        public void DecryptCallsCryptoGetDecryptorWhenSerializationStateIsEmpty()
        {
            var mockCrypto = new Mock<ICrypto>();
            var mockDecryptor = new Mock<IDecryptor>();

            mockCrypto.Setup(c => c.GetDecryptor(It.IsAny<object>())).Returns(() => mockDecryptor.Object);

            var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

            var keyIdentifier = new object();
            var serializationState = new SerializationState();

            encryptionMechanism.Decrypt("foo", keyIdentifier, serializationState);

            mockCrypto.Verify(c => c.GetDecryptor(It.Is<object>(obj => obj == keyIdentifier)), Times.Once());
        }

        [Test]
        public void GetDecryptorIsNotCalledWhenSerializationStateIsNotEmpty()
        {
            var mockCrypto = new Mock<ICrypto>();
            var mockDecryptor = new Mock<IDecryptor>();

            mockCrypto.Setup(c => c.GetDecryptor(It.IsAny<object>())).Returns(() => mockDecryptor.Object);

            var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

            var keyIdentifier = new object();
            var serializationState = new SerializationState();

            // Force the mock decryptor to be cached in the serialization state.
            serializationState.Get(() => mockDecryptor.Object);

            encryptionMechanism.Decrypt("foo", keyIdentifier, serializationState);

            mockCrypto.Verify(c => c.GetDecryptor(It.Is<object>(obj => obj == keyIdentifier)), Times.Never());
        }

        [Test]
        public void TheCachedDecryptorReturnsTheReturnValue()
        {
            var mockCrypto = new Mock<ICrypto>();
            var mockDecryptor = new Mock<IDecryptor>();

            mockCrypto.Setup(c => c.GetDecryptor(It.IsAny<object>())).Returns(() => mockDecryptor.Object);
            mockDecryptor.Setup(e => e.Decrypt(It.IsAny<string>())).Returns("bar");

            var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

            var keyIdentifier = new object();
            var serializationState = new SerializationState();

            // Force the mock decryptor to be cached in the serialization state.
            serializationState.Get(() => mockDecryptor.Object);

            var decrypted = encryptionMechanism.Decrypt("foo", keyIdentifier, serializationState);

            decrypted.Should().Be("bar");
        }
    }
}
