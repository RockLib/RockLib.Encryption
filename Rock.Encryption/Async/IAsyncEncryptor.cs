using System.Threading.Tasks;

namespace RockLib.Encryption.Async
{
    /// <summary>
    /// Defines methods for asynchronous encryption.
    /// </summary>
    public interface IAsyncEncryptor
    {
        /// <summary>
        /// Asynchronously encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>A task whose result represents the encrypted value as a string.</returns>
        Task<string> EncryptAsync(string plainText);

        /// <summary>
        /// Asynchronously encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>A task whose result represents the encrypted value as a byte array.</returns>
        Task<byte[]> EncryptAsync(byte[] plainText);
    }
}
