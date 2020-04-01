#if !NET451
using System;

namespace RockLib.Encryption.Symmetric.DependencyInjection
{
    /// <summary>
    /// Defines a symmetric credential set of options.
    /// </summary>
    public class SymmetricCredentialOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricCredentialOptions"/> class.
        /// </summary>
        /// <param name="name">The name of this credential.</param>
        /// <param name="key">A symmetric key.</param>
        /// <param name="algorithm">The <see cref="SymmetricAlgorithm"/> that will be used for a symmetric encryption or decryption operation.</param>
        /// <param name="ivSize">The size of the initialization vector that is used to add entropy to encryption or decryption operations.</param>
        public SymmetricCredentialOptions (string name, byte[] key, SymmetricAlgorithm algorithm = Credential.DefaultAlgorithm, ushort ivSize = Credential.DefaultIVSize)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            Name = name;
            Key = key;
            Algorithm = algorithm;
            IvSize = ivSize;
        }

        /// <summary>
        /// The name of this credential.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// A symmetric key.
        /// </summary>
        public byte[] Key { get; }

        /// <summary>
        /// The <see cref="SymmetricAlgorithm"/> that will be used for a symmetric encryption or decryption operation.
        /// </summary>
        public SymmetricAlgorithm Algorithm { get; }

        /// <summary>
        /// The size of the initialization vector that is used to add entropy to encryption or decryption operations.
        /// </summary>
        public ushort IvSize { get; }
    }
}
#endif
