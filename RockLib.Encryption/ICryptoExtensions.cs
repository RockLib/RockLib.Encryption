using System;

namespace RockLib.Encryption
{
    /// <summary>
    /// Defines extension methods for the <see cref="ICrypto"/> interface that allow
    /// the user to omit the <c>credentialName</c> parameter from its methods.
    /// </summary>
    public static class ICryptoExtensions
    {
        /// <summary>
        /// Encrypts the specified plain text using the default credential.
        /// </summary>
        /// <param name="crypto">An <see cref="ICrypto"/>.</param>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The encrypted value as a string.</returns>
        public static string Encrypt(this ICrypto crypto, string plainText) => 
            crypto?.Encrypt(plainText, null) ?? throw new ArgumentNullException(nameof(crypto));

        /// <summary>
        /// Decrypts the specified cipher text using the default credential.
        /// </summary>
        /// <param name="crypto">An <see cref="ICrypto"/>.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>The decrypted value as a string.</returns>
        public static string Decrypt(this ICrypto crypto, string cipherText) => 
            crypto?.Decrypt(cipherText, null) ?? throw new ArgumentNullException(nameof(crypto));

        /// <summary>
        /// Encrypts the specified plain text using the default credential.
        /// </summary>
        /// <param name="crypto">An <see cref="ICrypto"/>.</param>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The encrypted value as a byte array.</returns>
        public static byte[] Encrypt(this ICrypto crypto, byte[] plainText) => 
            crypto?.Encrypt(plainText, null) ?? throw new ArgumentNullException(nameof(crypto));

        /// <summary>
        /// Decrypts the specified cipher text using the default credential.
        /// </summary>
        /// <param name="crypto">An <see cref="ICrypto"/>.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>The decrypted value as a byte array.</returns>
        public static byte[] Decrypt(this ICrypto crypto, byte[] cipherText) => 
            crypto?.Decrypt(cipherText, null) ?? throw new ArgumentNullException(nameof(crypto));

        /// <summary>
        /// Gets an instance of <see cref="IEncryptor"/> using the default credential.
        /// </summary>
        /// <param name="crypto">An <see cref="ICrypto"/>.</param>
        /// <returns>An object that can be used for encryption operations.</returns>
        public static IEncryptor GetEncryptor(this ICrypto crypto) => 
            crypto?.GetEncryptor(null) ?? throw new ArgumentNullException(nameof(crypto));

        /// <summary>
        /// Gets an instance of <see cref="IDecryptor"/> using the default credential.
        /// </summary>
        /// <param name="crypto">An <see cref="ICrypto"/>.</param>
        /// <returns>An object that can be used for decryption operations.</returns>
        public static IDecryptor GetDecryptor(this ICrypto crypto) => 
            crypto?.GetDecryptor(null) ?? throw new ArgumentNullException(nameof(crypto));
    }
}
