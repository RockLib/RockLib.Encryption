using FluentAssertions;
using Moq;
using System;
using System.Text;
using XSerializer.Encryption;
using Xunit;

namespace RockLib.Encryption.XSerializer.Tests
{
    public class SerializingCryptoTests
    {
        private static readonly int _bar = 123;
        private static readonly string _baz = "abc";
        private static readonly double _qux = 543.21;

        private const string FooXmlFormat = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Foo xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Bar>{0}</Bar><Baz>{1}</Baz><Qux>{2}</Qux></Foo>";
        private static readonly string FooXml = string.Format(FooXmlFormat, EncryptRaw(_bar), EncryptRaw(_baz), EncryptRaw(_qux));

        private const string FooJsonFormat = "{{\"Bar\":\"{0}\",\"Baz\":\"{1}\",\"Qux\":\"{2}\"}}";
        private static readonly string FooJson = string.Format(FooJsonFormat, EncryptRaw(_bar), EncryptJsonString(_baz), EncryptRaw(_qux));

        private static readonly Mock<Base64Crypto> _mockCrypto = new Mock<Base64Crypto>() { CallBase = true };

        static SerializingCryptoTests() => Crypto.SetCurrent(_mockCrypto.Object);

        [Fact]
        public void TheDefaultEncryptionMechanismHasItsCryptoSetFromCryptoCurrent()
        {
            SerializingCrypto.EncryptionMechanism.Crypto.Should().BeSameAs(Crypto.Current);
        }

        [Fact]
        public void ToXmlWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            SerializingCrypto.ToXml(foo);

            _mockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [Fact]
        public void ToXmlWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            var credentialName = "foobar";

            SerializingCrypto.ToXml(foo, credentialName);

            _mockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [Fact]
        public void FromXmlWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            SerializingCrypto.FromXml<Foo>(FooXml);

            _mockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [Fact]
        public void FromXmlWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            var credentialName = "foobar";

            SerializingCrypto.FromXml<Foo>(FooXml, credentialName);

            _mockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [Fact]
        public void ToJsonWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            SerializingCrypto.ToJson(foo);

            _mockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [Fact]
        public void ToJsonWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            var credentialName = "foobar";

            SerializingCrypto.ToJson(foo, credentialName);

            _mockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [Fact]
        public void FromJsonWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            SerializingCrypto.FromJson<Foo>(FooJson);

            _mockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [Fact]
        public void FromJsonWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            var credentialName = "foobar";

            SerializingCrypto.FromJson<Foo>(FooJson, credentialName);

            _mockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [Fact]
        public void CryptoToXmlWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            Crypto.Current.ToXml(foo);

            _mockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [Fact]
        public void CryptoToXmlWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            var credentialName = "foobar";

            Crypto.Current.ToXml(foo, credentialName);

            _mockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [Fact]
        public void CryptoFromXmlWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            Crypto.Current.FromXml<Foo>(FooXml);

            _mockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [Fact]
        public void CryptoFromXmlWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            var credentialName = "foobar";

            Crypto.Current.FromXml<Foo>(FooXml, credentialName);

            _mockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [Fact]
        public void CryptoToJsonWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            Crypto.Current.ToJson(foo);

            _mockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [Fact]
        public void CryptoToJsonWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            var credentialName = "foobar";

            Crypto.Current.ToJson(foo, credentialName);

