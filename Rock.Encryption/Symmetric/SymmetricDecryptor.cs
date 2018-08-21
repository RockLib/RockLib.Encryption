using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RockLib.Encryption.Symmetric
{
    /// <summary>
    /// Defines an object that is capable of decrypting <c>string</c> values and
    /// <c>byte[]</c> values using all of the encryption algorithms that are native
    /// to the .NET Framework.
    /// </summary>
    public class SymmetricDecryptor : IDecryptor
    {
        private readonly ICredential _credential;
        private readonly Encoding _encoding;
        private readonly System.Security.Cryptography.SymmetricAlgorithm _algorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricDecryptor"/> class.
        /// </summary>
        /// <param name="credential">
        /// The <see cref="ICredential"/> that determines what kind of encryption operations
        /// are to be performed.
        /// </param>
        /// <param name="encoding">
        /// The <see cref="Encoding"/> that is used to convert a <c>string</c> object to a
        /// <c>byte[]</c> value.
        /// </param>
        public SymmetricDecryptor(ICredential credential, Encoding encoding)
        {
            _credential = credential;
            _encoding = encoding;
            _algorithm = credential.Algorithm.CreateSymmetricAlgorithm();
        }

        /// <summary>
        /// Releases all resources used by the current instance of the
        /// <see cref="SymmetricDecryptor"/> class.
        /// </summary>
        public void Dispose()
        {
            _algorithm.Dispose();
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>The decrypted value as a string.</returns>
        public string Decrypt(string cipherText)
        {
            if (!IsBase64(cipherText))
                return cipherText;

            var cipherTextData = Convert.FromBase64String(cipherText);
            var plainTextData = Decrypt(cipherTextData);
            var plainText = _encoding.GetString(plainTextData);
            return plainText;
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns>The decrypted value as a byte array.</returns>
        public byte[] Decrypt(byte[] cipherText)
        {
            if (!cipherText.IsEncrypted())
                return cipherText;

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

        private static readonly Regex SpaceRegex = new Regex(@"\s");
        private static readonly Regex Base64Regex = new Regex(@"^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$");

        private static bool IsBase64(string base64)
        {
            if (SpaceRegex.IsMatch(base64)) base64 = SpaceRegex.Replace(base64, "");
            if (base64.Length % 4 != 0) return false;
            return Base64Regex.IsMatch(base64);
        }
    }
}