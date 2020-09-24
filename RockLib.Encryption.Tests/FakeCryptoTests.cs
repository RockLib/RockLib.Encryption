using FluentAssertions;
using NUnit.Framework;
using RockLib.Encryption.Testing;

namespace RockLib.Encryption.Tests
{
    [TestFixture]
    public class FakeCryptoTests
    {
        [Test]
        public void EncryptStringAddsDoubleSquareBrackets()
        {
            ICrypto crypto = new FakeCrypto();

            var plainText = "Hello, world!";

            var cipherText = crypto.Encrypt(plainText);

            cipherText.Should().Be("[[Hello, world!]]");
        }

        [Test]
        public void DecryptStringRemovesDoubleSquareBrackets()
        {
            ICrypto crypto = new FakeCrypto();

            var cipherText = "[[Hello, world!]]";

            var plainText = crypto.Decrypt(cipherText);

            plainText.Should().Be("Hello, world!");
        }

        [Test]
        public void EncryptBinaryAddsDoubleSquareBrackets()
        {
            ICrypto crypto = new FakeCrypto();

            var plainText = new byte[] { 1, 2, 3 };

            var cipherText = crypto.Encrypt(plainText);

            cipherText.Should().BeEquivalentTo(new byte[] { (int)'[', (int)'[', 1, 2, 3, (int)']', (int)']' });
        }

        [Test]
        public void DecryptBinaryRemovesDoubleSquareBrackets()
        {
            ICrypto crypto = new FakeCrypto();

            var cipherText = new byte[] { (int)'[', (int)'[', 1, 2, 3, (int)']', (int)']' };

            var plainText = crypto.Decrypt(cipherText);

            plainText.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
        }

        [Test]
        public void GetEncryptorReturnsInstanceThatAddsDoubleSquareBrackets()
        {
            ICrypto crypto = new FakeCrypto();

            var encryptor = crypto.GetEncryptor();

            var plainTextString = "Hello, world!";
            var plainTextBinary = new byte[] { 1, 2, 3 };

            var cipherTextString = encryptor.Encrypt(plainTextString);
            var cipherTextBinary = encryptor.Encrypt(plainTextBinary);

            cipherTextString.Should().Be("[[Hello, world!]]");
            cipherTextBinary.Should().BeEquivalentTo(new byte[] { (int)'[', (int)'[', 1, 2, 3, (int)']', (int)']' });
        }

        [Test]
        public void GetDecryptorReturnsInstanceThatRemovesDoubleSquareBrackets()
        {
            ICrypto crypto = new FakeCrypto();

            var decryptor = crypto.GetDecryptor();

            var cipherTextString = "[[Hello, world!]]";
            var cipherTextBinary = new byte[] { (int)'[', (int)'[', 1, 2, 3, (int)']', (int)']' };

            var plainTextString = decryptor.Decrypt(cipherTextString);
            var plainTextBinary = decryptor.Decrypt(cipherTextBinary);

            plainTextString.Should().Be("Hello, world!");
            plainTextBinary.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
        }

        [Test]
        [TestCase("default")]
        [TestCase("literally any string")]
        [TestCase("")]
        [TestCase(null)]
        public void CanEncryptReturnsTrue(string credentialName)
        {
            ICrypto crypto = new FakeCrypto();

            crypto.CanEncrypt(credentialName).Should().BeTrue();
        }

        [Test]
        [TestCase("default")]
        [TestCase("literally any string")]
        [TestCase("")]
        [TestCase(null)]
        public void CanDecryptReturnsTrue(string credentialName)
        {
            ICrypto crypto = new FakeCrypto();

            crypto.CanDecrypt(credentialName).Should().BeTrue();
        }
    }
}
