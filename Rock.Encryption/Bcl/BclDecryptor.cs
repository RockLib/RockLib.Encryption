using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Rock.Encryption.Bcl
{
    public class BclDecryptor : IDecryptor
    {
        private readonly IBclCredential _credential;
        private readonly Encoding _encoding;
        private readonly SymmetricAlgorithm _algorithm;

        public BclDecryptor(IBclCredential credential, Encoding encoding)
        {
            _credential = credential;
            _encoding = encoding;
            _algorithm = credential.Algorithm.CreateSymmetricAlgorithm();
        }

        public void Dispose()
        {
            _algorithm.Dispose();
        }

        public string Decrypt(string cipherText)
        {
            var cipherTextData = Convert.FromBase64String(cipherText);
            var plainTextData = Decrypt(cipherTextData);
            var plainText = _encoding.GetString(plainTextData);
            return plainText;
        }

        public byte[] Decrypt(byte[] cipherText)
        {
            var decrypted = new List<byte>(cipherText.Length);

            using (var stream = new MemoryStream(cipherText))
            {
                var decryptor = _algorithm.CreateDecryptor(
                    _credential.GetKey(), stream.ReadIVFromCipherTextHeader());

                using (var cryptoStream = new CryptoStream(stream, decryptor, CryptoStreamMode.Read))
                {
                    const int bufferSize = 256;
                    var buffer = new byte[bufferSize];

                    while (true)
                    {
                        var readBytes = cryptoStream.Read(buffer, 0, buffer.Length);

                        if (readBytes == bufferSize)
                        {
                            decrypted.AddRange(buffer);
                        }
                        else
                        {
                            decrypted.AddRange(buffer.Take(readBytes));
                            break;
                        }
                    }
                }
            }

            return decrypted.ToArray();
        }
    }
}