using System.Security.Cryptography;
using System.Threading;

#if ROCKLIB
namespace RockLib.Encryption.Symmetric
#else
namespace Rock.Encryption.Symmetric
# endif
{
    internal static class RNG
    {
        private static readonly ThreadLocal<RandomNumberGenerator> _instance =
            new ThreadLocal<RandomNumberGenerator>(RandomNumberGenerator.Create);

        public static byte[] GetBytes(int size)
        {
            var data = new byte[size];
            _instance.Value.GetBytes(data);
            return data;
        }
    }
}