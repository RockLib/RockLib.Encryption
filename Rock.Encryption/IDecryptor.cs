using System;

namespace Rock.Encryption
{
    /// <summary>
    /// Defines methods for decryption.
    /// </summary>
    public interface IDecryptor : IDisposable
    {
        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>The decrypted value as a string.</returns>
        string Decrypt(string cipherText);

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>The decrypted value as a byte array.</returns>
        byte[] Decrypt(byte[] cipherText);
    }
}