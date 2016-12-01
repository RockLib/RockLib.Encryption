using System.Collections.Generic;
using System.Linq;

namespace Rock.Encryption.Bcl
{
    /// <summary>
    /// An implementation of <see cref="IBclCredentialRepository"/> that is backed by a list
    /// of <see cref="IBclCredential"/> objects.
    /// </summary>
    public class BclCredentialRepository : IBclCredentialRepository
    {
        private readonly IReadOnlyCollection<IBclCredential> _credentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="BclCredentialRepository"/> class.
        /// </summary>
        /// <param name="credentials">
        /// A collection of <see cref="IBclCredential"/> instances that are chosen from when
        /// invoking the <see cref="TryGet"/> method.
        /// </param>
        public BclCredentialRepository(IEnumerable<IBclCredential> credentials)
        {
            _credentials = credentials.ToList();
        }

        /// <summary>
        /// Gets the collection of <see cref="IBclCredential"/> instances that are chosen from when
        /// invoking the <see cref="TryGet"/> method.
        /// </summary>
        public IReadOnlyCollection<IBclCredential> Credentials
        {
            get { return _credentials; }
        }

        /// <summary>
        /// Attempts to retrieve a <see cref="IBclCredential"/> using the specified
        /// <paramref name="keyIdentifier"/> object.
        /// </summary>
        /// <param name="keyIdentifier">
        /// An <see cref="object"/> used to look up a <see cref="IBclCredential"/>
        /// </param>
        /// <param name="credential">
        /// The object that will contain the retrieved <see cref="IBclCredential"/>. If the method returns
        /// <c>true</c>, <paramref name="credential"/> equals the retrieved value. If the method
        /// return <c>false</c>, <paramref name="credential"/> equals null.
        /// </param>
        /// <returns>
        /// <c>true</c> if a <see cref="IBclCredential"/> was successfullly retrieved; otherwise, <c>false</c>.
        /// </returns>
        public bool TryGet(object keyIdentifier, out IBclCredential credential)
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