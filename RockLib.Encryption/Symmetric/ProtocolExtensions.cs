using System;
using System.IO;

namespace RockLib.Encryption.Symmetric;

internal static class ProtocolExtensions
{
    /* Crypto Protocol v1
    +---------------+----------------------+---------------------+
    | byte#         | size                 | field               |
    |---------------+----------------------+---------------------|
    | 0             | 1                    | version (value = 1) |
    | 1             | 2                    | iv length           |
    | 3             | iv length            | iv                  |
    | 3 + iv Length | payload size - byte# | ciphertext          |
    +---------------+----------------------+---------------------+ */

    public static void WriteCipherTextHeader(this Stream stream, byte[] iv)
    {
        stream.WriteByte(1);
        stream.WriteByte((byte)(iv.Length & 0xFF));
        stream.WriteByte((byte)(iv.Length >> 8));
        stream.Write(iv, 0, iv.Length);
    }

    public static byte[] ReadIVFromCipherTextHeader(this Stream stream)
    {
        var protocolVersion = stream.ReadByte();

        if (protocolVersion != 1)
        {
            throw new InvalidOperationException("Unknown protocol version (only version 1 is supported): " + protocolVersion);
        }

        var ivSize = (ushort)(stream.ReadByte() | (stream.ReadByte() << 8));
        var iv = new byte[ivSize];
        stream.Read(iv, 0, ivSize);
        return iv;
    }

    public static bool IsEncrypted(this byte[] cipherText)
    {
        if (cipherText.Length < 3 || cipherText[0] != 1)
        {
            return false;
        }

        var ivSize = (ushort)(cipherText[1] | (cipherText[2] << 8));

        return ivSize switch
        {
            8 or 16 => cipherText.Length >= 3 + ivSize,
            _ => false,
        };
    }
}