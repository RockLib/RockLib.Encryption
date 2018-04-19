using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Text;
using XSerializer.Encryption;

namespace RockLib.Encryption.XSerializer.Tests
{
    [TestFixture]
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

        [Test]
        public void TheDefaultEncryptionMechanismHasItsCryptoSetFromCryptoCurrent()
        {
            SerializingCrypto.EncryptionMechanism.Crypto.Should().BeSameAs(Crypto.Current);
        }

        [Test]
        public void ToXmlWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.ResetCalls();

            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            SerializingCrypto.ToXml(foo);

            _mockCrypto.Verify(c => c.GetEncryptor(It.Is<object>(o => o is Type && ((Type)o) == typeof(Foo))), Times.Once());
        }

        [Test]
        public void ToXmlWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.ResetCalls();

            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            var keyIdentifier = new object();

            SerializingCrypto.ToXml(foo, keyIdentifier);

            _mockCrypto.Verify(c => c.GetEncryptor(It.Is<object>(o => o == keyIdentifier)), Times.Once());
        }

        [Test]
        public void FromXmlWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.ResetCalls();

            SerializingCrypto.FromXml<Foo>(FooXml);

            _mockCrypto.Verify(c => c.GetDecryptor(It.Is<object>(o => o is Type && ((Type)o) == typeof(Foo))), Times.Once());
        }

        [Test]
        public void FromXmlWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.ResetCalls();

            var keyIdentifier = new object();

            SerializingCrypto.FromXml<Foo>(FooXml, keyIdentifier);

            _mockCrypto.Verify(c => c.GetDecryptor(It.Is<object>(o => o == keyIdentifier)), Times.Once());
        }

        [Test]
        public void ToJsonWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.ResetCalls();

            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            SerializingCrypto.ToJson(foo);

            _mockCrypto.Verify(c => c.GetEncryptor(It.Is<object>(o => o is Type && ((Type)o) == typeof(Foo))), Times.Once());
        }

        [Test]
        public void ToJsonWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.ResetCalls();

            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            var keyIdentifier = new object();

            SerializingCrypto.ToJson(foo, keyIdentifier);

            _mockCrypto.Verify(c => c.GetEncryptor(It.Is<object>(o => o == keyIdentifier)), Times.Once());
        }

        [Test]
        public void FromJsonWithNoKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.ResetCalls();

            SerializingCrypto.FromJson<Foo>(FooJson);

            _mockCrypto.Verify(c => c.GetDecryptor(It.Is<object>(o => o is Type && ((Type)o) == typeof(Foo))), Times.Once());
        }

        [Test]
        public void FromJsonWithKeyIdentifierCallsCryptoGetEncryptorOnce()
        {
            _mockCrypto.ResetCalls();

            var keyIdentifier = new object();

            SerializingCrypto.FromJson<Foo>(FooJson, keyIdentifier);

            _mockCrypto.Verify(c => c.GetDecryptor(It.Is<object>(o => o == keyIdentifier)), Times.Once());
        }

        [Test]
        public void ToXmlSerializesCorrectly()
        {
            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            var xml = SerializingCrypto.ToXml(foo);
            var expectedXml = string.Format(FooXmlFormat,
                EncryptRaw(foo.Bar), EncryptRaw(foo.Baz), EncryptRaw(foo.Qux));

            xml.Should().Be(expectedXml);
        }

        [Test]
        public void FromXmlDeserializesCorrectly()
        {
            var foo = SerializingCrypto.FromXml<Foo>(FooXml);

            foo.Bar.Should().Be(_bar);
            foo.Baz.Should().Be(_baz);
            foo.Qux.Should().Be(_qux);
        }

        [Test]
        public void ToJsonSerializesCorrectly()
        {
            var foo = new Foo { Bar = _bar, Baz = _baz, Qux = _qux };

            var json = SerializingCrypto.ToJson(foo);
            var expectedJson = string.Format(FooJsonFormat,
                EncryptRaw(foo.Bar), EncryptJsonString(foo.Baz), EncryptRaw(foo.Qux));

            json.Should().Be(expectedJson);
        }

        [Test]
        public void FromJsonDeserializesCorrectly()
        {
            var foo = SerializingCrypto.FromJson<Foo>(FooJson);

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
            public virtual bool CanDecrypt(object keyIdentifier) => true;
            public virtual bool CanEncrypt(object keyIdentifier) => true;
            public virtual string Decrypt(string cipherText, object keyIdentifier) => Base64.Decrypt(cipherText);
            public virtual byte[] Decrypt(byte[] cipherText, object keyIdentifier) => throw new NotImplementedException();
            public virtual string Encrypt(string plainText, object keyIdentifier) => Base64.Encrypt(plainText);
            public virtual byte[] Encrypt(byte[] plainText, object keyIdentifier) => throw new NotImplementedException();
            public virtual IDecryptor GetDecryptor(object keyIdentifier) => MockDecryptor.Object;
            public virtual IEncryptor GetEncryptor(object keyIdentifier) => MockEncryptor.Object;
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
