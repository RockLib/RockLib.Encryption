using System;
using System.Text.RegularExpressions;

namespace RockLib.Encryption.Testing;

/// <summary>
/// An implementation of the <see cref="ICrypto"/> interface meant for application testing. To
/// encrypt, it surrounds the plain text with [[double square brackets]]. To decrypt, it
/// removes the surrounding double square brackets from the fake cipher text.
/// </summary>
public sealed class FakeCrypto : ICrypto
{
    /// <summary>
    /// An implementation of the <see cref="ICrypto"/> interface meant for application testing.
    /// To encrypt, it surrounds the plain text with [[double square brackets]]. To decrypt, it
    /// removes the surrounding double square brackets from the fake cipher text.
    /// </summary>
    public FakeCrypto() { }

    string ICrypto.Encrypt(string plainText, string? credentialName) => Encrypt(plainText);

    string ICrypto.Decrypt(string cipherText, string? credentialName) => Decrypt(cipherText);

    byte[] ICrypto.Encrypt(byte[] plainText, string? credentialName) => Encrypt(plainText);

    byte[] ICrypto.Decrypt(byte[] cipherText, string? credentialName) => Decrypt(cipherText);

    IEncryptor ICrypto.GetEncryptor(string? credentialName) => new FakeEncryptor();

    IDecryptor ICrypto.GetDecryptor(string? credentialName) => new FakeDecryptor();

    bool ICrypto.CanDecrypt(string? credentialName) => true;

    bool ICrypto.CanEncrypt(string? credentialName) => true;

    private static string Decrypt(string cipherText) =>
        Regex.Replace(cipherText, @"^\[\[(.*)\]\]$", match => match.Groups[1].Value);

    private static byte[] Decrypt(byte[] cipherText)
    {
        if (cipherText.Length >= 4
            && cipherText[0] == '[' && cipherText[1] == '['
            && cipherText[cipherText.Length - 2] == ']' && cipherText[cipherText.Length - 1] == ']')
        {
            var plainText = new byte[cipherText.Length - 4];
            Array.Copy(cipherText, 2, plainText, 0, plainText.Length);
            return plainText;
        }

        return cipherText;
    }

    private static string Encrypt(string plainText) => "[[" + plainText + "]]";

    private static byte[] Encrypt(byte[] plainText)
    {
        var cipherText = new byte[plainText.Length + 4];

        Array.Copy(plainText, 0, cipherText, 2, plainText.Length);
        cipherText[0] = cipherText[1] = (int)'[';
        cipherText[cipherText.Length - 2] = cipherText[cipherText.Length - 1] = (int)']';

        return cipherText;
    }

    private class FakeDecryptor : IDecryptor
    {
        string IDecryptor.Decrypt(string cipherText) => Decrypt(cipherText);

        byte[] IDecryptor.Decrypt(byte[] cipherText) => Decrypt(cipherText);

        void IDisposable.Dispose() { }
    }

    private class FakeEncryptor : IEncryptor
    {
        void IDisposable.Dispose() { }

        string IEncryptor.Encrypt(string plainText) => Encrypt(plainText);

        byte[] IEncryptor.Encrypt(byte[] plainText) => Encrypt(plainText);
    }
}