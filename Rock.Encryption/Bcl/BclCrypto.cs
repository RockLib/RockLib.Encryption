using System.Collections.Generic;
using System.Text;

namespace Rock.Encryption.Bcl
{
    public class BclCrypto : ICrypto
    {
        private readonly IBclCredentialRepository _credentialRepository;
        private readonly Encoding _encoding;

        public BclCrypto(BclCryptoConfiguration encryptionSettings)
            : this(new BclCredentialRepository(encryptionSettings.Credentials),
                encryptionSettings.Encoding)
        {
        }

        public BclCrypto(IBclCredentialRepository credentialRepository,
            Encoding encoding = null)
        {
            _credentialRepository = credentialRepository;
            _encoding = encoding ?? Encoding.UTF8;
        }

        public string Encrypt(string plainText, object keyIdentifier)
        {
            using (var encryptor = GetEncryptor(keyIdentifier))
            {
                return encryptor.Encrypt(plainText);
            }
        }

        public string Decrypt(string cipherText, object keyIdentifier)
        {
            using (var decryptor = GetDecryptor(keyIdentifier))
            {
                return decryptor.Decrypt(cipherText);
            }
        }

        public byte[] Encrypt(byte[] plainText, object keyIdentifier)
        {
            using (var encryptor = GetEncryptor(keyIdentifier))
            {
                return encryptor.Encrypt(plainText);
            }
        }

        public byte[] Decrypt(byte[] cipherText, object keyIdentifier)
        {
            using (var decryptor = GetDecryptor(keyIdentifier))
            {
                return decryptor.Decrypt(cipherText);
            }
        }

        public IEncryptor GetEncryptor(object keyIdentifier)
        {
            IBclCredential credential;
            if (!_credentialRepository.TryGet(keyIdentifier, out credential))
            {
                throw new KeyNotFoundException();
            }

            return new BclEncryptor(credential, _encoding);
        }

        public IDecryptor GetDecryptor(object keyIdentifier)
        {
            IBclCredential credential;
            if (!_credentialRepository.TryGet(keyIdentifier, out credential))
            {
                throw new KeyNotFoundException();
            }

            return new BclDecryptor(credential, _encoding);
        }

        public bool CanEncrypt(object keyIdentifier)
        {
            IBclCredential dummy;
            return _credentialRepository.TryGet(keyIdentifier, out dummy);
        }

        public bool CanDecrypt(object keyIdentifier)
        {
            return CanEncrypt(keyIdentifier);
        }
    }
}