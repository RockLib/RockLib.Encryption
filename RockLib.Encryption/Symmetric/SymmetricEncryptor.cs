using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RockLib.Encryption.Symmetric;

/// <summary>
/// Defines an object that is capable of encrypting <c>string</c> values and
/// <c>byte[]</c> values using all of the encryption algorithms that are native
/// to the .NET Framework.
/// </summary>
public sealed class SymmetricEncryptor : IEncryptor
{
    private readonly Credential _credential;
    private readonly Encoding _encoding;
    private readonly System.Security.Cryptography.SymmetricAlgorithm _algorithm;

    /// <summary>
    /// Initializes a new instance of the <see cref="SymmetricEncryptor"/> class.
    /// </summary>
    /// <param name="credential">
    /// The <see cref="Credential"/> that determines what kind of encryption operations
    /// are to be performed.
    /// </param>
    /// <param name="encoding">
    /// The <see cref="Encoding"/> that is used to convert a <c>string</c> object to a
    /// <c>byte[]</c> value.
    /// </param>
    public SymmetricEncryptor(Credential credential, Encoding encoding)
    {
        _encoding = encoding;
        _credential = credential ?? throw new ArgumentNullException(nameof(credential));
        _algorithm = credential.Algorithm.CreateSymmetricAlgorithm();
    }

    /// <summary>
    /// Releases all resources used by the current instance of the
    /// <see cref="SymmetricEncryptor"/> class.
    /// </summary>
    public void Dispose()
    {
        _algorithm.Dispose();
    }

    /// <summary>
    /// Encrypts the specified plain text.
    /// </summary>
    /// <param name="plainText">The plain text.</param>
    /// <returns>The encrypted value as a string.</returns>
    public string Encrypt(string plainText)
    {
        var plainTextData = _encoding.GetBytes(plainText);
        var cipherTextData = Encrypt(plainTextData);
        return Convert.ToBase64String(cipherTextData);
    }

    /// <summary>
    /// Encrypts the specified plain text.
    /// </summary>
    /// <param name="plainText">The plain text.</param>
    /// <returns>The encrypted value as a byte array.</returns>
    public byte[] Encrypt(byte[] plainText)
    {
        if (plainText is null)
        {
            throw new ArgumentNullException(nameof(plainText));
        }

        using var stream = new MemoryStream();
        var iv = new byte[_credential.IVSize];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(iv);

        // We know the IV is generated in a non-repeatable way.
#pragma warning disable CA5401 // Do not use CreateEncryptor with non-default IV
        var encryptor = _algorithm.CreateEncryptor(_credential.GetKey(), iv);
#pragma warning restore CA5401 // Do not use CreateEncryptor with non-default IV

        stream.WriteCipherTextHeader(iv);

        using (var cryptoStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
        {
            cryptoStream.Write(plainText, 0, plainText.Length);
        }

        return stream.ToArray();
    }
}