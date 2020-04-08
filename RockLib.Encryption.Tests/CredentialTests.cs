using System;
using FluentAssertions;
using NUnit.Framework;
using RockLib.Encryption.Symmetric;

namespace RockLib.Encryption.Tests
{
    [TestFixture]
    public class CredentialTests
    {
        [Test]
        public void CanGetKey()
        {
            var credential = new Credential(
                () => Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="), SymmetricAlgorithm.Aes, 16);

            var key = credential.GetKey();

            key.Should().NotBeNull();
            key.Length.Should().BeGreaterThan(0);
        }

        [Test]
        public void CanGetAlgorithm()
        {
            var credential = new Credential(
                () => Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="), SymmetricAlgorithm.TripleDES, 16);

            var algorithm = credential.Algorithm;

            algorithm.Should().Be(SymmetricAlgorithm.TripleDES);
        }

        [Test]
        public void CanGetIVSize()
        {
            var credential = new Credential(
                () => Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="), SymmetricAlgorithm.Aes, 32);

            var ivSize = credential.IVSize;

            ivSize.Should().Be(32);
        }

        [Test]
        public void DefaultAlgorithmIsCorrect()
        {
            var credential = new Credential(
                () => Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="));

            var algorithm = credential.Algorithm;

            algorithm.Should().Be(Credential.DefaultAlgorithm);
        }

        [Test]
        public void DefaultIVSizeIsCorrect()
        {
            var credential = new Credential(
                () => Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="));

            var ivSize = credential.IVSize;

            ivSize.Should().Be(Credential.DefaultIVSize);
        }

        [Test]
        public void NullKeyThrowsArgumentNullException()
        {
            Action newCredential = () => new Credential(null, SymmetricAlgorithm.Aes, 16);
            newCredential.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void NullKeyValueThrowsInvalidOperationException()
        {
            var credential = new Credential(() => null, SymmetricAlgorithm.Aes, 16);
            Action getKey = () => credential.GetKey();
            getKey.Should().Throw<InvalidOperationException>().WithMessage("The value returned from the key function must not be null or empty.");
        }

        [Test]
        public void EmptyKeyValueThrowsInvalidOperationException()
        {
            var credential = new Credential(() => new byte[0], SymmetricAlgorithm.Aes, 16);
            Action getKey = () => credential.GetKey();
            getKey.Should().Throw<InvalidOperationException>().WithMessage("The value returned from the key function must not be null or empty.");
        }

        [Test]
        public void UndefinedAlgorithmThrowsArgumentOutOfRangeException()
        {
            Action newCredential = () => new Credential(
                () => Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="), (SymmetricAlgorithm)(-1), 16);
            newCredential.Should().Throw<ArgumentOutOfRangeException>().WithMessage("algorithm value is not defined: -1.\r\nParameter name: algorithm");
        }

        [Test]
        public void InvalidIvSizeThrowsArgumentOutOfRangeException()
        {
            Action newCredential = () => new Credential(
                () => Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="), SymmetricAlgorithm.Aes, 0);

            newCredential.Should().Throw<ArgumentOutOfRangeException>().WithMessage("ivSize must be greater than 0.\r\nParameter name: ivSize");
        }
    }
}
