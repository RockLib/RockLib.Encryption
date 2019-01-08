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
        public void CredentialCacheCredentialsContainsTheNonDefaultCredentialsPassedInConstructor()
        {
            SimpleTestCredentialInfo defaultCredential = new SimpleTestCredentialInfo();
            SimpleTestCredentialInfo fooCredential = new SimpleTestCredentialInfo("foo");
            SimpleTestCredentialInfo barCredential = new SimpleTestCredentialInfo("bar");

            var credentialsList = new List<ICredentialInfo>{ defaultCredential, fooCredential, barCredential };

            var credentialCache = new CredentialCache<ICredentialInfo>(credentialsList);

            credentialCache.Credentials.Should().Contain(new[] { fooCredential, barCredential });
        }

        [Test]
        public void CredentialCacheDefaultCredentialIsTheDefaultCredentialsPassedInConstructor()
        {
            SimpleTestCredentialInfo defaultCredential = new SimpleTestCredentialInfo();
            SimpleTestCredentialInfo fooCredential = new SimpleTestCredentialInfo("foo");
            SimpleTestCredentialInfo barCredential = new SimpleTestCredentialInfo("bar");

            var credentialsList = new List<ICredentialInfo> { defaultCredential, fooCredential, barCredential };

            var credentialCache = new CredentialCache<ICredentialInfo>(credentialsList);

            credentialCache.DefaultCredential.Should().BeSameAs(defaultCredential);
        }

        [Test]
        public void NullCredentialsThrowsArgumentNullException()
        {
            Action newCache = () => new CredentialCache<ICredentialInfo>(null);

            newCache.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: credentials");
        }

        private class SimpleTestCredentialInfo : ICredentialInfo
        {
            public SimpleTestCredentialInfo(string name = null)
            {
                Name = name;
            }
            public string Name { get; }
        }
    }
}
