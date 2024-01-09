---
sidebar_position: 4
---

# :warning: Deprecation Warning :warning:

This library has been deprecated and will no longer receive updates.

---

## Crypto static class

The `Crypto` static class provides simple access to a single instance of `ICrypto`, defined by its `Current` property. For convenience, it includes synchronous and asynchronous methods for encryption and decryption, each of which uses the `Current` property.

To programmatically change the value of the `Current` property, call the `SetCurrent` method at the very beginning of an application:

```csharp
ICrypto crypto = // TODO: instantiate

Crypto.SetCurrent(crypto);

// TODO: The rest of the application
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

`appsettings.json`

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

`app.config/web.config`

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
