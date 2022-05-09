using System;

namespace RockLib.Encryption.Symmetric;

/// <summary>
/// Defines a credential for symmetric encryption.
/// </summary>
public sealed class Credential
{
    /// <summary>
    /// Defines the default value of <see cref="SymmetricAlgorithm"/>.
    /// </summary>
    public const SymmetricAlgorithm DefaultAlgorithm = SymmetricAlgorithm.Aes;

    /// <summary>
    /// Defines the default initialization vector size.
    /// </summary>
    public const ushort DefaultIVSize = 16;
    private readonly Func<byte[]> _key;

    /// <summary>
    /// Initializes a new instance of the <see cref="Credential"/> class.
    /// </summary>
    /// <param name="key">A function that returns the symmetric key to be returned by the <see cref="GetKey()"/> method.</param>
    /// <param name="algorithm">The <see cref="SymmetricAlgorithm"/> that will be used for a symmetric encryption or decryption operation.</param>
    /// <param name="ivSize">The size of the initialization vector that is used to add entropy to encryption or decryption operations.</param>
    /// <param name="name">The name of this credential.</param>
    /// <param name="cacheKeyValue">Whether to cache the value of the <paramref name="key"/> function.</param>
    public Credential(Func<byte[]> key, SymmetricAlgorithm algorithm = DefaultAlgorithm, ushort ivSize = DefaultIVSize, string? name = null, bool cacheKeyValue = false)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (!Enum.IsDefined(typeof(SymmetricAlgorithm), algorithm))
        {
            // TODO: Throw different exception for invalid enum
            throw new ArgumentOutOfRangeException(nameof(algorithm), $"{nameof(algorithm)} value is not defined: {algorithm}.");
        }
        if (ivSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(ivSize), $"{nameof(ivSize)} must be greater than 0.");
        }

        Algorithm = algorithm;
        IVSize = ivSize;
        Name = name;

        if (!cacheKeyValue)
        {
            _key = key;
        }
        else
        {
            var k = new Lazy<byte[]>(key);
            _key = () =>
            {
                if (k.Value is null)
                {
                    return Array.Empty<byte>();
                }

                var copy = new byte[k.Value.Length];
                k.Value.CopyTo(copy, 0);
                return copy;
            };
        }
    }

    /// <summary>
    /// Gets the name of the credential.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Gets the <see cref="SymmetricAlgorithm"/> that is used for symmetric
    /// encryption or decryption operations.
    /// </summary>
    public SymmetricAlgorithm Algorithm { get; }

    /// <summary>
    /// Gets the size of the initialization vector that is used to add entropy to
    /// encryption or decryption operations.
    /// </summary>
    public ushort IVSize { get; }

    /// <summary>
    /// Gets the plain-text value of the symmetric key that is used for encryption
    /// or decryption operations.
    /// </summary>
    /// <returns>The symmetric key.</returns>
    public byte[] GetKey()
    {
        var key = _key();

        if (key is null || key.Length == 0)
        {
            throw new InvalidOperationException("The value returned from the key function must not be null or empty.");
        }

        return key;
    }
}
