namespace RockLib.Encryption
{
    /// <summary>
    /// Defines information about a credential so that it can be
    /// looked up by name.
    /// </summary>
    public interface ICredentialInfo
    {
        /// <summary>
        /// Gets the name of this credential.
        /// </summary>
        string Name { get; }
    }
}