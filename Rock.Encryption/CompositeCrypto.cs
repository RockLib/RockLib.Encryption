using System;
using System.Collections.Generic;
using System.Linq;

#if ROCKLIB
namespace RockLib.Encryption
#else
namespace Rock.Encryption
#endif
{
    /// <summary>
    /// An composite implementation of <see cref="ICrypto"/> that delegates logic to an arbitrary
    /// number of other <see cref="ICrypto"/> instances.
    /// </summary>
    public class CompositeCrypto : ICrypto
    {
        private readonly IEnumerable<ICrypto> _cryptos;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCrypto"/> class. Note that the order
        /// of items in the <paramref name="cryptos"/> parameter is significant: the first one to
        /// match a key identifier will "win".
        /// </summary>
        /// <param name="cryptos">
        /// The instances of <see cref="ICrypto"/> that this <see cref="CompositeCrypto"/> delegates
        /// logic to.
        /// </param>
        public CompositeCrypto(IEnumerable<ICrypto> cryptos)
        {
            _cryptos = cryptos as List<ICrypto> ?? cryptos.ToList();
        }

        /// <summary>
        /// Gets the instances of <see cref="ICrypto"/> that this <see cref="CompositeCrypto"/>
        /// delegates logic to.
        /// </summary>
        public IEnumerable<ICrypto> Cryptos { get { return _cryptos; } }

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
            return GetEncryptCrypto(keyIdentifier).Encrypt(plainText, keyIdentifier);
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
            return GetDecryptCrypto(keyIdentifier).Decrypt(cipherText, keyIdentifier);
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
            return GetEncryptCrypto(keyIdentifier).Encrypt(plainText, keyIdentifier);
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
            return GetDecryptCrypto(keyIdentifier).Decrypt(cipherText, keyIdentifier);
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
            return GetEncryptCrypto(keyIdentifier).GetEncryptor(keyIdentifier);
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
            return GetDecryptCrypto(keyIdentifier).GetDecryptor(keyIdentifier);
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
            return GetEncryptCrypto(keyIdentifier) != null;
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
            return GetDecryptCrypto(keyIdentifier) != null;
        }

        private ICrypto GetEncryptCrypto(object keyIdentifier)
        {
            return GetCrypto(keyIdentifier, (c, k) => c.CanEncrypt(k));
        }

        private ICrypto GetDecryptCrypto(object keyIdentifier)
        {
            return GetCrypto(keyIdentifier, (c, k) => c.CanDecrypt(k));
        }

        private ICrypto GetCrypto(object keyIdentifier, Func<ICrypto, object, bool> canGet)
        {
            var crypto = _cryptos.Where(c => canGet(c, keyIdentifier)).GetEnumerator();

            if (!crypto.MoveNext())
            {
                throw new KeyNotFoundException($"Unable to locate implementation of ICrypto that can locate a credential using keyIdentifier: {keyIdentifier}");
            }

            return crypto.Current;
        }
    }
}