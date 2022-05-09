using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using RockLib.Encryption.Symmetric;
using Xunit;

namespace RockLib.Encryption.Tests;

public static class SymmetricCryptoTests
{
    [Fact]
    public static void CanEncryptDecryptAes()
    {
        var credential = new Credential(() => GetSequentialByteArray(16), SymmetricAlgorithm.Aes, 16);

        var crypto = new SymmetricCrypto(new[] { credential });

        var plainText = "This is just some random text to encrypt/decrypt";
        var encrypted = crypto.Encrypt(plainText, null);
        var decrypted = crypto.Decrypt(encrypted, null);

        encrypted.Should().NotBe(plainText);
        decrypted.Should().NotBe(encrypted);
        decrypted.Should().Be(plainText);
    }

    [Fact]
    public static void CanGetSpecificEncryptorAndDecryptorWhenMultipleCredentialsExist()
    {
        var defaultCredential = new Credential(() => GetSequentialByteArray(16));
        var credential1 = new Credential(() => GetSequentialByteArray(16), name: "encryptor1");
        var credential2 = new Credential(() => GetSequentialByteArray(16), name: "encryptor2");

        var crypto = new SymmetricCrypto(new[] { defaultCredential, credential1, credential2 });

        crypto.CanEncrypt(null).Should().Be(true);
        crypto.CanEncrypt("encryptor1").Should().Be(true);
        crypto.CanEncrypt("encryptor2").Should().Be(true);
        crypto.CanEncrypt("encryptor3").Should().Be(false);
        crypto.CanEncrypt("something").Should().Be(false);

        crypto.GetEncryptor(null).Should().NotBe(null);
        crypto.GetEncryptor("encryptor1").Should().NotBe(null);
        crypto.GetEncryptor("encryptor2").Should().NotBe(null);
        crypto.Invoking(c => c.GetEncryptor("encryptor3")).Should().Throw<KeyNotFoundException>().WithMessage("The specified credential was not found: encryptor3.");
        crypto.Invoking(c => c.GetEncryptor("something")).Should().Throw<KeyNotFoundException>().WithMessage("The specified credential was not found: something.");

        crypto.CanDecrypt(null).Should().Be(true);
        crypto.CanDecrypt("encryptor1").Should().Be(true);
        crypto.CanDecrypt("encryptor2").Should().Be(true);
        crypto.CanDecrypt("encryptor3").Should().Be(false);
        crypto.CanDecrypt("something").Should().Be(false);

        crypto.GetDecryptor(null).Should().NotBe(null);
        crypto.GetDecryptor("encryptor1").Should().NotBe(null);
        crypto.GetDecryptor("encryptor2").Should().NotBe(null);
        crypto.Invoking(c => c.GetDecryptor("encryptor3")).Should().Throw<KeyNotFoundException>().WithMessage("The specified credential was not found: encryptor3.");
        crypto.Invoking(c => c.GetDecryptor("something")).Should().Throw<KeyNotFoundException>().WithMessage("The specified credential was not found: something.");
    }

    [Fact]
    public static void EncodingIsSetCorrectly()
    {
        var crypto = new SymmetricCrypto(Array.Empty<Credential>(), Encoding.ASCII);
        crypto.Encoding.Should().Be(Encoding.ASCII);
    }

    private static byte[] GetSequentialByteArray(int size)
    {
#if NET6_0
        return System.Security.Cryptography.RandomNumberGenerator.GetBytes(size);
#else
        var data = new byte[size];
        using var random = System.Security.Cryptography.RandomNumberGenerator.Create();
        random.GetBytes(data);
        return data;
#endif
    }
}