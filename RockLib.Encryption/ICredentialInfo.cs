using System.Collections.Generic;

namespace RockLib.Encryption
{
    /// <summary>
    /// Defines various parameters used to search for an encrypt or decrypt
    /// credential.
    /// </summary>
    public interface ICredentialInfo
    {
        /// <summary>
        /// Gets the name that qualifies as a match for this credential info.
        /// </summary>
        string Name { get; }
    }
}