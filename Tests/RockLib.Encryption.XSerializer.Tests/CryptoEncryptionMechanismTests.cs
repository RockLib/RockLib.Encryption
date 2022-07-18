using FluentAssertions;
using Moq;
using XSerializer;
using Xunit;

namespace RockLib.Encryption.XSerializer.Tests;

public static class CryptoEncryptionMechanismTests
{
    [Fact]
    public static void CryptoPropertyIsSetFromConstructorParameter()
    {
        var mockCrypto = new Mock<ICrypto>();

        var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

        encryptionMechanism.Crypto.Should().BeSameAs(mockCrypto.Object);
    }

    [Fact]
    public static void EncryptCallsCryptoGetEncryptorWhenSerializationStateIsEmpty()
    {
        var mockCrypto = new Mock<ICrypto>();
        var mockEncryptor = new Mock<IEncryptor>();

        mockCrypto.Setup(c => c.GetEncryptor(It.IsAny<string>())).Returns(() => mockEncryptor.Object);

        var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

        var credentialName = "foobar";
        var serializationState = new SerializationState();

        encryptionMechanism.Encrypt("foo", credentialName, serializationState);

        mockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(obj => obj == credentialName)), Times.Once());
    }

    [Fact]
    public static void GetEncryptorIsNotCalledWhenSerializationStateIsNotEmpty()
    {
        var mockCrypto = new Mock<ICrypto>();
        var mockEncryptor = new Mock<IEncryptor>();

        mockCrypto.Setup(c => c.GetEncryptor(It.IsAny<string>())).Returns(() => mockEncryptor.Object);

        var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

        var credentialName = "foobar";
        var serializationState = new SerializationState();

        // Force the mock encryptor to be cached in the serialization state.
        serializationState.Get(() => mockEncryptor.Object);

        encryptionMechanism.Encrypt("foo", credentialName, serializationState);

        mockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(obj => obj == credentialName)), Times.Never());
    }

    [Fact]
    public static void TheCachedEncryptorReturnsTheReturnValue()
    {
        var mockCrypto = new Mock<ICrypto>();
        var mockEncryptor = new Mock<IEncryptor>();

        mockCrypto.Setup(c => c.GetEncryptor(It.IsAny<string>())).Returns(() => mockEncryptor.Object);
        mockEncryptor.Setup(e => e.Encrypt(It.IsAny<string>())).Returns("bar");

        var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

        var credentialName = "foobar";
        var serializationState = new SerializationState();

        // Force the mock encryptor to be cached in the serialization state.
        serializationState.Get(() => mockEncryptor.Object);

        var encrypted = encryptionMechanism.Encrypt("foo", credentialName, serializationState);

        encrypted.Should().Be("bar");
    }

    [Fact]
    public static void DecryptCallsCryptoGetDecryptorWhenSerializationStateIsEmpty()
    {
        var mockCrypto = new Mock<ICrypto>();
        var mockDecryptor = new Mock<IDecryptor>();

        mockCrypto.Setup(c => c.GetDecryptor(It.IsAny<string>())).Returns(() => mockDecryptor.Object);

        var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

        var credentialName = "foobar";
        var serializationState = new SerializationState();

        encryptionMechanism.Decrypt("foo", credentialName, serializationState);

        mockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(obj => obj == credentialName)), Times.Once());
    }

    [Fact]
    public static void GetDecryptorIsNotCalledWhenSerializationStateIsNotEmpty()
    {
        var mockCrypto = new Mock<ICrypto>();
        var mockDecryptor = new Mock<IDecryptor>();

        mockCrypto.Setup(c => c.GetDecryptor(It.IsAny<string>())).Returns(() => mockDecryptor.Object);

        var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

        var credentialName = "foobar";
        var serializationState = new SerializationState();

        // Force the mock decryptor to be cached in the serialization state.
        serializationState.Get(() => mockDecryptor.Object);

        encryptionMechanism.Decrypt("foo", credentialName, serializationState);

        mockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(obj => obj == credentialName)), Times.Never());
    }

    [Fact]
    public static void TheCachedDecryptorReturnsTheReturnValue()
    {
        var mockCrypto = new Mock<ICrypto>();
        var mockDecryptor = new Mock<IDecryptor>();

        mockCrypto.Setup(c => c.GetDecryptor(It.IsAny<string>())).Returns(() => mockDecryptor.Object);
        mockDecryptor.Setup(e => e.Decrypt(It.IsAny<string>())).Returns("bar");

        var encryptionMechanism = new CryptoEncryptionMechanism(mockCrypto.Object);

        var credentialName = "foobar";
        var serializationState = new SerializationState();

        // Force the mock decryptor to be cached in the serialization state.
        serializationState.Get(() => mockDecryptor.Object);

        var decrypted = encryptionMechanism.Decrypt("foo", credentialName, serializationState);

        decrypted.Should().Be("bar");
    }
}
