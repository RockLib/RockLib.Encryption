using System;
using RockLib.Configuration.ObjectFactory;
using RockLib.Configuration;
using RockLib.Immutable;
using RockLib.Encryption.Async;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace RockLib.Encryption
{
    /// <summary>
    /// Provides a set of static methods used for encryption and decryption
    /// operations.
    /// </summary>
    public static class Crypto
    {
        private static readonly Semimutable<ICrypto> _current = new Semimutable<ICrypto>(GetDefaultCrypto);

        /// <summary>
        /// Gets the current instance of <see cref="ICrypto"/>.
        /// </summary>
        /// <remarks>
        /// Each method of the <see cref="Crypto"/> class ultimately uses the value
        /// of this property and calls one of its methods.
        /// </remarks>
        public static ICrypto Current
        {
            get { return _current.Value; }
        }

        /// <summary>
        /// Sets the value of the <see cref="Current"/> property.
        /// </summary>
        /// <param name="crypto">
        /// The new value for the <see cref="Current"/> property, or null to set
        /// to the default <see cref="ICrypto"/>.
        /// </param>
        /// <remarks>
        /// Each method of the <see cref="Crypto"/> class ultimately uses the value
        /// of the <see cref="Current"/> property and calls one of its methods.
        /// </remarks>
        public static void SetCurrent(ICrypto crypto)
        {
            _current.SetValue(() => crypto ?? GetDefaultCrypto());
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
        public static string Encrypt(string plainText, string credentialName = null)
        {
            return Current.Encrypt(plainText, credentialName);
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
        public static string Decrypt(string cipherText, string credentialName = null)
        {
            return Current.Decrypt(cipherText, credentialName);
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
        public static byte[] Encrypt(byte[] plainText, string credentialName = null)
        {
            return Current.Encrypt(plainText, credentialName);
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
        public static byte[] Decrypt(byte[] cipherText, string credentialName = null)
        {
            return Current.Decrypt(cipherText, credentialName);
        }

        /// <summary>
        /// Gets an instance of <see cref="IEncryptor"/> for the provided key identifier.
        /// </summary>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>An object that can be used for encryption operations.</returns>
        public static IEncryptor GetEncryptor(string credentialName = null)
        {
            return Current.GetEncryptor(credentialName);
        }

        /// <summary>
        /// Gets an instance of <see cref="IDecryptor"/> for the provided key identifier.
        /// </summary>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>An object that can be used for decryption operations.</returns>
        public static IDecryptor GetDecryptor(string credentialName = null)
        {
            return Current.GetDecryptor(credentialName);
        }

        /// <summary>
        /// Asynchronously encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The encrypted value as a string.</returns>
        public static Task<string> EncryptAsync(string plainText, string credentialName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Current.AsAsync().EncryptAsync(plainText, credentialName, cancellationToken);
        }

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The decrypted value as a string.</returns>
        public static Task<string> DecryptAsync(string cipherText, string credentialName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Current.AsAsync().DecryptAsync(cipherText, credentialName, cancellationToken);
        }

        /// <summary>
        /// Asynchronously encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The encrypted value as a byte array.</returns>
        public static Task<byte[]> EncryptAsync(byte[] plainText, string credentialName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Current.AsAsync().EncryptAsync(plainText, credentialName, cancellationToken);
        }

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The decrypted value as a byte array.</returns>
        public static Task<byte[]> DecryptAsync(byte[] cipherText, string credentialName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Current.AsAsync().DecryptAsync(cipherText, credentialName, cancellationToken);
        }

        /// <summary>
        /// Gets an instance of <see cref="IAsyncEncryptor"/> for the provided key identifier.
        /// </summary>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>An object that can be used for encryption operations.</returns>
        public static IAsyncEncryptor GetAsyncEncryptor(string credentialName = null)
        {
            return Current.AsAsync().GetAsyncEncryptor(credentialName);
        }

        /// <summary>
        /// Gets an instance of <see cref="IAsyncDecryptor"/> for the provided key identifier.
        /// </summary>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or null to use the default credential.
        /// </param>
        /// <returns>An object that can be used for decryption operations.</returns>
        public static IAsyncDecryptor GetAsyncDecryptor(string credentialName = null)
        {
            return Current.AsAsync().GetAsyncDecryptor(credentialName);
        }

        private static ICrypto GetDefaultCrypto()
        {
            var cryptos = Config.Root.GetCompositeSection("rocklib_encryption", "rocklib.encryption").Create<List<ICrypto>>();

            if (cryptos == null || cryptos.Count == 0)
            {
                throw new InvalidOperationException("No crypto implementations found in config.  See the Readme.md file for details on how to setup the configuration.");
            }

            if (cryptos.Count == 1)
            {
                return cryptos[0];
            }

            return new CompositeCrypto(cryptos);
        }
    }
}