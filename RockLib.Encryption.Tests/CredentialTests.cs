using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using RockLib.Configuration;
using RockLib.Encryption.Symmetric;

namespace RockLib.Encryption.Tests
{
    [TestFixture]
    public class CredentialTests
    {
        [Test]
        public void CanGetKeyWithEmptyConstructor()
        {
            var credential = new Credential
            {
                Algorithm = SymmetricAlgorithm.Aes,
                IVSize = 16,
                Key = "1J9Og/OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="
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
                .WithMessage("The Key property (or rocklib.encryption:CryptoFactories:0:Value:EncryptionSettings:Credentials:0:Key configuration element) is required, but was not provided.");
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
