using Microsoft.Extensions.Hosting;
using RockLib.Encryption;
using RockLib.Encryption.Symmetric;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Symmetric
{
    public class DependencyInjectionEncryptionService : IHostedService
    {
        private readonly ICrypto _crypto;
        private IReadOnlyCollection<Credential> _options;
        private readonly Thread _executionThread;

        public DependencyInjectionEncryptionService(ICrypto crypto)
        {
            _crypto = crypto;
            _options = ((InMemoryCredentialRepository)((SymmetricCrypto)_crypto).CredentialRepository).Credentials;
            _executionThread = new Thread(RunEncryptionPrompt);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executionThread.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void RunEncryptionPrompt() => Common.EncryptionPrompt(_crypto, _options, "Dependency Injection Encryption Service");
    }
}
