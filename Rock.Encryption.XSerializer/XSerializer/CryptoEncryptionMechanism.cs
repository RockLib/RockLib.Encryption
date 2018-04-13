using XSerializer;
using XSerializer.Encryption;

#if ROCKLIB
namespace RockLib.Encryption.XSerializer
#else
namespace Rock.Encryption.XSerializer
#endif
{
    /// <summary>
    /// An implementation of XSerializer's <see cref="IEncryptionMechanism"/> interface
    /// that uses an instance of <see cref="ICrypto"/> to perform the encryption and
    /// decryption operations.
    /// </summary>
    public class CryptoEncryptionMechanism : IEncryptionMechanism
    {
        private readonly ICrypto _crypto;

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoEncryptionMechanism"/> class.
        /// </summary>
        /// <param name="crypto">
        /// The instance of <see cref="ICrypto"/> that is responsible for encryption
        /// and decryption operations.
        /// </param>
        public CryptoEncryptionMechanism(ICrypto crypto)
        {
            _crypto = crypto;
        }
        /// <summary>
        /// Gets the instance of <see cref="ICrypto"/> that is responsible for encryption
        /// and decryption operations.
        /// </summary>
        public ICrypto Crypto { get { return _crypto; } }

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="encryptKey">
        /// An object to used to look up invokation-specific encryption parameters.
        /// </param>
        /// <param name="serializationState">
        /// An object that holds an arbitrary value that is passed to one or more encrypt
        /// operations within a single serialization operation.
        /// </param>
        /// <returns></returns>
        public string Encrypt(string plainText, object encryptKey, SerializationState serializationState)
        {
            var encryptor =
                serializationState == null
                    ? _crypto.GetEncryptor(encryptKey)
                    : serializationState.Get(() => _crypto.GetEncryptor(encryptKey));

            var cipherText = encryptor.Encrypt(plainText);
            return cipherText;
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="encryptKey">
        /// An object to used to look up invokation-specific encryption parameters.
        /// </param>
        /// <param name="serializationState">
        /// An object that holds an arbitrary value that is passed to one or more decrypt
        /// operations within a single serialization operation.
        /// </param>
        /// <returns>The decrypted text.</returns>
        public string Decrypt(string cipherText, object encryptKey, SerializationState serializationState)
        {
            var decryptor =
                serializationState == null
                    ? _crypto.GetDecryptor(encryptKey)
                    : serializationState.Get(() => _crypto.GetDecryptor(encryptKey));

            var plainText = decryptor.Decrypt(cipherText);
            return plainText;
        }
    }
}
