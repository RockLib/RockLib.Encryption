# RockLib.Encryption

_An easy-to-use, easy-to-configure crypto API._

------

##### Table of Contents
- [Build status](#build-status)
- [Nuget](#nuget)
- [Simple Usage](#simple-usage)
- [Concepts](#concepts)
  - [`ICrypto` interface](#icrypto-interface)
    - [`CanEncrypt` / `CanDecrypt` methods](#canencrypt--candecrypt-methods)
    - [`Encrypt` / `Decrypt` methods](#encrypt--decrypt-methods)
    - [`GetEncryptor` / `GetDecryptor` methods](#getencryptor--getdecryptor-methods)
-   [`Crypto` static class](#crypto-static-class)
  -   [Encryption methods](#encryption-methods)
  -   [`Current` property and `SetCurrent` method](#current-property-and-setcurrent-method)
- [Configuration](#configuration)
- [`ICrypto` implementations](#icrypto-implementations)
  - [`SymmetricCrypto` class](#symmetriccrypto-class)
  - [`CompositeCrypto` class](#compositecrypto-class)
- [Field-level encryption](#field-level-encryption)
  - [XPath / JSONPath](#xpath--jsonpath)
  - [RockLib.Encryption.XSerializer](#rocklibencryptionxserializer)
    - [SerializingCrypto Usage](#serializingcrypto-usage)

------

## Build Status

### RockLib.Encryption [![Build status](https://ci.appveyor.com/api/projects/status/q9s982i6d34tc318?svg=true)](https://ci.appveyor.com/project/RockLib/rocklib-encryption)

### RockLib.Encryption.XSerializer [![Build status](https://ci.appveyor.com/api/projects/status/v3jhqxxyx2w5ls9u?svg=true)](https://ci.appveyor.com/project/bfriesen/rocklib-encryption-d2ct4)

## Nuget

RockLib.Encryption is available via [nuget](http://www.nuget.org/packages/RockLib.Encryption). From the package manager console:

```
PM> Install-Package RockLib.Encryption
```

[RockLib.Encryption.XSerializer](#rocklibencryptionxserializer) is also available via [nuget](http://www.nuget.org/packages/RockLib.Encryption.XSerializer). From the package manager console:

```
PM> Install-Package RockLib.Encryption.XSerializer
```

## Simple Usage

Once [configured](#configuration), use the static `Crypto` class to encrypt your sensitive data:

```c#
// Our sensitive data is a "social security number".
string ssn = "123-45-6789";

// Encrypt the SSN. The resulting value should be different from the original SSN.
string encryptedSsn = Crypto.Encrypt(ssn);

// Decrypt the SSN. The resulting value should be the same as the original SSN.
string decryptedSsn = Crypto.Decrypt(encryptedSsn);
```

## Concepts

### `ICrypto` interface

`ICrypto` is the main abstraction in RockLib.Encryption. Here is its definition:

```c#
public interface ICrypto
{
    bool CanEncrypt(object keyIdentifier);
    bool CanDecrypt(object keyIdentifier);
    string Encrypt(string plainText, object keyIdentifier);
    string Decrypt(string cipherText, object keyIdentifier);
    byte[] Encrypt(byte[] plainText, object keyIdentifier);
    byte[] Decrypt(byte[] cipherText, object keyIdentifier);
    IEncryptor GetEncryptor(object keyIdentifier);
    IDecryptor GetDecryptor(object keyIdentifier);
}
```

#### `CanEncrypt` / `CanDecrypt` methods

These methods each take a `keyIdentifier` object as their parameter and return a boolean value indicating whether the instance of `ICrypto` is able to "recognize" the specified `keyIdentifier`. A `keyIdentifier` object is also used by each other `ICrypto` method to retrieve the key for the given encryption operation. For example, an implementation of the `ICrypto` interface might have, as part of its implementation, a symmetric key registered with a `"foo"` string. In that case, we would expect that `CanEncrypt` and `CanDecrypt` would return true if we passed them a parameter with a value of `"foo"`.

```c#
ICrypto crypto = // Assume you have an implementation of ICrypto that recognizes "foo" but not "bar".
bool canEncryptFoo = crypto.CanEncrypt("foo"); // Should return true.
bool canEncryptBar = crypto.CanEncrypt("bar"); // Should return false.
```

NOTE: If we want use the "default" key (as defined by the particular implementation of `ICrypto`), pass `null` as the value for the `keyIdentifier` parameter.

```c#
ICrypto cryptoA = // Assume you have an implementation of ICrypto that has a default key.
ICrypto cryptoB = // Assume you have an implementation of ICrypto that does NOT have a default key.
bool canEncryptFoo = cryptoA.CanEncrypt(null); // Should return true.
bool canEncryptBar = cryptoB.CanEncrypt(null); // Should return false.
```

#### `Encrypt` / `Decrypt` methods

These are the main methods of the interface. Each of the `Encrypt` and `Decrypt` methods take two parameters. The first one is the value to be operated upon, and has a type of either `string` or `byte[]`. Note that this type determines the return type of the method. The second parameter is a `keyIdentifier` object, as described above.

```c#
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

#### `GetEncryptor` / `GetDecryptor` methods

These methods are intended to be used when the lookup and/or retrieval of a key is expensive, and multiple related encryption operations need to be performed. In this situation, the result of the lookup/retrieval is cached in the returned `IEncryptor` or `IDecryptor` object. The object can then be used for multiple encryption operations.

```c#
ICrypto crypto = // Assume you have an implementation of ICrypto that has a default key.

IEncryptor encryptor = crypto.GetEncryptor(null);
string foo = encryptor.Encrypt("foo");
string bar = encryptor.Encrypt("bar");
byte[] baz = encryptor.Encrypt(new byte[] { 1, 2, 3 });
```

### `Crypto` static class

For convenience, RockLib.Encryption defines a static `Crypto` class with these public members:

```c#
public static class Crypto
{
    public static ICrypto Current { get; }
    public static void SetCurrent(ICrypto crypto);
    
    public static string Encrypt(string plainText);
    public static string Encrypt(string plainText, object keyIdentifier);
    public static string Decrypt(string cipherText);
    public static string Decrypt(string cipherText, object keyIdentifier);
    public static byte[] Encrypt(byte[] plainText);
    public static byte[] Encrypt(byte[] plainText, object keyIdentifier);
    public static byte[] Decrypt(byte[] cipherText);
    public static byte[] Decrypt(byte[] cipherText, object keyIdentifier);
    public static IEncryptor GetEncryptor();
    public static IEncryptor GetEncryptor(object keyIdentifier);
    public static IDecryptor GetDecryptor();
    public static IDecryptor GetDecryptor(object keyIdentifier);
}
```

#### Encryption methods

Each of the encryption methods delegates responsibility to the value of the `Current` property. For example, this is the implementation of the `Encrypt(string, object)` method:

```c#
public static string Encrypt(string plainText, object keyIdentifier)
{
    return Current.Encrypt(plainText, keyIdentifier);
}
```

There are overloads of each of the methods that do not have a `keyIdentifier` parameter - these methods call their overload, passing `null` for the `keyIdentifier` parameter. For example, this is the implementation of the `Encrypt(string)` method:

```c#
public static string Encrypt(string plainText)
{
    return Encrypt(plainText, null);
}
```

#### `Current` property and `SetCurrent` method

The `Crypto` class will attempt to set its `Current` property using the [`RockLib.Configuration.Config` static class](#configuration). If you do not wish to use the `Config` static class, you can programmatically set the value of the `Crypto.Current` property at the "beginning" of your application by calling the `Crypto.SetCurrent` method.

**NOTE: Once the `Crypto.Current` property has been read, its value is "locked" - any calls to `SetCurrent` will not succeed.**

```c#
class Program
{
    static void Main(string[] args)
    {
        ICrypto defaultCrypto = // Assume you have an instance of ICrypto
        Crypto.SetCurrent(defaultCrypto);

        // The rest of your application
    }
}
```

## Configuration

By default, the `Crypto` static class sets its `Current` property using the `RockLib.Configuration.Config` static class. Specifically, it loads the instance (or instances) of the `ICrypto` interface specified in this configuration section: `Config.Root.GetSection("rocklib.encryption")`. This is an example `appsettings.json` file, for .NET Core projects:

```json
{
  "rocklib.encryption": {
    "type": "Some.Assembly.Qualified.Name, SomeAssembly",
    "value": {
      "settings": {
        "specific": "to",
        "the": "type"
      },
      "specified": "above"
    }
  }
}
```

This is an equivalent app.config/web.config (applicable only to .NET Framework applications):

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="rocklib.encryption" type="RockLib.Configuration.RockLibConfigurationSection, RockLib.Configuration" />
  </configSections>

  <rocklib.encryption>
    <crypto type="Some.Assembly.Qualified.Name, SomeAssembly">
      <value>
        <settings specific="to" the="type" />
        <specified>above</specified>
      </value>
    </crypto>
  </rocklib.encryption>
</configuration>
```

Your configuration can define one or more implementation/instance of the `ICrypto` in config. Each of these elements describe an implementation of the `ICrypto` interface, and will be transformed into an item in the collection of an instance of [`CompositeCrypto`](#compositecrypto-class).

appsettings.json:

```json
{
  "rocklib.encryption": [
    {
      "type": "Some.Assembly.Qualified.Name, SomeAssembly",
      "value": {
        "setting1": 123,
        "setting2": false
      }
    },
    {
      "type": "Another.Assembly.Qualified.Name, AnotherAssembly",
      "value": {
        "settingA": "abc",
        "settingB": 123.45
      }
    }
  ]
}
```

app.config/web.config:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="rocklib.encryption" type="RockLib.Configuration.RockLibConfigurationSection, RockLib.Configuration" />
  </configSections>

  <rocklib.encryption>
    <crypto type="Some.Assembly.Qualified.Name, SomeAssembly">
      <value>
        <setting1>123<setting1/>
        <setting2>false<setting2/>
      </value>
    </crypto>
    <crypto type="Another.Assembly.Qualified.Name, AnotherAssembly">
      <value>
        <settingA>abc<settingA/>
        <settingB>123.45<settingB/>
      </value>
    </crypto>
  </rocklib.encryption>
</configuration>
```

## `ICrypto` implementations

### `SymmetricCrypto` class

RockLib.Encryption provides an implementation of `ICrypto` that uses the various symmetric encryption implementations that are provided by .NET. The supported algorithms are: `AES`, `DES`, `RC2`, `Rijndael`, and `Triple DES`.

appsettings.json:

```json
{
  "rocklib.encryption": {
    "type": "RockLib.Encryption.Symmetric.SymmetricCrypto, RockLib.Encryption",
    "value": {
      "encryptionSettings": {
        "credentials": [
          {
            "name": "default",
            "algorithm": "Rijndael",
            "ivsize": 16,
            "key": "bo3Vtyg4uBhcKgQKQ6H9LmeYXF+7BG42XMoS7AgZFz4="
          },
          {
            "name": "triple_des",
            "algorithm": "TripleDES",
            "ivsize": 8,
            "key": "bNYqGfSV6xqgoucDMqwGWFRZ8KHFXe+m"
          }
        ]
      }
    }
  }
}
```

app.config/web.config:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="rocklib.encryption" type="RockLib.Configuration.RockLibConfigurationSection, RockLib.Configuration" />
  </configSections>

  <rocklib.encryption>
    <crypto type="RockLib.Encryption.Symmetric.SymmetricCrypto, RockLib.Encryption">
      <value>
        <encryptionSettings>
          <credentials name="default"
                       algorithm="Rijndael"
                       ivsize="16"
                       key="bo3Vtyg4uBhcKgQKQ6H9LmeYXF+7BG42XMoS7AgZFz4=" />
          <credentials name="triple_des"
                       algorithm="TripleDES"
                       ivsize="8"
                       key="bNYqGfSV6xqgoucDMqwGWFRZ8KHFXe+m" />
        </encryptionSettings>
      </value>
    </crypto>
  </rocklib.encryption>
</configuration>
```

_Note that it is an **exceedingly** bad idea to store symmetric keys in configuration plaintext as shown above._

### `CompositeCrypto` class

If your application needs to support more than one implementation of the `ICrypto` interface, you can use the `CompositeCrypto` class.
It does so by implementing the [_composite_](http://www.blackwasp.co.uk/Composite.aspx) pattern. The constructor of this class takes a collection of `ICrypto` objects. Each method of the `CompositeCrypto` class is implemented by iterating through that collection. The first item in the collection that returns `true` from its `CanEncrypt` or `CanDecrypt` method is the `ICrypto` that is used for the current encryption operation.

## Field-level encryption

Sometime sensitive information exists within an XML or JSON document in specific fields. For example the following documents contain a clear text SSN:

```xml
<client>
  <first_name>John</first_name>
  <middle_initial>Q</middle_initial>
  <last_name>Public</last_name>
  <ssn>123-45-6789</ssn>
</client>
```

```json
{
  "first_name": "Public",
  "middle_initial": "Q",
  "last_name": "Public",
  "ssn": "123-45-6789"
}
```

This is what we want - to keep most of the document plain-text while encrypting just the sensitive fields:

```xml
<client>
  <first_name>John</first_name>
  <middle_initial>Q</middle_initial>
  <last_name>Public</last_name>
  <ssn>MTIzLTQ1LTY3ODk=</ssn>
</client>
```

```json
{
  "first_name": "Public",
  "middle_initial": "Q",
  "last_name": "Public",
  "ssn": "MTIzLTQ1LTY3ODk="
}
```

There are two mechanisms for identifying and then encrypting/decrypting just the fields that are sensitive: [XPath/JSONPath extension methods](#xpath--jsonpath) and [the RockLib.Encryption.XSerializer package](#rocklibencryptionxserializer).

### XPath / JSONPath

The main RockLib.Encryption package provides extension methods under the `RockLib.Encryption.FieldLevel` namespace that use either [XPath](https://msdn.microsoft.com/en-us/library/ms256086.aspx) or [JSONPath](http://goessner.net/articles/JsonPath/) to identify which fields need to be encrypted. There are many overloads with the same variation: take a string containing an XML/JSON document and provide one or more XPath/JSONPath strings, and they encrypt/decrypt just the fields specified by XPath/JSONPath.

The easiest extensions to use extend `string` and use `Crypto.Current` as the backing source of encryption:

```c#
string xml = // Use first XML example above
string xmlWithSsnEncrypted = xml.EncryptXml("/client/ssn"); // Should be similar to second XML example above
string xmlWithSsnDecrypted = xml.DecryptXml("/client/ssn"); // Round-trip should be same as original

string json = // Use first JSON example above
string jsonWithSsnEncrypted = json.EncryptJson("$.ssn"); // Should be similar to second JSON example above
string jsonWithSsnDecrypted = json.DecryptJson("$.ssn"); // Round-trip should be same as original
```

There are also extension methods that extend `ICrypto` - these are useful if your app is injecting instances of `ICrypto` (i.e. it isn't using the static `Crypto` class).

```c#
ICrypto crypto = // Get from somewhere

string xml = // Use first XML example above
string xmlWithSsnEncrypted = crypto.EncryptXml(xml, "/client/ssn"); // Should be similar to second XML example above
string xmlWithSsnDecrypted = crypto.DecryptXml(xml, "/client/ssn"); // Round-trip should be same as original

string json = // Use first JSON example above
string jsonWithSsnEncrypted = crypto.EncryptJson(json, "$.ssn"); // Should be similar to second JSON example above
string jsonWithSsnDecrypted = crypto.DecryptJson(json, "$.ssn"); // Round-trip should be same as original
```

### RockLib.Encryption.XSerializer

[XSerializer](https://github.com/QuickenLoans/XSerializer) includes a feature where it encrypts/decrypts properties marked with its `[Encrypt]` attribute in-line during JSON and XML serialization operations. RockLib.Encryption.XSerializer marries XSerializer's field-level encryption mechanism with RockLib.Encryption's standardized crypto API.

#### SerializingCrypto class

The main class in the RockLib.Encryption.XSerializer package is the `SerializingCrypto` class. This class behaves very similar to the `Crypto` class, and exposes methods for doing XML and JSON field-level encryption during serialization with XSerializer.

#### SerializingCrypto Usage

Start by configuring your application as usual for use with RockLib.Encryption, then add the RockLib.Encryption.XSerializer nuget package. Then access the serializing crypto functionality through the `SerializingCrypto` class.

```c#
static void Main(string[] args)
{
    string json = SerializingCrypto.ToJson(new Foo { Bar = 123, Baz = 456 });
    Console.WriteLine(json);
    Foo fooFromJson = SerializingCrypto.FromJson<Foo>(json);
    Console.WriteLine($"fooFromJson.Bar:{fooFromJson.Bar}, fooFromJson.Baz:{fooFromJson.Baz}");
    
    Console.WriteLine();
    
    string xml = SerializingCrypto.ToXml(new Foo { Bar = 123, Baz = 456 });
    Console.WriteLine(xml);
    Foo fooFromXml = SerializingCrypto.FromXml<Foo>(xml);
    Console.WriteLine($"fooFromXml.Bar:{fooFromXml.Bar}, fooFromXml.Baz:{fooFromXml.Baz}");
}

public class Foo
{
    public int Bar { get; set; }
    
    [Encrypt]
    public int Baz { get; set; }
}
```
