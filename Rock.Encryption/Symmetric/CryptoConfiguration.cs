using System.Collections.Generic;

namespace RockLib.Encryption.Symmetric
{
    /// <summary>
    /// Defines an xml-serializable object that contains the information needed to configure an
    /// instance of <see cref="SymmetricCrypto"/>.
    /// </summary>
    public class CryptoConfiguration
    {
        /// <summary>
        /// Gets the collection of credentials that will be available for encryption or
        /// decryption operations.
        /// </summary>
        public List<ICredential> Credentials { get; set; } = new List<ICredential>();
    }
}