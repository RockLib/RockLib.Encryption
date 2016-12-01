using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rock.Encryption.Symmetric.Configuration.Xml
{
    /// <summary>
    /// Defines an xml-serializable object that contains the information needed to configure an
    /// instance of <see cref="SymmetricCrypto"/>.
    /// </summary>
    public class CryptoConfiguration
    {
        private readonly List<Credential> _credentials = new List<Credential>();

        /// <summary>
        /// Gets the collection of credentials that will be available for encryption or
        /// decryption operations.
        /// </summary>
        [XmlArray("credentials")]
        [XmlArrayItem("credential")]
        public List<Credential> Credentials { get { return _credentials; } }

        /// <summary>
        /// Gets or sets the <see cref="System.Text.Encoding"/> that is used to
        /// convert a <c>string</c> values to a <c>byte[]</c> prior to encryption.
        /// </summary>
        [XmlIgnore]
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets a string representation of the <see cref="Encoding"/>
        /// property. For xml serialization only - not intended for direct use.
        /// </summary>
        [XmlAttribute("encoding")]
        public string XmlAttributeEncoding
        {
            get { return Encoding == null ? null : Encoding.BodyName; }
            set { Encoding = value == null ? null : Encoding.GetEncoding(value); }
        }
    }
}