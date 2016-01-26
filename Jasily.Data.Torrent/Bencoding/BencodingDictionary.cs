using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Jasily.Data.Torrent.Bencoding
{
    [DebuggerDisplay("[{Type}]")]
    public class BencodingDictionary : BencodingObject, IReadOnlyDictionary<string, BencodingObject>
    {
        private readonly IReadOnlyDictionary<BencodingString, BencodingObject> entities;
        private readonly IReadOnlyDictionary<string, BencodingObject> dict;

        private BencodingDictionary(IReadOnlyDictionary<BencodingString, BencodingObject> entities)
        {
            this.entities = entities;
            this.dict = this.entities.ToDictionary(entity => entity.Key.Value, entity => entity.Value);
        }

        public override BencodingObjectType Type => BencodingObjectType.Dictionary;

        protected override void WriteOriginBytes(List<byte> bytes)
        {
            bytes.Add((byte)this.Type);
            foreach (var item in this.entities)
            {
                bytes.AddRange(item.Key.OriginBytes());
                bytes.AddRange(item.Value.OriginBytes());
            }
            bytes.Add(Bencoding.EndByte);
        }

        public static BencodingDictionary Build(BinaryReader reader)
        {
            var dict = new Dictionary<BencodingString, BencodingObject>();

            byte b;
            while ((b = reader.ReadByte()) != Bencoding.EndByte)
            {
                var key = BencodingString.Build(b, reader);
                var value = Parse(reader);
                dict.Add(key, value);
            }

            return new BencodingDictionary(dict);
        }

        public IEnumerator<KeyValuePair<string, BencodingObject>> GetEnumerator() => this.dict.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public int Count => this.dict.Count;

        public bool ContainsKey(string key) => this.dict.ContainsKey(key);

        public bool TryGetValue(string key, out BencodingObject value) => this.dict.TryGetValue(key, out value);

        public BencodingObject this[string key] => this.dict[key];

        public IEnumerable<string> Keys => this.dict.Keys;

        public IEnumerable<BencodingObject> Values => this.dict.Values;
    }
}