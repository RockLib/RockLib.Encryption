# Rock.Encryption

_An easy-to-use, easy-to-configure crypto API._

### Configuration

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="rock.encryption" type="Rock.Encryption.Configuration.RockEncryptionSection, Rock.Encryption" />
  </configSections>
  <rock.encryption>
    <settings>
      <crypto type="Some.Assembly.Qualified.Name, SomeAssembly">
        <!-- settings specific to the type specified above -->
      </crypto>
    </settings>
  </rock.encryption>
</configuration>
```

##### SymmetricCrypto

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

### Crypto Usage

The `Crypto` class is the primary way of interacting with Rock.Encryption's API. It contains methods for encrypting and decrypting string and byte arrays.

```c#
string sensitiveData = "123-45-6789";
string encryptedData = Crypto.Encrypt(sensitiveData);
string decryptedData = Crypto.Decrypt(encryptedData);
```

### Rock.Encryption.XSerializer

##### Background

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
