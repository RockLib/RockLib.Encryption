using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RockLib.Encryption
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
        /// match a credential name will "win".
        /// </summary>
        /// <param name="cryptos">
        /// The instances of <see cref="ICrypto"/> that this <see cref="CompositeCrypto"/> delegates
        /// logic to.
        /// </param>
        public CompositeCrypto(IEnumerable<ICrypto> cryptos)
        {
            if (cryptos is null) throw new ArgumentNullException(nameof(cryptos));

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
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The encrypted value as a string.</returns>
        public string Encrypt(string plainText, string? credentialName) => 
            GetEncryptCrypto(credentialName).Encrypt(plainText, credentialName);

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The decrypted value as a string.</returns>
        public string Decrypt(string cipherText, string? credentialName) => 
            GetDecryptCrypto(credentialName).Decrypt(cipherText, credentialName);

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The encrypted value as a byte array.</returns>
        public byte[] Encrypt(byte[] plainText, string? credentialName) => 
            GetEncryptCrypto(credentialName).Encrypt(plainText, credentialName);

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>The decrypted value as a byte array.</returns>
        public byte[] Decrypt(byte[] cipherText, string? credentialName) => 
            GetDecryptCrypto(credentialName).Decrypt(cipherText, credentialName);

        /// <summary>
        /// Gets an instance of <see cref="IEncryptor"/> for the provided credential name.
        /// </summary>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>An object that can be used for encryption operations.</returns>
        public IEncryptor GetEncryptor(string? credentialName) => 
            GetEncryptCrypto(credentialName).GetEncryptor(credentialName);

        /// <summary>
        /// Gets an instance of <see cref="IDecryptor"/> for the provided credential name.
        /// </summary>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>An object that can be used for decryption operations.</returns>
        public IDecryptor GetDecryptor(string? credentialName) => 
            GetDecryptCrypto(credentialName).GetDecryptor(credentialName);


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
        public bool CanEncrypt(string? credentialName)
        {
            try
            {
                GetEncryptCrypto(credentialName);
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
            return true;
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
        public bool CanDecrypt(string? credentialName)
        {
            try
            {
                GetDecryptCrypto(credentialName);
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
            return true;
        }

        private ICrypto GetEncryptCrypto(string? credentialName)
        {
            return GetCrypto(credentialName, (c, k) => c.CanEncrypt(k));
        }

        private ICrypto GetDecryptCrypto(string? credentialName)
        {
            return GetCrypto(credentialName, (c, k) => c.CanDecrypt(k));
        }

        private ICrypto GetCrypto(string? credentialName, Func<ICrypto, string?, bool> canGet)
        {
            var crypto = _cryptos.FirstOrDefault(c => canGet(c, credentialName));

            if (crypto is null)
            {
                throw new KeyNotFoundException($"Unable to locate implementation of ICrypto that can locate a credential using credentialName: {credentialName}");
            }

            return crypto;
        }
    }
}