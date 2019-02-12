using System.Threading;
using System.Threading.Tasks;

namespace RockLib.Encryption.Async
{
    /// <summary>
    /// Defines various asynchronous encryption and decryption methods.
    /// </summary>
    public interface IAsyncCrypto
    {
        /// <summary>
        /// Asynchronously encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task whose result represents the encrypted value as a string.</returns>
        Task<string> EncryptAsync(string plainText, string credentialName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task whose result represents the decrypted value as a string.</returns>
        Task<string> DecryptAsync(string cipherText, string credentialName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Asynchronously encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task whose result represents the encrypted value as a byte array.</returns>
        Task<byte[]> EncryptAsync(byte[] plainText, string credentialName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task whose result represents the decrypted value as a byte array.</returns>
        Task<byte[]> DecryptAsync(byte[] cipherText, string credentialName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets an instance of <see cref="IAsyncEncryptor"/> for the provided credential name.
        /// </summary>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>
        /// A task whose result represents an object that can be used for encryption operations.
        /// </returns>
        IAsyncEncryptor GetAsyncEncryptor(string credentialName);

        /// <summary>
        /// Gets an instance of <see cref="IAsyncDecryptor"/> for the provided credential name.
        /// </summary>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>
        /// A task whose result represents an object that can be used for decryption operations.
        /// </returns>
        IAsyncDecryptor GetAsyncDecryptor(string credentialName);

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
