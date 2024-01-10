---
sidebar_position: 6
---

# :warning: Deprecation Warning :warning:

This library has been deprecated and will no longer receive updates.

---

## Field-Level Encryption

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

## XPath / JSONPath

The main RockLib.Encryption package provides extension methods under the `RockLib.Encryption.FieldLevel` namespace that use either [XPath](https://msdn.microsoft.com/en-us/library/ms256086.aspx) or [JSONPath](http://goessner.net/articles/JsonPath/) to identify which fields need to be encrypted. There are many overloads with the same variation: take a string containing an XML/JSON document and provide one or more XPath/JSONPath strings, and they encrypt/decrypt just the fields specified by XPath/JSONPath.

The easiest extensions to use extend `string` and use `Crypto.Current` as the backing source of encryption:

```csharp
string xml = // Use first XML example above
string xmlWithSsnEncrypted = xml.EncryptXml("/client/ssn"); // Should be similar to second XML example above
string xmlWithSsnDecrypted = xml.DecryptXml("/client/ssn"); // Round-trip should be same as original

string json = // Use first JSON example above
string jsonWithSsnEncrypted = json.EncryptJson("$.ssn"); // Should be similar to second JSON example above
string jsonWithSsnDecrypted = json.DecryptJson("$.ssn"); // Round-trip should be same as original
```

There are also extension methods that extend `ICrypto` - these are useful if your app is injecting instances of `ICrypto` (i.e. it isn't using the static `Crypto` class).

```csharp
ICrypto crypto = // Get from somewhere

string xml = // Use first XML example above
string xmlWithSsnEncrypted = crypto.EncryptXml(xml, "/client/ssn"); // Should be similar to second XML example above
string xmlWithSsnDecrypted = crypto.DecryptXml(xml, "/client/ssn"); // Round-trip should be same as original

string json = // Use first JSON example above
string jsonWithSsnEncrypted = crypto.EncryptJson(json, "$.ssn"); // Should be similar to second JSON example above
string jsonWithSsnDecrypted = crypto.DecryptJson(json, "$.ssn"); // Round-trip should be same as original
```

## RockLib.Encryption.XSerializer

[XSerializer](https://github.com/QuickenLoans/XSerializer) includes a feature where it encrypts/decrypts properties marked with its `[Encrypt]` attribute in-line during JSON and XML serialization operations. RockLib.Encryption.XSerializer marries XSerializer's field-level encryption mechanism with RockLib.Encryption's standardized crypto API.

### ICrypto extension methods

The may way of using this package is through its extension methods off of the `ICrypto` interface. These extension methods (`ToXml`, `FromXml`, `ToJson`, `FromJson`) enable seamless field-level encryption and decryption during serialization and deserialization with XSerializer by using its `[Encrypt]` attribute.

### Usage

```csharp
static void Main(string[] args)
{
    ICrypto crypto = // Get from somewhere

    string json = crypto.ToJson(new Foo { Bar = 123, Baz = 456 });
    Console.WriteLine(json);
    Foo fooFromJson = crypto.FromJson<Foo>(json);
    Console.WriteLine($"fooFromJson.Bar:{fooFromJson.Bar}, fooFromJson.Baz:{fooFromJson.Baz}");

    Console.WriteLine();

    string xml = crypto.ToXml(new Foo { Bar = 123, Baz = 456 });
    Console.WriteLine(xml);
    Foo fooFromXml = crypto.FromXml<Foo>(xml);
    Console.WriteLine($"fooFromXml.Bar:{fooFromXml.Bar}, fooFromXml.Baz:{fooFromXml.Baz}");
}

public class Foo
{
    public int Bar { get; set; }

    [Encrypt]
    public int Baz { get; set; }
}
```

### SerializingCrypto class

This class has the same named methods and behaves exactly the same as the above `ICrypto` extension methods. The only difference is that the `SerializingCrypto` class holds on to a single reference of `CryptoEncryptionMechanism` (which references an `ICrypto`), where the extension methods create a new instance each time using the supplied `ICrypto`.

### Usage

Start by configuring your application as usual for use with RockLib.Encryption, then add the RockLib.Encryption.XSerializer nuget package. Then access the serializing crypto functionality through the `SerializingCrypto` class directly or by using its `ICrypto` extension methods.

```csharp
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
