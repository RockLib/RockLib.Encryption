using System.Threading;
using System.Threading.Tasks;

namespace RockLib.Encryption.Async
{
    /// <summary>
    /// Defines methods for asynchronous decryption.
    /// </summary>
    public interface IAsyncDecryptor
    {
        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task whose result represents the decrypted value as a string.</returns>
        Task<string> DecryptAsync(string cipherText, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task whose result represents the decrypted value as a byte array.</returns>
        Task<byte[]> DecryptAsync(byte[] cipherText, CancellationToken cancellationToken = default(CancellationToken));
    }
}
