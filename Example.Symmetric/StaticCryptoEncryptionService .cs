using RockLib.Encryption;

namespace Example.Symmetric
{

    public class StaticCryptoEncryptionService : EncryptionService
    {
        public StaticCryptoEncryptionService()
            : base(Crypto.Current, "Static Crypto.Current Encryption Service")
        {
        }
    }
}
