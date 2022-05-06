using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using Formatting = Newtonsoft.Json.Formatting;

namespace RockLib.Encryption.FieldLevel
{
    /// <summary>
    /// Defines extension methods for performaing field-level encryption and deryption on XML and JSON documents.
    /// </summary>
    public static class FieldLevelEncryptionExtensions
    {
        /// <summary>
        /// Encrypts the fields, specified by XPath, that are contained in the given xml document string.
        /// </summary>
        /// <param name="xmlString">A string containing an xml document.</param>
        /// <param name="xpathToEncrypt">The XPath of the field to encrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same xml document, except with the specified fields encrypted.</returns>
        public static string EncryptXml(this string xmlString, string xpathToEncrypt, string? credentialName = null) =>
            Crypto.Current.EncryptXml(xmlString, xpathToEncrypt, credentialName);

        /// <summary>
        /// Encrypts the fields, specified by XPath, that are contained in the given xml document string.
        /// </summary>
        /// <param name="xmlString">A string containing an xml document.</param>
        /// <param name="xpathsToEncrypt">One or more XPaths of the fields to encrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same xml document, except with the specified fields encrypted.</returns>
        public static string EncryptXml(this string xmlString, IEnumerable<string> xpathsToEncrypt, string? credentialName = null) =>
            Crypto.Current.EncryptXml(xmlString, xpathsToEncrypt, credentialName);

        /// <summary>
        /// Encrypts the fields, specified by XPath, that are contained in the given xml document string.
        /// </summary>
        /// <param name="crypto">
        /// The instance of <see cref="ICrypto"/> that ultimately responsible for performing encryption operations
        /// on field values.
        /// </param>
        /// <param name="xmlString">A string containing an xml document.</param>
        /// <param name="xpathToEncrypt">The XPath of the field to encrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same xml document, except with the specified fields encrypted.</returns>
        public static string EncryptXml(this ICrypto crypto, string xmlString, string xpathToEncrypt, string? credentialName = null) =>
            crypto.EncryptXml(xmlString, new[] { xpathToEncrypt }, credentialName);

        /// <summary>
        /// Encrypts the fields, specified by XPath, that are contained in the given xml document string.
        /// </summary>
        /// <param name="crypto">
        /// The instance of <see cref="ICrypto"/> that ultimately responsible for performing encryption operations
        /// on field values.
        /// </param>
        /// <param name="xmlString">A string containing an xml document.</param>
        /// <param name="xpathsToEncrypt">One or more XPaths of the fields to encrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same xml document, except with the specified fields encrypted.</returns>
        public static string EncryptXml(this ICrypto crypto, string xmlString, IEnumerable<string> xpathsToEncrypt, string? credentialName = null)
        {
            if (crypto is null)
            {
                throw new ArgumentNullException(nameof(crypto));
            }
            if (xmlString is null)
            {
                throw new ArgumentNullException(nameof(xmlString));
            }
            if (xpathsToEncrypt is null)
            {
                throw new ArgumentNullException(nameof(xpathsToEncrypt));
            }

            var doc = new XmlDocument();
            doc.LoadXml(xmlString);
            var navigator = doc.CreateNavigator()!;

            // TODO: Why make a Lazy<> here?
            var encryptor = new Lazy<IEncryptor>(() => crypto.GetEncryptor(credentialName));

            var anyPaths = false;

            foreach (var xpath in xpathsToEncrypt)
            {
                if (xpath is null)
                {
                    throw new ArgumentException($"{nameof(xpathsToEncrypt)} cannot have null items.", nameof(xpathsToEncrypt));
                }

                anyPaths = true;

                foreach (XPathNavigator match in navigator.Select(xpath))
                {
                    // If there are any child elements, or the value contains escaped characters...
                    if (match.HasChildren && match.Value != match.InnerXml)
                    {
                        // ...encrypt the InnerXml property.
                        var plaintext = match.InnerXml;

                        // SetValue throws if the navigator has any child elements.
                        while (match.MoveToFirstChild())
                            match.DeleteSelf();

                        match.SetValue(encryptor.Value.Encrypt(plaintext));
                    }
                    else
                    {
                        // Else, this is a plain value, so encrypt the Value property.
                        match.SetValue(encryptor.Value.Encrypt(match.Value));
                    }
                }
            }

            if (!anyPaths)
            {
                throw new ArgumentException($"{nameof(xpathsToEncrypt)} must have at least one item.", nameof(xpathsToEncrypt));
            }

            return doc.OuterXml;
        }

