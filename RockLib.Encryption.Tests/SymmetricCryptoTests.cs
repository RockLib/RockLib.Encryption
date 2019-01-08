using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using RockLib.Encryption.Symmetric;

namespace RockLib.Encryption.Tests
{
    [TestFixture]
    public class SymmetricCryptoTests
    {
        [Test]
        public void CanEncryptDecryptAes()
        {
            var credential = new Credential(GetSequentialByteArray(16), SymmetricAlgorithm.Aes, 16);

            var crypto = new SymmetricCrypto(new[] { credential });

            var plainText = "This is just some random text to encrypt/decrypt";
            var encrypted = crypto.Encrypt(plainText, null);
            var decrypted = crypto.Decrypt(encrypted, null);

            encrypted.Should().NotBe(plainText);
            decrypted.Should().NotBe(encrypted);
            decrypted.Should().Be(plainText);
        }

        [Test]
        public void CanEncryptDecryptDES()
        {
            var credential = new Credential(GetSequentialByteArray(8), SymmetricAlgorithm.DES, 8);

            var crypto = new SymmetricCrypto(new[] { credential });

            var plainText = "This is just some random text to encrypt/decrypt";
            var encrypted = crypto.Encrypt(plainText, null);
            var decrypted = crypto.Decrypt(encrypted, null);

            encrypted.Should().NotBe(plainText);
            decrypted.Should().NotBe(encrypted);
            decrypted.Should().Be(plainText);
        }

        [Test]
        public void CanEncryptDecryptRC2()
        {
            var credential = new Credential(GetSequentialByteArray(8), SymmetricAlgorithm.RC2, 8);

            var crypto = new SymmetricCrypto(new[] { credential });

            var plainText = "This is just some random text to encrypt/decrypt";
            var encrypted = crypto.Encrypt(plainText, null);
            var decrypted = crypto.Decrypt(encrypted, null);

            encrypted.Should().NotBe(plainText);
            decrypted.Should().NotBe(encrypted);
            decrypted.Should().Be(plainText);
        }

        [Test]
        public void CanEncryptDecryptRijndael()
        {
            var credential = new Credential(GetSequentialByteArray(16), SymmetricAlgorithm.Rijndael, 16);

            var crypto = new SymmetricCrypto(new[] { credential });

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
            var credential = new Credential(GetSequentialByteArray(24), SymmetricAlgorithm.TripleDES, 8);

            var crypto = new SymmetricCrypto(new[] { credential });

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
            var defaultCredential = new Credential(GetSequentialByteArray(16));
            var credential1 = new Credential(GetSequentialByteArray(16), name: "encryptor1");
            var credential2 = new Credential(GetSequentialByteArray(16), name: "encryptor2");

            var crypto = new SymmetricCrypto(new[] { defaultCredential, credential1, credential2 });

            crypto.CanEncrypt(null).Should().Be(true);
            crypto.CanEncrypt("encryptor1").Should().Be(true);
            crypto.CanEncrypt("encryptor2").Should().Be(true);
            crypto.CanEncrypt("encryptor3").Should().Be(false);
            crypto.CanEncrypt("something").Should().Be(false);

            crypto.GetEncryptor(null).Should().NotBe(null);
            crypto.GetEncryptor("encryptor1").Should().NotBe(null);
            crypto.GetEncryptor("encryptor2").Should().NotBe(null);
            crypto.Invoking(c => c.GetEncryptor("encryptor3")).ShouldThrow<KeyNotFoundException>().WithMessage("Unable to locate credential using credentialName: encryptor3");
            crypto.Invoking(c => c.GetEncryptor("something")).ShouldThrow<KeyNotFoundException>().WithMessage("Unable to locate credential using credentialName: something");

            crypto.CanDecrypt(null).Should().Be(true);
            crypto.CanDecrypt("encryptor1").Should().Be(true);
            crypto.CanDecrypt("encryptor2").Should().Be(true);
            crypto.CanDecrypt("encryptor3").Should().Be(false);
            crypto.CanDecrypt("something").Should().Be(false);

            crypto.GetDecryptor(null).Should().NotBe(null);
            crypto.GetDecryptor("encryptor1").Should().NotBe(null);
            crypto.GetDecryptor("encryptor2").Should().NotBe(null);
            crypto.Invoking(c => c.GetDecryptor("encryptor3")).ShouldThrow<KeyNotFoundException>().WithMessage("Unable to locate credential using credentialName: encryptor3");
            crypto.Invoking(c => c.GetDecryptor("something")).ShouldThrow<KeyNotFoundException>().WithMessage("Unable to locate credential using credentialName: something");
        }

        [Test]
        public void EncodingIsSetCorrectly()
        {
            var crypto = new SymmetricCrypto(new Credential[0], Encoding.ASCII);
            crypto.Encoding.Should().Be(Encoding.ASCII);
        }

        private byte[] GetSequentialByteArray(int size, int seed = 12345)
        {
            var random = new Random(seed);

            var array = new byte[size];

            random.NextBytes(array);

            return array;
        }
    }
}