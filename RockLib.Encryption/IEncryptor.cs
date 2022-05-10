using System;

namespace RockLib.Encryption;

/// <summary>
/// Defines methods for encryption.
/// </summary>
public interface IEncryptor : IDisposable
{
    /// <summary>
    /// Encrypts the specified plain text.
    /// </summary>
    /// <param name="plainText">The plain text.</param>
    /// <returns>The encrypted value as a string.</returns>
    string Encrypt(string plainText);

    /// <summary>
    /// Encrypts the specified plain text.
    /// </summary>
    /// <param name="plainText">The plain text.</param>
    /// <returns>The encrypted value as a byte array.</returns>
    byte[] Encrypt(byte[] plainText);
}