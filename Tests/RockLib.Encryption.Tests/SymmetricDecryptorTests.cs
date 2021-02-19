using System;
using System.Text;
using FluentAssertions;
using RockLib.Encryption.Symmetric;
using Xunit;

namespace RockLib.Encryption.Tests
{
    public class SymmetricDecryptorTests
    {
        [Fact]
        public void CanDecryptByString()
        {
            var credential = new Credential(
                () => new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 },
                SymmetricAlgorithm.Aes, 16);

            var symmetricDecryptor = new SymmetricDecryptor(credential, Encoding.UTF8);

            var encrypted = "ARAAR0wt0bewMNdNByQ5OuJmKj6AfWMNWYSIrPaLR0h/bBF4fcSjCXwJrxZ1upPDByFp";
            var decrypted = symmetricDecryptor.Decrypt(encrypted);

            decrypted.Should().NotBeNullOrEmpty();
            decrypted.Should().NotBe(encrypted);
        }

        [Fact]
        public void CanDecryptByByteArray()
        {
            var credential = new Credential(
                () => new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 },
                SymmetricAlgorithm.Aes, 16);

            var symmetricDecryptor = new SymmetricDecryptor(credential, Encoding.UTF8);

            var encryptedString = "ARAAR0wt0bewMNdNByQ5OuJmKj6AfWMNWYSIrPaLR0h/bBF4fcSjCXwJrxZ1upPDByFp";
            var encrypted = Convert.FromBase64String(encryptedString);
            var decrypted = symmetricDecryptor.Decrypt(encrypted);
            var decryptedString = Encoding.UTF8.GetString(decrypted);

            decrypted.Should().NotBeEmpty();
            decryptedString.Should().NotBeNullOrEmpty();
            decryptedString.Should().NotBe(encryptedString);
        }

        [Fact]
        public void DecryptByStringReturnsTheCipherTextParameterWhenItIsNotBase64Encoded()
        {
            var credential = new Credential(
                () => new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 },
                SymmetricAlgorithm.Aes, 16);

            var symmetricDecryptor = new SymmetricDecryptor(credential, Encoding.UTF8);

            var plaintext = "This is not a base-64 encoded string.";
            var decrypted = symmetricDecryptor.Decrypt(plaintext);

            decrypted.Should().BeSameAs(plaintext);
        }

        [Fact]
        public void DecryptByByteArrayReturnsTheCipherTextParameterWhenItIsNotLongEnoughForTheHeader()
        {
            var credential = new Credential(
                () => new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 },
                SymmetricAlgorithm.Aes, 16);

            var symmetricDecryptor = new SymmetricDecryptor(credential, Encoding.UTF8);

            var plaintext = new byte[] { 1, 16 };
            var decrypted = symmetricDecryptor.Decrypt(plaintext);

            decrypted.Should().BeSameAs(plaintext);
        }

        [Fact]
        public void DecryptByByteArrayReturnsTheCipherTextParameterWhenTheVersionIsNot1()
        {
            var credential = new Credential(
                () => new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 },
                SymmetricAlgorithm.Aes, 16);

            var symmetricDecryptor = new SymmetricDecryptor(credential, Encoding.UTF8);

            for (int i = 0; i < 256; i++)
            {
                if (i == 1) continue;

                var plaintext = new byte[] { (byte)i, 16, 0, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };
                var decrypted = symmetricDecryptor.Decrypt(plaintext);

                decrypted.Should().BeSameAs(plaintext);
            }
        }

        [Fact]
        public void DecryptByByteArrayReturnsTheCipherTextParameterWhenTheIVSizeIsNot8Or16()
        {
            var credential = new Credential(
                () => new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 },
                SymmetricAlgorithm.Aes, 16);

            var symmetricDecryptor = new SymmetricDecryptor(credential, Encoding.UTF8);

            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    if ((i == 8 || i == 16) && j == 0) continue;

                    var plaintext = new byte[] { 1, (byte)i, (byte)j, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };
                    var decrypted = symmetricDecryptor.Decrypt(plaintext);

                    decrypted.Should().BeSameAs(plaintext);
                }
            }
        }

        [Fact]
        public void DecryptByByteArrayReturnsTheCipherTextParameterWhenItIsNotLongEnoughForTheHeaderAndIV()
        {
            var credential = new Credential(
                () => new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 },
                SymmetricAlgorithm.Aes, 16);

            var symmetricDecryptor = new SymmetricDecryptor(credential, Encoding.UTF8);

            var plaintext = new byte[] { 1, 16, 0, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };
            var decrypted = symmetricDecryptor.Decrypt(plaintext);

            decrypted.Should().BeSameAs(plaintext);

            plaintext = new byte[] { 1, 8, 0, 6, 5, 4, 3, 2, 1, 0 };
            decrypted = symmetricDecryptor.Decrypt(plaintext);

            decrypted.Should().BeSameAs(plaintext);
        }
    }
}
