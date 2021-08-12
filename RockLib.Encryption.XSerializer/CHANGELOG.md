# RockLib.Encryption.XSerializer Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased

#### Changed

- Changes "Quicken Loans" to "Rocket Mortgage".
- Updates RockLib.Encryption to latest version, [2.3.3](https://github.com/RockLib/RockLib.Encryption/blob/main/RockLib.Encryption/CHANGELOG.md#233---2021-08-12).

## 2.1.4 - 2021-05-06

#### Added

- Adds SourceLink to nuget package.

#### Changed

- Updates RockLib.Encryption package to latest versions, which include SourceLink.

----

**Note:** Release notes in the above format are not available for earlier versions of
RockLib.Secrets. What follows below are the original release notes.

----

## 2.1.3

Adds net5.0 target

## 2.1.2

Adds icon to project and nuget package.

## 2.1.1

Updates to align with nuget conventions.

## 2.1.0

- Adds extension methods off of the ICrypto interface for field-level encryption.
- Updates RockLib.Encryption dependency to the latest version, 2.2.0.

## 2.0.0

- Updates to the latest RockLib.Encryption package.
- Changes the object parameter named keyIdentifier to be of type string and named credentialName.

## 1.0.4

Updates the libsodium and RockLib.Encryption packages to latest.

## 2.0.0-alpha04

Updates RockLib.Encryption version to support RockLib_Encryption

## 2.0.0-alpha03

Updates RockLib.Encryption package to 2.0.0-alpha05.

## 2.0.0-alpha02

Updates RockLib.Encryption package to 2.0.0-alpha04.

## 2.0.0-alpha01

Initial prerelease of 2.0.0 version.

## 1.0.3

Updated RockLib.Encryption package.

## 1.0.2

Updates RockLib.Encryption and XSerializer dependencies to latest versions.

## 1.0.1

SerializingCrypto can deserialize/decrypt payloads with properties marked with [Encrypt] where the actual property value is not encrypted. This allows for safer deployment strategies when existing properties have been identified as sensitive and in need of encryption. Receiving applications can be deployed first, able to handle both the current plain-text property values and also the encrypted values when sending systems are deployed with the matching change.

## 1.0.0

- Adds RockLib.Encryption.XSerializer.CryptoEncryptionMechanism, an adapter class.
  - Allows any implementation of ICrypto to used in XSerializer's field-level encryption mechanism.
  - Implements the XSerializer.Encryption.IEncryptionMechanism interface.
  - Behavior delegated to the instance of RockLib.Encryption.ICrypto passed to its constructor.
- Adds RockLib.Encryption.SerializingCrypto static class.
  - Defines convenient methods for field-level encryption: ToXml, FromXml, ToJson, FromJson.
    - Each method serializes/deserializes with XSerializer.
    - Injects the value of its EncryptionMechanism property into each serialization operation.
  - public static CryptoEncryptionMechanism EncryptionMechanism { get; }
    - Default value is new RockLib.Encryption.XSerializer.CryptoEncryptionMechanism(RockLib.Encryption.Crypto.Current).
    - Call the SetEncryptionMechanism method at beginning of an application to change the value.
    - After this property is accessed, its value can no longer be changed.