using RockLib.Encryption;

namespace Example.Symmetric
{
    public class DependencyInjectionEncryptionService : EncryptionService
    {
        public DependencyInjectionEncryptionService(ICrypto crypto)
            : base(crypto, "Dependency Injection Encryption Service")
        {
        }
    }
}
