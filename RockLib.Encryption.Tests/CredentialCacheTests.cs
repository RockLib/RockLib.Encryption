using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace RockLib.Encryption.Tests
{
    [TestFixture]
    public class CredentialCacheTests
    {
        [Test]
        public void CanGetDefaultCredentialWithNoName()
        {
            var credentialCache = new CredentialCache<ICredentialInfo>(new List<ICredentialInfo>
            {
                new SimpleTestCredentialInfo()
            });

            var getResult = credentialCache.TryGetCredential(null, out var credInfo);

            getResult.Should().BeTrue();
            credInfo.Should().NotBeNull();
        }

        [Test]
        public void CanGetDefaultCredentialWithDefaultName()
        {
            var credentialCache = new CredentialCache<ICredentialInfo>(new List<ICredentialInfo>
            {
                new SimpleTestCredentialInfo("default")
            });

            var getResult = credentialCache.TryGetCredential(null, out var credInfo);

            getResult.Should().BeTrue();
            credInfo.Should().NotBeNull();
        }

        [Test]
        public void CanGetDefaultCredentialWhenSearchingWithNonStringNonType()
        {
            var credentialCache = new CredentialCache<ICredentialInfo>(new List<ICredentialInfo>
            {
                new SimpleTestCredentialInfo()
            });

            var getResult = credentialCache.TryGetCredential(new object(), out var credInfo);

            getResult.Should().BeTrue();
            credInfo.Should().NotBeNull();
        }

        [Test]
        public void CannotGetDefaultCredentialWhenNoneExists()
        {
            var credentialCache = new CredentialCache<ICredentialInfo>(new List<ICredentialInfo>
            {
                new SimpleTestCredentialInfo("foo")
            });

            var getResult = credentialCache.TryGetCredential(null, out var credInfo);

            getResult.Should().BeFalse();
            credInfo.Should().BeNull();
        }

        [Test]
        public void CanGetCredentialsByName()
        {
            var foo = new SimpleTestCredentialInfo("foo");
            var bar = new SimpleTestCredentialInfo("bar");

            var credentialCache = new CredentialCache<ICredentialInfo>(new List<ICredentialInfo>
            {
                foo,
                bar
            });

            var getFooResult = credentialCache.TryGetCredential("foo", out var credentialFooInfo);
            var getBarResult = credentialCache.TryGetCredential("bar", out var credentialBarInfo);

            getFooResult.Should().BeTrue();
            credentialFooInfo.Should().BeSameAs(foo);

            getBarResult.Should().BeTrue();
            credentialBarInfo.Should().BeSameAs(bar);
        }

        [Test]
        public void CannotGetCredentialWhenNameDoesNotExist()
        {
            var credentialCache = new CredentialCache<ICredentialInfo>(new List<ICredentialInfo>
            {
                new SimpleTestCredentialInfo("foo")
            });

            var getResult = credentialCache.TryGetCredential("bar", out var credInfo);

            getResult.Should().BeFalse();
            credInfo.Should().BeNull();
        }

        [Test]
        public void CanGetCredentialByType()
        {
            var foo = new SimpleTestCredentialInfo(types: new List<string> { "RockLib.Encryption.Tests.CredentialCacheTests+SimpleTestCredentialInfo" });
            var bar = new SimpleTestCredentialInfo(types: new List<string> { "RockLib.Encryption.ICredentialInfo" });


            var credentialCache = new CredentialCache<ICredentialInfo>(new List<ICredentialInfo>
            {
                foo,
                bar
            });

            var getFooResult = credentialCache.TryGetCredential(typeof(SimpleTestCredentialInfo), out var credentialFooInfo);
            var getBarResult = credentialCache.TryGetCredential(typeof(ICredentialInfo), out var credentialBarInfo);

            getFooResult.Should().BeTrue();
            credentialFooInfo.Should().BeSameAs(foo);

            getBarResult.Should().BeTrue();
            credentialBarInfo.Should().BeSameAs(bar);
        }

        [Test]
        public void CanGetCredentialByNameSpace()
        {
            var foo = new SimpleTestCredentialInfo(namespaces: new List<string> { "RockLib.Encryption.Tests" });
            var bar = new SimpleTestCredentialInfo(namespaces: new List<string> { "RockLib" });


            var credentialCache = new CredentialCache<ICredentialInfo>(new List<ICredentialInfo>
            {
                foo,
                bar
            });

            var getFooResult = credentialCache.TryGetCredential(typeof(SimpleTestCredentialInfo), out var credentialFooInfo);
            var getBarResult = credentialCache.TryGetCredential(typeof(ICredentialInfo), out var credentialBarInfo);

            getFooResult.Should().BeTrue();
            credentialFooInfo.Should().BeSameAs(foo);

            getBarResult.Should().BeTrue();
            credentialBarInfo.Should().BeSameAs(bar);
        }

        [Test]
        public void CredentialCacheCredentialsIsSameAsCredentialListPassedInConstructor()
        {
            var credentialsList = new List<ICredentialInfo>
            {
                new SimpleTestCredentialInfo()
            };

            var credentialCache = new CredentialCache<ICredentialInfo>(credentialsList);

            credentialCache.Credentials.Should().BeSameAs(credentialsList);
        }

        [Test]
        public void NullCredentialsThrowsArgumentNullException()
        {
            Action newCache = () => new CredentialCache<ICredentialInfo>(null);

            newCache.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: credentials");
        }

        private class SimpleTestCredentialInfo : ICredentialInfo
        {
            public SimpleTestCredentialInfo(string name = null, IEnumerable<string> types = null, IEnumerable<string> namespaces = null)
            {
                Name = name;
                Types = types;
                Namespaces = namespaces;
            }
            public string Name { get; }
            public IEnumerable<string> Types { get; }
            public IEnumerable<string> Namespaces { get; }
        }
    }
}
