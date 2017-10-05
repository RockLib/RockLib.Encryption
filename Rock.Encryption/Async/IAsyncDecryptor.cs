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
        /// <returns>A task whose result represents the decrypted value as a string.</returns>
        Task<string> DecryptAsync(string cipherText);

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>A task whose result represents the decrypted value as a byte array.</returns>
        Task<byte[]> DecryptAsync(byte[] cipherText);
    }
}
