using RockLib.Encryption.XSerializer;
using XSerializer;

namespace RockLib.Encryption;

/// <summary>
/// Provides a set of static methods used for doing field-level encryption
/// during a serialization operation.
/// </summary>
public static class FieldLevelEncryptionExtensions
{
    /// <summary>
    /// Serializes the specified instance to XML, encrypting any properties marked
    /// with the [Encrypt] attribute.
    /// </summary>
    /// <typeparam name="T">The type of the instance</typeparam>
    /// <param name="crypto">The instance of <see cref="ICrypto"/> that will perform encryption operations.</param>
    /// <param name="instance">The instance to serialize.</param>
    /// <param name="credentialName">
    /// The name of the credential to use for this encryption operation,
    /// or null to use the default credential.
    /// </param>
    /// <returns>An XML document that represents the instance.</returns>
    public static string ToXml<T>(this ICrypto crypto, T instance, string? credentialName = null)
    {
        var serializer = new XmlSerializer<T>(x => x
            .WithEncryptionMechanism(new CryptoEncryptionMechanism(crypto))
            .WithEncryptKey(credentialName));
        return serializer.Serialize(instance);
    }

    /// <summary>
    /// Deserializes the specified XML to an object, decrypting any properties marked
    /// with the [Encrypt] attribute.
    /// </summary>
    /// <typeparam name="T">The type to deserialize into.</typeparam>
    /// <param name="crypto">The instance of <see cref="ICrypto"/> that will perform decryption operations.</param>
    /// <param name="xml">The XML to deserialize.</param>
    /// <param name="credentialName">
    /// The name of the credential to use for this encryption operation,
    /// or null to use the default credential.
    /// </param>
    /// <returns>The deserialized object.</returns>
    public static T FromXml<T>(this ICrypto crypto, string? xml, string? credentialName = null)
    {
        var serializer = new XmlSerializer<T>(x => x
            .WithEncryptionMechanism(new CryptoEncryptionMechanism(crypto))
            .WithEncryptKey(credentialName));
        return serializer.Deserialize(xml);
    }

    /// <summary>
    /// Serializes the specified instance to JSON, encrypting any properties marked
    /// with the [Encrypt] attribute.
    /// </summary>
    /// <typeparam name="T">The type of the instance</typeparam>
    /// <param name="crypto">The instance of <see cref="ICrypto"/> that will perform encryption operations.</param>
    /// <param name="instance">The instance to serialize.</param>
    /// <param name="credentialName">
    /// The name of the credential to use for this encryption operation,
    /// or null to use the default credential.
    /// </param>
    /// <returns>A JSON document that represents the instance.</returns>
    public static string ToJson<T>(this ICrypto crypto, T instance, string? credentialName = null)
    {
        var serializer =
            new JsonSerializer<T>(new JsonSerializerConfiguration
            {
                EncryptionMechanism = new CryptoEncryptionMechanism(crypto),
                EncryptKey = credentialName
            });

        return serializer.Serialize(instance);
    }

    /// <summary>
    /// Deserializes the specified JSON to an object, decrypting any properties marked
    /// with the [Encrypt] attribute.
    /// </summary>
    /// <typeparam name="T">The type to deserialize into.</typeparam>
    /// <param name="crypto">The instance of <see cref="ICrypto"/> that will perform decryption operations.</param>
    /// <param name="json">The JSON to deserialize.</param>
    /// <param name="credentialName">
    /// The name of the credential to use for this encryption operation,
    /// or null to use the default credential.
    /// </param>
    /// <returns>The deserialized object.</returns>
    public static T FromJson<T>(this ICrypto crypto, string? json, string? credentialName = null)
    {
        var serializer =
            new JsonSerializer<T>(new JsonSerializerConfiguration
            {
                EncryptionMechanism = new CryptoEncryptionMechanism(crypto),
                EncryptKey = credentialName
            });

        return serializer.Deserialize(json);
    }
}
