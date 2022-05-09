# RockLib.Encryption Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased

This will be the last release of this project.

#### Added
- Added `.editorconfig` and `Directory.Build.props` files to ensure consistency.

#### Changed
- Supported targets: net6.0, netcoreapp3.1, and net48.
- Updated all NuGet package dependencies to their latest versions.
- As the package now uses nullable reference types, some method parameters now specify if they can accept nullable values.
- "Async-over-sync" code has been removed. This means that the following members have been removed:
  - `AsAsyncExtension`
  - `IAsyncCrypto`
  - `IAsyncCryptoExtensions`
  - `IAsyncDecryptor`
  - `IAsyncEncryptor`
  - `SynchronousAsyncCrypto`
  - `SynchronousAsyncDecryptor`
  - `SynchronousAsyncEncryptor`
  - `FieldLevelEncryptionExtensions`
    - `DecryptJsonAsync`
    - `DecryptXmlAsync`
    - `EncryptJsonAsync`
    - `EncryptXmlAsync`
  - `Crypto`
    - `DecryptAsync`
    - `EncryptAsync`
    - `GetAsyncDecryptor`
    - `GetAsyncEncryptor`
  - `CompositeCrypto`
    - `DecryptAsync`
    - `EncryptAsync`
    - `GetAsyncDecryptor`
    - `GetAsyncEncryptor`
- Algorithms that have been deemed [weak](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca5350), [broken](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca5351), or are now obsolete. have been removed from `SymmetricAlgorithm` and `SymmetricAlgorithmExtensions.CreateSymmetricAlgorithm()`. These include:
  - `DES`
  - `RC2`
  - `Rijndael`
  - `TripleDES`
- The following classes are now `sealed`
  - `SymmetricDecryptor`
  - `SymmetricEncryptor`
- TODO: Consider removing `Crypto`

## 2.3.3 - 2021-08-12

#### Changed

- Changes "Quicken Loans" to "Rocket Mortgage".
- Updates RockLib.Collections to latest version, [1.0.6](https://github.com/RockLib/RockLib.Collections/blob/main/RockLib.Collections/CHANGELOG.md#106---2021-08-11).
- Updates RockLib.Configuration to latest version, [2.5.3](https://github.com/RockLib/RockLib.Configuration/blob/main/RockLib.Configuration/CHANGELOG.md#253---2021-08-11).
- Updates RockLib.Configuration.ObjectFactory to latest version, [1.6.9](https://github.com/RockLib/RockLib.Configuration/blob/main/RockLib.Configuration.ObjectFactory/CHANGELOG.md#169---2021-08-11).

## 2.3.2 - 2021-05-06

#### Added

- Adds SourceLink to nuget package.

#### Changed

- Updates RockLib.Configuration, RockLib.Configuration.ObjectFactory and RockLib.Collections packages to latest versions, which include SourceLink.

----

**Note:** Release notes in the above format are not available for earlier versions of
RockLib.Secrets. What follows below are the original release notes.

----

## 2.3.1

Fixs bugs in field-level encryption when using FakeCrypto or other implementations of ICrypto that don't encrypt strings to base-64 encoded values.

## 2.3.0

- Adds FakeCrypto class, intended to be used in application testing.
- Adds net5.0 target.

## 2.2.2

Adds icon to project and nuget package.

## 2.2.1

Updates to align with nuget conventions.

## 2.2.0

Adds extension method overloads for dependency injection that don't include a credential name. Allows for easier registration of the default credential.

## 2.1.0

Changes CompositeCrypto to also implement IAsyncCrypto. Previously, when applied to a CompositeCrypto, the .AsAsync() extension method would always return a SynchronousAsyncCrypto, even if the CompositeCrypto contained cryptos that implement IAsyncCrypto. With the change, CompositeCrypto implements IAsyncCrypto itself and does so by calling .AsAsync() on whatever crypto matches the credential name.

## 2.0.0

- Changes the object parameter named keyIdentifier, found in many classes and methods, to be of type string and named credentialName.
- Configuration for the SymmetricCrypto class is flattened.
- Removes the CryptoConfiguration classes in the RockLib.Encryption.Symmetric namespace.
- Replaces the CredentialCache class with NamedCollection from the RockLib.Collections package.
- CredentialRepository renamed to InMemoryCredentialRepository and refactored to use the NamedCollection.
- Removes the RockLib.Encryption.Symmetric.ICredential interface.
- Changes the type of the key parameter in the Symmetric.Credential class from byte[] to Func<byte[]>.
- Removes the ICredentialInfo interface.
- Makes CancellationToken parameters optional wherever possible.
- Adds field-level encryption extension methods for the IAsyncCrypto interface (not just ICrypto).
- Updates the RockLib.Configuration and RockLib.Configuration.ObjectFactory packages to the latest versions.
- Drops support for .NET Standard 1.6, adds support for .NET Framework 4.6.2.
- Adds support for rocklib_encryption config section.
- Adds ConfigSection attribute for the Rockifier tool.
- Adds dependency injection extensions for service collections.

## 2.0.0-alpha07

Adds dependency injection extensions for service collections

## 1.0.5

Adds support for "RockLib_Encryption" config section (in addition to "RockLib.Encryption").

## 2.0.0-alpha06

- Adds support for rocklib_encryption config section.
- Adds ConfigSection attribute for the Rockifier tool.

## 2.0.0-alpha05

- Makes CancellationToken parameters optional wherever possible.
- Adds field-level encryption extension methods for the IAsyncCrypto interface (not just ICrypto).

## 2.0.0-alpha04

- Adds additional constructor to SymmetricCrypto taking an ICredentialRepository parameter. This gives users the ability to retrieve their credentials from any source, not just an in-memory collection. The existing constructor is unchanged, but the Credentials property has been replaced by a CredentialRepository property.
- Updates RockLib.Collections package to 1.0.1.

## 2.0.0-alpha03

- Replaces the CredentialCache class with NamedCollection from the RockLib.Collections package.
- Removes the ICredentialInfo interface.

## 2.0.0-alpha02

Changes the type of the key parameter in the Symmetric.Credential class from byte[] to Func<byte[]>.

## 2.0.0-alpha01

- Changes the object parameter named keyIdentifier, found in many classes and methods, to be of type string and named credentialName.
- Simplifies the SymmetricCrypto class by including only one constructor.
  - This allows the ICredentialRepository interface and CredentialRepository and CryptoConfiguration classes in the RockLib.Encryption.Symmetric namespace to be deleted.
  - Configuration is flattened.
- Removes the RockLib.Encryption.Symmetric.ICredential interface.
- Updates the RockLib.Configuration and RockLib.Configuration.ObjectFactory packages to the latest versions.
- Drops support for .NET Standard 1.6, adds support for .NET Framework 4.6.2.

## 1.0.4

Updated configuration packages, allowing .net applications to configure from app.config/web.config.

## 1.0.3

- Adds extension methods for field-level encryption via xpath/jsonpath.
- Updates RockLib.Configuration.ObjectFactory dependency to latest version.

## 1.0.2

The various Decrypt methods assume that the cipherText parameter is plain-text if it doesn't match protocol and just returns the value passed to it.

## 1.0.1

Update RockLib.Configuration and RockLib.Configuration.ObjectFactory nuget packages to latest versions.

## 1.0.0

Initial Release