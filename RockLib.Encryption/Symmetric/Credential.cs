using System;

namespace RockLib.Encryption.Symmetric
{
    /// <summary>
    /// Defines a credential for symmetric encryption.
    /// </summary>
    public sealed class Credential : ICredentialInfo
    {
        /// <summary>
        /// Defines the default value of <see cref="SymmetricAlgorithm"/>.
        /// </summary>
        public const SymmetricAlgorithm DefaultAlgorithm = SymmetricAlgorithm.Aes;

        /// <summary>
        /// Defines the default initialization vector size.
        /// </summary>
        public const ushort DefaultIVSize = 16;

        private readonly byte[] _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="Credential"/> class.
        /// </summary>
        /// <param name="algorithm">The <see cref="SymmetricAlgorithm"/> that will be used for a symmetric encryption or decryption operation.</param>
        /// <param name="ivSize">The size of the initialization vector that is used to add entropy to encryption or decryption operations.</param>
        /// <param name="name">The name of this credential.</param>
        /// <param name="key">The symmetric key to be returned by the <see cref="GetKey()"/> method.</param>
        public Credential(byte[] key, SymmetricAlgorithm algorithm = DefaultAlgorithm, ushort ivSize = DefaultIVSize, string name = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (key.Length == 0) throw new ArgumentException($"{nameof(algorithm)} must not be empty.", nameof(key));
            if (!Enum.IsDefined(typeof(SymmetricAlgorithm), algorithm)) throw new ArgumentOutOfRangeException(nameof(algorithm), $"{nameof(algorithm)} value is not defined: {algorithm}.");
            if (ivSize <= 0) throw new ArgumentOutOfRangeException(nameof(ivSize), $"{nameof(ivSize)} must be greater than 0.");

            Algorithm = algorithm;
            IVSize = ivSize;
            Name = name;
            _key = key;
        }

        /// <summary>
        /// Gets the name of this credential.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the <see cref="SymmetricAlgorithm"/> that will be used for a symmetric
        /// encryption or decryption operation.
        /// </summary>
        public SymmetricAlgorithm Algorithm { get; }

        /// <summary>
        /// Gets the size of the initialization vector that is used to add entropy to
        /// encryption or decryption operations.
        /// </summary>
        public ushort IVSize { get; }

        /// <summary>
        /// Gets the plain-text value of the symmetric key that is used for encryption
        /// or decryption operations.
        /// </summary>
        /// <returns>The symmetric key.</returns>
        public byte[] GetKey()
        {
            var copy = new byte[_key.Length];
            _key.CopyTo(copy, 0);
            return copy;
        }
    }
}