using Microsoft.Extensions.Hosting;
using RockLib.Encryption;
using RockLib.Encryption.Symmetric;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Symmetric
{
    public class StaticCryptoEncryptionService : IHostedService
    {
        private IReadOnlyCollection<Credential> _credentials;
        private readonly Thread _executionThread;

        public StaticCryptoEncryptionService()
        {
            _credentials = ((InMemoryCredentialRepository)((SymmetricCrypto)Crypto.Current).CredentialRepository).Credentials;
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

        private void RunEncryptionPrompt() => Common.EncryptionPrompt(Crypto.Current, _credentials, "Static Crypto.Current Encryption Service");
    }
}
