using System;
using System.Collections.Generic;
using RockLib.Configuration;
using RockLib.DataProtection;

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
        /// for obtaining the actual symmetric key returned by the <see cref="GetKey()"/>
        /// method.
        /// </summary>
        /// <remarks>
        /// This method is not intended for direct use - it exists for xml serialization purposes only.
        /// </remarks>
        public LateBoundConfigurationSection<IProtectedValue> Key { get; set; }

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
            if (Key == null)
            {
                throw new InvalidOperationException("The KeyFactory property (or <key> xml element) is required, but was not provided.");
            }

            return Key.CreateInstance().GetValue();
        }
    }
}