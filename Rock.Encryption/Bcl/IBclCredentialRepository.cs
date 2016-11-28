namespace Rock.Encryption.Bcl
{
    public interface IBclCredentialRepository
    {
        bool TryGet(object keyIdentifier, out IBclCredential credential);
    }
}