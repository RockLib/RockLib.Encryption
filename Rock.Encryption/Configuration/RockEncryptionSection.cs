using System.Configuration;

namespace Rock.Encryption.Configuration
{
    /// <summary>
    /// Defines a configuration section for encryption settings.
    /// </summary>
    public class RockEncryptionSection : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the <see cref="CryptoElementCollection"/> that contains
        /// <see cref="CryptoElement"/> objects that can create instances of the
        /// <see cref="ICrypto"/> interface.
        /// </summary>
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