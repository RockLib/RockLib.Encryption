using System.Collections.Generic;
using System.Text;
using Rock.Encryption.Bcl.Configuration.Xml;

namespace Rock.Encryption.Bcl
{
    /// <summary>
    /// An implementation of <see cref="ICrypto"/> that uses the symmetric encryption
    /// algorithms that are in the .NET base class library.
    /// </summary>
    public class BclCrypto : ICrypto
    {
        private readonly IBclCredentialRepository _credentialRepository;
        private readonly Encoding _encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="BclCrypto"/> class.
        /// </summary>
        /// <param name="encryptionSettings">
        /// An xml-deserializable object whose properties are the source of the
        /// <see cref="IBclCredentialRepository"/> and <see cref="Encoding"/> required by
        /// the <see cref="BclCrypto(IBclCredentialRepository,Encoding)"/> constructor.
        /// </param>
        public BclCrypto(BclCryptoConfiguration encryptionSettings)
            : this(new BclCredentialRepository(encryptionSettings.Credentials),
                encryptionSettings.Encoding)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BclCrypto"/> class.
        /// </summary>
        /// <param name="credentialRepository">
        /// An object that can retrieve <see cref="IBclCredential"/> objects.
        /// </param>
        /// <param name="encoding">
        /// The <see cref="Encoding"/> that is used to convert a <c>string</c> object to a
        /// <c>byte[]</c> value.
        /// </param>
        public BclCrypto(IBclCredentialRepository credentialRepository,
            Encoding encoding = null)
        {
            _credentialRepository = credentialRepository;
            _encoding = encoding ?? Encoding.UTF8;
        }

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>The encrypted value as a string.</returns>
        public string Encrypt(string plainText, object keyIdentifier)
        {
            using (var encryptor = GetEncryptor(keyIdentifier))
            {
                return encryptor.Encrypt(plainText);
            }
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>The decrypted value as a string.</returns>
        public string Decrypt(string cipherText, object keyIdentifier)
        {
            using (var decryptor = GetDecryptor(keyIdentifier))
            {
                return decryptor.Decrypt(cipherText);
            }
        }

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>The encrypted value as a byte array.</returns>
        public byte[] Encrypt(byte[] plainText, object keyIdentifier)
        {
            using (var encryptor = GetEncryptor(keyIdentifier))
            {
                return encryptor.Encrypt(plainText);
            }
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>The decrypted value as a byte array.</returns>
        public byte[] Decrypt(byte[] cipherText, object keyIdentifier)
        {
            using (var decryptor = GetDecryptor(keyIdentifier))
            {
                return decryptor.Decrypt(cipherText);
            }
        }

        /// <summary>
        /// Gets an instance of <see cref="IEncryptor"/> for the provided encrypt key.
        /// </summary>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>An object that can be used for encryption operations.</returns>
        public IEncryptor GetEncryptor(object keyIdentifier)
        {
            IBclCredential credential;
            if (!_credentialRepository.TryGet(keyIdentifier, out credential))
            {
                throw new KeyNotFoundException();
            }

            return new BclEncryptor(credential, _encoding);
        }

        /// <summary>
        /// Gets an instance of <see cref="IDecryptor"/> for the provided encrypt key.
        /// </summary>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>An object that can be used for decryption operations.</returns>
        public IDecryptor GetDecryptor(object keyIdentifier)
        {
            IBclCredential credential;
            if (!_credentialRepository.TryGet(keyIdentifier, out credential))
            {
                throw new KeyNotFoundException();
            }

            return new BclDecryptor(credential, _encoding);
        }

        /// <summary>
        /// Returns a value indicating whether this instance of <see cref="ICrypto"/>
        /// is able to handle the provided key identifier for an encrypt operation.
        /// </summary>
        /// <param name="keyIdentifier">The key identifier to check.</param>
        /// <returns>
        /// True, if this instance can handle the key identifier for an encrypt operation.
        /// Otherwise, false.
        /// </returns>
        public bool CanEncrypt(object keyIdentifier)
        {
            IBclCredential dummy;
            return _credentialRepository.TryGet(keyIdentifier, out dummy);
        }

        /// <summary>
        /// Returns a value indicating whether this instance of <see cref="ICrypto"/>
        /// is able to handle the provided key identifier for an decrypt operation.
        /// </summary>
        /// <param name="keyIdentifier">The key identifier to check.</param>
        /// <returns>
        /// True, if this instance can handle the key identifier for an encrypt operation.
        /// Otherwise, false.
        /// </returns>
        public bool CanDecrypt(object keyIdentifier)
        {
            return CanEncrypt(keyIdentifier);
        }
    }
}