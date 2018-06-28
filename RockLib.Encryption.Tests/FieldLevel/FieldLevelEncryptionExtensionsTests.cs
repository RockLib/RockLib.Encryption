using Moq;
using NUnit.Framework;
using RockLib.Encryption.Async;
using RockLib.Encryption.FieldLevel;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RockLib.Encryption.Tests.FieldLevel
{
    [TestFixture]
    public class FieldLevelEncryptionExtensionsTests
    {
        [Test]
        public void EncryptXmlWorks()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = new object();

            var encryptedXml = mockCrypto.Object.EncryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);

            Assert.That(encryptedXml, Is.EqualTo("<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>"));
            mockCrypto.Verify(m => m.GetEncryptor(keyIdentifier), Times.Once);
            mockEncryptor.Verify(m => m.Encrypt(It.IsAny<string>()), Times.Exactly(4));
        }

        [Test]
        public async Task EncryptXmlAsyncWorks()
        {
            var mockAsyncEncryptor = new Mock<IAsyncEncryptor>();
            var mockCrypto = new Mock<ICrypto>();
            var mockAsyncCrypto = mockCrypto.As<IAsyncCrypto>();

            mockAsyncCrypto.Setup(m => m.GetAsyncEncryptor(It.IsAny<object>())).Returns(mockAsyncEncryptor.Object);
            mockAsyncEncryptor.Setup(m => m.EncryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string plainText, CancellationToken token) => Task.FromResult(Base64.Encrypt(plainText)));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = new object();

            var encryptedXml = await mockCrypto.Object.EncryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);

            Assert.That(encryptedXml, Is.EqualTo("<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>"));
            mockAsyncCrypto.Verify(m => m.GetAsyncEncryptor(keyIdentifier), Times.Once);
            mockAsyncEncryptor.Verify(m => m.EncryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(4));
        }

        [Test]
        public void DecryptXmlWorks()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = new object();

            var decryptedXml = mockCrypto.Object.DecryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);

            Assert.That(decryptedXml, Is.EqualTo("<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>"));
            mockCrypto.Verify(m => m.GetDecryptor(keyIdentifier), Times.Once);
            mockDecryptor.Verify(m => m.Decrypt(It.IsAny<string>()), Times.Exactly(4));
        }

        [Test]
        public async Task DecryptXmlAsyncWorks()
        {
            var mockAsyncDecryptor = new Mock<IAsyncDecryptor>();
            var mockCrypto = new Mock<ICrypto>();
            var mockAsyncCrypto = mockCrypto.As<IAsyncCrypto>();

            mockAsyncCrypto.Setup(m => m.GetAsyncDecryptor(It.IsAny<object>())).Returns(mockAsyncDecryptor.Object);
            mockAsyncDecryptor.Setup(m => m.DecryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string plainText, CancellationToken token) => Task.FromResult(Base64.Decrypt(plainText)));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = new object();

            var decryptedXml = await mockCrypto.Object.DecryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);

            Assert.That(decryptedXml, Is.EqualTo("<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>"));
            mockAsyncCrypto.Verify(m => m.GetAsyncDecryptor(keyIdentifier), Times.Once);
            mockAsyncDecryptor.Verify(m => m.DecryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(4));
        }

        [Test]
        public void EncryptJsonWorks()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = new object();

            var encryptedJson = mockCrypto.Object.EncryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            Assert.That(encryptedJson, Is.EqualTo("{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}"));
            mockCrypto.Verify(m => m.GetEncryptor(keyIdentifier), Times.Once);
            mockEncryptor.Verify(m => m.Encrypt(It.IsAny<string>()), Times.Exactly(6));
        }

        [Test]
        public async Task EncryptJsonAsyncWorks()
        {
            var mockAsyncEncryptor = new Mock<IAsyncEncryptor>();
            var mockCrypto = new Mock<ICrypto>();
            var mockAsyncCrypto = mockCrypto.As<IAsyncCrypto>();

            mockAsyncCrypto.Setup(m => m.GetAsyncEncryptor(It.IsAny<object>())).Returns(mockAsyncEncryptor.Object);
            mockAsyncEncryptor.Setup(m => m.EncryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string plainText, CancellationToken token) => Task.FromResult(Base64.Encrypt(plainText)));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = new object();

            var encryptedJson = await mockCrypto.Object.EncryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            Assert.That(encryptedJson, Is.EqualTo("{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}"));
            mockAsyncCrypto.Verify(m => m.GetAsyncEncryptor(keyIdentifier), Times.Once);
            mockAsyncEncryptor.Verify(m => m.EncryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(6));
        }

        [Test]
        public void DecryptJsonWorks()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = new object();

            var decryptedJson = mockCrypto.Object.DecryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            Assert.That(decryptedJson, Is.EqualTo("{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}"));
            mockCrypto.Verify(m => m.GetDecryptor(keyIdentifier), Times.Once);
            mockDecryptor.Verify(m => m.Decrypt(It.IsAny<string>()), Times.Exactly(6));
        }

        [Test]
        public async Task DecryptJsonAsyncWorks()
        {
            var mockAsyncDecryptor = new Mock<IAsyncDecryptor>();
            var mockCrypto = new Mock<ICrypto>();
            var mockAsyncCrypto = mockCrypto.As<IAsyncCrypto>();

            mockAsyncCrypto.Setup(m => m.GetAsyncDecryptor(It.IsAny<object>())).Returns(mockAsyncDecryptor.Object);
            mockAsyncDecryptor.Setup(m => m.DecryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string plainText, CancellationToken token) => Task.FromResult(Base64.Decrypt(plainText)));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = new object();

            var decryptedJson = await mockCrypto.Object.DecryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            Assert.That(decryptedJson, Is.EqualTo("{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}"));
            mockAsyncCrypto.Verify(m => m.GetAsyncDecryptor(keyIdentifier), Times.Once);
            mockAsyncDecryptor.Verify(m => m.DecryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(6));
        }

        [Test]
        [TestCase("null")]
        [TestCase("true")]
        [TestCase("false")]
        [TestCase("123")]
        [TestCase("123.45")]
        [TestCase("\"foo \\\"bar\\\"\"")]
        [TestCase("[1,2,3]")]
        [TestCase("{\"foo\":123}")]
        public void EncryptJsonAndDecryptJsonWithRootJsonPathTargetTheWholeInputString(string json)
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string cipherText) => Base64.Decrypt(cipherText));
            
            var root = "$";
            var keyIdentifier = new object();

            var encryptedJson = mockCrypto.Object.EncryptJson(json, root, keyIdentifier);

            Assert.That(encryptedJson, Is.EqualTo("\"" + Base64.Encrypt(json) + "\""));

            var decryptedJson = mockCrypto.Object.DecryptJson(encryptedJson, root, keyIdentifier);

            Assert.That(decryptedJson, Is.EqualTo(json));
        }

        [Test]
        [TestCase("null")]
        [TestCase("true")]
        [TestCase("false")]
        [TestCase("123")]
        [TestCase("123.45")]
        [TestCase("\"foo \\\"bar\\\"\"")]
        [TestCase("[1,2,3]")]
        [TestCase("{\"foo\":123}")]
        public async Task EncryptJsonAsyncAndDecryptJsonAsyncWithRootJsonPathTargetTheWholeInputString(string json)
        {
            var mockAsyncEncryptor = new Mock<IAsyncEncryptor>();
            var mockAsyncDecryptor = new Mock<IAsyncDecryptor>();
            var mockCrypto = new Mock<ICrypto>();
            var mockAsyncCrypto = mockCrypto.As<IAsyncCrypto>();

            mockAsyncCrypto.Setup(m => m.GetAsyncEncryptor(It.IsAny<object>())).Returns(mockAsyncEncryptor.Object);
            mockAsyncEncryptor.Setup(m => m.EncryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string plainText, CancellationToken token) => Task.FromResult(Base64.Encrypt(plainText)));

            mockAsyncCrypto.Setup(m => m.GetAsyncDecryptor(It.IsAny<object>())).Returns(mockAsyncDecryptor.Object);
            mockAsyncDecryptor.Setup(m => m.DecryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string cipherText, CancellationToken token) => Task.FromResult(Base64.Decrypt(cipherText)));

            var root = "$";
            var keyIdentifier = new object();

            var encryptedJson = await mockCrypto.Object.EncryptJsonAsync(json, root, keyIdentifier);

            Assert.That(encryptedJson, Is.EqualTo("\"" + Base64.Encrypt(json) + "\""));

            var decryptedJson = await mockCrypto.Object.DecryptJsonAsync(encryptedJson, root, keyIdentifier);

            Assert.That(decryptedJson, Is.EqualTo(json));
        }

        [Test]
        public void EncryptJsonAndDecryptJsonCanTargetSpecificArrayElements()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string cipherText) => Base64.Decrypt(cipherText));

            var json = "{\"foo\":[\"abc\",\"xyz\"]}";
            var foo1 = "$.foo[1]";
            var keyIdentifier = new object();

            var encryptedJson = mockCrypto.Object.EncryptJson(json, foo1, keyIdentifier);

            Assert.That(encryptedJson, Is.EqualTo($"{{\"foo\":[\"abc\",\"{Base64.Encrypt("\"xyz\"")}\"]}}"));

            var decryptedJson = mockCrypto.Object.DecryptJson(encryptedJson, foo1, keyIdentifier);

            Assert.That(decryptedJson, Is.EqualTo(json));
        }

        [Test]
        public async Task EncryptJsonAsyncAndDecryptJsonAsyncCanTargetSpecificArrayElements()
        {
            var mockAsyncEncryptor = new Mock<IAsyncEncryptor>();
            var mockAsyncDecryptor = new Mock<IAsyncDecryptor>();
            var mockCrypto = new Mock<ICrypto>();
            var mockAsyncCrypto = mockCrypto.As<IAsyncCrypto>();

            mockAsyncCrypto.Setup(m => m.GetAsyncEncryptor(It.IsAny<object>())).Returns(mockAsyncEncryptor.Object);
            mockAsyncEncryptor.Setup(m => m.EncryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string plainText, CancellationToken token) => Task.FromResult(Base64.Encrypt(plainText)));

            mockAsyncCrypto.Setup(m => m.GetAsyncDecryptor(It.IsAny<object>())).Returns(mockAsyncDecryptor.Object);
            mockAsyncDecryptor.Setup(m => m.DecryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string cipherText, CancellationToken token) => Task.FromResult(Base64.Decrypt(cipherText)));

            var json = "{\"foo\":[\"abc\",\"xyz\"]}";
            var foo1 = "$.foo[1]";
            var keyIdentifier = new object();

            var encryptedJson = await mockCrypto.Object.EncryptJsonAsync(json, foo1, keyIdentifier);

            Assert.That(encryptedJson, Is.EqualTo($"{{\"foo\":[\"abc\",\"{Base64.Encrypt("\"xyz\"")}\"]}}"));

            var decryptedJson = await mockCrypto.Object.DecryptJsonAsync(encryptedJson, foo1, keyIdentifier);

            Assert.That(decryptedJson, Is.EqualTo(json));
        }

        [Test]
        public void EncryptXmlThrowsWhenCryptoIsNull()
        {
            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = new object();

            ICrypto crypto = null;

            Assert.That(() => crypto.EncryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void EncryptXmlThrowsWhenXmlStringIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            string xml = null;
            var keyIdentifier = new object();

            Assert.That(() => mockCrypto.Object.EncryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void EncryptXmlThrowsWhenXPathsToEncryptIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = new object();

            string[] xpathsToEncrypt = null;

            Assert.That(() => mockCrypto.Object.EncryptXml(xml, xpathsToEncrypt, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void EncryptXmlThrowsWhenXPathsToEncryptIsEmpty()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = new object();

            string[] xpathsToEncrypt = { };

            Assert.That(() => mockCrypto.Object.EncryptXml(xml, xpathsToEncrypt, keyIdentifier), Throws.ArgumentException);
        }

        [Test]
        public void EncryptXmlThrowsWhenAnXPathItemIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = new object();

            string[] xpathsToEncrypt = { null };

            Assert.That(() => mockCrypto.Object.EncryptXml(xml, xpathsToEncrypt, keyIdentifier), Throws.ArgumentException);
        }

        [Test]
        public void EncryptXmlAsyncThrowsWhenCryptoIsNull()
        {
            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = new object();

            ICrypto crypto = null;

            Assert.That(async () => await crypto.EncryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void EncryptXmlAsyncThrowsWhenXmlStringIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            string xml = null;
            var keyIdentifier = new object();

            Assert.That(async () => await mockCrypto.Object.EncryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void EncryptXmlAsyncThrowsWhenXPathsToEncryptIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = new object();

            string[] xpathsToEncrypt = null;

            Assert.That(async () => await mockCrypto.Object.EncryptXmlAsync(xml, xpathsToEncrypt, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void EncryptXmlAsyncThrowsWhenXPathsToEncryptIsEmpty()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = new object();

            string[] xpathsToEncrypt = { };

            Assert.That(async () => await mockCrypto.Object.EncryptXmlAsync(xml, xpathsToEncrypt, keyIdentifier), Throws.ArgumentException);
        }

        [Test]
        public void EncryptXmlAsyncThrowsWhenAnXPathItemIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = new object();

            string[] xpathsToEncrypt = { null };

            Assert.That(() => mockCrypto.Object.EncryptXmlAsync(xml, xpathsToEncrypt, keyIdentifier), Throws.ArgumentException);
        }

        [Test]
        public void DecryptXmlThrowsWhenCryptoIsNull()
        {
            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = new object();

            ICrypto crypto = null;

            Assert.That(() => crypto.DecryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void DecryptXmlThrowsWhenXmlStringIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            string xml = null;
            var keyIdentifier = new object();

            Assert.That(() => mockCrypto.Object.DecryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void DecryptXmlThrowsWhenXPathsToDecryptIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = new object();

            string[] xpathsToDecrypt = null;

            Assert.That(() => mockCrypto.Object.DecryptXml(xml, xpathsToDecrypt, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void DecryptXmlThrowsWhenXPathsToDecryptIsEmpty()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = new object();

            string[] xpathsToDecrypt = { };

            Assert.That(() => mockCrypto.Object.DecryptXml(xml, xpathsToDecrypt, keyIdentifier), Throws.ArgumentException);
        }

        [Test]
        public void DecryptXmlThrowsWhenAnXPathItemIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = new object();

            string[] xpathsToDecrypt = { null };

            Assert.That(() => mockCrypto.Object.DecryptXml(xml, xpathsToDecrypt, keyIdentifier), Throws.ArgumentException);
        }

        [Test]
        public void DecryptXmlAsyncThrowsWhenCryptoIsNull()
        {
            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = new object();

            ICrypto crypto = null;

            Assert.That(async () => await crypto.DecryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void DecryptXmlAsyncThrowsWhenXmlStringIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            string xml = null;
            var keyIdentifier = new object();

            Assert.That(async () => await mockCrypto.Object.DecryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void DecryptXmlAsyncThrowsWhenXPathsToDecryptIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = new object();

            string[] xpathsToDecrypt = null;

            Assert.That(async () => await mockCrypto.Object.DecryptXmlAsync(xml, xpathsToDecrypt, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void DecryptXmlAsyncThrowsWhenXPathsToDecryptIsEmpty()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = new object();

            string[] xpathsToDecrypt = { };

            Assert.That(async () => await mockCrypto.Object.DecryptXmlAsync(xml, xpathsToDecrypt, keyIdentifier), Throws.ArgumentException);
        }

        [Test]
        public void DecryptXmlAsyncThrowsWhenAnXPathItemIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = new object();

            string[] xpathsToDecrypt = { null };

            Assert.That(() => mockCrypto.Object.DecryptXmlAsync(xml, xpathsToDecrypt, keyIdentifier), Throws.ArgumentException);
        }

        [Test]
        public void EncryptJsonThrowsWhenCryptoIsNull()
        {
            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = new object();

            ICrypto crypto = null;

            Assert.That(() => crypto.EncryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void EncryptJsonThrowsWhenJsonStringIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            string json = null;
            var keyIdentifier = new object();

            Assert.That(() => mockCrypto.Object.EncryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void EncryptJsonThrowsWhenJsonPathsToEncryptIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = new object();

            string[] jsonPathsToEncrypt = null;

            Assert.That(() => mockCrypto.Object.EncryptJson(json, jsonPathsToEncrypt, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void EncryptJsonThrowsWhenJsonPathsToEncryptIsEmpty()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = new object();

            string[] jsonPathsToEncrypt = { };

            Assert.That(() => mockCrypto.Object.EncryptJson(json, jsonPathsToEncrypt, keyIdentifier), Throws.ArgumentException);
        }

        [Test]
        public void EncryptJsonThrowsWhenAnJsonPathItemIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = new object();

            string[] jsonPathsToEncrypt = { null };

            Assert.That(() => mockCrypto.Object.EncryptJson(json, jsonPathsToEncrypt, keyIdentifier), Throws.ArgumentException);
        }

        [Test]
        public void EncryptJsonAsyncThrowsWhenCryptoIsNull()
        {
            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = new object();

            ICrypto crypto = null;

            Assert.That(async () => await crypto.EncryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void EncryptJsonAsyncThrowsWhenJsonStringIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            string json = null;
            var keyIdentifier = new object();

            Assert.That(async () => await mockCrypto.Object.EncryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void EncryptJsonAsyncThrowsWhenJsonPathsToEncryptIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = new object();

            string[] jsonPathsToEncrypt = null;

            Assert.That(async () => await mockCrypto.Object.EncryptJsonAsync(json, jsonPathsToEncrypt, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void EncryptJsonAsyncThrowsWhenJsonPathsToEncryptIsEmpty()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = new object();

            string[] jsonPathsToEncrypt = { };

            Assert.That(async () => await mockCrypto.Object.EncryptJsonAsync(json, jsonPathsToEncrypt, keyIdentifier), Throws.ArgumentException);
        }

        [Test]
        public void EncryptJsonAsyncThrowsWhenAnJsonPathItemIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<object>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = new object();

            string[] jsonPathsToEncrypt = { null };

            Assert.That(() => mockCrypto.Object.EncryptJsonAsync(json, jsonPathsToEncrypt, keyIdentifier), Throws.ArgumentException);
        }

        [Test]
        public void DecryptJsonThrowsWhenCryptoIsNull()
        {
            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = new object();

            ICrypto crypto = null;

            Assert.That(() => crypto.DecryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void DecryptJsonThrowsWhenJsonStringIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            string json = null;
            var keyIdentifier = new object();

            Assert.That(() => mockCrypto.Object.DecryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void DecryptJsonThrowsWhenJsonPathsToDecryptIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = new object();

            string[] jsonPathsToDecrypt = null;

            Assert.That(() => mockCrypto.Object.DecryptJson(json, jsonPathsToDecrypt, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void DecryptJsonThrowsWhenJsonPathsToDecryptIsEmpty()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = new object();

            string[] jsonPathsToDecrypt = { };

            Assert.That(() => mockCrypto.Object.DecryptJson(json, jsonPathsToDecrypt, keyIdentifier), Throws.ArgumentException);
        }

        [Test]
        public void DecryptJsonThrowsWhenAnJsonPathItemIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = new object();

            string[] jsonPathsToDecrypt = { null };

            Assert.That(() => mockCrypto.Object.DecryptJson(json, jsonPathsToDecrypt, keyIdentifier), Throws.ArgumentException);
        }

        [Test]
        public void DecryptJsonAsyncThrowsWhenCryptoIsNull()
        {
            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = new object();

            ICrypto crypto = null;

            Assert.That(async () => await crypto.DecryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void DecryptJsonAsyncThrowsWhenJsonStringIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            string json = null;
            var keyIdentifier = new object();

            Assert.That(async () => await mockCrypto.Object.DecryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void DecryptJsonAsyncThrowsWhenJsonPathsToDecryptIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = new object();

            string[] jsonPathsToDecrypt = null;

            Assert.That(async () => await mockCrypto.Object.DecryptJsonAsync(json, jsonPathsToDecrypt, keyIdentifier), Throws.ArgumentNullException);
        }

        [Test]
        public void DecryptJsonAsyncThrowsWhenJsonPathsToDecryptIsEmpty()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = new object();

            string[] jsonPathsToDecrypt = { };

            Assert.That(async () => await mockCrypto.Object.DecryptJsonAsync(json, jsonPathsToDecrypt, keyIdentifier), Throws.ArgumentException);
        }

        [Test]
        public void DecryptJsonAsyncThrowsWhenAnJsonPathItemIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<object>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = new object();

            string[] jsonPathsToDecrypt = { null };

            Assert.That(() => mockCrypto.Object.DecryptJsonAsync(json, jsonPathsToDecrypt, keyIdentifier), Throws.ArgumentException);
        }
        
        private static class Base64
        {
            public static string Encrypt(string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
            public static string Decrypt(string value) => Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }
    }
}
