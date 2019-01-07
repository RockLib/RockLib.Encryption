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
    public class CredentialCache<TCredentialInfo>
        where TCredentialInfo : class, ICredentialInfo
    {
        private readonly ConcurrentDictionary<string, TCredentialInfo> _credentialCache = new ConcurrentDictionary<string, TCredentialInfo>();
        private readonly Lazy<TCredentialInfo> _defaultCredential;

        private readonly IReadOnlyCollection<TCredentialInfo> _credentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="CredentialCache{TCredentialInfo}"/> class.
        /// </summary>
        /// <param name="credentials">The backing collection of credentials.</param>
        public CredentialCache(IReadOnlyCollection<TCredentialInfo> credentials)
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));
            _credentials = credentials;

            _defaultCredential = new Lazy<TCredentialInfo>(() => _credentials.FirstOrDefault(x => string.IsNullOrEmpty(x.Name) || x.Name.ToLower() == "default"));
        }

        /// <summary>
        /// Gets the collection of <typeparamref name="TCredentialInfo"/> instances that are chosen from when
        /// invoking the <see cref="TryGetCredential"/> method.
        /// </summary>
        public IReadOnlyCollection<TCredentialInfo> Credentials
        {
            get { return _credentials; }
        }

        /// <summary>
        /// Attempt to retrieve a credential using the provided key identifier.
        /// </summary>
        /// <param name="credentialName">An object that identifies a credential.</param>
        /// <param name="credential">
        /// Contains the resulting credential if <c>true</c> is returned. Otherwise,
        /// contains null.
        /// </param>
        /// <returns>True, if a credential could be retrieved, otherwise false.</returns>
        public virtual bool TryGetCredential(string credentialName, out TCredentialInfo credential)
        {
            if (credentialName == null)
            {
                credential = _defaultCredential.Value;
                return credential != null;
            }
            credential = _credentialCache.GetOrAdd(credentialName, FindCredential);
            return credential != null;
        }

        private TCredentialInfo FindCredential(string credentialName)
        {
            if (credentialName != null)
            {
                return _credentials.FirstOrDefault(candidate => candidate.Name == credentialName);
            }

            return _defaultCredential.Value;
        }

        private static IEnumerable<string> GetTargetNamespaces(Type targetType)
        {
            var targetNamespaces = new List<string>();

            var targetNamespace = targetType.Namespace;

            while (!string.IsNullOrEmpty(targetNamespace))
            {
                targetNamespaces.Add(targetNamespace);

                var lastDot = targetNamespace.LastIndexOf('.');

                if (lastDot == -1)
                {
                    break;
                }

                targetNamespace = targetNamespace.Substring(0, lastDot);
            }

            return targetNamespaces;
        }
    }
}
