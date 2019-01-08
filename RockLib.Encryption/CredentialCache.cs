using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RockLib.Encryption
{
    /// <summary>
    /// Defines an object that retrieves and caches instances of the
    /// <see cref="ICredentialInfo"/> interface.
    /// </summary>
    /// <typeparam name="TCredentialInfo">The type of credential to return.</typeparam>
    public sealed class CredentialCache<TCredentialInfo>
        where TCredentialInfo : class, ICredentialInfo
    {
        private readonly ConcurrentDictionary<string, TCredentialInfo> _cache = new ConcurrentDictionary<string, TCredentialInfo>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="CredentialCache{TCredentialInfo}"/> class.
        /// </summary>
        /// <param name="credentials">The backing collection of credentials.</param>
        public CredentialCache(IEnumerable<TCredentialInfo> credentials)
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));

            Credentials = credentials.Where(c => !IsDefaultCredential(c.Name)).ToList();
            DefaultCredential = credentials.FirstOrDefault(c => IsDefaultCredential(c.Name));
        }

        /// <summary>
        /// Gets the non-default credentials.
        /// </summary>
        public IReadOnlyCollection<TCredentialInfo> Credentials { get; }

        /// <summary>
        /// Gets the default credential.
        /// </summary>
        public TCredentialInfo DefaultCredential { get; }

        /// <summary>
        /// Attempt to retrieve a credential using the provided key identifier.
        /// </summary>
        /// <param name="credentialName">An object that identifies a credential.</param>
        /// <param name="credential">
        /// Contains the resulting credential if <c>true</c> is returned. Otherwise,
        /// contains null.
        /// </param>
        /// <returns>True, if a credential could be retrieved, otherwise false.</returns>
        public bool TryGetCredential(string credentialName, out TCredentialInfo credential)
        {
            if (IsDefaultCredential(credentialName))
            {
                credential = DefaultCredential;
                return credential != null;
            }
            credential = _cache.GetOrAdd(credentialName, FindCredential);
            return credential != null;
        }

        private static bool IsDefaultCredential(string credentialName) =>
            string.IsNullOrEmpty(credentialName) || string.Equals(credentialName, "default", StringComparison.OrdinalIgnoreCase);

        private TCredentialInfo FindCredential(string credentialName) =>
            Credentials.FirstOrDefault(c => string.Equals(c.Name, credentialName, StringComparison.OrdinalIgnoreCase));
    }
}
