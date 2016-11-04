using System.Configuration;

namespace Rock.Encryption.Configuration
{
    public class RockEncryptionSection : ConfigurationSection
    {
        [ConfigurationProperty("settings", IsDefaultCollection = true)]
        public CryptoElementCollection CryptoFactories
        {
            get
            {
                return (CryptoElementCollection)base["settings"];
            }
            set
            {
                this["settings"] = value;
            }
        }
    }
}