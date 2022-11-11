---
sidebar_position: 2
---

# Concepts

## `ICrypto` interface

`ICrypto` is the main abstraction in RockLib.Encryption. Here is its definition:

```csharp
public interface ICrypto
{
    string Encrypt(string plainText, string credentialName);
    string Decrypt(string cipherText, string credentialName);
    byte[] Encrypt(byte[] plainText, string credentialName);
    byte[] Decrypt(byte[] cipherText, string credentialName);
    IEncryptor GetEncryptor(string credentialName);
    IDecryptor GetDecryptor(string credentialName);
    bool CanEncrypt(string credentialName);
    bool CanDecrypt(string credentialName);
}
```

## Methods

### `CanEncrypt` / `CanDecrypt` methods

These methods each take a string `credentialName` as their parameter and return a boolean value indicating whether the instance of `ICrypto` is able to "recognize" the specified `credentialName`. A `credentialName` is also used by each other `ICrypto` method to retrieve the key for the given encryption operation. For example, an implementation of the `ICrypto` interface might have, as part of its implementation, a symmetric key registered with `"foo"` as the credential name. In that case, we would expect that `CanEncrypt` and `CanDecrypt` would return true if we passed them a parameter with a value of `"foo"`.

```csharp
ICrypto crypto = // Assume you have an implementation of ICrypto that recognizes "foo" but not "bar".
bool canEncryptFoo = crypto.CanEncrypt("foo"); // Should return true.
bool canEncryptBar = crypto.CanEncrypt("bar"); // Should return false.
```

NOTE: If we want use the "default" key (as defined by the particular implementation of `ICrypto`), pass `null` as the value for the `credentialName` parameter.

```csharp
ICrypto cryptoA = // Assume you have an implementation of ICrypto that has a default key.
ICrypto cryptoB = // Assume you have an implementation of ICrypto that does NOT have a default key.
bool canEncryptFoo = cryptoA.CanEncrypt(null); // Should return true.
bool canEncryptBar = cryptoB.CanEncrypt(null); // Should return false.
```

### `Encrypt` / `Decrypt` methods

These are the main methods of the interface. Each of the `Encrypt` and `Decrypt` methods take two parameters. The first one is the value to be operated upon, and has a type of either `string` or `byte[]`. Note that this type determines the return type of the method. The second parameter is a `credentialName`, as described above.

```csharp
ICrypto crypto = // Assume you have an implementation of ICrypto that has a default key.

// String-based API
string plainTextString = "Hello, world!";
string cipherTextString = crypto.Encrypt(plainTextString, null);
string decryptedString = crypto.Decrypt(cipherTextString, null);

// Binary-based API
byte[] plainTextByteArray = Encoding.UTF8.GetBytes(plainTextString);
byte[] cipherTextByteArray = crypto.Encrypt(plainTextByteArray, null);
byte[] decryptedByteArray = crypto.Decrypt(cipherTextByteArray, null);
```

### `GetEncryptor` / `GetDecryptor` methods

These methods are intended to be used when the lookup and/or retrieval of a key is expensive, and multiple related encryption operations need to be performed. In this situation, the result of the lookup/retrieval is cached in the returned `IEncryptor` or `IDecryptor` object. The object can then be used for multiple encryption operations.

```csharp
ICrypto crypto = // Assume you have an implementation of ICrypto that has a default key.

IEncryptor encryptor = crypto.GetEncryptor();
string fooEncrypted = encryptor.Encrypt("foo");
string barEncrypted = encryptor.Encrypt("bar");
byte[] bazEncrypted = encryptor.Encrypt(new byte[] { 1, 2, 3 });

IDecryptor decryptor = crypto.GetDecryptor();
string foo = decryptor.Decrypt(fooEncrypted);
string bar = decryptor.Decrypt(barEncrypted);
byte[] baz = decryptor.Decrypt(bazEncrypted);
```
