using System;
#if ROCKLIB
using RockLib.Configuration.ObjectFactory;
using RockLib.Configuration;
using RockLib.Immutable;
using RockLib.Encryption.Async;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
#else
using Rock.Encryption.Configuration;
using Rock.Immutable;
using System.Configuration;
using System.Linq;
#endif

#if ROCKLIB
namespace RockLib.Encryption
#else
namespace Rock.Encryption
#endif
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
        /// <param name="crypto"></param>
        /// <remarks>
        /// Each method of the <see cref="Crypto"/> class ultimately uses the value
        /// of this property and calls one of its methods.
        /// </remarks>
        public static void SetCurrent(ICrypto crypto)
        {
            _current.SetValue(() => crypto ?? GetDefaultCrypto());
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
        public static string Encrypt(string plainText, object keyIdentifier = null)
        {
            return Current.Encrypt(plainText, keyIdentifier);
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
        public static string Decrypt(string cipherText, object keyIdentifier = null)
        {
            return Current.Decrypt(cipherText, keyIdentifier);
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
        public static byte[] Encrypt(byte[] plainText, object keyIdentifier = null)
        {
            return Current.Encrypt(plainText, keyIdentifier);
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
        public static byte[] Decrypt(byte[] cipherText, object keyIdentifier = null)
        {
            return Current.Decrypt(cipherText, keyIdentifier);
        }

        /// <summary>
        /// Gets an instance of <see cref="IEncryptor"/> for the provided encrypt key.
        /// </summary>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>An object that can be used for encryption operations.</returns>
        public static IEncryptor GetEncryptor(object keyIdentifier = null)
        {
            return Current.GetEncryptor(keyIdentifier);
        }

        /// <summary>
        /// Gets an instance of <see cref="IDecryptor"/> for the provided encrypt key.
        /// </summary>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>An object that can be used for decryption operations.</returns>
        public static IDecryptor GetDecryptor(object keyIdentifier = null)
        {
            return Current.GetDecryptor(keyIdentifier);
        }

#if ROCKLIB
        /// <summary>
        /// Asynchronously wncrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The encrypted value as a string.</returns>
        public static Task<string> EncryptAsync(string plainText, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Current.AsAsync().EncryptAsync(plainText, keyIdentifier, cancellationToken);
        }

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The decrypted value as a string.</returns>
        public static Task<string> DecryptAsync(string cipherText, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Current.AsAsync().DecryptAsync(cipherText, keyIdentifier, cancellationToken);
        }

        /// <summary>
        /// Asynchronously encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The encrypted value as a byte array.</returns>
        public static Task<byte[]> EncryptAsync(byte[] plainText, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Current.AsAsync().EncryptAsync(plainText, keyIdentifier, cancellationToken);
        }

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The decrypted value as a byte array.</returns>
        public static Task<byte[]> DecryptAsync(byte[] cipherText, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Current.AsAsync().DecryptAsync(cipherText, keyIdentifier, cancellationToken);
        }

        /// <summary>
        /// Gets an instance of <see cref="IAsyncEncryptor"/> for the provided encrypt key.
        /// </summary>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>An object that can be used for encryption operations.</returns>
        public static IAsyncEncryptor GetAsyncEncryptor(object keyIdentifier = null)
        {
            return Current.AsAsync().GetAsyncEncryptor(keyIdentifier);
        }

        /// <summary>
        /// Asynchronously gets an instance of <see cref="IAsyncDecryptor"/> for the provided
        /// Asynchronously gets an instance of <see cref="IAsyncDecryptor"/> for the provided
        /// encrypt key.
        /// </summary>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>An object that can be used for decryption operations.</returns>
        public static IAsyncDecryptor GetAsyncDecryptor(object keyIdentifier = null)
        {
            return Current.AsAsync().GetAsyncDecryptor(keyIdentifier);
        }
#endif

        private static ICrypto GetDefaultCrypto()
        {
#if ROCKLIB
            var cryptos = Config.Root.GetSection("rocklib.encryption").Create<List<ICrypto>>();

            if (cryptos == null || cryptos.Count == 0)
            {
                throw new InvalidOperationException("No crypto implementations found in config.  See the Readme.md file for details on how to setup the configuration.");
            }

            if (cryptos.Count == 1)
            {
                return cryptos[0];
            }

            return new CompositeCrypto(cryptos);
#else
            var section = (RockEncryptionSection)ConfigurationManager.GetSection("rock.encryption");

            if (section == null || section.CryptoFactories == null || section.CryptoFactories.Count == 0)
            {
                throw new InvalidOperationException("No crypto implementations found in config.  See the Readme.md file for details on how to setup the configuration.");
            }

            if (section.CryptoFactories.Count == 1)
            {
                return section.CryptoFactories
                    .Cast<CryptoElement>()
                    .Select(c => c.CreateInstance())
                    .First();
            }

            return new CompositeCrypto(section.CryptoFactories.Cast<CryptoElement>().Select(c => c.CreateInstance()));
#endif
        }
    }
}