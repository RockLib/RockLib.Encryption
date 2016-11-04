using System;
using System.Collections.Generic;
using System.Linq;

namespace Rock.Encryption
{
    public class CompositeCrypto : ICrypto
    {
        private readonly IEnumerable<ICrypto> _cryptos;

        public CompositeCrypto(IEnumerable<ICrypto> cryptos)
        {
            _cryptos = cryptos as List<ICrypto> ?? cryptos.ToList();
        }

        public IEnumerable<ICrypto> Cryptos { get { return _cryptos; } }

        public string Encrypt(string plainText, object keyIdentifier)
        {
            return GetEncryptCrypto(keyIdentifier).Encrypt(plainText, keyIdentifier);
        }

        public string Decrypt(string cipherText, object keyIdentifier)
        {
            return GetDecryptCrypto(keyIdentifier).Decrypt(cipherText, keyIdentifier);
        }

        public byte[] Encrypt(byte[] plainText, object keyIdentifier)
        {
            return GetEncryptCrypto(keyIdentifier).Encrypt(plainText, keyIdentifier);
        }

        public byte[] Decrypt(byte[] cipherText, object keyIdentifier)
        {
            return GetDecryptCrypto(keyIdentifier).Decrypt(cipherText, keyIdentifier);
        }

        public IEncryptor GetEncryptor(object keyIdentifier)
        {
            return GetEncryptCrypto(keyIdentifier).GetEncryptor(keyIdentifier);
        }

        public IDecryptor GetDecryptor(object keyIdentifier)
        {
            return GetDecryptCrypto(keyIdentifier).GetDecryptor(keyIdentifier);
        }

        public bool CanEncrypt(object keyIdentifier)
        {
            return GetEncryptCrypto(keyIdentifier) != null;
        }

        public bool CanDecrypt(object keyIdentifier)
        {
            return GetDecryptCrypto(keyIdentifier) != null;
        }

        private ICrypto GetEncryptCrypto(object keyIdentifier)
        {
            return GetCrypto(keyIdentifier, (c, k) => c.CanEncrypt(k));
        }

        private ICrypto GetDecryptCrypto(object keyIdentifier)
        {
            return GetCrypto(keyIdentifier, (c, k) => c.CanDecrypt(k));
        }

        private ICrypto GetCrypto(object keyIdentifier, Func<ICrypto, object, bool> canGet)
        {
            var crypto = _cryptos.Where(c => canGet(c, keyIdentifier)).GetEnumerator();

            if (!crypto.MoveNext())
            {
                throw new Exception();
            }

            return crypto.Current;
        }
    }
}