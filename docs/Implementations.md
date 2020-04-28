## `ICrypto` implementations

### `SymmetricCrypto` class

RockLib.Encryption provides an implementation of `ICrypto`, `SymmetricCrypto`, that uses the various symmetric encryption implementations that are provided by .NET. The supported algorithms are: `AES`, `DES`, `RC2`, `Rijndael`, and `Triple DES`. This class has public constructors and can be instantiated directly.

---

To register a `SymmetricCrypto` with the Microsoft.Extensions.DependencyInjection container, use the `AddSymmetricCrypto` extension method, and add credentials with the `AddCredential` method:

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddSymmetricCrypto()
        .AddCredential("CQSImVlbvJMZcnrkzT3/ouW1klt6STljrDjRiBzIsSk=", SymmetricAlgorithm.Rijndael) // This is the default (unnamed) credential.
        .AddCredential("MyDESCredential", "2LQliivTtNo=", SymmetricAlgorithm.DES, 8); // This credential is named "MyDESCredential".
}
```

---

If using the default value for the static `Crypto` class, it can be configured as follows:

appsettings.json:

```json
{
  "rocklib.encryption": {
    "type": "RockLib.Encryption.Symmetric.SymmetricCrypto, RockLib.Encryption",
    "value": {
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
