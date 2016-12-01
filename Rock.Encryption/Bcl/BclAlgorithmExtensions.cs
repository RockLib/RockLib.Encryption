using System;
using System.Security.Cryptography;

namespace Rock.Encryption.Bcl
{
    internal static class BclAlgorithmExtensions
    {
        public static System.Security.Cryptography.SymmetricAlgorithm CreateSymmetricAlgorithm(
            this BclAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case BclAlgorithm.Aes:
                    return Aes.Create();
                case BclAlgorithm.DES:
                    return DES.Create();
                case BclAlgorithm.RC2:
                    return RC2.Create();
                case BclAlgorithm.Rijndael:
                    return Rijndael.Create();
                case BclAlgorithm.TripleDES:
                    return TripleDES.Create();
                default:
                    throw new ArgumentOutOfRangeException("algorithm");
            }
        }
    }
}