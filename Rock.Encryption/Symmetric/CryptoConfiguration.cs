using System.Collections.Generic;
using System.Text;

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
        public List<Credential> Credentials { get; } = new List<Credential>();

        /// <summary>
        /// Gets or sets the <see cref="System.Text.Encoding"/> to be used to for string/binary conversions.
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }
}