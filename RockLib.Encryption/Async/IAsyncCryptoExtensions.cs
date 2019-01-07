using System.Threading;
using System.Threading.Tasks;

namespace RockLib.Encryption.Async
{
    /// <summary>
    /// Defines extension methods for the <see cref="IAsyncCrypto"/> interface that
    /// make the <c>credentialName</c> and <c>cancellationToken</c> parameters
    /// optional for its methods.
    /// </summary>
    public static class IAsyncCryptoExtensions
    {
        /// <summary>
        /// Asynchronously encrypts the specified plain text.
        /// </summary>
        /// <param name="crypto">An <see cref="IAsyncCrypto"/>.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The encrypted value as a string.</returns>
        public static Task<string> EncryptAsync(this IAsyncCrypto crypto, string plainText, string credentialName = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            crypto.EncryptAsync(plainText, credentialName, cancellationToken);

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="crypto">An <see cref="IAsyncCrypto"/>.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The decrypted value as a string.</returns>
        public static Task<string> DecryptAsync(this IAsyncCrypto crypto, string cipherText, string credentialName = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            crypto.DecryptAsync(cipherText, credentialName, cancellationToken);

        /// <summary>
        /// Asynchronously encrypts the specified plain text.
        /// </summary>
        /// <param name="crypto">An <see cref="IAsyncCrypto"/>.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The encrypted value as a byte array.</returns>
        public static Task<byte[]> EncryptAsync(this IAsyncCrypto crypto, byte[] plainText, string credentialName = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            crypto.EncryptAsync(plainText, credentialName, cancellationToken);

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="crypto">An <see cref="IAsyncCrypto"/>.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The decrypted value as a byte array.</returns>
        public static Task<byte[]> DecryptAsync(this IAsyncCrypto crypto, byte[] cipherText, string credentialName = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            crypto.DecryptAsync(cipherText, credentialName, cancellationToken);

        /// <summary>
        /// Gets an instance of <see cref="IAsyncEncryptor"/> using the default credential.
        /// </summary>
        /// <param name="crypto">An <see cref="IAsyncCrypto"/>.</param>
        /// <returns>An object that can be used for encryption operations.</returns>
        public static IAsyncEncryptor GetAsyncEncryptor(this IAsyncCrypto crypto) =>
            crypto.GetAsyncEncryptor(null);

        /// <summary>
        /// Gets an instance of <see cref="IAsyncDecryptor"/> using the default credential.
        /// </summary>
        /// <param name="crypto">An <see cref="IAsyncCrypto"/>.</param>
        /// <returns>An object that can be used for decryption operations.</returns>
        public static IAsyncDecryptor GetAsyncDecryptor(this IAsyncCrypto crypto) =>
            crypto.GetAsyncDecryptor(null);
    }
}