            _mockCrypto.Verify(c => c.GetEncryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [Fact]
        public void CryptoFromJsonWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            Crypto.Current.FromJson<Foo>(FooJson);

            _mockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == null)), Times.Once());
        }

        [Fact]
        public void CryptoFromJsonWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.Invocations.Clear();

            var credentialName = "foobar";

            Crypto.Current.FromJson<Foo>(FooJson, credentialName);

            _mockCrypto.Verify(c => c.GetDecryptor(It.Is<string>(o => o == credentialName)), Times.Once());
        }

        [Fact]
        public void ToXmlSerializesCorrectly()
        {
            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            var xml = SerializingCrypto.ToXml(foo);
            var expectedXml = string.Format(FooXmlFormat,
                EncryptRaw(foo.Bar), EncryptRaw(foo.Baz), EncryptRaw(foo.Qux));

            xml.Should().Be(expectedXml);
        }

        [Fact]
        public void FromXmlDeserializesCorrectly()
        {
            var foo = SerializingCrypto.FromXml<Foo>(FooXml);

            foo.Bar.Should().Be(_bar);
            foo.Baz.Should().Be(_baz);
            foo.Qux.Should().Be(_qux);
        }

        [Fact]
        public void ToJsonSerializesCorrectly()
        {
            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            var json = SerializingCrypto.ToJson(foo);
            var expectedJson = string.Format(FooJsonFormat,
                EncryptRaw(foo.Bar), EncryptJsonString(foo.Baz), EncryptRaw(foo.Qux));

            json.Should().Be(expectedJson);
        }

        [Fact]
        public void FromJsonDeserializesCorrectly()
        {
            var foo = SerializingCrypto.FromJson<Foo>(FooJson);

            foo.Bar.Should().Be(_bar);
            foo.Baz.Should().Be(_baz);
            foo.Qux.Should().Be(_qux);
        }

        [Fact]
        public void CryptoToXmlSerializesCorrectly()
        {
            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            var xml = Crypto.Current.ToXml(foo);
            var expectedXml = string.Format(FooXmlFormat,
                EncryptRaw(foo.Bar), EncryptRaw(foo.Baz), EncryptRaw(foo.Qux));

            xml.Should().Be(expectedXml);
        }

        [Fact]
        public void CryptoFromXmlDeserializesCorrectly()
        {
            var foo = Crypto.Current.FromXml<Foo>(FooXml);

            foo.Bar.Should().Be(_bar);
            foo.Baz.Should().Be(_baz);
            foo.Qux.Should().Be(_qux);
        }

        [Fact]
        public void CryptoToJsonSerializesCorrectly()
        {
            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            var json = Crypto.Current.ToJson(foo);
            var expectedJson = string.Format(FooJsonFormat,
                EncryptRaw(foo.Bar), EncryptJsonString(foo.Baz), EncryptRaw(foo.Qux));

            json.Should().Be(expectedJson);
        }

        [Fact]
        public void CryptoFromJsonDeserializesCorrectly()
        {
            var foo = Crypto.Current.FromJson<Foo>(FooJson);

            foo.Bar.Should().Be(_bar);
            foo.Baz.Should().Be(_baz);
            foo.Qux.Should().Be(_qux);
        }

        public class Foo
        {
            [Encrypt]
            public int Bar { get; set; }

            [Encrypt]
            public string Baz { get; set; }

            [Encrypt]
            public double Qux { get; set; }
        }

        public class Base64Crypto : ICrypto
        {
            public virtual bool CanDecrypt(string credentialName) => true;
            public virtual bool CanEncrypt(string credentialName) => true;
            public virtual string Decrypt(string cipherText, string credentialName) => Base64.Decrypt(cipherText);
            public virtual byte[] Decrypt(byte[] cipherText, string credentialName) => throw new NotImplementedException();
            public virtual string Encrypt(string plainText, string credentialName) => Base64.Encrypt(plainText);
            public virtual byte[] Encrypt(byte[] plainText, string credentialName) => throw new NotImplementedException();
            public virtual IDecryptor GetDecryptor(string credentialName) => MockDecryptor.Object;
            public virtual IEncryptor GetEncryptor(string credentialName) => MockEncryptor.Object;
            public Mock<Base64Encryptor> MockEncryptor { get; } = new Mock<Base64Encryptor>() { CallBase = true };
            public Mock<Base64Decryptor> MockDecryptor { get; } = new Mock<Base64Decryptor>() { CallBase = true };
        }

        public class Base64Encryptor : IEncryptor
        {
            public void Dispose() { }
            public string Encrypt(string plainText) => Base64.Encrypt(plainText);
            public byte[] Encrypt(byte[] plainText) => throw new NotImplementedException();
        }

        public class Base64Decryptor : IDecryptor
        {
            public void Dispose() { }
            public string Decrypt(string cipherText) => Base64.Decrypt(cipherText);
            public byte[] Decrypt(byte[] cipherText) => throw new NotImplementedException();
        }

        private static class Base64
        {
            public static string Encrypt(string plainText) => Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
            public static string Decrypt(string cipherText) => Encoding.UTF8.GetString(Convert.FromBase64String(cipherText));
        }

        private static string EncryptJsonString(string value) => Base64.Encrypt("\"" + value + "\"");
        private static string EncryptRaw(object value) => Base64.Encrypt(value.ToString());
    }
}
