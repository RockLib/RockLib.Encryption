using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RockLib.Configuration;
using RockLib.DataProtection;
using RockLib.Encryption.Symmetric;

namespace RockLib.Encryption.Tests
{
    [TestFixture]
    public class SymmetricCryptoTests
    {
        [Test]
        public void CanCreateSymmetricCryptoByCredentialRepository()
        {
            var credentialMock = new Mock<ICredential>();
            credentialMock.Setup(cm => cm.Algorithm).Returns(SymmetricAlgorithm.Aes);
            credentialMock.Setup(cm => cm.IVSize).Returns(16);
            credentialMock.Setup(cm => cm.GetKey()).Returns(GetSequentialByteArray(16));

            var credentialRepo = new CredentialRepository(new List<ICredential> { credentialMock.Object });

            var crypto = new SymmetricCrypto(credentialRepo);

            crypto.Should().NotBe(null);
        }

        [Test]
        public void CanEncryptDecryptByCredentialRepository()
        {
            var credentialMock = new Mock<ICredential>();
            credentialMock.Setup(cm => cm.Algorithm).Returns(SymmetricAlgorithm.Aes);
            credentialMock.Setup(cm => cm.IVSize).Returns(16);
            credentialMock.Setup(cm => cm.GetKey()).Returns(GetSequentialByteArray(16));

            var credentialRepo = new CredentialRepository(new List<ICredential> { credentialMock.Object });

            var crypto = new SymmetricCrypto(credentialRepo);

            var plainText = "This is just some random text to encrypt/decrypt";
            var encrypted = crypto.Encrypt(plainText, null);
            var decrypted = crypto.Decrypt(encrypted, null);

            encrypted.Should().NotBe(plainText);
            decrypted.Should().NotBe(encrypted);
            decrypted.Should().Be(plainText);
        }

        [Test]
        public void CanCreateSymmetricCryptoByEncryptionSettings()
        {
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>
            {
                {"Section:Value", "1J9Og/OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="}
            });

            var crypto = new SymmetricCrypto();
            crypto.EncryptionSettings = new CryptoConfiguration
            {
                Credentials = new List<Credential>
                {
                    new Credential
                    {
                        Algorithm = SymmetricAlgorithm.Aes,
                        IVSize = 16,
                        Key = new LateBoundConfigurationSection<IProtectedValue>
                        {
                            Type = "RockLib.DataProtection.UnprotectedBase64Value, RockLib.DataProtection",
                            Value = builder.Build().GetSection("Section")
                        }
                    }
                }
            };

            crypto.Should().NotBe(null);
        }

        [Test]
        public void CanEncryptDecryptByRepoByEncryptionSettings()
        {
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>
            {
                {"Section:Value", "1J9Og/OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="}
            });

            var crypto = new SymmetricCrypto();
            crypto.EncryptionSettings = new CryptoConfiguration
            {
                Credentials = new List<Credential> 
                {
                    new Credential
                    {
                        Algorithm = SymmetricAlgorithm.Aes,
                        IVSize = 16,
                        Key = new LateBoundConfigurationSection<IProtectedValue>
                        {
                            Type = "RockLib.DataProtection.UnprotectedBase64Value, RockLib.DataProtection",
                            Value = builder.Build().GetSection("Section")
                        }
                    }
                }
            };

            var plainText = "This is just some random text to encrypt/decrypt";
            var encrypted = crypto.Encrypt(plainText, null);
            var decrypted = crypto.Decrypt(encrypted, null);

