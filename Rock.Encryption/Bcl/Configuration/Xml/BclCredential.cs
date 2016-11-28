using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Rock.DataProtection.Xml;

namespace Rock.Encryption.Bcl.Configuration.Xml
{
    public class BclCredential : IBclCredential
    {
        public const BclAlgorithm DefaultAlgorithm = BclAlgorithm.Rijndael;
        public const ushort DefaultIVSize = 16;

        private readonly Lazy<byte[]> _key;
        private readonly List<string> _types = new List<string>();
        private readonly List<string> _namespaces = new List<string>();

        public BclCredential()
        {
            _key = new Lazy<byte[]>(LoadKey);
            Algorithm = DefaultAlgorithm;
            IVSize = DefaultIVSize;
        }

        /// <summary>
        /// Gets or sets the name that qualifies as a match for this credential info.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets the types that qualify as a match for this credential info.
        /// </summary>
        [XmlArray("types")]
        [XmlArrayItem("type")]
        public List<string> Types { get { return _types; } }
        IEnumerable<string> ICredentialInfo.Types { get { return Types; } }

        /// <summary>
        /// Gets the namespaces of types that qualify as a match for this credential info.
        /// </summary>
        [XmlArray("namespaces")]
        [XmlArrayItem("namespace")]
        public List<string> Namespaces { get { return _namespaces; } }
        IEnumerable<string> ICredentialInfo.Namespaces { get { return Namespaces; } }

        [XmlAttribute("algorithm")]
        public BclAlgorithm Algorithm { get; set; }

        [XmlAttribute("ivsize")]
        public ushort IVSize { get; set; }

        [XmlElement("key")]
        public ProtectedValueProxy KeyFactory { get; set; }

        public byte[] GetKey()
        {
            var key = new byte[_key.Value.Length];
            _key.Value.CopyTo(key, 0);
            return key;
        }

        private byte[] LoadKey()
        {
            if (KeyFactory == null)
            {
                throw new InvalidOperationException("Key is required.");
            }

            return KeyFactory.CreateInstance().GetValue();
        }
    }
}