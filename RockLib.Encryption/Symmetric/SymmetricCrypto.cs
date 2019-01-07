using System.Collections.Generic;
using System.Text;

namespace RockLib.Encryption.Symmetric
{
    /// <summary>
    /// An implementation of <see cref="ICrypto"/> that uses the symmetric encryption
    /// algorithms that are in the .NET base class library.
    /// </summary>
    public class SymmetricCrypto : ICrypto
    {
        private readonly Encoding _encoding;
        private readonly ICredentialRepository _credentialRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricCrypto"/> class.
        /// </summary>
        /// <param name="encryptionSettings">
        /// An object whose properties are the source of the
        /// <see cref="ICredentialRepository"/> and <see cref="Encoding"/> required by
        /// the <see cref="SymmetricCrypto(ICredentialRepository,Encoding)"/> constructor.
        /// </param>
        public SymmetricCrypto(CryptoConfiguration encryptionSettings)
            : this(new CredentialRepository(encryptionSettings.Credentials),
                encryptionSettings.Encoding)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricCrypto"/> class.
        /// </summary>
        /// <param name="credentialRepository">
        /// An object that can retrieve <see cref="ICredential"/> objects.
        /// </param>
        /// <param name="encoding">
        /// The <see cref="Encoding"/> that is used to convert a <c>string</c> object to a
        /// <c>byte[]</c> value.
        /// </param>
        public SymmetricCrypto(ICredentialRepository credentialRepository,
            Encoding encoding = null)
        {
            _credentialRepository = credentialRepository;
            _encoding = encoding ?? Encoding.UTF8;
        }

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The encrypted value as a string.</returns>
        public string Encrypt(string plainText, string credentialName)
        {
            using (var encryptor = GetEncryptor(credentialName))
            {
                return encryptor.Encrypt(plainText);
            }
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The decrypted value as a string.</returns>
        public string Decrypt(string cipherText, string credentialName)
        {
            using (var decryptor = GetDecryptor(credentialName))
            {
                return decryptor.Decrypt(cipherText);
            }
        }

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The encrypted value as a byte array.</returns>
        public byte[] Encrypt(byte[] plainText, string credentialName)
        {
            using (var encryptor = GetEncryptor(credentialName))
            {
                return encryptor.Encrypt(plainText);
            }
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The decrypted value as a byte array.</returns>
        public byte[] Decrypt(byte[] cipherText, string credentialName)
        {
            using (var decryptor = GetDecryptor(credentialName))
            {
                return decryptor.Decrypt(cipherText);
            }
        }

        /// <summary>
        /// Gets an instance of <see cref="IEncryptor"/> for the provided credential name.
        /// </summary>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>An object that can be used for encryption operations.</returns>
        public IEncryptor GetEncryptor(string credentialName)
        {
            ICredential credential;
            if (!_credentialRepository.TryGet(credentialName, out credential))
            {
                throw new KeyNotFoundException($"Unable to locate credential using credentialName: {credentialName}");
            }

            return new SymmetricEncryptor(credential, _encoding);
        }

        /// <summary>
        /// Gets an instance of <see cref="IDecryptor"/> for the provided credential name.
        /// </summary>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>An object that can be used for decryption operations.</returns>
        public IDecryptor GetDecryptor(string credentialName)
        {
            ICredential credential;
            if (!_credentialRepository.TryGet(credentialName, out credential))
            {
                throw new KeyNotFoundException($"Unable to locate credential using credentialName: {credentialName}");
            }

            return new SymmetricDecryptor(credential, _encoding);
        }

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
        public bool CanEncrypt(string credentialName)
        {
            ICredential dummy;
            return _credentialRepository.TryGet(credentialName, out dummy);
        }

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
        public bool CanDecrypt(string credentialName)
        {
            return CanEncrypt(credentialName);
        }
    }
}