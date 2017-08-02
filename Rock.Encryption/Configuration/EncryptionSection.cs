using System.Collections.Generic;
using RockLib.Configuration;

namespace RockLib.Encryption.Configuration
{
    public class EncryptionSection
    {
        public List<LateBoundConfigurationSection<ICrypto>> CryptoFactories { get; set; }
    }
}