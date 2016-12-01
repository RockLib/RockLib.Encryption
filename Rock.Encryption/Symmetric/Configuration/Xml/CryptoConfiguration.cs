using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rock.Encryption.Bcl.Configuration.Xml
{
    /// <summary>
    /// Defines an xml-serializable object that contains the information needed to configure an
    /// instance of <see cref="BclCrypto"/>.
    /// </summary>
    public class BclCryptoConfiguration
    {
        private readonly List<BclCredential> _credentials = new List<BclCredential>();

        /// <summary>
        /// Gets the collection of credentials that will be available for encryption or
        /// decryption operations.
        /// </summary>
        [XmlArray("credentials")]
        [XmlArrayItem("credential")]
        public List<BclCredential> Credentials { get { return _credentials; } }

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