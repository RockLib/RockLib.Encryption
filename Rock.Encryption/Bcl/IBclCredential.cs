namespace Rock.Encryption.Bcl
{
    public interface IBclCredential : ICredentialInfo
    {
        BclAlgorithm Algorithm { get; }
        ushort IVSize { get; }
        byte[] GetKey();
    }
}