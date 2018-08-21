using System;
using System.Collections.Generic;
using System.Linq;

namespace RockLib.Encryption.Symmetric
{
    /// <summary>
    /// An implementation of <see cref="ICredentialRepository"/> that is backed by a list
    /// of <see cref="ICredential"/> objects.
    /// </summary>
    public class CredentialRepository : ICredentialRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CredentialRepository"/> class.
        /// </summary>
        /// <param name="credentials">
        /// A collection of <see cref="ICredential"/> instances that are chosen from when
        /// invoking the <see cref="TryGet"/> method.
        /// </param>
        public CredentialRepository(IEnumerable<ICredential> credentials)
            : this(GetCredentialCache(credentials))
        {
        }

        private static CredentialCache<ICredential> GetCredentialCache(IEnumerable<ICredential> credentials)
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));

            return new CredentialCache<ICredential>(credentials.ToList());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CredentialRepository"/> class.
        /// </summary>
        /// <param name="cache">
        /// An object that caches and retrieves credential instances.
        /// </param>
        public CredentialRepository(CredentialCache<ICredential> cache)
        {
            if (cache == null) throw new ArgumentNullException(nameof(cache));

            Cache = cache;
        }

        /// <summary>
        /// Gets the object that caches and retrieves credential instances.
        /// </summary>
        public CredentialCache<ICredential> Cache { get; }

        /// <summary>
        /// Attempts to retrieve a <see cref="ICredential"/> using the specified
        /// <paramref name="keyIdentifier"/> object.
        /// </summary>
        /// <param name="keyIdentifier">
        /// An <see cref="object"/> used to look up a <see cref="ICredential"/>
        /// </param>
        /// <param name="credential">
        /// The object that will contain the retrieved <see cref="ICredential"/>. If the method returns
        /// <c>true</c>, <paramref name="credential"/> equals the retrieved value. If the method
        /// return <c>false</c>, <paramref name="credential"/> equals null.
        /// </param>
        /// <returns>
        /// <c>true</c> if a <see cref="ICredential"/> was successfullly retrieved; otherwise, <c>false</c>.
        /// </returns>
        public bool TryGet(object keyIdentifier, out ICredential credential)
        {
            return Cache.TryGetCredential(keyIdentifier, out credential);
        }
    }
}