        /// <summary>
        /// Decrypts the fields, specified by XPath, that are contained in the given xml document string.
        /// </summary>
        /// <param name="xmlString">A string containing an xml document.</param>
        /// <param name="xpathToDecrypt">The XPath of the field to decrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same xml document, except with the specified fields decrypted.</returns>
        public static string DecryptXml(this string xmlString, string xpathToDecrypt, string? credentialName = null) =>
            Crypto.Current.DecryptXml(xmlString, xpathToDecrypt, credentialName);

        /// <summary>
        /// Decrypts the fields, specified by XPath, that are contained in the given xml document string.
        /// </summary>
        /// <param name="xmlString">A string containing an xml document.</param>
        /// <param name="xpathsToDecrypt">One or more XPaths of the fields to decrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same xml document, except with the specified fields decrypted.</returns>
        public static string DecryptXml(this string xmlString, IEnumerable<string> xpathsToDecrypt, string? credentialName = null) =>
            Crypto.Current.DecryptXml(xmlString, xpathsToDecrypt, credentialName);

        /// <summary>
        /// Decrypts the fields, specified by XPath, that are contained in the given xml document string.
        /// </summary>
        /// <param name="crypto">
        /// The instance of <see cref="ICrypto"/> that ultimately responsible for performing decryption operations
        /// on field values.
        /// </param>
        /// <param name="xmlString">A string containing an xml document.</param>
        /// <param name="xpathToDecrypt">The XPath of the field to decrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same xml document, except with the specified fields decrypted.</returns>
        public static string DecryptXml(this ICrypto crypto, string xmlString, string xpathToDecrypt, string? credentialName = null) =>
            crypto.DecryptXml(xmlString, new[] { xpathToDecrypt }, credentialName);

        /// <summary>
        /// Decrypts the fields, specified by XPath, that are contained in the given xml document string.
        /// </summary>
        /// <param name="crypto">
        /// The instance of <see cref="ICrypto"/> that ultimately responsible for performing decryption operations
        /// on field values.
        /// </param>
        /// <param name="xmlString">A string containing an xml document.</param>
        /// <param name="xpathsToDecrypt">One or more XPaths of the fields to decrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same xml document, except with the specified fields decrypted.</returns>
        public static string DecryptXml(this ICrypto crypto, string xmlString, IEnumerable<string> xpathsToDecrypt, string? credentialName = null)
        {
            if (crypto is null)
            {
                throw new ArgumentNullException(nameof(crypto));
            }
            if (xmlString is null)
            {
                throw new ArgumentNullException(nameof(xmlString));
            }
            if (xpathsToDecrypt is null)
            {
                throw new ArgumentNullException(nameof(xpathsToDecrypt));
            }

            var doc = new XmlDocument();
            doc.LoadXml(xmlString);
            var navigator = doc.CreateNavigator()!;

            var decryptor = new Lazy<IDecryptor>(() => crypto.GetDecryptor(credentialName));

            var anyPaths = false;

            foreach (var xpath in xpathsToDecrypt)
            {
                if (xpath is null)
                {
                    throw new ArgumentException($"{nameof(xpathsToDecrypt)} cannot have null items.", nameof(xpathsToDecrypt));
                }

                anyPaths = true;

                foreach (XPathNavigator match in navigator.Select(xpath))
                {
                    var decrypted = decryptor.Value.Decrypt(HttpUtility.HtmlDecode(match.InnerXml));
                    if (decrypted != match.InnerXml)
                    {
                        try
                        {
                            match.InnerXml = decrypted;
                        }
                        catch (InvalidOperationException)
                        {
                            match.SetValue(decrypted);
                        }
                    }
                }
            }

            if (!anyPaths)
            {
                throw new ArgumentException($"{nameof(xpathsToDecrypt)} must have at least one item.", nameof(xpathsToDecrypt));
            }

            return doc.OuterXml;
        }

