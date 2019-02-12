using RockLib.Collections;
using System;
using System.Collections.Generic;

namespace RockLib.Encryption.Symmetric
{
    /// <summary>
    /// An implementation of the <see cref="ICredentialRepository"/> interface that has
    /// a finite collection of <see cref="Credential"/> objects from which to choose.
    /// </summary>
    public class InMemoryCredentialRepository : ICredentialRepository
    {
        private readonly NamedCollection<Credential> _credentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCredentialRepository"/> class.
        /// </summary>
        /// <param name="credentials">
        /// The credentials available for encryption or decryption operations.
        /// </param>
        public InMemoryCredentialRepository(IEnumerable<Credential> credentials) =>
            _credentials = credentials?.ToNamedCollection(c => c.Name) ?? throw new ArgumentNullException(nameof(credentials));

        /// <summary>
        /// Gets the credentials available for encryption or decryption operations.
        /// </summary>
        public IReadOnlyCollection<Credential> Credentials => _credentials;

        /// <summary>
        /// Determines whether the given credential name is found in this repository.
        /// </summary>
        /// <param name="credentialName">The credential name to check.</param>
        /// <returns>
        /// <see langword="true"/>, if the credential exists; otherwise <see langword="false"/>.
        /// </returns>
        public bool ContainsCredential(string credentialName) => _credentials.Contains(credentialName);

        /// <summary>
        /// Gets the credential by name.
        /// </summary>
        /// <param name="credentialName">The name of the credential to retrieve.</param>
        /// <returns>The matching credential.</returns>
        public Credential GetCredential(string credentialName) =>
            _credentials.TryGetValue(credentialName, out var credential)
                ? credential
                : throw CredentialNotFound(credentialName);

        private Exception CredentialNotFound(string credentialName) =>
            new KeyNotFoundException(
                _credentials.IsDefaultName(credentialName)
                    ? "No default credential was found."
                    : $"The specified credential was not found: {credentialName}.");
    }
}