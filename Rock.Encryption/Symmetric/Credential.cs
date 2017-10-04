using System;
using System.Collections.Generic;

namespace RockLib.Encryption.Symmetric
{
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Credential"/> class.
        /// </summary>
        public Credential()
        {
            Algorithm = DefaultAlgorithm;
            IVSize = DefaultIVSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Credential"/> class.
        /// </summary>
        /// <param name="algorithm">The <see cref="SymmetricAlgorithm"/> that will be used for a symmetric encryption or decryption operation.</param>
        /// <param name="ivSize">The size of the initialization vector that is used to add entropy to encryption or decryption operations.</param>
        /// <param name="key">The symmetric key returned by the <see cref="GetKey()"/> method.</param>
        public Credential(SymmetricAlgorithm algorithm, ushort ivSize, byte[] key)
        {
            if (ivSize <= 0) throw new ArgumentOutOfRangeException(nameof(ivSize), $"{nameof(ivSize)} must be greater than 0");
            if (key == null) throw new ArgumentNullException(nameof(key));

            Algorithm = algorithm;
            IVSize = ivSize;
            Key = Convert.ToBase64String(key);
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
        /// Gets or sets the base-64 representation of the credential's symmetric key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets the plain-text value of the symmetric key that is used for encryption
        /// or decryption operations.
        /// </summary>
        /// <returns>The symmetric key.</returns>
        public byte[] GetKey()
        {
            if (Key == null)
            {
                throw new InvalidOperationException("The Key property (or rocklib.encryption:CryptoFactories:0:Value:EncryptionSettings:Credentials:0:Key configuration element) is required, but was not provided.");
            }

            return Convert.FromBase64String(Key);
        }
    }
}