        /// <summary>
        /// Encrypts the fields, specified by JSONPath, that are contained in the given json document string.
        /// </summary>
        /// <param name="jsonString">A string containing an json document.</param>
        /// <param name="jsonPathToEncrypt">The JSONPath of the field to encrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same json document, except with the specified fields encrypted.</returns>
        public static string EncryptJson(this string jsonString, string jsonPathToEncrypt, string? credentialName = null) =>
            Crypto.Current.EncryptJson(jsonString, jsonPathToEncrypt, credentialName);

        /// <summary>
        /// Encrypts the fields, specified by JSONPath, that are contained in the given json document string.
        /// </summary>
        /// <param name="jsonString">A string containing an json document.</param>
        /// <param name="jsonPathsToEncrypt">One or more JSONPaths of the fields to encrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same json document, except with the specified fields encrypted.</returns>
        public static string EncryptJson(this string jsonString, IEnumerable<string> jsonPathsToEncrypt, string? credentialName = null) =>
            Crypto.Current.EncryptJson(jsonString, jsonPathsToEncrypt, credentialName);

        /// <summary>
        /// Encrypts the fields, specified by JSONPath, that are contained in the given json document string.
        /// </summary>
        /// <param name="crypto">
        /// The instance of <see cref="ICrypto"/> that ultimately responsible for performing encryption operations
        /// on field values.
        /// </param>
        /// <param name="jsonString">A string containing an json document.</param>
        /// <param name="jsonPathToEncrypt">The JSONPath of the field to encrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same json document, except with the specified fields encrypted.</returns>
        public static string EncryptJson(this ICrypto crypto, string jsonString, string jsonPathToEncrypt, string? credentialName = null) =>
            crypto.EncryptJson(jsonString, new[] { jsonPathToEncrypt }, credentialName);

        /// <summary>
        /// Encrypts the fields, specified by JSONPath, that are contained in the given json document string.
        /// </summary>
        /// <param name="crypto">
        /// The instance of <see cref="ICrypto"/> that ultimately responsible for performing encryption operations
        /// on field values.
        /// </param>
        /// <param name="jsonString">A string containing an json document.</param>
        /// <param name="jsonPathsToEncrypt">One or more JSONPaths of the fields to encrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same json document, except with the specified fields encrypted.</returns>
        public static string EncryptJson(this ICrypto crypto, string jsonString, IEnumerable<string> jsonPathsToEncrypt, string? credentialName = null)
        {
            if (crypto is null)
            {
                throw new ArgumentNullException(nameof(crypto));
            }
            if (jsonString is null)
            {
                throw new ArgumentNullException(nameof(jsonString));
            }
            if (jsonPathsToEncrypt is null)
            {
                throw new ArgumentNullException(nameof(jsonPathsToEncrypt));
            }

            var token = JToken.Parse(jsonString);

            var encryptor = new Lazy<IEncryptor>(() => crypto.GetEncryptor(credentialName));

            var anyPaths = false;

            foreach (var jsonPath in jsonPathsToEncrypt)
            {
                if (jsonPath is null)
                {
                    throw new ArgumentException($"{nameof(jsonPathsToEncrypt)} cannot have null items.", nameof(jsonPathsToEncrypt));
                }

                anyPaths = true;

                foreach (var match in token.SelectTokens(jsonPath).ToArray())
                {
                    var encryptedToken = JValue.CreateString(encryptor.Value.Encrypt(match.ToString(Formatting.None)));

                    if (ReferenceEquals(token, match))
                    {
                        return encryptedToken.ToString(Formatting.None);
                    }

                    switch (match.Parent)
                    {
                        case JProperty property:
                            property.Value = encryptedToken;
                            break;
                        case JArray array:
                            array[array.IndexOf(match)] = encryptedToken;
                            break;
                    }
                }
            }

            if (!anyPaths)
            {
                throw new ArgumentException($"{nameof(jsonPathsToEncrypt)} must have at least one item.", nameof(jsonPathsToEncrypt));
            }

            return token.ToString(Formatting.None);
        }

