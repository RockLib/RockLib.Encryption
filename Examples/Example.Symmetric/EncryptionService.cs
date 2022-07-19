﻿using Microsoft.Extensions.Hosting;
using RockLib.Encryption;
using RockLib.Encryption.Symmetric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CA1303 // Do not pass literals as localized parameters
namespace Example.Symmetric;

public class EncryptionService : IHostedService
{
    private readonly ICrypto _crypto;
    private readonly Thread _executionThread;

    public EncryptionService(ICrypto crypto)
    {
        _crypto = crypto;
        _executionThread = new Thread(RunEncryptionPrompt) { IsBackground = true };
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

    private void RunEncryptionPrompt()
    {
        Thread.Sleep(1000);
        Console.WriteLine();
        Console.WriteLine($"Starting...");
        Console.WriteLine();

        while (true)
        {
            try
            {
                Console.Write("Please enter the text you would like to encrypt: ");

                var input = Console.ReadLine();

                var credentialNames = GetCredentialNames();

                var selectedIndex = Prompt("Select the credential to encrypt the text:", credentialNames);

                if (selectedIndex == -1)
                    return;

                var encryptCredentialName = credentialNames[selectedIndex];

                string encrypted;
#pragma warning disable CA1031 // Do not catch general exception types
                try
                {
                    encrypted = _crypto.Encrypt(input!, encryptCredentialName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to encrypt using '{encryptCredentialName}'. Error: {ex.Message}");
                    continue;
                }
#pragma warning restore CA1031 // Do not catch general exception types

                Console.WriteLine("Encryption successful: " + encrypted);
                Console.WriteLine();

                selectedIndex = Prompt("Select the credential to decrypt the text:", credentialNames);

                if (selectedIndex == -1)
                    return;

                var decryptCredentialName = credentialNames[selectedIndex];

                string decrypted;
#pragma warning disable CA1031 // Do not catch general exception types
                try
                {
                    decrypted = _crypto.Decrypt(encrypted, decryptCredentialName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to decrypt using '{decryptCredentialName}'. Error: {ex.Message}");
                    continue;
                }
#pragma warning restore CA1031 // Do not catch general exception types

                Console.WriteLine("Decryption successful: " + decrypted);
            }
            finally
            {
                Console.WriteLine("Restarting...");
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }

    private static int Prompt(string prompt, IReadOnlyList<string?> credentialNames)
    {
        Console.WriteLine(prompt);
        for (var i = 0; i < credentialNames.Count; i++)
            Console.WriteLine($"{i + 1}) {credentialNames[i]}");

        Console.Write(">");

        while (true)
        {
            var line = Console.ReadLine();
            if (line is null)
                return -1;
            if (int.TryParse(line, out var selection)
                && selection > 0 && selection <= credentialNames.Count)
                return selection - 1;
            Console.WriteLine($" Invalid input. Enter a number between 1 and {credentialNames.Count}.");
        }
    }

    private IReadOnlyList<string?> GetCredentialNames()
    {
        // The ICrypto interface doesn't provide a mechanism to enumerate the credential names
        // that it supports. But the expected type of ICrypto, SymmetricCrypto, has a public
        // CredentialRepository property. And the expected type of that property,
        // InMemoryCredentialRepository, has a public Credentials property, and this property
        // allows us to enumerate the credential names. Dynamic is the easiest way to accomplish
        // this task.
        dynamic crypto = _crypto;
        IReadOnlyCollection<Credential> credentials = crypto.CredentialRepository.Credentials;
        return credentials.Select(credential => credential.Name).ToList();
    }
}
#pragma warning restore CA1303 // Do not pass literals as localized parameters
