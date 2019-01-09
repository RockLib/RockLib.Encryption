using XSerializer;
using XSerializer.Encryption;

namespace RockLib.Encryption.XSerializer
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
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <param name="serializationState">
        /// An object that holds an arbitrary value that is passed to one or more encrypt
        /// operations within a single serialization operation.
        /// </param>
        /// <returns>The encrypted text.</returns>
        public string Encrypt(string plainText, string credentialName, SerializationState serializationState)
        {
            var encryptor = serializationState.Get(() => _crypto.GetEncryptor(credentialName?.ToString()));
            var cipherText = encryptor.Encrypt(plainText);
            return cipherText;
        }

        string IEncryptionMechanism.Encrypt(string plainText, object encryptKey, SerializationState serializationState) =>
            Encrypt(plainText, encryptKey?.ToString(), serializationState);

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <param name="serializationState">
        /// An object that holds an arbitrary value that is passed to one or more decrypt
        /// operations within a single serialization operation.
        /// </param>
        /// <returns>The decrypted text.</returns>
        public string Decrypt(string cipherText, string credentialName, SerializationState serializationState)
        {
            var decryptor = serializationState.Get(() => _crypto.GetDecryptor(credentialName?.ToString()));
            var plainText = decryptor.Decrypt(cipherText);
            return plainText;
        }

        string IEncryptionMechanism.Decrypt(string cipherText, object encryptKey, SerializationState serializationState) =>
            Decrypt(cipherText, encryptKey?.ToString(), serializationState);
    }
}
