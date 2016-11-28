using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rock.Encryption.Bcl.Configuration.Xml
{
    public class BclCryptoConfiguration
    {
        private readonly List<BclCredential> _credentials = new List<BclCredential>();

        [XmlArray("credentials")]
        [XmlArrayItem("credential")]
        public List<BclCredential> Credentials { get { return _credentials; } }

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