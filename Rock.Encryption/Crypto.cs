using System;
using Rock.Immutable;

namespace Rock.Encryption
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
        /// 
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

        private static ICrypto GetDefaultCrypto()
        {
            // TODO: Load from config
            throw new NotImplementedException();
        }
    }
}