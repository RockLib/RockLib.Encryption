using System.Security.Cryptography;
using System.Threading;

namespace Rock.Encryption.Bcl
{
    internal static class RNG
    {
        private static readonly ThreadLocal<RNGCryptoServiceProvider> _instance =
            new ThreadLocal<RNGCryptoServiceProvider>(() => new RNGCryptoServiceProvider());

        public static byte[] GetBytes(int size)
        {
            var data = new byte[size];
            _instance.Value.GetBytes(data);
            return data;
        }
    }
}