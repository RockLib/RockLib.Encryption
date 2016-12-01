using Rock.Configuration;

namespace Rock.Encryption.Configuration
{
    /// <summary>
    /// A configuration element that can create instances of <see cref="ICrypto"/> objects.
    /// </summary>
    public class CryptoElement : LateBoundConfigurationElement<ICrypto>
    {
    }
}