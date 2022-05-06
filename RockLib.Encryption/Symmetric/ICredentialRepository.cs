namespace RockLib.Encryption.Symmetric
{
    /// <summary>
    /// Defines an interface for retrieving symmetric credentials.
    /// </summary>
    public interface ICredentialRepository
    {
        /// <summary>
        /// Determines whether the given credential name is found in this repository.
        /// </summary>
        /// <param name="credentialName">The credential name to check.</param>
        /// <returns>
        /// <see langword="true"/>, if the credential exists; otherwise <see langword="false"/>.
        /// </returns>
        bool ContainsCredential(string? credentialName);

        /// <summary>
        /// Gets the credential by name.
        /// </summary>
        /// <param name="credentialName">The name of the credential to retrieve.</param>
        /// <returns>The matching credential.</returns>
        Credential GetCredential(string? credentialName);
    }
}