        /// <summary>
        /// Decrypts the fields, specified by JSONPath, that are contained in the given json document string.
        /// </summary>
        /// <param name="jsonString">A string containing an json document.</param>
        /// <param name="jsonPathToDecrypt">The JSONPath of the field to decrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same json document, except with the specified fields decrypted.</returns>
        public static string DecryptJson(this string jsonString, string jsonPathToDecrypt, string? credentialName = null) =>
            Crypto.Current.DecryptJson(jsonString, jsonPathToDecrypt, credentialName);

        /// <summary>
        /// Decrypts the fields, specified by JSONPath, that are contained in the given json document string.
        /// </summary>
        /// <param name="jsonString">A string containing an json document.</param>
        /// <param name="jsonPathsToDecrypt">One or more JSONPaths of the fields to decrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same json document, except with the specified fields decrypted.</returns>
        public static string DecryptJson(this string jsonString, IEnumerable<string> jsonPathsToDecrypt, string? credentialName = null) =>
            Crypto.Current.DecryptJson(jsonString, jsonPathsToDecrypt, credentialName);

        /// <summary>
        /// Decrypts the fields, specified by JSONPath, that are contained in the given json document string.
        /// </summary>
        /// <param name="crypto">
        /// The instance of <see cref="ICrypto"/> that ultimately responsible for performing decryption operations
        /// on field values.
        /// </param>
        /// <param name="jsonString">A string containing an json document.</param>
        /// <param name="jsonPathToDecrypt">The JSONPath of the field to decrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same json document, except with the specified fields decrypted.</returns>
        public static string DecryptJson(this ICrypto crypto, string jsonString, string jsonPathToDecrypt, string? credentialName = null) =>
            crypto.DecryptJson(jsonString, new[] { jsonPathToDecrypt }, credentialName);

        /// <summary>
        /// Decrypts the fields, specified by JSONPath, that are contained in the given json document string.
        /// </summary>
        /// <param name="crypto">
        /// The instance of <see cref="ICrypto"/> that ultimately responsible for performing decryption operations
        /// on field values.
        /// </param>
        /// <param name="jsonString">A string containing an json document.</param>
        /// <param name="jsonPathsToDecrypt">One or more JSONPaths of the fields to decrypt.</param>
        /// <param name="credentialName">
        /// The name of the credential to use for this encryption operation,
        /// or <c>null</c> to use the default credential.
        /// </param>
        /// <returns>The same json document, except with the specified fields decrypted.</returns>
        public static string DecryptJson(this ICrypto crypto, string jsonString, IEnumerable<string> jsonPathsToDecrypt, string? credentialName = null)
        {
            if (crypto is null)
            {
                throw new ArgumentNullException(nameof(crypto));
            }
            if (jsonString is null)
            {
                throw new ArgumentNullException(nameof(jsonString));
            }
            if (jsonPathsToDecrypt is null)
            {
                throw new ArgumentNullException(nameof(jsonPathsToDecrypt));
            }

            var token = JToken.Parse(jsonString);

            var decryptor = new Lazy<IDecryptor>(() => crypto.GetDecryptor(credentialName));

            var anyPaths = false;

            foreach (var jsonPath in jsonPathsToDecrypt)
            {
                if (jsonPath is null)
                {
                    throw new ArgumentException($"{nameof(jsonPathsToDecrypt)} cannot have null items.", nameof(jsonPathsToDecrypt));
                }

                anyPaths = true;

                foreach (var match in token.SelectTokens(jsonPath).ToArray())
                {
                    var decryptedToken = JToken.Parse(decryptor.Value.Decrypt(match.Value<string>()!));

                    if (ReferenceEquals(token, match))
                    {
                        token = decryptedToken;
                        continue;
                    }

                    switch (match.Parent)
                    {
                        case JProperty property:
                            property.Value = decryptedToken;
                            break;
                        case JArray array:
                            array[array.IndexOf(match)] = decryptedToken;
                            break;
                    }
                }
            }

            if (!anyPaths)
            {
                throw new ArgumentException($"{nameof(jsonPathsToDecrypt)} must have at least one item.", nameof(jsonPathsToDecrypt));
            }

            return token.ToString(Formatting.None);
        }
    }
}
