using System.Configuration;

namespace Rock.Encryption.Configuration
{
    /// <summary>
    /// Represents a configuration element containing a collection of child
    /// <see cref="CryptoElement"/> elements.
    /// </summary>
    [ConfigurationCollection(typeof(CryptoElement), AddItemName = "crypto")]
    public class CryptoElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new <see cref="ConfigurationElement"/>.
        /// </summary>
        /// <returns>A newly created <see cref="ConfigurationElement"/>.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CryptoElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element.
        /// </summary>
        /// <param name="element">The <see cref="ConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="object"/> that acts as the key for the specified <see cref="ConfigurationElement"/>.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CryptoElement)element).TypeAssemblyQualifiedName;
        }
    }
}