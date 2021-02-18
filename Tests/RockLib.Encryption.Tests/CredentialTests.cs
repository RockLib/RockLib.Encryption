using System;
using FluentAssertions;
using RockLib.Encryption.Symmetric;
using Xunit;

namespace RockLib.Encryption.Tests
{
    public class CredentialTests
    {
        [Fact]
        public void CanGetKey()
        {
            var credential = new Credential(
                () => Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="), SymmetricAlgorithm.Aes, 16);

            var key = credential.GetKey();

            key.Should().NotBeNull();
            key.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void CanGetAlgorithm()
        {
            var credential = new Credential(
                () => Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="), SymmetricAlgorithm.TripleDES, 16);

            var algorithm = credential.Algorithm;

            algorithm.Should().Be(SymmetricAlgorithm.TripleDES);
        }

        [Fact]
        public void CanGetIVSize()
        {
            var credential = new Credential(
                () => Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="), SymmetricAlgorithm.Aes, 32);

            var ivSize = credential.IVSize;

            ivSize.Should().Be(32);
        }

        [Fact]
        public void DefaultAlgorithmIsCorrect()
        {
            var credential = new Credential(
                () => Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="));

            var algorithm = credential.Algorithm;

            algorithm.Should().Be(Credential.DefaultAlgorithm);
        }

        [Fact]
        public void DefaultIVSizeIsCorrect()
        {
            var credential = new Credential(
                () => Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="));

            var ivSize = credential.IVSize;

            ivSize.Should().Be(Credential.DefaultIVSize);
        }

        [Fact]
        public void NullKeyThrowsArgumentNullException()
        {
            Action newCredential = () => new Credential(null, SymmetricAlgorithm.Aes, 16);
            newCredential.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void NullKeyValueThrowsInvalidOperationException()
        {
            var credential = new Credential(() => null, SymmetricAlgorithm.Aes, 16);
            Action getKey = () => credential.GetKey();
            getKey.Should().Throw<InvalidOperationException>().WithMessage("The value returned from the key function must not be null or empty.");
        }

        [Fact]
        public void EmptyKeyValueThrowsInvalidOperationException()
        {
            var credential = new Credential(() => new byte[0], SymmetricAlgorithm.Aes, 16);
            Action getKey = () => credential.GetKey();
            getKey.Should().Throw<InvalidOperationException>().WithMessage("The value returned from the key function must not be null or empty.");
        }

        [Fact]
        public void UndefinedAlgorithmThrowsArgumentOutOfRangeException()
        {
            Action newCredential = () => new Credential(
                () => Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="), (SymmetricAlgorithm)(-1), 16);
            newCredential.Should().Throw<ArgumentOutOfRangeException>().WithMessage("algorithm value is not defined: -1.*Parameter*algorithm*");
        }

        [Fact]
        public void InvalidIvSizeThrowsArgumentOutOfRangeException()
        {
            Action newCredential = () => new Credential(
                () => Convert.FromBase64String("1J9Og / OaZKWdfdwM6jWMpvlr3q3o7r20xxFDN7TEj6s="), SymmetricAlgorithm.Aes, 0);

            newCredential.Should().Throw<ArgumentOutOfRangeException>().WithMessage("ivSize must be greater than 0.*Parameter*ivSize*");
        }
    }
}
