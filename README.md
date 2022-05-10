# RockLib.Encryption

*An easy-to-use, easy-to-configure crypto API.*

Note: 3.0.0 is the last major versoin release for `RockLib.Encryption`.

### RockLib.Encryption [![Build status](https://ci.appveyor.com/api/projects/status/q9s982i6d34tc318?svg=true)](https://ci.appveyor.com/project/RockLib/rocklib-encryption) [![NuGet](https://img.shields.io/nuget/vpre/RockLib.Encryption.svg)](https://www.nuget.org/packages/RockLib.Encryption)

### RockLib.Encryption.XSerializer [![Build status](https://ci.appveyor.com/api/projects/status/q9s982i6d34tc318?svg=true)](https://ci.appveyor.com/project/RockLib/rocklib-encryption-rss3w) [![NuGet](https://img.shields.io/nuget/vpre/RockLib.Encryption.XSerializer.svg)](https://www.nuget.org/packages/RockLib.Encryption.XSerializer)

---

- [Getting started](docs/GettingStarted.md)
- [Concepts](docs/Concepts.md)
  - [`ICrypto` interface](docs/Concepts.md#icrypto-interface)
  - [`CanEncrypt` / `CanDecrypt` methods](docs/Concepts.md#canencrypt--candecrypt-methods)
  - [`Encrypt` / `Decrypt` methods](docs/Concepts.md#encrypt--decrypt-methods)
  - [`GetEncryptor` / `GetDecryptor` methods](docs/Concepts.md#getencryptor--getdecryptor-methods)
- [Dependency injection](docs/DependencyInjection.md)
- [Crypto static class](docs/Crypto.md)
  - [Configuration](docs/Crypto.md#configuration)
- [ICrypto implementations](docs/Implementations.md)
  - [SymmetricCrypto](docs/Implementations.md#symmetriccrypto-class)
  - [CompositeCrypto](docs/Implementations.md#compositecrypto-class)
- [Field-level encryption](docs/FieldLevelEncryption.md)
  - [XPath / JSONPath](docs/FieldLevelEncryption.md#xpath--jsonpath)
  - [RockLib.Encryption.XSerializer](docs/FieldLevelEncryption.md#rocklibencryptionxserializer)
