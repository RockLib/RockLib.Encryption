using System;
using System.Linq;
using System.Security.Cryptography;

#if ROCKLIB
namespace RockLib.Encryption.Symmetric
#else
namespace Rock.Encryption.Symmetric
# endif
{
    internal static class SymmetricAlgorithmExtensions
    {
        public static System.Security.Cryptography.SymmetricAlgorithm CreateSymmetricAlgorithm(
            this SymmetricAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case SymmetricAlgorithm.Aes:
                    return Aes.Create();
#if !ROCKLIB
                case SymmetricAlgorithm.DES:
                    return DES.Create();
                case SymmetricAlgorithm.RC2:
                    return RC2.Create();
                case SymmetricAlgorithm.Rijndael:
                    return Rijndael.Create();
#endif
                case SymmetricAlgorithm.TripleDES:
                    return TripleDES.Create();
                default:
                    throw new ArgumentOutOfRangeException("algorithm", algorithm,
                        $@"Invalid SymmetricAlgorithm. Valid values are: {
                            string.Join(", ", Enum.GetValues(typeof(SymmetricAlgorithm))
                                .Cast<SymmetricAlgorithm>().Select(x => x.ToString()))}.");
            }
        }
    }
}