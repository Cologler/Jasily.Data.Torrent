using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Jasily.Data.Torrent.Bencoding
{
    [DebuggerDisplay("[{Type}] {Value}")]
    public class BencodingDigit : BencodingObject
    {
        private readonly byte[] bytes;

        private BencodingDigit(byte[] bytes, long value)
        {
            this.bytes = bytes;
            this.Value = value;
        }

        public long Value { get; }

        public override BencodingObjectType Type => BencodingObjectType.Digit;

        protected override void WriteOriginBytes(List<byte> bytes)
        {
            bytes.Add((byte)this.Type);
            bytes.AddRange(this.bytes);
            bytes.Add(Bencoding.EndByte);
        }

        internal static BencodingDigit Build(BinaryReader reader)
        {
            var origin = new List<byte>();
            byte b;
            while ((b = reader.ReadByte()) != Bencoding.EndByte)
                origin.Add(b);
            var value = long.Parse(Encoding.UTF8.GetString(origin.ToArray(), 0, origin.Count));

            return new BencodingDigit(origin.ToArray(), value);
        }

        public static explicit operator long(BencodingDigit value) => value.Value;

        public static explicit operator int(BencodingDigit value) => Convert.ToInt32(value.Value);
    }
}