using System.Collections.Generic;
using System.Linq;

namespace Rock.Encryption.Symmetric
{
    /// <summary>
    /// An implementation of <see cref="ICredentialRepository"/> that is backed by a list
    /// of <see cref="ICredential"/> objects.
    /// </summary>
    public class CredentialRepository : ICredentialRepository
    {
        private readonly IReadOnlyCollection<ICredential> _credentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="CredentialRepository"/> class.
        /// </summary>
        /// <param name="credentials">
        /// A collection of <see cref="ICredential"/> instances that are chosen from when
        /// invoking the <see cref="TryGet"/> method.
        /// </param>
        public CredentialRepository(IEnumerable<ICredential> credentials)
        {
            _credentials = credentials.ToList();
        }

        /// <summary>
        /// Gets the collection of <see cref="ICredential"/> instances that are chosen from when
        /// invoking the <see cref="TryGet"/> method.
        /// </summary>
        public IReadOnlyCollection<ICredential> Credentials
        {
            get { return _credentials; }
        }

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
            if (keyIdentifier == null)
            {
                credential = _credentials.FirstOrDefault(x => string.IsNullOrEmpty(x.Name) || x.Name.ToLower() == "default");
                return credential != null;
            }

            return _credentials.TryGetCredential(keyIdentifier, out credential);
        }
    }
}