using System.Configuration;

namespace Rock.Encryption.Configuration
{
    [ConfigurationCollection(typeof(CryptoElement), AddItemName = "crypto")]
    public class CryptoElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CryptoElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CryptoElement)element).TypeAssemblyQualifiedName;
        }
    }
}