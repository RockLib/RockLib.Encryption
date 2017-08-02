#if ROCKLIB
namespace RockLib.Encryption.Symmetric
#else
namespace Rock.Encryption.Symmetric
# endif
{
    /// <summary>
    /// Defines the supported symmetric algorithms.
    /// </summary>
    public enum SymmetricAlgorithm
    {
        /// <summary>
        /// The Advanced Encryption Standard (AES) symmetric algorithm. Corresponds to the
        /// <see cref="System.Security.Cryptography.Aes"/> class.
        /// </summary>
        Aes,

#if !ROCKLIB
        /// <summary>
        /// The Data Encryption Standard (DES) algorithm. Corresponds to the
        /// <see cref="System.Security.Cryptography.DES"/> class.
        /// </summary>
        DES,

        /// <summary>
        /// The RC2 algorithm. Corresponds to the
        /// <see cref="System.Security.Cryptography.RC2"/> class.
        /// </summary>
        RC2,

        /// <summary>
        /// The Rijndael symmetric algorithm. Corresponds to the
        /// <see cref="System.Security.Cryptography.Rijndael"/> class.
        /// </summary>
        Rijndael,

#endif
        /// <summary>
        /// The Triple Data Encryption Standard algorithm Corresponds to the
        /// <see cref="System.Security.Cryptography.TripleDES"/> class.
        /// </summary>
        TripleDES,
    }
}