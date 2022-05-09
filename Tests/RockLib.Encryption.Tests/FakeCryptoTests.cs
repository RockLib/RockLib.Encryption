using FluentAssertions;
using RockLib.Encryption.Testing;
using Xunit;

namespace RockLib.Encryption.Tests;

public static class FakeCryptoTests
{
    [Fact]
    public static void EncryptStringAddsDoubleSquareBrackets()
    {
        var crypto = new FakeCrypto();

        var plainText = "Hello, world!";

        var cipherText = crypto.Encrypt(plainText);

        cipherText.Should().Be("[[Hello, world!]]");
    }

    [Fact]
    public static void DecryptStringRemovesDoubleSquareBrackets()
    {
        var crypto = new FakeCrypto();

        var cipherText = "[[Hello, world!]]";

        var plainText = crypto.Decrypt(cipherText);

        plainText.Should().Be("Hello, world!");
    }

    [Fact]
    public static void DecryptStringReturnsTheCipherTextIfNotSurroundedByDoubleSquareBrackets()
    {
        var crypto = new FakeCrypto();

        var cipherText = "Hello, world!";

        var plainText = crypto.Decrypt(cipherText);

        plainText.Should().BeSameAs(cipherText);
    }

    [Fact]
    public static void EncryptBinaryAddsDoubleSquareBrackets()
    {
        var crypto = new FakeCrypto();

        var plainText = new byte[] { 1, 2, 3 };

        var cipherText = crypto.Encrypt(plainText);

        cipherText.Should().BeEquivalentTo(new byte[] { (int)'[', (int)'[', 1, 2, 3, (int)']', (int)']' });
    }

    [Fact]
    public static void DecryptBinaryRemovesDoubleSquareBrackets()
    {
        var crypto = new FakeCrypto();

        var cipherText = new byte[] { (int)'[', (int)'[', 1, 2, 3, (int)']', (int)']' };

        var plainText = crypto.Decrypt(cipherText);

        plainText.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
    }

    [Fact]
    public static void DecryptBinaryReturnsTheCipherTextIfNotSurroundedByDoubleSquareBrackets()
    {
        var crypto = new FakeCrypto();

        var cipherText = new byte[] { 1, 2, 3 };

        var plainText = crypto.Decrypt(cipherText);

        plainText.Should().BeSameAs(cipherText);
    }

    [Fact]
    public static void GetEncryptorReturnsInstanceThatAddsDoubleSquareBrackets()
    {
        var crypto = new FakeCrypto();

        var encryptor = crypto.GetEncryptor();

        var plainTextString = "Hello, world!";
        var plainTextBinary = new byte[] { 1, 2, 3 };

        var cipherTextString = encryptor.Encrypt(plainTextString);
        var cipherTextBinary = encryptor.Encrypt(plainTextBinary);

        cipherTextString.Should().Be("[[Hello, world!]]");
        cipherTextBinary.Should().BeEquivalentTo(new byte[] { (int)'[', (int)'[', 1, 2, 3, (int)']', (int)']' });
    }

    [Fact]
    public static void GetDecryptorReturnsInstanceThatRemovesDoubleSquareBrackets()
    {
        var crypto = new FakeCrypto();

        var decryptor = crypto.GetDecryptor();

        var cipherTextString = "[[Hello, world!]]";
        var cipherTextBinary = new byte[] { (int)'[', (int)'[', 1, 2, 3, (int)']', (int)']' };

        var plainTextString = decryptor.Decrypt(cipherTextString);
        var plainTextBinary = decryptor.Decrypt(cipherTextBinary);

        plainTextString.Should().Be("Hello, world!");
        plainTextBinary.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
    }

    [Theory]
    [InlineData("default")]
    [InlineData("literally any string")]
    [InlineData("")]
    [InlineData(null)]
    public static void CanEncryptReturnsTrue(string credentialName)
    {
        ICrypto crypto = new FakeCrypto();

        crypto.CanEncrypt(credentialName).Should().BeTrue();
    }

    [Theory]
    [InlineData("default")]
    [InlineData("literally any string")]
    [InlineData("")]
    [InlineData(null)]
    public static void CanDecryptReturnsTrue(string credentialName)
    {
        ICrypto crypto = new FakeCrypto();

        crypto.CanDecrypt(credentialName).Should().BeTrue();
    }
}
