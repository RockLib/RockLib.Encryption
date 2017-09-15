using System.Text;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RockLib.Encryption.Symmetric;

namespace RockLib.Encryption.Tests
{
    [TestFixture]
    public class SymmetricEncryptorTests
    {
        [Test]
        public void CanEncryptByString()
        {
            var credentialMock = new Mock<ICredential>();
            credentialMock.Setup(cm => cm.Algorithm).Returns(SymmetricAlgorithm.Aes);
            credentialMock.Setup(cm => cm.IVSize).Returns(16);
            credentialMock.Setup(cm => cm.GetKey()).Returns(new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 });

            var symmetricEncryptor = new SymmetricEncryptor(credentialMock.Object, Encoding.UTF8);

            var unencrypted = "This is some string";
            var encrypted = symmetricEncryptor.Encrypt(unencrypted);

            encrypted.Should().NotBeNullOrEmpty();
            encrypted.Should().NotBe(unencrypted);
        }

        [Test]
        public void CanEncryptByByteArray()
        {
            var credentialMock = new Mock<ICredential>();
            credentialMock.Setup(cm => cm.Algorithm).Returns(SymmetricAlgorithm.Aes);
            credentialMock.Setup(cm => cm.IVSize).Returns(16);
            credentialMock.Setup(cm => cm.GetKey()).Returns(new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 });

            var symmetricEncryptor = new SymmetricEncryptor(credentialMock.Object, Encoding.UTF8);

            var unencryptedString = "This is some string";
            var unencrypted = Encoding.UTF8.GetBytes(unencryptedString);
            var encrypted = symmetricEncryptor.Encrypt(unencrypted);
            var encryptedString = Encoding.UTF8.GetString(encrypted);

            encrypted.Should().NotBeEmpty();
            encryptedString.Should().NotBeNullOrEmpty();
            encryptedString.Should().NotBe(unencryptedString);
        }

        //[Test]
        //public void CanDispose()
        //{
        //    var credentialMock = new Mock<ICredential>();
        //    credentialMock.Setup(cm => cm.Algorithm).Returns(SymmetricAlgorithm.Aes);
        //    credentialMock.Setup(cm => cm.IVSize).Returns(16);
        //    credentialMock.Setup(cm => cm.GetKey()).Returns(new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 });

        //    var symmetricEncryptor = new SymmetricEncryptor(credentialMock.Object, Encoding.UTF8);
        //    symmetricEncryptor.Encrypt("This should not fail");

        //    symmetricEncryptor.Dispose();

        //    Action action = () => symmetricEncryptor.Encrypt("This should fail");
        //    action.ShouldThrow<SomeException>();
        //}
    }
}
