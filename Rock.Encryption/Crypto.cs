using System;
using System.Linq;
#if ROCKLIB
using Microsoft.Extensions.Configuration;
using RockLib.Encryption.Configuration;
using RockLib.Configuration;
using RockLib.Immutable;
using RockLib.Encryption.Async;
using System.Threading.Tasks;
#else
using Rock.Encryption.Configuration;
using Rock.Immutable;
using System.Configuration;
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
        /// <returns>The encrypted value as a string.</returns>
        public static string Encrypt(string plainText)
        {
            return Encrypt(plainText, null);
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
        public static string Encrypt(string plainText, object keyIdentifier)
        {
            return Current.Encrypt(plainText, keyIdentifier);
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>The decrypted value as a string.</returns>
        public static string Decrypt(string cipherText)
        {
            return Decrypt(cipherText, null);
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
        public static string Decrypt(string cipherText, object keyIdentifier)
        {
            return Current.Decrypt(cipherText, keyIdentifier);
        }

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The encrypted value as a byte array.</returns>
        public static byte[] Encrypt(byte[] plainText)
        {
            return Encrypt(plainText, null);
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
        public static byte[] Encrypt(byte[] plainText, object keyIdentifier)
        {
            return Current.Encrypt(plainText, keyIdentifier);
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>The decrypted value as a byte array.</returns>
        public static byte[] Decrypt(byte[] cipherText)
        {
            return Decrypt(cipherText, null);
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
        public static byte[] Decrypt(byte[] cipherText, object keyIdentifier)
        {
            return Current.Decrypt(cipherText, keyIdentifier);
        }

        /// <summary>
        /// Gets an instance of <see cref="IEncryptor"/> for the provided encrypt key.
        /// </summary>
        /// <returns>An object that can be used for encryption operations.</returns>
        public static IEncryptor GetEncryptor()
        {
            return GetEncryptor(null);
        }

        /// <summary>
        /// Gets an instance of <see cref="IEncryptor"/> for the provided encrypt key.
        /// </summary>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>An object that can be used for encryption operations.</returns>
        public static IEncryptor GetEncryptor(object keyIdentifier)
        {
            return Current.GetEncryptor(keyIdentifier);
        }

        /// <summary>
        /// Gets an instance of <see cref="IDecryptor"/> for the provided encrypt key.
        /// </summary>
        /// <returns>An object that can be used for decryption operations.</returns>
        public static IDecryptor GetDecryptor()
        {
            return GetDecryptor(null);
        }

        /// <summary>
        /// Gets an instance of <see cref="IDecryptor"/> for the provided encrypt key.
        /// </summary>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>An object that can be used for decryption operations.</returns>
        public static IDecryptor GetDecryptor(object keyIdentifier)
        {
            return Current.GetDecryptor(keyIdentifier);
        }

#if ROCKLIB
        /// <summary>
        /// Asynchronously encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The encrypted value as a string.</returns>
        public static Task<string> EncryptAsync(string plainText)
        {
            return EncryptAsync(plainText, null);
        }

        /// <summary>
        /// Asynchronously wncrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>The encrypted value as a string.</returns>
        public static Task<string> EncryptAsync(string plainText, object keyIdentifier)
        {
            return Current.AsAsync().EncryptAsync(plainText, keyIdentifier);
        }

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>The decrypted value as a string.</returns>
        public static Task<string> DecryptAsync(string cipherText)
        {
            return DecryptAsync(cipherText, null);
        }

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>The decrypted value as a string.</returns>
        public static Task<string> DecryptAsync(string cipherText, object keyIdentifier)
        {
            return Current.AsAsync().DecryptAsync(cipherText, keyIdentifier);
        }

        /// <summary>
        /// Asynchronously encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>The encrypted value as a byte array.</returns>
        public static Task<byte[]> EncryptAsync(byte[] plainText)
        {
            return EncryptAsync(plainText, null);
        }

        /// <summary>
        /// Asynchronously encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>The encrypted value as a byte array.</returns>
        public static Task<byte[]> EncryptAsync(byte[] plainText, object keyIdentifier)
        {
            return Current.AsAsync().EncryptAsync(plainText, keyIdentifier);
        }

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>The decrypted value as a byte array.</returns>
        public static Task<byte[]> DecryptAsync(byte[] cipherText)
        {
            return DecryptAsync(cipherText, null);
        }

        /// <summary>
        /// Asynchronously decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>The decrypted value as a byte array.</returns>
        public static Task<byte[]> DecryptAsync(byte[] cipherText, object keyIdentifier)
        {
            return Current.AsAsync().DecryptAsync(cipherText, keyIdentifier);
        }

        /// <summary>
        /// Gets an instance of <see cref="IAsyncEncryptor"/> for the provided encrypt key.
        /// </summary>
        /// <returns>An object that can be used for encryption operations.</returns>
        public static Task<IAsyncEncryptor> GetEncryptorAsync()
        {
            return GetEncryptorAsync(null);
        }

        /// <summary>
        /// Gets an instance of <see cref="IAsyncEncryptor"/> for the provided encrypt key.
        /// </summary>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>An object that can be used for encryption operations.</returns>
        public static Task<IAsyncEncryptor> GetEncryptorAsync(object keyIdentifier)
        {
            return Current.AsAsync().GetEncryptorAsync(keyIdentifier);
        }

        /// <summary>
        /// Asynchronously gets an instance of <see cref="IAsyncDecryptor"/> for the provided
        /// encrypt key.
        /// </summary>
        /// <returns>An object that can be used for decryption operations.</returns>
        public static Task<IAsyncDecryptor> GetDecryptorAsync()
        {
            return GetDecryptorAsync(null);
        }

        /// <summary>
        /// Asynchronously gets an instance of <see cref="IAsyncDecryptor"/> for the provided
        /// encrypt key.
        /// </summary>
        /// <param name="keyIdentifier">
        /// An implementation-specific object used to identify the key for this
        /// encryption operation.
        /// </param>
        /// <returns>An object that can be used for decryption operations.</returns>
        public static Task<IAsyncDecryptor> GetDecryptorAsync(object keyIdentifier)
        {
            return Current.AsAsync().GetDecryptorAsync(keyIdentifier);
        }
#endif

        private static ICrypto GetDefaultCrypto()
        {
#if ROCKLIB
            var section = Config.Root.GetSection("rocklib.encryption").Get<EncryptionSection>();
#else
            var section = (RockEncryptionSection)ConfigurationManager.GetSection("rock.encryption");
#endif

            if (section == null || section.CryptoFactories.Count == 0)
            {
                throw new InvalidOperationException("No crypto implementations found in config.  See the Readme.md file for details on how to setup the configuration.");
            }

            if (section.CryptoFactories.Count == 1)
            {
                return section.CryptoFactories
#if !ROCKLIB
                    .Cast<CryptoElement>()
#endif
                    .First().CreateInstance();
            }

            return new CompositeCrypto(section.CryptoFactories
#if !ROCKLIB
                .Cast<CryptoElement>()
#endif
                .Select(c => c.CreateInstance()));
        }
    }
}