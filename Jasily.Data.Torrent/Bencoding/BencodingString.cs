using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Jasily.Data.Torrent.Bencoding
{
    [DebuggerDisplay("[{Type}] {Value}")]
    public sealed class BencodingString : BencodingObject, IEquatable<BencodingString>
    {
        private readonly byte[] bytes;

        private BencodingString(byte[] bytes, string value)
        {
            this.bytes = bytes;
            this.Value = value;
        }

        public string Value { get; }

        public override BencodingObjectType Type => BencodingObjectType.String;

        protected override void WriteOriginBytes(List<byte> bytes)
        {
            bytes.AddRange(this.bytes);
        }

        internal static BencodingString Build(byte header, BinaryReader reader)
        {
            var origin = new List<byte>() { header };
            byte b;
            while ((b = reader.ReadByte()) != 58)
                origin.Add(b);
            var count = int.Parse(Encoding.UTF8.GetString(origin.ToArray(), 0, origin.Count));
            origin.Add(b);
            var buf = reader.ReadBytes(count);
            if (buf.Length != count) throw new BencodingFormatException();
            origin.AddRange(buf);

            return new BencodingString(origin.ToArray(), Encoding.UTF8.GetString(buf, 0, buf.Length));
        }

        public static explicit operator string(BencodingString value) => value.Value;

        public override int GetHashCode() => this.Value?.GetHashCode() ?? 0;

        public bool Equals(BencodingString other)
        {
            if (other == null) return false;

            return this.Value == other.Value;
        }

        public override bool Equals(object obj) => this.Equals(obj as BencodingString);
    }
}