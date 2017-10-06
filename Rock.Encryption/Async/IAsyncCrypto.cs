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
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>A task whose result represents the encrypted value as a string.</returns>
        Task<string> EncryptAsync(string plainText, object keyIdentifier);

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>A task whose result represents the decrypted value as a string.</returns>
        Task<string> DecryptAsync(string cipherText, object keyIdentifier);

        /// <summary>
        /// Asynchronously encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>A task whose result represents the encrypted value as a byte array.</returns>
        Task<byte[]> EncryptAsync(byte[] plainText, object keyIdentifier);

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>A task whose result represents the decrypted value as a byte array.</returns>
        Task<byte[]> DecryptAsync(byte[] cipherText, object keyIdentifier);

        /// <summary>
        /// Asynchronously gets an instance of <see cref="IAsyncEncryptor"/> for the provided
        /// encrypt key.
        /// </summary>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>
        /// A task whose result represents an object that can be used for encryption operations.
        /// </returns>
        IAsyncEncryptor GetAsyncEncryptor(object keyIdentifier);

        /// <summary>
        /// Asynchronously ets an instance of <see cref="IAsyncDecryptor"/> for the provided
        /// encrypt key.
        /// </summary>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>
        /// A task whose result represents an object that can be used for decryption operations.
        /// </returns>
        IAsyncDecryptor GetAsyncDecryptor(object keyIdentifier);

        /// <summary>
        /// Returns a value indicating whether this instance of <see cref="ICrypto"/>
        /// is able to handle the provided key identifier for an encrypt operation.
        /// </summary>
        /// <param name="keyIdentifier">The key identifier to check.</param>
        /// <returns>
        /// True, if this instance can handle the key identifier for an encrypt operation.
        /// Otherwise, false.
        /// </returns>
        bool CanEncrypt(object keyIdentifier);

        /// <summary>
        /// Returns a value indicating whether this instance of <see cref="ICrypto"/>
        /// is able to handle the provided key identifier for an decrypt operation.
        /// </summary>
        /// <param name="keyIdentifier">The key identifier to check.</param>
        /// <returns>
        /// True, if this instance can handle the key identifier for an encrypt operation.
        /// Otherwise, false.
        /// </returns>
        bool CanDecrypt(object keyIdentifier);
    }
}
