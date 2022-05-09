using FluentAssertions;
using RockLib.Encryption.FieldLevel;
using RockLib.Encryption.Testing;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RockLib.Encryption.Tests.FieldLevel;

public static class FieldLevelEncryptionExtensionsTests
{
    [Fact]
    public static void EncryptXmlWorks()
    {
        var fakeCrypto = new FakeCrypto();

        var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
        
        var encryptedXml = fakeCrypto.EncryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });

        encryptedXml.Should().Be("<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>");
    }

    [Fact]
    public static void DecryptXmlWorks()
    {
        var fakeCrypto = new FakeCrypto();

        var xml = "<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>";
        
        var decryptedXml = fakeCrypto.DecryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });

        decryptedXml.Should().Be("<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>");
    }

    [Fact]
    public static void EncryptJsonWorks()
    {
        var fakeCrypto = new FakeCrypto();

        var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
        
        var encryptedJson = fakeCrypto.EncryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

        encryptedJson.Should().Be("{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}");
    }

    [Fact]
    public static void DecryptJsonWorks()
    {
        var fakeCrypto = new FakeCrypto();

        var json = "{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}";
        
        var decryptedJson = fakeCrypto.DecryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

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
    public static void EncryptJsonAndDecryptJsonWithRootJsonPathTargetTheWholeInputString(string json)
    {
        var fakeCrypto = new FakeCrypto();

        var root = "$";
        
        var encryptedJson = fakeCrypto.EncryptJson(json, root);

        encryptedJson.Should().NotBe(json);
        encryptedJson.Should().StartWith("\"").And.EndWith("\"");

        var decryptedJson = fakeCrypto.DecryptJson(encryptedJson, root);

        decryptedJson.Should().Be(json);
    }

    [Fact]
    public static void EncryptJsonAndDecryptJsonCanTargetSpecificArrayElements()
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
    public static void EncryptJsonAndDecryptJsonCanTargetAllArrayElements()
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
    public static void EncryptXmlThrowsWhenCryptoIsNull()
    {
        var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
        
        ICrypto crypto = null!;

        var act = () => crypto.EncryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });
        
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public static void EncryptXmlThrowsWhenXmlStringIsNull()
    {
        var fakeCrypto = new FakeCrypto();

        string xml = null!;
        
        var act = () => fakeCrypto.EncryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });
        
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public static void EncryptXmlThrowsWhenXPathsToEncryptIsNull()
    {
        var fakeCrypto = new FakeCrypto();

        var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
        
        string[] xpathsToEncrypt = null!;

        var act = () => fakeCrypto.EncryptXml(xml, xpathsToEncrypt);
        
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public static void EncryptXmlThrowsWhenXPathsToEncryptIsEmpty()
    {
        var fakeCrypto = new FakeCrypto();

        var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";

        var xpathsToEncrypt = Array.Empty<string>();

        var act = () => fakeCrypto.EncryptXml(xml, xpathsToEncrypt);

        act.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public static void EncryptXmlThrowsWhenAnXPathItemIsNull()
    {
        var fakeCrypto = new FakeCrypto();

        var xml = "<foo bar=\"123\"><baz>456</baz><baz>789</baz><qux><garply grault=\"abc\" /></qux></foo>";
        
        string[] xpathsToEncrypt = { null! };

        var act = () => fakeCrypto.EncryptXml(xml, xpathsToEncrypt);

        act.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public static void DecryptXmlThrowsWhenCryptoIsNull()
    {
        var xml = "<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>";
        
        ICrypto crypto = null!;

        var act = () => crypto.DecryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public static void DecryptXmlThrowsWhenXmlStringIsNull()
    {
        var fakeCrypto = new FakeCrypto();

        string xml = null!;
        
        var act = () => fakeCrypto.DecryptXml(xml, new[] { "/foo/@bar", "/foo/baz", "/foo/qux" });

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public static void DecryptXmlThrowsWhenXPathsToDecryptIsNull()
    {
        var fakeCrypto = new FakeCrypto();

        var xml = "<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>";
        
        var xpathsToDecrypt = Array.Empty<string>();

        var act = () => fakeCrypto.DecryptXml(xml, xpathsToDecrypt);

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public static void DecryptXmlThrowsWhenXPathsToDecryptIsEmpty()
    {
        var fakeCrypto = new FakeCrypto();

        var xml = "<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>";

        var xpathsToDecrypt = Array.Empty<string>();

        var act = () => fakeCrypto.DecryptXml(xml, xpathsToDecrypt);

        act.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public static void DecryptXmlThrowsWhenAnXPathItemIsNull()
    {
        var fakeCrypto = new FakeCrypto();

        var xml = "<foo bar=\"[[123]]\"><baz>[[456]]</baz><baz>[[789]]</baz><qux>[[&lt;garply grault=\"abc\" /&gt;]]</qux></foo>";
        
        string[] xpathsToDecrypt = { null! };

        var act = () => fakeCrypto.DecryptXml(xml, xpathsToDecrypt);

        act.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public static void EncryptJsonThrowsWhenCryptoIsNull()
    {
        var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
        
        ICrypto crypto = null!;

        var act = () => crypto.EncryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public static void EncryptJsonThrowsWhenJsonStringIsNull()
    {
        var fakeCrypto = new FakeCrypto();

        string json = null!;
        
        var act = () => fakeCrypto.EncryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public static void EncryptJsonThrowsWhenJsonPathsToEncryptIsNull()
    {
        var fakeCrypto = new FakeCrypto();

        var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
        
        string[] jsonPathsToEncrypt = null!;

        var act = () => fakeCrypto.EncryptJson(json, jsonPathsToEncrypt);

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public static void EncryptJsonThrowsWhenJsonPathsToEncryptIsEmpty()
    {
        var fakeCrypto = new FakeCrypto();

        var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";
        
        var jsonPathsToEncrypt = Array.Empty<string>();

        var act = () => fakeCrypto.EncryptJson(json, jsonPathsToEncrypt);

        act.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public static void EncryptJsonThrowsWhenAnJsonPathItemIsNull()
    {
        var fakeCrypto = new FakeCrypto();

        var json = "{\"foo\":\"abc\",\"bar\":123,\"baz\":true,\"qux\":[1,2,3],\"garply\":{\"grault\":\"xyz\"},\"fred\":null}";

        var jsonPathsToEncrypt = Array.Empty<string>();

        var act = () => fakeCrypto.EncryptJson(json, jsonPathsToEncrypt);

        act.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public static void DecryptJsonThrowsWhenCryptoIsNull()
    {
        var json = "{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}";
        
        ICrypto crypto = null!;

        var act = () => crypto.DecryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public static void DecryptJsonThrowsWhenJsonStringIsNull()
    {
        var fakeCrypto = new FakeCrypto();

        string json = null!;
        
        var act = () => fakeCrypto.DecryptJson(json, new[] { "$.foo", "$.bar", "$.baz", "$.qux", "$.garply", "$.fred" });

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public static void DecryptJsonThrowsWhenJsonPathsToDecryptIsNull()
    {
        var fakeCrypto = new FakeCrypto();

        var json = "{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}";
        
        string[] jsonPathsToDecrypt = null!;

        var act = () => fakeCrypto.DecryptJson(json, jsonPathsToDecrypt);

        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public static void DecryptJsonThrowsWhenJsonPathsToDecryptIsEmpty()
    {
        var fakeCrypto = new FakeCrypto();

        var json = "{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}";
        
        var jsonPathsToDecrypt = Array.Empty<string>();

        var act = () => fakeCrypto.DecryptJson(json, jsonPathsToDecrypt);

        act.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public static void DecryptJsonThrowsWhenAnJsonPathItemIsNull()
    {
        var fakeCrypto = new FakeCrypto();

        var json = "{\"foo\":\"[[\\\"abc\\\"]]\",\"bar\":\"[[123]]\",\"baz\":\"[[true]]\",\"qux\":\"[[[1,2,3]]]\",\"garply\":\"[[{\\\"grault\\\":\\\"xyz\\\"}]]\",\"fred\":\"[[null]]\"}";
        
        string[] jsonPathsToDecrypt = { null! };

        var act = () => fakeCrypto.DecryptJson(json, jsonPathsToDecrypt);

        act.Should().ThrowExactly<ArgumentException>();
    }

    private static class Base64
    {
        public static string Encrypt(string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        public static string Decrypt(string value) => Encoding.UTF8.GetString(Convert.FromBase64String(value));
    }
}
