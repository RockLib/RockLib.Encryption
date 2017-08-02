using System.Collections.Generic;

#if ROCKLIB
namespace RockLib.Encryption
#else
namespace Rock.Encryption
#endif
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

        /// <summary>
        /// Gets the types that qualify as a match for this credential info.
        /// </summary>
        IEnumerable<string> Types { get; }

        /// <summary>
        /// Gets the namespaces of types that qualify as a match for this credential info.
        /// </summary>
        IEnumerable<string> Namespaces { get; }
    }
}