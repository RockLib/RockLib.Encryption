using System;
using System.Linq;
using System.Security.Cryptography;

namespace RockLib.Encryption.Symmetric;

internal static class SymmetricAlgorithmExtensions
{
    public static System.Security.Cryptography.SymmetricAlgorithm CreateSymmetricAlgorithm(
        this SymmetricAlgorithm algorithm)
    {
        return algorithm switch
        {
            SymmetricAlgorithm.Aes => Aes.Create(),
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm,
               $@"Invalid SymmetricAlgorithm. Valid values are: {
                   string.Join(", ", Enum.GetValues(typeof(SymmetricAlgorithm))
                       .Cast<SymmetricAlgorithm>().Select(x => x.ToString()))}."),
        };
    }
}