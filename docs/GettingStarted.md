# Getting Started

RockLib.Encryption provides a simple API for encryption and decryption text and binary data. In this tutorial, we will be building a console application that encrypts and decrypts sample text using the Rijndael algorithm.

---

Create a new .NET Core 2.0 (or above) console application named "EncryptionApp".

---

Add a nuget references for "RockLib.Encryption" and "Microsoft.Extensions.Hosting" to the project.

---

Add a class named "EncryptionService" to the project. Replace the default code with the following:

```c#
using Microsoft.Extensions.Hosting;
using RockLib.Encryption;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EncryptionApp
{
    public class EncryptionService : IHostedService
    {
        private readonly ICrypto _crypto;

        public EncryptionService(ICrypto crypto)
        {
            _crypto = crypto;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Work should not be done in the actual StartAsync method of an IHostedService.
            ThreadPool.QueueUserWorkItem(DoEncryptionWork);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Nothing to do.
            return Task.CompletedTask;
        }

        private void DoEncryptionWork(object state)
        {
            // Slight delay to allow the app starting log messages to be written.
            Thread.Sleep(500);

            Console.WriteLine();

            // Our sensitive data is a "social security number".
            string ssn = "123-45-6789";
            Console.WriteLine($"Original SSN: {ssn}");

            // Encrypt the SSN. The resulting value should be different from the original SSN.
            string encryptedSsn = _crypto.Encrypt(ssn);
            Console.WriteLine($"Encrypted SSN: {encryptedSsn}");

            // Decrypt the SSN. The resulting value should be the same as the original SSN.
            string decryptedSsn = _crypto.Decrypt(encryptedSsn);
            Console.WriteLine($"Decrypted SSN: {decryptedSsn}");
            
            Console.WriteLine();
        }
    }
}
```

---

Replace the contents of Program.cs with the following:

```c#
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RockLib.Encryption.Symmetric;
using RockLib.Encryption.Symmetric.DependencyInjection;

namespace EncryptionApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSymmetricCrypto()
                        .AddCredential("CQSImVlbvJMZcnrkzT3/ouW1klt6STljrDjRiBzIsSk=", SymmetricAlgorithm.Rijndael);

                    // ExampleService has a dependency on ICrypto.
                    services.AddHostedService<EncryptionService>();
                });
        }
    }
}
```

---

Start the application. The output should look something like this (press Ctrl+C to exit):

```
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\bfriesen\source\repos\ConsoleApp24\EncryptionApp\bin\Debug\netcoreapp3.1

Original SSN: 123-45-6789
Encrypted SSN: ARAAq19rPjvpARsGunkHfYjxxG+NJ/1BlnZSklYBGcr6McA=
Decrypted SSN: 123-45-6789

info: Microsoft.Hosting.Lifetime[0]
      Application is shutting down...
```
