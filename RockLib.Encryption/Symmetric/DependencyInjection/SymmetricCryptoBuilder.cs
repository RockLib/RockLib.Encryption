#if !NET451
using System;
using System.Collections.Generic;

namespace RockLib.Encryption.Symmetric.DependencyInjection
{
    /// <summary>
    /// A builder used to register a <see cref="SymmetricCrypto"/>.
    /// </summary>
    public class SymmetricCryptoBuilder
    {
        private List<SymmetricCredentialOptions> _credentialOptions = new List<SymmetricCredentialOptions>();

        /// <summary>
        /// Adds a credential to the builder.
        /// </summary>
        /// <param name="name">The name of this credential.</param>
        /// <param name="key">A symmetric key.</param>
        /// <param name="algorithm">The <see cref="SymmetricAlgorithm"/> that will be used for a symmetric encryption or decryption operation.</param>
        /// <param name="ivSize">The size of the initialization vector that is used to add entropy to encryption or decryption operations.</param>
        /// <returns>The same <see cref="SymmetricCryptoBuilder"/>.</returns>
        public SymmetricCryptoBuilder AddCredential(string name, string key, SymmetricAlgorithm algorithm = Credential.DefaultAlgorithm, ushort ivSize = Credential.DefaultIVSize)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            _credentialOptions.Add(new SymmetricCredentialOptions(name, Convert.FromBase64String(key), algorithm, ivSize));
            return this;
        }

        /// <summary>
        /// Adds a credential to the builder.
        /// </summary>
        /// <param name="name">The name of this credential.</param>
        /// <param name="key">A symmetric key.</param>
        /// <param name="algorithm">The <see cref="SymmetricAlgorithm"/> that will be used for a symmetric encryption or decryption operation.</param>
        /// <param name="ivSize">The size of the initialization vector that is used to add entropy to encryption or decryption operations.</param>
        /// <returns>The same <see cref="SymmetricCryptoBuilder"/>.</returns>
        public SymmetricCryptoBuilder AddCredential(string name, byte[] key, SymmetricAlgorithm algorithm = Credential.DefaultAlgorithm, ushort ivSize = Credential.DefaultIVSize)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            _credentialOptions.Add(new SymmetricCredentialOptions(name, key, algorithm, ivSize));
            return this;
        }

        /// <summary>
        /// Creates an instance of <see cref="ICrypto"/> using the registered credentials.
        /// </summary>
        /// <param name="serviceProvider">
        /// The <see cref="IServiceProvider"/> that retrieves the services required to create the <see cref="SymmetricCrypto"/>.
        /// </param>
        /// <returns>An instance of <see cref="SymmetricCrypto"/>.</returns>
        public SymmetricCrypto Build(IServiceProvider serviceProvider)
        {
            var credentials = new List<Credential>();

            foreach(var option in _credentialOptions)
                credentials.Add(new Credential(() => option.Key, option.Algorithm, option.IvSize, option.Name));

            return new SymmetricCrypto(credentials);
        }
    }
}
#endif