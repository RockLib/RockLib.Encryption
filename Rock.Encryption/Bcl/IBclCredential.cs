namespace Rock.Encryption.Bcl
{
    /// <summary>
    /// Represents the information required to perform a symmetric encryption or
    /// decryption operation.
    /// </summary>
    public interface IBclCredential : ICredentialInfo
    {
        /// <summary>
        /// Gets the <see cref="BclAlgorithm"/> that will be used for a symmetric
        /// encryption or decryption operation.
        /// </summary>
        BclAlgorithm Algorithm { get; }

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