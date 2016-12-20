# Rock.Encryption

_An easy-to-use, easy-to-configure crypto API._

## Concepts

### `ICrypto` interface

`ICrypto` is the main abstraction in Rock.Encryption. Here is its definition:

```c#
public interface ICrypto
{
    string Encrypt(string plainText, object keyIdentifier);
    string Decrypt(string cipherText, object keyIdentifier);
    byte[] Encrypt(byte[] plainText, object keyIdentifier);
    byte[] Decrypt(byte[] cipherText, object keyIdentifier);
    IEncryptor GetEncryptor(object keyIdentifier);
    IDecryptor GetDecryptor(object keyIdentifier);
    bool CanEncrypt(object keyIdentifier);
    bool CanDecrypt(object keyIdentifier);
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

Each of the `Encrypt` and `Decrypt` methods should be self-explainatory: they encrypt or decrypt values, where a value can be a string or a byte array.

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

For convenience, Rock.Encryption defines a static `Crypto` class with these public members:

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
}
```

This class contains a `Current` property, which holds an implementation of `ICrypto`. This `Current` property is used by the static methods in the `Crypto` class to perform the various encryption operations. The value can be changed by calling the `SetCurrent` method. However, once the `Current` property has been accessed, it can no longer be changed.

You'll note that most of the methods mirror the methods of the `ICrypto` inteface. There are also methods without a `keyIdentifier` parameter - these methods merely call their corresponding method _with_ a `keyIdentifier` parameter, passing null as that value.

## `ICrypto` Implementations

### `CompositeCrypto` class

Rock.Encryption contains an implementation of `ICrypto` that uses the [_composite_](http://www.blackwasp.co.uk/Composite.aspx) pattern.

#### SymmetricCrypto

The default symmetric encryption algorithms that ship with .NET are implemented in the `Rock.Encryption.Symmetric.SymmetricCrypto` class.

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

_Note that it is an **exceedingly** bad idea to store symmetric keys in configuration plaintext as shown above. Rock.Core provides a mechanism for securing configuration data. That topic will be added here at a later date._

## Configuration

The easiest way to configure Rock.Encryption is through a `app.config` or `web.config`. Add a custom `rock.encryption` section to your configuration as shown below.

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

Your configuration can define one or more `crypto` elements. Each of these elements describe an implementation of the `ICrypto` interface. 

## Rock.Encryption.XSerializer

### Background

XSerializer includes a feature where it encrypts/decrypts properties marked with its `[Encrypt]` attribute in-line during JSON and XML serialization operations. Rock.Encryption.XSerializer marries XSerializer's field-level encryption mechanism with Rock.Encryption's standardized crypto API.

### SerializingCrypto Usage

Start by configuring your application as usual for use with Rock.Encryption, then add the Rock.Encryption.XSerializer nuget package. Then access the serializing crypto functionality through the `SerializingCrypto` class.

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
