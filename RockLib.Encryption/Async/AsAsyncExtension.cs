using System.Runtime.CompilerServices;

namespace RockLib.Encryption.Async
{
    /// <summary>
    /// Defines an extension method to convert <see cref="ICrypto"/> to <see cref="IAsyncCrypto"/>.
    /// </summary>
    public static class AsAsyncExtension
    {
        private static readonly ConditionalWeakTable<ICrypto, IAsyncCrypto> _table = new ConditionalWeakTable<ICrypto, IAsyncCrypto>();

        /// <summary>
        /// Returns the input as <see cref="IAsyncCrypto"/>.
        /// <para>If the <paramref name="crypto"/> parameter implements the <see cref="IAsyncCrypto"/>
        /// interface, then it is cast and returned. If the <paramref name="crypto"/> parameter does
        /// *not* implement the <see cref="IAsyncCrypto"/> interface, then an instance of
        /// <see cref="SynchronousAsyncCrypto"/> is returned. Note that subsequent calls to this
        /// method with the same instance of <see cref="ICrypto"/> will return the same instance of
        /// <see cref="SynchronousAsyncCrypto"/>.</para>
        /// </summary>
        /// <param name="crypto">
        /// An instance of <see cref="ICrypto"/> to be converted to <see cref="IAsyncCrypto"/>
        /// </param>
        /// <returns>
        /// An instance of <see cref="IAsyncCrypto"/> that corresponds to the <paramref name="crypto"/> parameter.
        /// </returns>
        public static IAsyncCrypto AsAsync(this ICrypto crypto)
        {
            return crypto as IAsyncCrypto
                ?? _table.GetValue(crypto, c => new SynchronousAsyncCrypto(c));
        }
    }
}
