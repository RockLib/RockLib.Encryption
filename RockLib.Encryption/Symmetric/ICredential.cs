
namespace RockLib.Encryption.Symmetric
{
    /// <summary>
    /// Represents the information required to perform a symmetric encryption or
    /// decryption operation.
    /// </summary>
    public interface ICredential : ICredentialInfo
    {
        /// <summary>
        /// Gets the <see cref="SymmetricAlgorithm"/> that will be used for a symmetric
        /// encryption or decryption operation.
        /// </summary>
        SymmetricAlgorithm Algorithm { get; }

        /// <summary>
        /// Gets the size of the initialization vector that is used to add entropy to
        /// encryption or decryption operations.
        /// </summary>
        ushort IVSize { get; }

        /// <summary>
        /// Gets the plain-text value of the symmetric key that is used for encryption
        /// or decryption operations.
        /// </summary>
        /// <returns>The symmetric key.</returns>
        byte[] GetKey();
    }
}