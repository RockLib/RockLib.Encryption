using System;
using System.IO;

namespace Rock.Encryption.Bcl
{
    internal static class BclProtocolExtensions
    {
        /* Crypto Protocol:
         * 
         * byte# : size         | Field
         * ---------------------+-----------------
         * 0 : 1                | Protocol version
         * 1 : 2                | IV length
         * 3 : IV length        | IV
         * 3 + IV Length : eof  | Ciphertext
         */

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
            if (protocolVersion != 1) throw new InvalidOperationException("Unknown protocol version: " + protocolVersion);
            var ivSize = (ushort)(stream.ReadByte() | (stream.ReadByte() << 8));
            var iv = new byte[ivSize];
            stream.Read(iv, 0, ivSize);
            return iv;
        }
    }
}