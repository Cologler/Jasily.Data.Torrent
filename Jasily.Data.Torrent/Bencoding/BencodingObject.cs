using System.Collections.Generic;
using System.IO;

namespace Jasily.Data.Torrent.Bencoding
{
    public abstract class BencodingObject
    {
        public abstract BencodingObjectType Type { get; }

        protected abstract void WriteOriginBytes(List<byte> bytes);

        public byte[] OriginBytes()
        {
            var list = new List<byte>();
            this.WriteOriginBytes(list);
            return list.ToArray();
        }

        public static BencodingObject Parse(BinaryReader reader)
        {
            return Parse(reader.ReadByte(), reader);
        }

        protected static BencodingObject Parse(byte header, BinaryReader reader)
        {
            switch ((BencodingObjectType)header)
            {
                case BencodingObjectType.Dictionary:
                    return BencodingDictionary.Build(reader);

                case BencodingObjectType.List:
                    return BencodingList.Build(reader);

                case BencodingObjectType.Digit:
                    return BencodingDigit.Build(reader);

                case BencodingObjectType.String:
                default:
                    return BencodingString.Build(header, reader);
            }
        }
    }
}