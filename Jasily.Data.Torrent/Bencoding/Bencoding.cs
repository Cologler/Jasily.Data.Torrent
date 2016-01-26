using System.IO;

namespace Jasily.Data.Torrent.Bencoding
{
    public static class Bencoding
    {
        internal const byte EndByte = 101;

        public static BencodingDictionary Parse(Stream torrentStream)
        {
            using (var reader = new BinaryReader(torrentStream))
            {
                return (BencodingDictionary)BencodingObject.Parse(reader);
            }
        }
    }
}