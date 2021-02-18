using FluentAssertions;
using Moq;
using RockLib.Encryption.Async;
using RockLib.Encryption.FieldLevel;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RockLib.Encryption.Tests.FieldLevel
{
    public class FieldLevelEncryptionExtensionsTests
    {
        [Fact]
        public void EncryptXmlWorks()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            var encryptedXml = mockCrypto.Object.EncryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);

            encryptedXml.Should().Be("<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>");
            mockCrypto.Verify(m => m.GetEncryptor(keyIdentifier), Times.Once);
            mockEncryptor.Verify(m => m.Encrypt(It.IsAny<string>()), Times.Exactly(4));
        }

        [Fact]
        public async Task EncryptXmlAsyncWorks()
        {
            var mockAsyncEncryptor = new Mock<IAsyncEncryptor>();
            var mockCrypto = new Mock<ICrypto>();
            var mockAsyncCrypto = mockCrypto.As<IAsyncCrypto>();

            mockAsyncCrypto.Setup(m => m.GetAsyncEncryptor(It.IsAny<string>())).Returns(mockAsyncEncryptor.Object);
            mockAsyncEncryptor.Setup(m => m.EncryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string plainText, CancellationToken token) => Task.FromResult(Base64.Encrypt(plainText)));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            var encryptedXml = await mockCrypto.Object.EncryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);

            encryptedXml.Should().Be("<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>");
            mockAsyncCrypto.Verify(m => m.GetAsyncEncryptor(keyIdentifier), Times.Once);
            mockAsyncEncryptor.Verify(m => m.EncryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(4));
        }

        [Fact]
        public void DecryptXmlWorks()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            var decryptedXml = mockCrypto.Object.DecryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);

            decryptedXml.Should().Be("<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>");
            mockCrypto.Verify(m => m.GetDecryptor(keyIdentifier), Times.Once);
            mockDecryptor.Verify(m => m.Decrypt(It.IsAny<string>()), Times.Exactly(4));
        }

        [Fact]
        public async Task DecryptXmlAsyncWorks()
        {
            var mockAsyncDecryptor = new Mock<IAsyncDecryptor>();
            var mockCrypto = new Mock<ICrypto>();
            var mockAsyncCrypto = mockCrypto.As<IAsyncCrypto>();

            mockAsyncCrypto.Setup(m => m.GetAsyncDecryptor(It.IsAny<string>())).Returns(mockAsyncDecryptor.Object);
            mockAsyncDecryptor.Setup(m => m.DecryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string plainText, CancellationToken token) => Task.FromResult(Base64.Decrypt(plainText)));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            var decryptedXml = await mockCrypto.Object.DecryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);

            decryptedXml.Should().Be("<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>");
            mockAsyncCrypto.Verify(m => m.GetAsyncDecryptor(keyIdentifier), Times.Once);
            mockAsyncDecryptor.Verify(m => m.DecryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(4));
        }

        [Fact]
        public void EncryptJsonWorks()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = "myKeyIdentifier";

            var encryptedJson = mockCrypto.Object.EncryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            encryptedJson.Should().Be("{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}");
            mockCrypto.Verify(m => m.GetEncryptor(keyIdentifier), Times.Once);
            mockEncryptor.Verify(m => m.Encrypt(It.IsAny<string>()), Times.Exactly(6));
        }

        [Fact]
        public async Task EncryptJsonAsyncWorks()
        {
            var mockAsyncEncryptor = new Mock<IAsyncEncryptor>();
            var mockCrypto = new Mock<ICrypto>();
            var mockAsyncCrypto = mockCrypto.As<IAsyncCrypto>();

            mockAsyncCrypto.Setup(m => m.GetAsyncEncryptor(It.IsAny<string>())).Returns(mockAsyncEncryptor.Object);
            mockAsyncEncryptor.Setup(m => m.EncryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string plainText, CancellationToken token) => Task.FromResult(Base64.Encrypt(plainText)));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = "myKeyIdentifier";

            var encryptedJson = await mockCrypto.Object.EncryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            encryptedJson.Should().Be("{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}");
            mockAsyncCrypto.Verify(m => m.GetAsyncEncryptor(keyIdentifier), Times.Once);
            mockAsyncEncryptor.Verify(m => m.EncryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(6));
        }

        [Fact]
        public void DecryptJsonWorks()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = "myKeyIdentifier";

            var decryptedJson = mockCrypto.Object.DecryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            decryptedJson.Should().Be("{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}");
            mockCrypto.Verify(m => m.GetDecryptor(keyIdentifier), Times.Once);
            mockDecryptor.Verify(m => m.Decrypt(It.IsAny<string>()), Times.Exactly(6));
        }

        [Fact]
        public async Task DecryptJsonAsyncWorks()
        {
            var mockAsyncDecryptor = new Mock<IAsyncDecryptor>();
            var mockCrypto = new Mock<ICrypto>();
            var mockAsyncCrypto = mockCrypto.As<IAsyncCrypto>();

            mockAsyncCrypto.Setup(m => m.GetAsyncDecryptor(It.IsAny<string>())).Returns(mockAsyncDecryptor.Object);
            mockAsyncDecryptor.Setup(m => m.DecryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string plainText, CancellationToken token) => Task.FromResult(Base64.Decrypt(plainText)));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = "myKeyIdentifier";

            var decryptedJson = await mockCrypto.Object.DecryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            decryptedJson.Should().Be("{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}");
            mockAsyncCrypto.Verify(m => m.GetAsyncDecryptor(keyIdentifier), Times.Once);
            mockAsyncDecryptor.Verify(m => m.DecryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(6));
        }

        [Theory]
        [InlineData("null")]
        [InlineData("true")]
        [InlineData("false")]
        [InlineData("123")]
        [InlineData("123.45")]
        [InlineData("\"foo \\\"bar\\\"\"")]
        [InlineData("[1,2,3]")]
        [InlineData("{\"foo\":123}")]
        public void EncryptJsonAndDecryptJsonWithRootJsonPathTargetTheWholeInputString(string json)
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string cipherText) => Base64.Decrypt(cipherText));
            
            var root = "$";
            var keyIdentifier = "myKeyIdentifier";

            var encryptedJson = mockCrypto.Object.EncryptJson(json, root, keyIdentifier);

            encryptedJson.Should().Be("\"" + Base64.Encrypt(json) + "\"");

            var decryptedJson = mockCrypto.Object.DecryptJson(encryptedJson, root, keyIdentifier);

            decryptedJson.Should().Be(json);
        }

        [Theory]
        [InlineData("null")]
        [InlineData("true")]
        [InlineData("false")]
        [InlineData("123")]
        [InlineData("123.45")]
        [InlineData("\"foo \\\"bar\\\"\"")]
        [InlineData("[1,2,3]")]
        [InlineData("{\"foo\":123}")]
        public async Task EncryptJsonAsyncAndDecryptJsonAsyncWithRootJsonPathTargetTheWholeInputString(string json)
        {
            var mockAsyncEncryptor = new Mock<IAsyncEncryptor>();
            var mockAsyncDecryptor = new Mock<IAsyncDecryptor>();
            var mockCrypto = new Mock<ICrypto>();
            var mockAsyncCrypto = mockCrypto.As<IAsyncCrypto>();

            mockAsyncCrypto.Setup(m => m.GetAsyncEncryptor(It.IsAny<string>())).Returns(mockAsyncEncryptor.Object);
            mockAsyncEncryptor.Setup(m => m.EncryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string plainText, CancellationToken token) => Task.FromResult(Base64.Encrypt(plainText)));

            mockAsyncCrypto.Setup(m => m.GetAsyncDecryptor(It.IsAny<string>())).Returns(mockAsyncDecryptor.Object);
            mockAsyncDecryptor.Setup(m => m.DecryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string cipherText, CancellationToken token) => Task.FromResult(Base64.Decrypt(cipherText)));

            var root = "$";
            var keyIdentifier = "myKeyIdentifier";

            var encryptedJson = await mockCrypto.Object.EncryptJsonAsync(json, root, keyIdentifier);

            encryptedJson.Should().Be("\"" + Base64.Encrypt(json) + "\"");

            var decryptedJson = await mockCrypto.Object.DecryptJsonAsync(encryptedJson, root, keyIdentifier);

            decryptedJson.Should().Be(json);
        }

        [Fact]
        public void EncryptJsonAndDecryptJsonCanTargetSpecificArrayElements()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string cipherText) => Base64.Decrypt(cipherText));

            var json = "{\"foo\":[\"abc\",\"xyz\"]}";
            var foo1 = "$.foo[1]";
            var keyIdentifier = "myKeyIdentifier";

            var encryptedJson = mockCrypto.Object.EncryptJson(json, foo1, keyIdentifier);

            encryptedJson.Should().Be($"{{\"foo\":[\"abc\",\"{Base64.Encrypt("\"xyz\"")}\"]}}");

            var decryptedJson = mockCrypto.Object.DecryptJson(encryptedJson, foo1, keyIdentifier);

            decryptedJson.Should().Be(json);
        }

        [Fact]
        public async Task EncryptJsonAsyncAndDecryptJsonAsyncCanTargetSpecificArrayElements()
        {
            var mockAsyncEncryptor = new Mock<IAsyncEncryptor>();
            var mockAsyncDecryptor = new Mock<IAsyncDecryptor>();
            var mockCrypto = new Mock<ICrypto>();
            var mockAsyncCrypto = mockCrypto.As<IAsyncCrypto>();

            mockAsyncCrypto.Setup(m => m.GetAsyncEncryptor(It.IsAny<string>())).Returns(mockAsyncEncryptor.Object);
            mockAsyncEncryptor.Setup(m => m.EncryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string plainText, CancellationToken token) => Task.FromResult(Base64.Encrypt(plainText)));

            mockAsyncCrypto.Setup(m => m.GetAsyncDecryptor(It.IsAny<string>())).Returns(mockAsyncDecryptor.Object);
            mockAsyncDecryptor.Setup(m => m.DecryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string cipherText, CancellationToken token) => Task.FromResult(Base64.Decrypt(cipherText)));

            var json = "{\"foo\":[\"abc\",\"xyz\"]}";
            var foo1 = "$.foo[1]";
            var keyIdentifier = "myKeyIdentifier";

            var encryptedJson = await mockCrypto.Object.EncryptJsonAsync(json, foo1, keyIdentifier);

            encryptedJson.Should().Be($"{{\"foo\":[\"abc\",\"{Base64.Encrypt("\"xyz\"")}\"]}}");

            var decryptedJson = await mockCrypto.Object.DecryptJsonAsync(encryptedJson, foo1, keyIdentifier);

            decryptedJson.Should().Be(json);
        }

        [Fact]
        public void EncryptJsonAndDecryptJsonCanTargetAllArrayElements()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string cipherText) => Base64.Decrypt(cipherText));

            var json = "{\"foo\":[\"abc\",\"xyz\"]}";
            var foo1 = "$.foo[*]";
            var keyIdentifier = "myKeyIdentifier";

            var encryptedJson = mockCrypto.Object.EncryptJson(json, foo1, keyIdentifier);

            encryptedJson.Should().Be($"{{\"foo\":[\"{Base64.Encrypt("\"abc\"")}\",\"{Base64.Encrypt("\"xyz\"")}\"]}}");

            var decryptedJson = mockCrypto.Object.DecryptJson(encryptedJson, foo1, keyIdentifier);

            decryptedJson.Should().Be(json);
        }

        [Fact]
        public async Task EncryptJsonAsyncAndDecryptJsonAsyncCanTargetAllArrayElements()
        {
            var mockAsyncEncryptor = new Mock<IAsyncEncryptor>();
            var mockAsyncDecryptor = new Mock<IAsyncDecryptor>();
            var mockCrypto = new Mock<ICrypto>();
            var mockAsyncCrypto = mockCrypto.As<IAsyncCrypto>();

            mockAsyncCrypto.Setup(m => m.GetAsyncEncryptor(It.IsAny<string>())).Returns(mockAsyncEncryptor.Object);
            mockAsyncEncryptor.Setup(m => m.EncryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string plainText, CancellationToken token) => Task.FromResult(Base64.Encrypt(plainText)));

            mockAsyncCrypto.Setup(m => m.GetAsyncDecryptor(It.IsAny<string>())).Returns(mockAsyncDecryptor.Object);
            mockAsyncDecryptor.Setup(m => m.DecryptAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns((string cipherText, CancellationToken token) => Task.FromResult(Base64.Decrypt(cipherText)));

            var json = "{\"foo\":[\"abc\",\"xyz\"]}";
            var foo1 = "$.foo[*]";
            var keyIdentifier = "myKeyIdentifier";

            var encryptedJson = await mockCrypto.Object.EncryptJsonAsync(json, foo1, keyIdentifier);

            encryptedJson.Should().Be($"{{\"foo\":[\"{Base64.Encrypt("\"abc\"")}\",\"{Base64.Encrypt("\"xyz\"")}\"]}}");

            var decryptedJson = await mockCrypto.Object.DecryptJsonAsync(encryptedJson, foo1, keyIdentifier);

            decryptedJson.Should().Be(json);
        }

        [Fact]
        public void EncryptXmlThrowsWhenCryptoIsNull()
        {
            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            ICrypto crypto = null;

            Action act = () => crypto.EncryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);
            
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptXmlThrowsWhenXmlStringIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            string xml = null;
            var keyIdentifier = "myKeyIdentifier";

            Action act = () => mockCrypto.Object.EncryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);
            
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptXmlThrowsWhenXPathsToEncryptIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            string[] xpathsToEncrypt = null;

            Action act = () => mockCrypto.Object.EncryptXml(xml, xpathsToEncrypt, keyIdentifier);
            
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptXmlThrowsWhenXPathsToEncryptIsEmpty()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            string[] xpathsToEncrypt = { };

            Action act = () => mockCrypto.Object.EncryptXml(xml, xpathsToEncrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void EncryptXmlThrowsWhenAnXPathItemIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            string[] xpathsToEncrypt = { null };

            Action act = () => mockCrypto.Object.EncryptXml(xml, xpathsToEncrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void EncryptXmlAsyncThrowsWhenCryptoIsNull()
        {
            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            ICrypto crypto = null;

            Func<Task> act = async () => await crypto.EncryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptXmlAsyncThrowsWhenXmlStringIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            string xml = null;
            var keyIdentifier = "myKeyIdentifier";

            Func<Task> act = async () => await mockCrypto.Object.EncryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptXmlAsyncThrowsWhenXPathsToEncryptIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            string[] xpathsToEncrypt = null;

            Func<Task> act = async () => await mockCrypto.Object.EncryptXmlAsync(xml, xpathsToEncrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptXmlAsyncThrowsWhenXPathsToEncryptIsEmpty()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            string[] xpathsToEncrypt = { };

            Func<Task> act = async () => await mockCrypto.Object.EncryptXmlAsync(xml, xpathsToEncrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void EncryptXmlAsyncThrowsWhenAnXPathItemIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            string[] xpathsToEncrypt = { null };

            Func<Task> act = () => mockCrypto.Object.EncryptXmlAsync(xml, xpathsToEncrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptXmlThrowsWhenCryptoIsNull()
        {
            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            ICrypto crypto = null;

            Action act = () => crypto.DecryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptXmlThrowsWhenXmlStringIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            string xml = null;
            var keyIdentifier = "myKeyIdentifier";

            Action act = () => mockCrypto.Object.DecryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptXmlThrowsWhenXPathsToDecryptIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            string[] xpathsToDecrypt = null;

            Action act = () => mockCrypto.Object.DecryptXml(xml, xpathsToDecrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptXmlThrowsWhenXPathsToDecryptIsEmpty()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            string[] xpathsToDecrypt = { };

            Action act = () => mockCrypto.Object.DecryptXml(xml, xpathsToDecrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptXmlThrowsWhenAnXPathItemIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            string[] xpathsToDecrypt = { null };

            Action act = () => mockCrypto.Object.DecryptXml(xml, xpathsToDecrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptXmlAsyncThrowsWhenCryptoIsNull()
        {
            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            ICrypto crypto = null;

            Func<Task> act = async () => await crypto.DecryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptXmlAsyncThrowsWhenXmlStringIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            string xml = null;
            var keyIdentifier = "myKeyIdentifier";

            Func<Task> act = async () => await mockCrypto.Object.DecryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" }, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptXmlAsyncThrowsWhenXPathsToDecryptIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            string[] xpathsToDecrypt = null;

            Func<Task> act = async () => await mockCrypto.Object.DecryptXmlAsync(xml, xpathsToDecrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptXmlAsyncThrowsWhenXPathsToDecryptIsEmpty()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            string[] xpathsToDecrypt = { };

            Func<Task> act = async () => await mockCrypto.Object.DecryptXmlAsync(xml, xpathsToDecrypt, keyIdentifier);
            
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptXmlAsyncThrowsWhenAnXPathItemIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var xml = "<foo bar=\"MTIz\"><baz>NDU2</baz><baz>Nzg5</baz><qux>PGdhcnBseSBncmF1bHQ9ImFiYyIgLz4=</qux></foo>";
            var keyIdentifier = "myKeyIdentifier";

            string[] xpathsToDecrypt = { null };

            Func<Task> act = () => mockCrypto.Object.DecryptXmlAsync(xml, xpathsToDecrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void EncryptJsonThrowsWhenCryptoIsNull()
        {
            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = "myKeyIdentifier";

            ICrypto crypto = null;

            Action act = () => crypto.EncryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptJsonThrowsWhenJsonStringIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            string json = null;
            var keyIdentifier = "myKeyIdentifier";

            Action act = () => mockCrypto.Object.EncryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptJsonThrowsWhenJsonPathsToEncryptIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = "myKeyIdentifier";

            string[] jsonPathsToEncrypt = null;

            Action act = () => mockCrypto.Object.EncryptJson(json, jsonPathsToEncrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptJsonThrowsWhenJsonPathsToEncryptIsEmpty()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = "myKeyIdentifier";

            string[] jsonPathsToEncrypt = { };

            Action act = () => mockCrypto.Object.EncryptJson(json, jsonPathsToEncrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void EncryptJsonThrowsWhenAnJsonPathItemIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = "myKeyIdentifier";

            string[] jsonPathsToEncrypt = { null };

            Action act = () => mockCrypto.Object.EncryptJson(json, jsonPathsToEncrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void EncryptJsonAsyncThrowsWhenCryptoIsNull()
        {
            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = "myKeyIdentifier";

            ICrypto crypto = null;

            Func<Task> act = async () => await crypto.EncryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptJsonAsyncThrowsWhenJsonStringIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            string json = null;
            var keyIdentifier = "myKeyIdentifier";

            Func<Task> act = async () => await mockCrypto.Object.EncryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptJsonAsyncThrowsWhenJsonPathsToEncryptIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = "myKeyIdentifier";

            string[] jsonPathsToEncrypt = null;

            Func<Task> act = async () => await mockCrypto.Object.EncryptJsonAsync(json, jsonPathsToEncrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptJsonAsyncThrowsWhenJsonPathsToEncryptIsEmpty()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = "myKeyIdentifier";

            string[] jsonPathsToEncrypt = { };

            Func<Task> act = async () => await mockCrypto.Object.EncryptJsonAsync(json, jsonPathsToEncrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void EncryptJsonAsyncThrowsWhenAnJsonPathItemIsNull()
        {
            var mockEncryptor = new Mock<IEncryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetEncryptor(It.IsAny<string>())).Returns(mockEncryptor.Object);
            mockEncryptor.Setup(m => m.Encrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Encrypt(plainText));

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            var keyIdentifier = "myKeyIdentifier";

            string[] jsonPathsToEncrypt = { null };

            Func<Task> act = () => mockCrypto.Object.EncryptJsonAsync(json, jsonPathsToEncrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptJsonThrowsWhenCryptoIsNull()
        {
            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = "myKeyIdentifier";

            ICrypto crypto = null;

            Action act = () => crypto.DecryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptJsonThrowsWhenJsonStringIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            string json = null;
            var keyIdentifier = "myKeyIdentifier";

            Action act = () => mockCrypto.Object.DecryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptJsonThrowsWhenJsonPathsToDecryptIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = "myKeyIdentifier";

            string[] jsonPathsToDecrypt = null;

            Action act = () => mockCrypto.Object.DecryptJson(json, jsonPathsToDecrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptJsonThrowsWhenJsonPathsToDecryptIsEmpty()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = "myKeyIdentifier";

            string[] jsonPathsToDecrypt = { };

            Action act = () => mockCrypto.Object.DecryptJson(json, jsonPathsToDecrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptJsonThrowsWhenAnJsonPathItemIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = "myKeyIdentifier";

            string[] jsonPathsToDecrypt = { null };

            Action act = () => mockCrypto.Object.DecryptJson(json, jsonPathsToDecrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptJsonAsyncThrowsWhenCryptoIsNull()
        {
            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = "myKeyIdentifier";

            ICrypto crypto = null;

            Func<Task> act = async () => await crypto.DecryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptJsonAsyncThrowsWhenJsonStringIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            string json = null;
            var keyIdentifier = "myKeyIdentifier";

            Func<Task> act = async () => await mockCrypto.Object.DecryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" }, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptJsonAsyncThrowsWhenJsonPathsToDecryptIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = "myKeyIdentifier";

            string[] jsonPathsToDecrypt = null;

            Func<Task> act = async () => await mockCrypto.Object.DecryptJsonAsync(json, jsonPathsToDecrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptJsonAsyncThrowsWhenJsonPathsToDecryptIsEmpty()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = "myKeyIdentifier";

            string[] jsonPathsToDecrypt = { };

            Func<Task> act = async () => await mockCrypto.Object.DecryptJsonAsync(json, jsonPathsToDecrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptJsonAsyncThrowsWhenAnJsonPathItemIsNull()
        {
            var mockDecryptor = new Mock<IDecryptor>();
            var mockCrypto = new Mock<ICrypto>();

            mockCrypto.Setup(m => m.GetDecryptor(It.IsAny<string>())).Returns(mockDecryptor.Object);
            mockDecryptor.Setup(m => m.Decrypt(It.IsAny<string>())).Returns((string plainText) => Base64.Decrypt(plainText));

            var json = "{\"foo\":\"ImFiYyI=\",\"bar\":\"MTIz\",\"baz\":\"dHJ1ZQ==\",\"qux\":\"WzEsMiwzXQ==\",\"garply\":\"eyJncmF1bHQiOiJ4eXoifQ==\",\"fred\":\"bnVsbA==\"}";
            var keyIdentifier = "myKeyIdentifier";

            string[] jsonPathsToDecrypt = { null };

            Func<Task> act = () => mockCrypto.Object.DecryptJsonAsync(json, jsonPathsToDecrypt, keyIdentifier);

            act.Should().ThrowExactly<ArgumentException>();
        }
        
        private static class Base64
        {
            public static string Encrypt(string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
            public static string Decrypt(string value) => Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }
    }
}
