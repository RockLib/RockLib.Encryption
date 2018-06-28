using Newtonsoft.Json.Linq;
using RockLib.Encryption.Async;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace RockLib.Encryption.FieldLevel
{
    public static class FieldLevelEncryptionExtensions
    {
        public static string EncryptXml(this string xmlString, string xpathToEncrypt, object keyIdentifier = null) =>
            Crypto.Current.EncryptXml(xmlString, xpathToEncrypt, keyIdentifier);

        public static string EncryptXml(this string xmlString, IEnumerable<string> xpathsToEncrypt, object keyIdentifier = null) =>
            Crypto.Current.EncryptXml(xmlString, xpathsToEncrypt, keyIdentifier);

        public static string EncryptXml(this ICrypto crypto, string xmlString, string xpathToEncrypt, object keyIdentifier = null) =>
            crypto.EncryptXml(xmlString, new[] { xpathToEncrypt }, keyIdentifier);

        public static string EncryptXml(this ICrypto crypto, string xmlString, IEnumerable<string> xpathsToEncrypt, object keyIdentifier = null)
        {
            if (crypto == null) throw new ArgumentNullException(nameof(crypto));
            if (xmlString == null) throw new ArgumentNullException(nameof(xmlString));
            if (xpathsToEncrypt == null) throw new ArgumentNullException(nameof(xpathsToEncrypt));

            var doc = new XmlDocument();
            doc.LoadXml(xmlString);
            var navigator = doc.CreateNavigator();

            var encryptor = new Lazy<IEncryptor>(() => crypto.GetEncryptor(keyIdentifier));

            var anyPaths = false;

            foreach (var xpath in xpathsToEncrypt)
            {
                if (xpath == null)
                    throw new ArgumentException($"{nameof(xpathsToEncrypt)} cannot have null items.", nameof(xpathsToEncrypt));

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
                throw new ArgumentException($"{nameof(xpathsToEncrypt)} must have at least one item.", nameof(xpathsToEncrypt));

            return doc.OuterXml;
        }

        public static Task<string> EncryptXmlAsync(this string xmlString, string xpathToEncrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            Crypto.Current.EncryptXmlAsync(xmlString, xpathToEncrypt, keyIdentifier, cancellationToken);

        public static Task<string> EncryptXmlAsync(this string xmlString, IEnumerable<string> xpathsToEncrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            Crypto.Current.EncryptXmlAsync(xmlString, xpathsToEncrypt, keyIdentifier, cancellationToken);

        public static Task<string> EncryptXmlAsync(this ICrypto crypto, string xmlString, string xpathToEncrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            crypto.EncryptXmlAsync(xmlString, new[] { xpathToEncrypt }, keyIdentifier, cancellationToken);

        public static async Task<string> EncryptXmlAsync(this ICrypto crypto, string xmlString, IEnumerable<string> xpathsToEncrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (crypto == null) throw new ArgumentNullException(nameof(crypto));
            if (xmlString == null) throw new ArgumentNullException(nameof(xmlString));
            if (xpathsToEncrypt == null) throw new ArgumentNullException(nameof(xpathsToEncrypt));

            var doc = new XmlDocument();
            doc.LoadXml(xmlString);
            var navigator = doc.CreateNavigator();

            var encryptor = new Lazy<IAsyncEncryptor>(() => crypto.AsAsync().GetAsyncEncryptor(keyIdentifier));

            var anyPaths = false;

            foreach (var xpath in xpathsToEncrypt)
            {
                if (xpath == null)
                    throw new ArgumentException($"{nameof(xpathsToEncrypt)} cannot have null items.", nameof(xpathsToEncrypt));

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

                        match.SetValue(await encryptor.Value.EncryptAsync(plaintext, cancellationToken).ConfigureAwait(false));
                    }
                    else
                    {
                        // Else, this is a plain value, so encrypt the Value property.
                        match.SetValue(await encryptor.Value.EncryptAsync(match.Value, cancellationToken).ConfigureAwait(false));
                    }
                }
            }

            if (!anyPaths)
                throw new ArgumentException($"{nameof(xpathsToEncrypt)} must have at least one item.", nameof(xpathsToEncrypt));

            return doc.OuterXml;
        }

        public static string DecryptXml(this string xmlString, string xpathToDecrypt, object keyIdentifier = null) =>
            Crypto.Current.DecryptXml(xmlString, xpathToDecrypt, keyIdentifier);

        public static string DecryptXml(this string xmlString, IEnumerable<string> xpathsToDecrypt, object keyIdentifier = null) =>
            Crypto.Current.DecryptXml(xmlString, xpathsToDecrypt, keyIdentifier);

        public static string DecryptXml(this ICrypto crypto, string xmlString, string xpathToDecrypt, object keyIdentifier = null) =>
            crypto.DecryptXml(xmlString, new[] { xpathToDecrypt }, keyIdentifier);

        public static string DecryptXml(this ICrypto crypto, string xmlString, IEnumerable<string> xpathsToDecrypt, object keyIdentifier = null)
        {
            if (crypto == null) throw new ArgumentNullException(nameof(crypto));
            if (xmlString == null) throw new ArgumentNullException(nameof(xmlString));
            if (xpathsToDecrypt == null) throw new ArgumentNullException(nameof(xpathsToDecrypt));

            var doc = new XmlDocument();
            doc.LoadXml(xmlString);
            var navigator = doc.CreateNavigator();

            var decryptor = new Lazy<IDecryptor>(() => crypto.GetDecryptor(keyIdentifier));

            var anyPaths = false;

            foreach (var xpath in xpathsToDecrypt)
            {
                if (xpath == null)
                    throw new ArgumentException($"{nameof(xpathsToDecrypt)} cannot have null items.", nameof(xpathsToDecrypt));

                anyPaths = true;

                foreach (XPathNavigator match in navigator.Select(xpath))
                {
                    var decrypted = decryptor.Value.Decrypt(match.InnerXml);
                    if (decrypted != match.InnerXml)
                    {
                        try
                        {
                            match.InnerXml = decrypted;
                        }
                        catch
                        {
                            match.SetValue(decrypted);
                        }
                    }
                }
            }

            if (!anyPaths)
                throw new ArgumentException($"{nameof(xpathsToDecrypt)} must have at least one item.", nameof(xpathsToDecrypt));

            return doc.OuterXml;
        }

        public static Task<string> DecryptXmlAsync(this string xmlString, string xpathToDecrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            Crypto.Current.DecryptXmlAsync(xmlString, xpathToDecrypt, keyIdentifier, cancellationToken);

        public static Task<string> DecryptXmlAsync(this string xmlString, IEnumerable<string> xpathsToDecrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            Crypto.Current.DecryptXmlAsync(xmlString, xpathsToDecrypt, keyIdentifier, cancellationToken);

        public static Task<string> DecryptXmlAsync(this ICrypto crypto, string xmlString, string xpathToDecrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            crypto.DecryptXmlAsync(xmlString, new[] { xpathToDecrypt }, keyIdentifier, cancellationToken);

        public static async Task<string> DecryptXmlAsync(this ICrypto crypto, string xmlString, IEnumerable<string> xpathsToDecrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (crypto == null) throw new ArgumentNullException(nameof(crypto));
            if (xmlString == null) throw new ArgumentNullException(nameof(xmlString));
            if (xpathsToDecrypt == null) throw new ArgumentNullException(nameof(xpathsToDecrypt));

            var doc = new XmlDocument();
            doc.LoadXml(xmlString);
            var navigator = doc.CreateNavigator();

            var decryptor = new Lazy<IAsyncDecryptor>(() => crypto.AsAsync().GetAsyncDecryptor(keyIdentifier));

            var anyPaths = false;

            foreach (var xpath in xpathsToDecrypt)
            {
                if (xpath == null)
                    throw new ArgumentException($"{nameof(xpathsToDecrypt)} cannot have null items.", nameof(xpathsToDecrypt));

                anyPaths = true;

                foreach (XPathNavigator match in navigator.Select(xpath))
                {
                    var decrypted = await decryptor.Value.DecryptAsync(match.InnerXml, cancellationToken).ConfigureAwait(false);
                    if (decrypted != match.InnerXml)
                    {
                        try
                        {
                            match.InnerXml = decrypted;
                        }
                        catch
                        {
                            match.SetValue(decrypted);
                        }
                    }
                }
            }

            if (!anyPaths)
                throw new ArgumentException($"{nameof(xpathsToDecrypt)} must have at least one item.", nameof(xpathsToDecrypt));

            return doc.OuterXml;
        }

        public static string EncryptJson(this string jsonString, string jsonPathToEncrypt, object keyIdentifier = null) =>
            Crypto.Current.EncryptJson(jsonString, jsonPathToEncrypt, keyIdentifier);

        public static string EncryptJson(this string jsonString, IEnumerable<string> jsonPathsToEncrypt, object keyIdentifier = null) =>
            Crypto.Current.EncryptJson(jsonString, jsonPathsToEncrypt, keyIdentifier);

        public static string EncryptJson(this ICrypto crypto, string jsonString, string jsonPathToEncrypt, object keyIdentifier = null) =>
            crypto.EncryptJson(jsonString, new[] { jsonPathToEncrypt }, keyIdentifier);

        public static string EncryptJson(this ICrypto crypto, string jsonString, IEnumerable<string> jsonPathsToEncrypt, object keyIdentifier = null)
        {
            if (crypto == null) throw new ArgumentNullException(nameof(crypto));
            if (jsonString == null) throw new ArgumentNullException(nameof(jsonString));
            if (jsonPathsToEncrypt == null) throw new ArgumentNullException(nameof(jsonPathsToEncrypt));

            var jObject = JObject.Parse(jsonString);

            var encryptor = new Lazy<IEncryptor>(() => crypto.GetEncryptor(keyIdentifier));

            var anyPaths = false;

            foreach (var jsonPath in jsonPathsToEncrypt)
            {
                if (jsonPath == null)
                    throw new ArgumentException($"{nameof(jsonPathsToEncrypt)} cannot have null items.", nameof(jsonPathsToEncrypt));

                anyPaths = true;

                foreach (var match in jObject.SelectTokens(jsonPath))
                {
                    var property = (JProperty)match.Parent;
                    property.Value = encryptor.Value.Encrypt(match.ToString(Newtonsoft.Json.Formatting.None));
                }
            }

            if (!anyPaths)
                throw new ArgumentException($"{nameof(jsonPathsToEncrypt)} must have at least one item.", nameof(jsonPathsToEncrypt));

            return jObject.ToString(Newtonsoft.Json.Formatting.None);
        }

        public static Task<string> EncryptJsonAsync(this string jsonString, string jsonPathToEncrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            Crypto.Current.EncryptJsonAsync(jsonString, jsonPathToEncrypt, keyIdentifier, cancellationToken);

        public static Task<string> EncryptJsonAsync(this string jsonString, IEnumerable<string> jsonPathsToEncrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            Crypto.Current.EncryptJsonAsync(jsonString, jsonPathsToEncrypt, keyIdentifier, cancellationToken);

        public static Task<string> EncryptJsonAsync(this ICrypto crypto, string jsonString, string jsonPathToEncrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            crypto.EncryptJsonAsync(jsonString, new[] { jsonPathToEncrypt }, keyIdentifier, cancellationToken);

        public static async Task<string> EncryptJsonAsync(this ICrypto crypto, string jsonString, IEnumerable<string> jsonPathsToEncrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (crypto == null) throw new ArgumentNullException(nameof(crypto));
            if (jsonString == null) throw new ArgumentNullException(nameof(jsonString));
            if (jsonPathsToEncrypt == null) throw new ArgumentNullException(nameof(jsonPathsToEncrypt));

            var jObject = JObject.Parse(jsonString);

            var encryptor = new Lazy<IAsyncEncryptor>(() => crypto.AsAsync().GetAsyncEncryptor(keyIdentifier));

            var anyPaths = false;

            foreach (var jsonPath in jsonPathsToEncrypt)
            {
                if (jsonPath == null)
                    throw new ArgumentException($"{nameof(jsonPathsToEncrypt)} cannot have null items.", nameof(jsonPathsToEncrypt));

                anyPaths = true;

                foreach (var match in jObject.SelectTokens(jsonPath))
                {
                    var property = (JProperty)match.Parent;
                    property.Value = await encryptor.Value.EncryptAsync(match.ToString(Newtonsoft.Json.Formatting.None), cancellationToken).ConfigureAwait(false);
                }
            }

            if (!anyPaths)
                throw new ArgumentException($"{nameof(jsonPathsToEncrypt)} must have at least one item.", nameof(jsonPathsToEncrypt));

            return jObject.ToString(Newtonsoft.Json.Formatting.None);
        }

        public static string DecryptJson(this string jsonString, string jsonPathToDecrypt, object keyIdentifier = null) =>
            Crypto.Current.DecryptJson(jsonString, jsonPathToDecrypt, keyIdentifier);

        public static string DecryptJson(this string jsonString, IEnumerable<string> jsonPathsToDecrypt, object keyIdentifier = null) =>
            Crypto.Current.DecryptJson(jsonString, jsonPathsToDecrypt, keyIdentifier);

        public static string DecryptJson(this ICrypto crypto, string jsonString, string jsonPathToDecrypt, object keyIdentifier = null) =>
            crypto.DecryptJson(jsonString, new[] { jsonPathToDecrypt }, keyIdentifier);

        public static string DecryptJson(this ICrypto crypto, string jsonString, IEnumerable<string> jsonPathsToDecrypt, object keyIdentifier = null)
        {
            if (crypto == null) throw new ArgumentNullException(nameof(crypto));
            if (jsonString == null) throw new ArgumentNullException(nameof(jsonString));
            if (jsonPathsToDecrypt == null) throw new ArgumentNullException(nameof(jsonPathsToDecrypt));

            var jObject = JObject.Parse(jsonString);

            var decryptor = new Lazy<IDecryptor>(() => crypto.GetDecryptor(keyIdentifier));

            var anyPaths = false;

            foreach (var jsonPath in jsonPathsToDecrypt)
            {
                if (jsonPath == null)
                    throw new ArgumentException($"{nameof(jsonPathsToDecrypt)} cannot have null items.", nameof(jsonPathsToDecrypt));

                anyPaths = true;

                foreach (var match in jObject.SelectTokens(jsonPath))
                {
                    var property = (JProperty)match.Parent;
                    property.Value = JToken.Parse(decryptor.Value.Decrypt(match.Value<string>()));
                }
            }

            if (!anyPaths)
                throw new ArgumentException($"{nameof(jsonPathsToDecrypt)} must have at least one item.", nameof(jsonPathsToDecrypt));

            return jObject.ToString(Newtonsoft.Json.Formatting.None);
        }

        public static Task<string> DecryptJsonAsync(this string jsonString, string jsonPathToDecrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            Crypto.Current.DecryptJsonAsync(jsonString, jsonPathToDecrypt, keyIdentifier, cancellationToken);

        public static Task<string> DecryptJsonAsync(this string jsonString, IEnumerable<string> jsonPathsToDecrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            Crypto.Current.DecryptJsonAsync(jsonString, jsonPathsToDecrypt, keyIdentifier, cancellationToken);

        public static Task<string> DecryptJsonAsync(this ICrypto crypto, string jsonString, string jsonPathToDecrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken)) =>
            crypto.DecryptJsonAsync(jsonString, new[] { jsonPathToDecrypt }, keyIdentifier, cancellationToken);

        public static async Task<string> DecryptJsonAsync(this ICrypto crypto, string jsonString, IEnumerable<string> jsonPathsToDecrypt, object keyIdentifier = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (crypto == null) throw new ArgumentNullException(nameof(crypto));
            if (jsonString == null) throw new ArgumentNullException(nameof(jsonString));
            if (jsonPathsToDecrypt == null) throw new ArgumentNullException(nameof(jsonPathsToDecrypt));

            var jObject = JObject.Parse(jsonString);

            var decryptor = new Lazy<IAsyncDecryptor>(() => crypto.AsAsync().GetAsyncDecryptor(keyIdentifier));

            var anyPaths = false;

            foreach (var jsonPath in jsonPathsToDecrypt)
            {
                if (jsonPath == null)
                    throw new ArgumentException($"{nameof(jsonPathsToDecrypt)} cannot have null items.", nameof(jsonPathsToDecrypt));

                anyPaths = true;

                foreach (var match in jObject.SelectTokens(jsonPath))
                {
                    var property = (JProperty)match.Parent;
                    property.Value = JToken.Parse(await decryptor.Value.DecryptAsync(match.Value<string>(), cancellationToken).ConfigureAwait(false));
                }
            }

            if (!anyPaths)
                throw new ArgumentException($"{nameof(jsonPathsToDecrypt)} must have at least one item.", nameof(jsonPathsToDecrypt));

            return jObject.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}
