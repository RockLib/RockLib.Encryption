using System;
using System.Text;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RockLib.Encryption.Symmetric;

namespace RockLib.Encryption.Tests
{
    [TestFixture]
    public class SymmetricDecryptorTests
    {
        [Test]
        public void CanDecryptByString()
        {
            var credentialMock = new Mock<ICredential>();
            credentialMock.Setup(cm => cm.Algorithm).Returns(SymmetricAlgorithm.Aes);
            credentialMock.Setup(cm => cm.IVSize).Returns(16);
            credentialMock.Setup(cm => cm.GetKey()).Returns(new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 });

            var symmetricDecryptor = new SymmetricDecryptor(credentialMock.Object, Encoding.UTF8);

            var encrypted = "ARAAR0wt0bewMNdNByQ5OuJmKj6AfWMNWYSIrPaLR0h/bBF4fcSjCXwJrxZ1upPDByFp";
            var decrypted = symmetricDecryptor.Decrypt(encrypted);

            decrypted.Should().NotBeNullOrEmpty();
            decrypted.Should().NotBe(encrypted);
        }

        [Test]
        public void CanDecryptByByteArray()
        {
            var credentialMock = new Mock<ICredential>();
            credentialMock.Setup(cm => cm.Algorithm).Returns(SymmetricAlgorithm.Aes);
            credentialMock.Setup(cm => cm.IVSize).Returns(16);
            credentialMock.Setup(cm => cm.GetKey()).Returns(new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 });

            var symmetricDecryptor = new SymmetricDecryptor(credentialMock.Object, Encoding.UTF8);

            var encryptedString = "ARAAR0wt0bewMNdNByQ5OuJmKj6AfWMNWYSIrPaLR0h/bBF4fcSjCXwJrxZ1upPDByFp";
            var encrypted = Convert.FromBase64String(encryptedString);
            var decrypted = symmetricDecryptor.Decrypt(encrypted);
            var decryptedString = Encoding.UTF8.GetString(decrypted);

            decrypted.Should().NotBeEmpty();
            decryptedString.Should().NotBeNullOrEmpty();
            decryptedString.Should().NotBe(encryptedString);
        }
    }
}
