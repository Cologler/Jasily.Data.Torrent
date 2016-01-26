using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Jasily.Data.Torrent.Bencoding
{
    [DebuggerDisplay("[{Type}]")]
    public class BencodingList : BencodingObject, IReadOnlyCollection<BencodingObject>
    {
        private readonly List<BencodingObject> items;

        private BencodingList(IEnumerable<BencodingObject> items)
        {
            this.items = items.ToList();
        }

        public override BencodingObjectType Type => BencodingObjectType.List;

        protected override void WriteOriginBytes(List<byte> bytes)
        {
            bytes.Add((byte)this.Type);
            foreach (var item in this.items)
                bytes.AddRange(item.OriginBytes());
            bytes.Add(Bencoding.EndByte);
        }

        public static BencodingList Build(BinaryReader reader)
        {
            var origin = new List<BencodingObject>();
            byte b;
            while ((b = reader.ReadByte()) != Bencoding.EndByte)
                origin.Add(Parse(b, reader));

            return new BencodingList(origin);
        }

        public IEnumerator<BencodingObject> GetEnumerator() => this.items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public int Count => this.items.Count;

        public static explicit operator BencodingObject[] (BencodingList value) => value.items.ToArray();
    }
}