using System.Collections.Generic;
namespace RockLib.Encryption.Configuration
{
    public class EncryptionSection
    {
        public List<ICrypto> CryptoFactories { get; set; }
    }
}