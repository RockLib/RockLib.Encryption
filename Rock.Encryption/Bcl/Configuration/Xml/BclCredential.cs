using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Rock.DataProtection.Xml;
using Rock.DataProtection;

namespace Rock.Encryption.Bcl.Configuration.Xml
{
    /// <summary>
    /// Defines an implementation of <see cref="IBclCredential"/> that is suitable for initialization
    /// via xml-deserialization.
    /// </summary>
    public class BclCredential : IBclCredential
    {
        /// <summary>
        /// Defines the default value of <see cref="BclAlgorithm"/>.
        /// </summary>
        public const BclAlgorithm DefaultAlgorithm = BclAlgorithm.Rijndael;

        /// <summary>
        /// Defines the default initialization vector size.
        /// </summary>
        public const ushort DefaultIVSize = 16;

        private readonly Lazy<byte[]> _key;
        private readonly List<string> _types = new List<string>();
        private readonly List<string> _namespaces = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BclCredential"/> class.
        /// </summary>
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


        /// <summary>
        /// Gets the <see cref="BclAlgorithm"/> that will be used for a symmetric
        /// encryption or decryption operation.
        /// </summary>
        [XmlAttribute("algorithm")]
        public BclAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Gets the size of the initialization vector that is used to add entropy to
        /// encryption or decryption operations.
        /// </summary>
        [XmlAttribute("ivsize")]
        public ushort IVSize { get; set; }

        /// <summary>
        /// Gets the <see cref="ProtectedValueProxy"/> that is used to create the an
        /// instance of <see cref="IProtectedValue"/>, which in turn is responsible
        /// for obtaining the actual symmetric key returned by the <see cref="GetKey"/>
        /// method.
        /// </summary>
        /// <remarks>
        /// This method is not intended for direct use - it exists for xml serialization purposes only.
        /// </remarks>
        [XmlElement("key")]
        public ProtectedValueProxy KeyFactory { get; set; }

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
            if (KeyFactory == null)
            {
                throw new InvalidOperationException("Key is required.");
            }

            return KeyFactory.CreateInstance().GetValue();
        }
    }
}