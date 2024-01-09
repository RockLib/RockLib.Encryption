---
sidebar_position: 3
---

# :warning: Deprecation Warning :warning:

This library has been deprecated and will no longer receive updates.

---

## Dependency Injection

To register an implementation of `ICrypto` with the Microsoft.Extensions.DependencyInjection container, the RockLib.Encryption package provides a `AddCrypto` extension method with several overloads. Each of the methods registers both the `ICrypto` and `IAsyncCrypto` interfaces.

Implemenations of the `ICrypto` interface *should* also define dependency injection extension methods specific to the implemenation. See [SymmetricCrypto](Implementations.md#symmetriccrypto-class) for an example.

---

The overload with no additional parameters registers (as singleton) the `ICrypto` implementation that is returned by the `Crypto.Current` static property. By default, this instance is defined in [configuration](Crypto.md#configuration).

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddCrypto();
}
```

---

The overload with an `ICrypto` parameter registers the specified instance as singleton.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ICrypto crypto = // TODO: instantiate an ICrypto implementation.

    services.AddCrypto(crypto);
}
```

---

The overload with a `Func<IServiceProvider, ICrypto>` parameter registers the `ICrypto` returned by the `cryptoFactory` function as singleton.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IMyDependency, MyConcreteDependency>();

    services.AddCrypto(serviceProvider =>
    {
        IMyDependency myDependency = serviceProvider.GetRequiredService<IMyDependency>();
        return new MyCrypto(myDependency);
    });
}
```
