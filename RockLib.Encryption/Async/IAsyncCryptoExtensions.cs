using System.Threading;
using System.Threading.Tasks;

namespace RockLib.Encryption.Async
{
    /// <summary>
    /// Defines extension methods for the <see cref="IAsyncCrypto"/> interface that
    /// allow the user to omit the <c>credentialName</c> parameter from its methods.
    /// </summary>
    public static class IAsyncCryptoExtensions
    {
        /// <summary>
        /// Asynchronously encrypts the specified plain text using the default credential.
        /// </summary>
        /// <param name="crypto">An <see cref="IAsyncCrypto"/>.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The encrypted value as a string.</returns>
        public static Task<string> EncryptAsync(this IAsyncCrypto crypto, string plainText, CancellationToken cancellationToken = default(CancellationToken)) =>
            crypto.EncryptAsync(plainText, null, cancellationToken);

        /// <summary>
        /// Asynchronously decrypts the specified cipher text using the default credential.
        /// </summary>
        /// <param name="crypto">An <see cref="IAsyncCrypto"/>.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The decrypted value as a string.</returns>
        public static Task<string> DecryptAsync(this IAsyncCrypto crypto, string cipherText, CancellationToken cancellationToken = default(CancellationToken)) =>
            crypto.DecryptAsync(cipherText, null, cancellationToken);

        /// <summary>
        /// Asynchronously encrypts the specified plain text using the default credential.
        /// </summary>
        /// <param name="crypto">An <see cref="IAsyncCrypto"/>.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The encrypted value as a byte array.</returns>
        public static Task<byte[]> EncryptAsync(this IAsyncCrypto crypto, byte[] plainText, CancellationToken cancellationToken = default(CancellationToken)) =>
            crypto.EncryptAsync(plainText, null, cancellationToken);

        /// <summary>
        /// Asynchronously decrypts the specified cipher text using the default credential.
        /// </summary>
        /// <param name="crypto">An <see cref="IAsyncCrypto"/>.</param>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The decrypted value as a byte array.</returns>
        public static Task<byte[]> DecryptAsync(this IAsyncCrypto crypto, byte[] cipherText, CancellationToken cancellationToken = default(CancellationToken)) =>
            crypto.DecryptAsync(cipherText, null, cancellationToken);

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
