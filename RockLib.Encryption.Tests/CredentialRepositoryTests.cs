using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RockLib.Encryption.Symmetric;

namespace RockLib.Encryption.Tests
{
    [TestFixture]
    public class CredentialRepositoryTests
    {
        [Test]
        public void CanGetCredentialUsingICredentialListConstuctor()
        {
            var foo = new SimpleCredential();
            var bar = new SimpleCredential("bar");

            var credentialRepository = new CredentialRepository(new List<ICredential>{ foo, bar});

            credentialRepository.TryGet(null, out var fooCredential);
            credentialRepository.TryGet("bar", out var barCredential);

            fooCredential.Should().BeSameAs(foo);
            barCredential.Should().BeSameAs(bar);
        }

        [Test]
        public void CanGetCredentialUsingCredentialCacheConstuctor()
        {
            var foo = new SimpleCredential();
            var bar = new SimpleCredential("bar");

            var credentialCache = new CredentialCache<ICredential>(new List<ICredential> { foo, bar });

            var credentialRepository = new CredentialRepository(credentialCache);

            credentialRepository.TryGet(null, out var fooCredential);
            credentialRepository.TryGet("bar", out var barCredential);

            credentialRepository.Cache.Should().BeSameAs(credentialCache);
            fooCredential.Should().BeSameAs(foo);
            barCredential.Should().BeSameAs(bar);
        }

        [Test]
        public void CredentialListConstuctorThrowsArgumentNullExcptionIfCredentialsIsNull()
        {
            Action newRepo = () => new CredentialRepository((IEnumerable<ICredential>)null);

            newRepo.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: credentials");
        }

        [Test]
        public void CredentialCacheConstuctorThrowsArgumentNullExcptionIfCacheIsNull()
        {
            Action newRepo = () => new CredentialRepository((CredentialCache<ICredential>)null);

            newRepo.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: cache");
        }

        private class SimpleCredential : ICredential
        {
            public SimpleCredential(string name = null)
            {
                Name = name;
            }
            public string Name { get; }
            public SymmetricAlgorithm Algorithm { get; }
            public ushort IVSize { get; }
            public byte[] GetKey() => new byte[0];
        }
    }
}
