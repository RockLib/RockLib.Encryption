using FluentAssertions;
using RockLib.Encryption.Async;
using RockLib.Encryption.FieldLevel;
using RockLib.Encryption.Testing;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RockLib.Encryption.Tests.FieldLevel
{
    public class FieldLevelEncryptionExtensionsTests
    {
        [Fact]
        public void EncryptXmlWorks()
        {
            var fakeCrypto = new FakeCrypto();

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            
            var encryptedXml = fakeCrypto.EncryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });

            encryptedXml.Should().Be("<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>");
        }

        [Fact]
        public async Task EncryptXmlAsyncWorks()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            
            var encryptedXml = await fakeCrypto.EncryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });

            encryptedXml.Should().Be("<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>");
        }

        [Fact]
        public void DecryptXmlWorks()
        {
            var fakeCrypto = new FakeCrypto();

            var xml = "<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>";
            
            var decryptedXml = fakeCrypto.DecryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });

            decryptedXml.Should().Be("<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>");
        }

        [Fact]
        public async Task DecryptXmlAsyncWorks()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var xml = "<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>";
            
            var decryptedXml = await fakeCrypto.DecryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });

            decryptedXml.Should().Be("<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>");
        }

        [Fact]
        public void EncryptJsonWorks()
        {
            var fakeCrypto = new FakeCrypto();

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            
            var encryptedJson = fakeCrypto.EncryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

            encryptedJson.Should().Be("{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}");
        }

        [Fact]
        public async Task EncryptJsonAsyncWorks()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            
            var encryptedJson = await fakeCrypto.EncryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

            encryptedJson.Should().Be("{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}");
        }

        [Fact]
        public void DecryptJsonWorks()
        {
            var fakeCrypto = new FakeCrypto();

            var json = "{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}";
            
            var decryptedJson = fakeCrypto.DecryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

            decryptedJson.Should().Be("{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}");
        }

        [Fact]
        public async Task DecryptJsonAsyncWorks()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var json = "{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}";
            
            var decryptedJson = await fakeCrypto.DecryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

            decryptedJson.Should().Be("{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}");
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
            var fakeCrypto = new FakeCrypto();

            var root = "$";
            
            var encryptedJson = fakeCrypto.EncryptJson(json, root);

            encryptedJson.Should().NotBe(json);
            encryptedJson.Should().StartWith("\"").And.EndWith("\"");

            var decryptedJson = fakeCrypto.DecryptJson(encryptedJson, root);

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
            var fakeCrypto = new FakeCrypto().AsAsync();

            var root = "$";
            
            var encryptedJson = await fakeCrypto.EncryptJsonAsync(json, root);

            encryptedJson.Should().NotBe(json);
            encryptedJson.Should().StartWith("\"").And.EndWith("\"");

            var decryptedJson = await fakeCrypto.DecryptJsonAsync(encryptedJson, root);

            decryptedJson.Should().Be(json);
        }

        [Fact]
        public void EncryptJsonAndDecryptJsonCanTargetSpecificArrayElements()
        {
            var fakeCrypto = new FakeCrypto();

            var json = "{\"foo\":[\"abc\",\"xyz\"]}";
            var foo1 = "$.foo[1]";
            
            var encryptedJson = fakeCrypto.EncryptJson(json, foo1);

            encryptedJson.Should().Be($"{{\"foo\":[\"abc\",\"[[\\\"xyz\\\"]]\"]}}");

            var decryptedJson = fakeCrypto.DecryptJson(encryptedJson, foo1);

            decryptedJson.Should().Be(json);
        }

        [Fact]
        public async Task EncryptJsonAsyncAndDecryptJsonAsyncCanTargetSpecificArrayElements()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var json = "{\"foo\":[\"abc\",\"xyz\"]}";
            var foo1 = "$.foo[1]";
            
            var encryptedJson = await fakeCrypto.EncryptJsonAsync(json, foo1);

            encryptedJson.Should().Be("{\"foo\":[\"abc\",\"[[\\\"xyz\\\"]]\"]}");

            var decryptedJson = await fakeCrypto.DecryptJsonAsync(encryptedJson, foo1);

            decryptedJson.Should().Be(json);
        }

        [Fact]
        public void EncryptJsonAndDecryptJsonCanTargetAllArrayElements()
        {
            var fakeCrypto = new FakeCrypto();

            var json = "{\"foo\":[\"abc\",\"xyz\"]}";
            var foo1 = "$.foo[*]";
            
            var encryptedJson = fakeCrypto.EncryptJson(json, foo1);

            encryptedJson.Should().Be("{\"foo\":[\"[[\\\"abc\\\"]]\",\"[[\\\"xyz\\\"]]\"]}");

            var decryptedJson = fakeCrypto.DecryptJson(encryptedJson, foo1);

            decryptedJson.Should().Be(json);
        }

        [Fact]
        public async Task EncryptJsonAsyncAndDecryptJsonAsyncCanTargetAllArrayElements()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var json = "{\"foo\":[\"abc\",\"xyz\"]}";
            var foo1 = "$.foo[*]";
            
            var encryptedJson = await fakeCrypto.EncryptJsonAsync(json, foo1);

            encryptedJson.Should().Be("{\"foo\":[\"[[\\\"abc\\\"]]\",\"[[\\\"xyz\\\"]]\"]}");

            var decryptedJson = await fakeCrypto.DecryptJsonAsync(encryptedJson, foo1);

            decryptedJson.Should().Be(json);
        }

        [Fact]
        public void EncryptXmlThrowsWhenCryptoIsNull()
        {
            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            
            ICrypto crypto = null;

            Action act = () => crypto.EncryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });
            
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptXmlThrowsWhenXmlStringIsNull()
        {
            var fakeCrypto = new FakeCrypto();

            string xml = null;
            
            Action act = () => fakeCrypto.EncryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });
            
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptXmlThrowsWhenXPathsToEncryptIsNull()
        {
            var fakeCrypto = new FakeCrypto();

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            
            string[] xpathsToEncrypt = null;

            Action act = () => fakeCrypto.EncryptXml(xml, xpathsToEncrypt);
            
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptXmlThrowsWhenXPathsToEncryptIsEmpty()
        {
            var fakeCrypto = new FakeCrypto();

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            
            string[] xpathsToEncrypt = { };

            Action act = () => fakeCrypto.EncryptXml(xml, xpathsToEncrypt);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void EncryptXmlThrowsWhenAnXPathItemIsNull()
        {
            var fakeCrypto = new FakeCrypto();

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            
            string[] xpathsToEncrypt = { null };

            Action act = () => fakeCrypto.EncryptXml(xml, xpathsToEncrypt);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void EncryptXmlAsyncThrowsWhenCryptoIsNull()
        {
            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            
            ICrypto crypto = null;

            Func<Task> act = async () => await crypto.EncryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptXmlAsyncThrowsWhenXmlStringIsNull()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            string xml = null;
            
            Func<Task> act = async () => await fakeCrypto.EncryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptXmlAsyncThrowsWhenXPathsToEncryptIsNull()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            
            string[] xpathsToEncrypt = null;

            Func<Task> act = async () => await fakeCrypto.EncryptXmlAsync(xml, xpathsToEncrypt);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptXmlAsyncThrowsWhenXPathsToEncryptIsEmpty()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            
            string[] xpathsToEncrypt = { };

            Func<Task> act = async () => await fakeCrypto.EncryptXmlAsync(xml, xpathsToEncrypt);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void EncryptXmlAsyncThrowsWhenAnXPathItemIsNull()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
            
            string[] xpathsToEncrypt = { null };

            Func<Task> act = () => fakeCrypto.EncryptXmlAsync(xml, xpathsToEncrypt);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptXmlThrowsWhenCryptoIsNull()
        {
            var xml = "<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>";
            
            ICrypto crypto = null;

            Action act = () => crypto.DecryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptXmlThrowsWhenXmlStringIsNull()
        {
            var fakeCrypto = new FakeCrypto();

            string xml = null;
            
            Action act = () => fakeCrypto.DecryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptXmlThrowsWhenXPathsToDecryptIsNull()
        {
            var fakeCrypto = new FakeCrypto();

            var xml = "<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>";
            
            string[] xpathsToDecrypt = null;

            Action act = () => fakeCrypto.DecryptXml(xml, xpathsToDecrypt);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptXmlThrowsWhenXPathsToDecryptIsEmpty()
        {
            var fakeCrypto = new FakeCrypto();

            var xml = "<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>";
            
            string[] xpathsToDecrypt = { };

            Action act = () => fakeCrypto.DecryptXml(xml, xpathsToDecrypt);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptXmlThrowsWhenAnXPathItemIsNull()
        {
            var fakeCrypto = new FakeCrypto();

            var xml = "<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>";
            
            string[] xpathsToDecrypt = { null };

            Action act = () => fakeCrypto.DecryptXml(xml, xpathsToDecrypt);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptXmlAsyncThrowsWhenCryptoIsNull()
        {
            var xml = "<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>";
            
            ICrypto crypto = null;

            Func<Task> act = async () => await crypto.DecryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptXmlAsyncThrowsWhenXmlStringIsNull()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            string xml = null;
            
            Func<Task> act = async () => await fakeCrypto.DecryptXmlAsync(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptXmlAsyncThrowsWhenXPathsToDecryptIsNull()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var xml = "<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>";
            
            string[] xpathsToDecrypt = null;

            Func<Task> act = async () => await fakeCrypto.DecryptXmlAsync(xml, xpathsToDecrypt);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptXmlAsyncThrowsWhenXPathsToDecryptIsEmpty()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var xml = "<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>";
            
            string[] xpathsToDecrypt = { };

            Func<Task> act = async () => await fakeCrypto.DecryptXmlAsync(xml, xpathsToDecrypt);
            
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptXmlAsyncThrowsWhenAnXPathItemIsNull()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var xml = "<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>";
            
            string[] xpathsToDecrypt = { null };

            Func<Task> act = () => fakeCrypto.DecryptXmlAsync(xml, xpathsToDecrypt);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void EncryptJsonThrowsWhenCryptoIsNull()
        {
            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            
            ICrypto crypto = null;

            Action act = () => crypto.EncryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptJsonThrowsWhenJsonStringIsNull()
        {
            var fakeCrypto = new FakeCrypto();

            string json = null;
            
            Action act = () => fakeCrypto.EncryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptJsonThrowsWhenJsonPathsToEncryptIsNull()
        {
            var fakeCrypto = new FakeCrypto();

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            
            string[] jsonPathsToEncrypt = null;

            Action act = () => fakeCrypto.EncryptJson(json, jsonPathsToEncrypt);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptJsonThrowsWhenJsonPathsToEncryptIsEmpty()
        {
            var fakeCrypto = new FakeCrypto();

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            
            string[] jsonPathsToEncrypt = { };

            Action act = () => fakeCrypto.EncryptJson(json, jsonPathsToEncrypt);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void EncryptJsonThrowsWhenAnJsonPathItemIsNull()
        {
            var fakeCrypto = new FakeCrypto();

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            
            string[] jsonPathsToEncrypt = { null };

            Action act = () => fakeCrypto.EncryptJson(json, jsonPathsToEncrypt);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void EncryptJsonAsyncThrowsWhenCryptoIsNull()
        {
            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            
            ICrypto crypto = null;

            Func<Task> act = async () => await crypto.EncryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptJsonAsyncThrowsWhenJsonStringIsNull()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            string json = null;
            
            Func<Task> act = async () => await fakeCrypto.EncryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptJsonAsyncThrowsWhenJsonPathsToEncryptIsNull()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            
            string[] jsonPathsToEncrypt = null;

            Func<Task> act = async () => await fakeCrypto.EncryptJsonAsync(json, jsonPathsToEncrypt);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void EncryptJsonAsyncThrowsWhenJsonPathsToEncryptIsEmpty()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            
            string[] jsonPathsToEncrypt = { };

            Func<Task> act = async () => await fakeCrypto.EncryptJsonAsync(json, jsonPathsToEncrypt);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void EncryptJsonAsyncThrowsWhenAnJsonPathItemIsNull()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
            
            string[] jsonPathsToEncrypt = { null };

            Func<Task> act = () => fakeCrypto.EncryptJsonAsync(json, jsonPathsToEncrypt);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptJsonThrowsWhenCryptoIsNull()
        {
            var json = "{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}";
            
            ICrypto crypto = null;

            Action act = () => crypto.DecryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptJsonThrowsWhenJsonStringIsNull()
        {
            var fakeCrypto = new FakeCrypto();

            string json = null;
            
            Action act = () => fakeCrypto.DecryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptJsonThrowsWhenJsonPathsToDecryptIsNull()
        {
            var fakeCrypto = new FakeCrypto();

            var json = "{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}";
            
            string[] jsonPathsToDecrypt = null;

            Action act = () => fakeCrypto.DecryptJson(json, jsonPathsToDecrypt);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptJsonThrowsWhenJsonPathsToDecryptIsEmpty()
        {
            var fakeCrypto = new FakeCrypto();

            var json = "{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}";
            
            string[] jsonPathsToDecrypt = { };

            Action act = () => fakeCrypto.DecryptJson(json, jsonPathsToDecrypt);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptJsonThrowsWhenAnJsonPathItemIsNull()
        {
            var fakeCrypto = new FakeCrypto();

            var json = "{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}";
            
            string[] jsonPathsToDecrypt = { null };

            Action act = () => fakeCrypto.DecryptJson(json, jsonPathsToDecrypt);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptJsonAsyncThrowsWhenCryptoIsNull()
        {
            var json = "{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}";
            
            ICrypto crypto = null;

            Func<Task> act = async () => await crypto.DecryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptJsonAsyncThrowsWhenJsonStringIsNull()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            string json = null;
            
            Func<Task> act = async () => await fakeCrypto.DecryptJsonAsync(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptJsonAsyncThrowsWhenJsonPathsToDecryptIsNull()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var json = "{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}";
            
            string[] jsonPathsToDecrypt = null;

            Func<Task> act = async () => await fakeCrypto.DecryptJsonAsync(json, jsonPathsToDecrypt);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void DecryptJsonAsyncThrowsWhenJsonPathsToDecryptIsEmpty()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var json = "{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}";
            
            string[] jsonPathsToDecrypt = { };

            Func<Task> act = async () => await fakeCrypto.DecryptJsonAsync(json, jsonPathsToDecrypt);

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void DecryptJsonAsyncThrowsWhenAnJsonPathItemIsNull()
        {
            var fakeCrypto = new FakeCrypto().AsAsync();

            var json = "{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}";
            
            string[] jsonPathsToDecrypt = { null };

            Func<Task> act = () => fakeCrypto.DecryptJsonAsync(json, jsonPathsToDecrypt);

            act.Should().ThrowExactly<ArgumentException>();
        }
        
        private static class Base64
        {
            public static string Encrypt(string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
            public static string Decrypt(string value) => Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }
    }
}
