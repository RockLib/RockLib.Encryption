using System.Collections.Generic;
using System.Linq;

namespace Rock.Encryption.Bcl
{
    public class BclCredentialRepository : IBclCredentialRepository
    {
        private readonly IReadOnlyCollection<IBclCredential> _credentials;

        public BclCredentialRepository(IEnumerable<IBclCredential> credentials)
        {
            _credentials = credentials.ToList();
        }

        public IReadOnlyCollection<IBclCredential> Credentials
        {
            get { return _credentials; }
        }

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