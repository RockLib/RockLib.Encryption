# RockLib.Encryption

_An easy-to-use, easy-to-configure crypto API._

------

##### Table of Contents
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
- [RockLib.Encryption.XSerializer](#rocklibencryptionxserializer)
  - [Background](#background)
  - [SerializingCrypto Usage](#serializingcrypto-usage)

------


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
ICrypto crypto = // TODO: initialize with an implementation of ICrypto that recognizes "foo" but not "bar".
bool canEncryptFoo = crypto.CanEncrypt("foo"); // Should return true.
bool canEncryptBar = crypto.CanEncrypt("bar"); // Should return false.
```

NOTE: If we want use the "default" key (as defined by the particular implementation of `ICrypto`), pass `null` as the value for the `keyIdentifier` parameter.

```c#
ICrypto cryptoA = // TODO: initialize with an implementation of ICrypto that has a default key.
ICrypto cryptoB = // TODO: initialize with an implementation of ICrypto that does NOT have a default key.
bool canEncryptFoo = cryptoA.CanEncrypt(null); // Should return true.
bool canEncryptBar = cryptoB.CanEncrypt(null); // Should return false.
```

#### `Encrypt` / `Decrypt` methods

These are the main methods of the interface. Each of the `Encrypt` and `Decrypt` methods take two parameters. The first one is the value to be operated upon, and has a type of either `string` or `byte[]`. Note that this type determines the return type of the method. The second parameter is a `keyIdentifier` object, as described above.

```c#
ICrypto crypto = // TODO: initialize with an implementation of ICrypto that has a default key.

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
ICrypto crypto = // TODO: initialize with an implementation of ICrypto that has a default key.

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

The `Crypto` class will attempt to set its `Current` property by reading from your [App.config or Web.config](#configuration). If your application does not have a App.config or Web.config, or you wish to override what is in your App.config or Web.config, you can programmatically set the value of the `Current` property at the "beginning" of your application, e.g. `Program.Main` or global.asax's `application_start` by calling the `SetCurrent` method.

**If you wish to programmatically set the value of the `Current` property, you must do so at the "beginnning" of your application, e.g. `Program.Main` or global.asax's `application_start` method by calling the `SetCurrent` method. Once the `Current` property has been read, its value is "locked" - any calls to `SetCurrent` will not succeed.**

```c#
class Program
{
    static void Main(string[] args)
    {
        ICrypto defaultCrypto = // TODO: get an instance of ICrypto
        Crypto.SetCurrent(defaultCrypto);

        // TODO: The rest of your application
    }
}
```

## Configuration

The easiest way to configure RockLib.Encryption is through a `app.config` or `web.config`. When you add a custom `rock.encryption` section to your configuration as shown below, the [`Crypto`](#crypto-static-class) class will discover it and set its `Current` property according to your configuration.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <!-- Declare a section with a name of "rock.encryption" -->
    <section name="rock.encryption" type="Rock.Encryption.Configuration.RockEncryptionSection, Rock.Encryption" />
  </configSections>
  
  <!-- Define the section -->
  <rock.encryption>
    <settings>
      <crypto type="Some.Assembly.Qualified.Name, SomeAssembly">
        <!-- settings specific to the type specified above -->
      </crypto>
    </settings>
  </rock.encryption>
</configuration>
```

Your configuration can define one or more `crypto` elements. Each of these elements describe an implementation of the `ICrypto` interface, and will be transformed into an item in the collection of an instance of [`CompositeCrypto`](#compositecrypto-class).

## `ICrypto` implementations

### `SymmetricCrypto` class

RockLib.Encryption provides an implementation of `ICrypto` that uses the various symmetric encryption implementations that are provided by .NET. The supported algorithms are: `AES`, `DES`, `RC2`, `Rijndael`, and `Triple DES`.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="rock.encryption" type="Rock.Encryption.Configuration.RockEncryptionSection, Rock.Encryption" />
  </configSections>
  <rock.encryption>
    <settings>
      <crypto type="Rock.Encryption.Symmetric.SymmetricCrypto, Rock.Encryption">
        <encryptionSettings>
          <credentials>
            <credential name="default" algorithm="Rijndael" ivsize="16">
              <key value="bo3Vtyg4uBhcKgQKQ6H9LmeYXF+7BG42XMoS7AgZFz4=" />
            </credential>
            <credential name="triple_des" algorithm="TripleDES" ivsize="8">
              <key value="bNYqGfSV6xqgoucDMqwGWFRZ8KHFXe+m" />
            </credential>
          </credentials>
        </encryptionSettings>
      </crypto>
    </settings>
  </rock.encryption>
</configuration>
```

_Note that it is an **exceedingly** bad idea to store symmetric keys in configuration plaintext as shown above._

### `CompositeCrypto` class

If your application needs to support more than one implementation of the `ICrypto` interface, you can use the `CompositeCrypto` class.
It does so by implementing the [_composite_](http://www.blackwasp.co.uk/Composite.aspx) pattern. The constructor of this class takes a collection of `ICrypto` objects. Each method of the `CompositeCrypto` class is implemented by iterating through that collection. The first item in the collection that returns `true` from its `CanEncrypt` or `CanDecrypt` method is the `ICrypto` that is used for the current encryption operation.

## RockLib.Encryption.XSerializer

### Background

XSerializer includes a feature where it encrypts/decrypts properties marked with its `[Encrypt]` attribute in-line during JSON and XML serialization operations. RockLib.Encryption.XSerializer marries XSerializer's field-level encryption mechanism with RockLib.Encryption's standardized crypto API.

### SerializingCrypto class

The RockLib.Encryption.XSerializer package contains the

### SerializingCrypto Usage

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
