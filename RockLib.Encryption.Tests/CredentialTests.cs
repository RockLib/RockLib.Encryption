using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using RockLib.Configuration;
using RockLib.DataProtection;
using RockLib.Encryption.Symmetric;

namespace RockLib.Encryption.Tests
{
    [TestFixture]
    public class CredentialTests
    {
        [Test]
        public void CanGetKeyWithEmptyConstructor()
        {
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string> { { "Section:Value", "1J9Og/OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s=" } });

            var credential = new Credential
            {
                Algorithm = SymmetricAlgorithm.Aes,
                IVSize = 16,
                Key = new LateBoundConfigurationSection<IProtectedValue>
                {
                    Type = "RockLib.DataProtection.UnprotectedBase64Value, RockLib.DataProtection",
                    Value = builder.Build().GetSection("Section")
                }
            };

            var key = credential.GetKey();

            key.Should().NotBeNull();
            key.Length.Should().BeGreaterThan(0);
        }

        [Test]
        public void CanGetKeyWithFullConstructor()
        {
            var credential = new Credential(SymmetricAlgorithm.Aes, 16, Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="));

            var key = credential.GetKey();

            key.Should().NotBeNull();
            key.Length.Should().BeGreaterThan(0);
        }

        [Test]
        public void NullKeyThrowsInvalidOperationExceptionOnGetKey()
        {
            var credential = new Credential
            {
                Algorithm = SymmetricAlgorithm.Aes,
                IVSize = 16
            };

            credential.Invoking(c => c.GetKey()).ShouldThrow<InvalidOperationException>()
                .WithMessage("The Key property (or rocklib.encryption:CryptoFactories:0:Value:EncryptionSettings:Credentials:0:Key:Value configuration element) is required, but was not provided.");
        }

        [Test]
        public void InvalidIvSizeOnFullConstructorThrowsNullArgumentException()
        {
            Action newCredential = () => new Credential(SymmetricAlgorithm.Aes, 0, Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="));

            newCredential.ShouldThrow<ArgumentOutOfRangeException>().WithMessage("ivSize must be greater than 0\r\nParameter name: ivSize");
        }

        [Test]
        public void NullKeyOnFullConstructorThrowsNullArgumentException()
        {
            Action newCredential = () => new Credential(SymmetricAlgorithm.Aes, 16, null);

            newCredential.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: key");
        }
    }
}
