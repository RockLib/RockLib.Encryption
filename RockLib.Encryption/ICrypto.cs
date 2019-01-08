namespace RockLib.Encryption
{
    /// <summary>
    /// Defines various encryption and decryption methods.
    /// </summary>
    public interface ICrypto
    {
        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The encrypted value as a string.</returns>
        string Encrypt(string plainText, string credentialName);

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The decrypted value as a string.</returns>
        string Decrypt(string cipherText, string credentialName);

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The encrypted value as a byte array.</returns>
        byte[] Encrypt(byte[] plainText, string credentialName);

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The decrypted value as a byte array.</returns>
        byte[] Decrypt(byte[] cipherText, string credentialName);

        /// <summary>
        /// Gets an instance of <see cref="IEncryptor"/> for the provided credential name.
        /// </summary>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>An object that can be used for encryption operations.</returns>
        IEncryptor GetEncryptor(string credentialName);

        /// <summary>
        /// Gets an instance of <see cref="IDecryptor"/> for the provided credential name.
        /// </summary>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>An object that can be used for decryption operations.</returns>
        IDecryptor GetDecryptor(string credentialName);

        /// <summary>
        /// Returns a value indicating whether this instance of <see cref="ICrypto"/>
        /// is able to handle the provided credential name for an encrypt operation.
        /// </summary>
        /// <param name="credentialName">
        /// The credential name to check, or null to check if the default credential exists.
        /// </param>
        /// <returns>
        /// True, if this instance can handle the credential name for an encrypt operation.
        /// Otherwise, false.
        /// </returns>
        bool CanEncrypt(string credentialName);

        /// <summary>
        /// Returns a value indicating whether this instance of <see cref="ICrypto"/>
        /// is able to handle the provided credential name for an decrypt operation.
        /// </summary>
        /// <param name="credentialName">
        /// The credential name to check, or null to check if the default credential exists.
        /// </param>
        /// <returns>
        /// True, if this instance can handle the credential name for an encrypt operation.
        /// Otherwise, false.
        /// </returns>
        bool CanDecrypt(string credentialName);
    }
}