namespace Rock.Encryption.Bcl
{
    /// <summary>
    /// Defines an interface for retrieving instances of <see cref="IBclCredential"/> using
    /// a key identifier object.
    /// </summary>
    public interface IBclCredentialRepository
    {
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
        bool TryGet(object keyIdentifier, out IBclCredential credential);
    }
}