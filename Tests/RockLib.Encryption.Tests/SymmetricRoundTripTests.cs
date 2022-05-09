﻿using FluentAssertions;
using RockLib.Encryption.Symmetric;
using System.Security.Cryptography;
using System.Text;
using Xunit;
using SymmetricAlgorithm = RockLib.Encryption.Symmetric.SymmetricAlgorithm;

namespace RockLib.Encryption.Tests;

public static class SymmetricRoundTripTests
{
    [Fact]
    public static void CanRoundTripByString()
    {
        var credential = new Credential(
            () => new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 },
            SymmetricAlgorithm.Aes, 16);

        using var symmetricEncryptor = new SymmetricEncryptor(credential, Encoding.UTF8);
        using var symmetricDecryptor = new SymmetricDecryptor(credential, Encoding.UTF8);

        var unencrypted = "This is some string";
        var encrypted = symmetricEncryptor.Encrypt(unencrypted);
        var decrypted = symmetricDecryptor.Decrypt(encrypted);

        encrypted.Should().NotBe(unencrypted);
        decrypted.Should().NotBe(encrypted);
        decrypted.Should().Be(unencrypted);
    }

    [Fact]
    public static void CanRoundTripByByteArray()
    {
        var credential = new Credential(
            () => new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 },
            SymmetricAlgorithm.Aes, 16);

        using var symmetricEncryptor = new SymmetricEncryptor(credential, Encoding.UTF8);
        using var symmetricDecryptor = new SymmetricDecryptor(credential, Encoding.UTF8);

        var unencryptedString = "This is some string";
        var unencrypted = Encoding.UTF8.GetBytes(unencryptedString);
        var encrypted = symmetricEncryptor.Encrypt(unencrypted);
        var decrypted = symmetricDecryptor.Decrypt(encrypted);

        encrypted.Should().NotEqual(unencrypted);
        decrypted.Should().NotEqual(encrypted);
        decrypted.Should().Equal(unencrypted);
    }

    [Fact]
    public static void CannotRoundTripByStringWithMismatchedCredentials()
    {
        var credential1 = new Credential(
            () => new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 },
            SymmetricAlgorithm.Aes, 16);

        var credential2 = new Credential(
            () => new byte[] { 0x15, 0x14, 0x13, 0x12, 0x11, 0x10, 0x9, 0x8, 0x7, 0x6, 0x5, 0x4, 0x3, 0x2, 0x1, 0x0 },
            SymmetricAlgorithm.Aes, 16);

        using var symmetricEncryptor = new SymmetricEncryptor(credential1, Encoding.UTF8);
        using var symmetricDecryptor = new SymmetricDecryptor(credential2, Encoding.UTF8);

        var unencrypted = "This is some string";
        var encrypted = symmetricEncryptor.Encrypt(unencrypted);

        var action = () => symmetricDecryptor.Decrypt(encrypted);

        action.Should().Throw<CryptographicException>();
    }

    [Fact]
    public static void CannotRoundTripByByteArrayWithMismatchedCredentials()
    {
        var credential1 = new Credential(
            () => new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 },
            SymmetricAlgorithm.Aes, 16);

        var credential2 = new Credential(
            () => new byte[] { 0x15, 0x14, 0x13, 0x12, 0x11, 0x10, 0x9, 0x8, 0x7, 0x6, 0x5, 0x4, 0x3, 0x2, 0x1, 0x0 },
            SymmetricAlgorithm.Aes, 16);

        using var symmetricEncryptor = new SymmetricEncryptor(credential1, Encoding.UTF8);
        using var symmetricDecryptor = new SymmetricDecryptor(credential2, Encoding.UTF8);

        var unencryptedString = "This is some string";
        var unencrypted = Encoding.UTF8.GetBytes(unencryptedString);
        var encrypted = symmetricEncryptor.Encrypt(unencrypted);

        var action = () => symmetricDecryptor.Decrypt(encrypted);

        action.Should().Throw<CryptographicException>();
    }

    [Fact]
    public static void MismatchedEncodingCausesEncodingDiscrepency()
    {
        var credential = new Credential(
            () => new byte[] { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15 },
            SymmetricAlgorithm.Aes, 16);

        using var symmetricEncryptor = new SymmetricEncryptor(credential, Encoding.UTF8);
        using var symmetricDecryptor = new SymmetricDecryptor(credential, Encoding.UTF32);

        var unencrypted = "This is some string. 😂🤣";
        var encrypted = symmetricEncryptor.Encrypt(unencrypted);
        var decrypted = symmetricDecryptor.Decrypt(encrypted);

        encrypted.Should().NotBe(unencrypted);
        decrypted.Should().NotBe(encrypted);
        decrypted.Should().NotBe(unencrypted);
    }
}