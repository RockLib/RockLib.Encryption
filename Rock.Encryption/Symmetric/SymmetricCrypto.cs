using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RockLib.Configuration;
using RockLib.DataProtection;
using RockLib.Immutable;

#if ROCKLIB
#else
using Rock.Encryption.Symmetric.Xml;
#endif

#if ROCKLIB
namespace RockLib.Encryption.Symmetric
#else
namespace Rock.Encryption.Symmetric
#endif
{
    /// <summary>
    /// An implementation of <see cref="ICrypto"/> that uses the symmetric encryption
    /// algorithms that are in the .NET base class library.
    /// </summary>
    public class SymmetricCrypto : ICrypto
    {
        private readonly Encoding _encoding;

#if ROCKLIB
        private CryptoConfiguration _encryptionSettings;
        private readonly Semimutable<ICredentialRepository> _credentialRepositoryField = new Semimutable<ICredentialRepository>();
        // ReSharper disable once InconsistentNaming
        private ICredentialRepository _credentialRepository => _credentialRepositoryField.Value;

        public SymmetricCrypto()
        {
        }

        public CryptoConfiguration EncryptionSettings
        {
            get => _encryptionSettings;
            set
            {
                _credentialRepositoryField.Value = new CredentialRepository(value.Credentials);
                _encryptionSettings = value;
            }
        }
#else
        private readonly ICredentialRepository _credentialRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricCrypto"/> class.
        /// </summary>
        /// <param name="encryptionSettings">
        /// An xml-deserializable object whose properties are the source of the
        /// <see cref="ICredentialRepository"/> and <see cref="Encoding"/> required by
        /// the <see cref="SymmetricCrypto(ICredentialRepository,Encoding)"/> constructor.
        /// </param>
        public SymmetricCrypto(CryptoConfiguration encryptionSettings)
            : this(new CredentialRepository(encryptionSettings.Credentials),
                encryptionSettings.Encoding)
        {
        }
#endif

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
#if ROCKLIB
            _credentialRepositoryField.Value = credentialRepository;
#else
            _credentialRepository = credentialRepository;
#endif
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
            ICredential credential;
            if (!_credentialRepository.TryGet(keyIdentifier, out credential))
            {
                throw new KeyNotFoundException($"Unable to locate credential using keyIdentifier: {keyIdentifier}");
            }

            return new SymmetricEncryptor(credential, _encoding);
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
            ICredential credential;
            if (!_credentialRepository.TryGet(keyIdentifier, out credential))
            {
                throw new KeyNotFoundException($"Unable to locate credential using keyIdentifier: {keyIdentifier}");
            }

            return new SymmetricDecryptor(credential, _encoding);
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
            ICredential dummy;
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

    /// <summary>
    /// Defines an xml-serializable object that contains the information needed to configure an
    /// instance of <see cref="SymmetricCrypto"/>.
    /// </summary>
    public class CryptoConfiguration
    {
        /// <summary>
        /// Gets the collection of credentials that will be available for encryption or
        /// decryption operations.
        /// </summary>
        public List<Credential> Credentials { get; set; } = new List<Credential>();
    }


    /// <summary>
    /// Defines an implementation of <see cref="ICredential"/> that is suitable for initialization
    /// via xml-deserialization.
    /// </summary>
    public class Credential : ICredential
    {

        /// <summary>
        /// Defines the default value of <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        public const SymmetricAlgorithm DefaultAlgorithm = SymmetricAlgorithm.Aes;

        /// <summary>
        /// Defines the default initialization vector size.
        /// </summary>
        public const ushort DefaultIVSize = 16;

        private readonly Lazy<byte[]> _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="Credential"/> class.
        /// </summary>
        public Credential()
        {
            _key = new Lazy<byte[]>(LoadKey);
            Algorithm = DefaultAlgorithm;
            IVSize = DefaultIVSize;
        }

        /// <summary>
        /// Gets or sets the name that qualifies as a match for this credential info.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the types that qualify as a match for this credential info.
        /// </summary>
        public List<string> Types { get; } = new List<string>();

        IEnumerable<string> ICredentialInfo.Types => Types;

        /// <summary>
        /// Gets the namespaces of types that qualify as a match for this credential info.
        /// </summary>
        public List<string> Namespaces { get; } = new List<string>();

        IEnumerable<string> ICredentialInfo.Namespaces => Namespaces;


        /// <summary>
        /// Gets the <see cref="SymmetricAlgorithm"/> that will be used for a symmetric
        /// encryption or decryption operation.
        /// </summary>
        public SymmetricAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Gets the size of the initialization vector that is used to add entropy to
        /// encryption or decryption operations.
        /// </summary>
        public ushort IVSize { get; set; }

        /// <summary>
        /// Gets the <see cref="LateBoundConfigurationSection{T}"/> that is used to create the an
        /// instance of <see cref="IProtectedValue"/>, which in turn is responsible
        /// for obtaining the actual symmetric key returned by the <see cref="GetKey"/>
        /// method.
        /// </summary>
        /// <remarks>
        /// This method is not intended for direct use - it exists for xml serialization purposes only.
        /// </remarks>
        public LateBoundConfigurationSection<IProtectedValue> KeyFactory { get; set; }

        /// <summary>
        /// Gets the plain-text value of the symmetric key that is used for encryption
        /// or decryption operations.
        /// </summary>
        /// <returns>The symmetric key.</returns>
        public byte[] GetKey()
        {
            var key = new byte[_key.Value.Length];
            _key.Value.CopyTo(key, 0);
            return key;
        }

        private byte[] LoadKey()
        {
            if (KeyFactory == null)
            {
                throw new InvalidOperationException("The KeyFactory property (or <key> xml element) is required, but was not provided.");
            }

            return KeyFactory.CreateInstance().GetValue();
        }
    }
}