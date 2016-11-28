using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Rock.Encryption.Bcl
{
    public class BclEncryptor : IEncryptor
    {
        private readonly IBclCredential _credential;
        private readonly Encoding _encoding;
        private readonly SymmetricAlgorithm _algorithm;

        public BclEncryptor(IBclCredential credential, Encoding encoding)
        {
            _encoding = encoding;
            _credential = credential;
            _algorithm = credential.Algorithm.CreateSymmetricAlgorithm();
        }

        public void Dispose()
        {
            _algorithm.Dispose();
        }

        public string Encrypt(string plainText)
        {
            var plainTextData = _encoding.GetBytes(plainText);
            var cipherTextData = Encrypt(plainTextData);
            var cipherText = Convert.ToBase64String(cipherTextData);
            return cipherText;
        }

        public byte[] Encrypt(byte[] plainText)
        {
            using (var stream = new MemoryStream())
            {
                var iv = RNG.GetBytes(_credential.IVSize);
                var encryptor = _algorithm.CreateEncryptor(_credential.GetKey(), iv);

                stream.WriteCipherTextHeader(iv);

                using (var cryptoStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainText, 0, plainText.Length);
                }

                stream.Flush();
                return stream.ToArray();
            }
        }
    }
}