            encrypted.Should().NotBe(plainText);
            decrypted.Should().NotBe(encrypted);
            decrypted.Should().Be(plainText);
        }

        [Test]
        public void CanEncryptDecryptTripleDes()
        {
            var credentialMock = new Mock<ICredential>();
            credentialMock.Setup(cm => cm.Algorithm).Returns(SymmetricAlgorithm.TripleDES);
            credentialMock.Setup(cm => cm.IVSize).Returns(8);
            credentialMock.Setup(cm => cm.GetKey()).Returns(GetSequentialByteArray(24));

            var credentialRepo = new CredentialRepository(new List<ICredential> { credentialMock.Object });

            var crypto = new SymmetricCrypto(credentialRepo);

            var plainText = "This is just some random text to encrypt/decrypt";
            var encrypted = crypto.Encrypt(plainText, null);
            var decrypted = crypto.Decrypt(encrypted, null);

            encrypted.Should().NotBe(plainText);
            decrypted.Should().NotBe(encrypted);
            decrypted.Should().Be(plainText);
        }

        [Test]
        public void CanGetSpecificEncryptorAndDecryptorWhenMultipleCredentialsExist()
        {
            var credentialMock = new Mock<ICredential>();
            credentialMock.Setup(cm => cm.Algorithm).Returns(SymmetricAlgorithm.Aes);
            credentialMock.Setup(cm => cm.IVSize).Returns(16);
            credentialMock.Setup(cm => cm.GetKey()).Returns(GetSequentialByteArray(16));

            var credentialRepoMock = new Mock<ICredentialRepository>();

            ICredential outCredential;
            credentialRepoMock
                .Setup(cr => cr.TryGet(null, out outCredential))
                .OutCallback((object keyIdentifier, out ICredential credential) => credential = credentialMock.Object)
                .Returns(true);

            credentialRepoMock
                .Setup(cr => cr.TryGet("encryptor1", out outCredential))
                .OutCallback((object keyIdentifier, out ICredential credential) => credential = credentialMock.Object)
                .Returns(true);

            credentialRepoMock
                .Setup(cr => cr.TryGet("encryptor2", out outCredential))
                .OutCallback((object keyIdentifier, out ICredential credential) => credential = credentialMock.Object)
                .Returns(true);

            var crypto = new SymmetricCrypto(credentialRepoMock.Object);

            crypto.CanEncrypt(null).Should().Be(true);
            crypto.CanEncrypt("encryptor1").Should().Be(true);
            crypto.CanEncrypt("encryptor2").Should().Be(true);
            crypto.CanEncrypt("encryptor3").Should().Be(false);
            crypto.CanEncrypt("something").Should().Be(false);

            crypto.GetEncryptor(null).Should().NotBe(null);
            crypto.GetEncryptor("encryptor1").Should().NotBe(null);
            crypto.GetEncryptor("encryptor2").Should().NotBe(null);
            crypto.Invoking(c => c.GetEncryptor("encryptor3")).ShouldThrow<KeyNotFoundException>().WithMessage("Unable to locate credential using keyIdentifier: encryptor3");
            crypto.Invoking(c => c.GetEncryptor("something")).ShouldThrow<KeyNotFoundException>().WithMessage("Unable to locate credential using keyIdentifier: something");

            crypto.CanDecrypt(null).Should().Be(true);
            crypto.CanDecrypt("encryptor1").Should().Be(true);
            crypto.CanDecrypt("encryptor2").Should().Be(true);
            crypto.CanDecrypt("encryptor3").Should().Be(false);
            crypto.CanDecrypt("something").Should().Be(false);

            crypto.GetDecryptor(null).Should().NotBe(null);
            crypto.GetDecryptor("encryptor1").Should().NotBe(null);
            crypto.GetDecryptor("encryptor2").Should().NotBe(null);
            crypto.Invoking(c => c.GetDecryptor("encryptor3")).ShouldThrow<KeyNotFoundException>().WithMessage("Unable to locate credential using keyIdentifier: encryptor3");
            crypto.Invoking(c => c.GetDecryptor("something")).ShouldThrow<KeyNotFoundException>().WithMessage("Unable to locate credential using keyIdentifier: something");
        }

        private byte[] GetSequentialByteArray(int size)
        {
            if (size > 255) throw new InvalidOperationException("This uses only the first byte, so it cannot be over size 255");

            var array = new byte[size];

            for (var i = 0; i < size; i++)
            {
                array[i] = BitConverter.GetBytes(i)[3];
            }

            return array;
        }

        public string SerializeToString<T>(T item, Type type)
        {
            var serializer = new JsonSerializer();
            var sb = new StringBuilder();

            using (var stringWriter = new StringWriter())
            {
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    serializer.Serialize(jsonWriter, item, type);
                }
            }

            return sb.ToString();
        }
    }
}