using Jasily.Data.Torrent.Bencoding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Jasily.Data.Torrent
{
    public class TorrentInfo
    {
        BencodingDictionary innerDictionary;
        readonly List<TorrentFileInfo> files = new List<TorrentFileInfo>();

        private TorrentInfo()
        {
        }

        private TorrentInfo Load(Stream torrentStream)
        {
            this.innerDictionary = Bencoding.Bencoding.Parse(torrentStream);

            var info = this.innerDictionary["info"] as BencodingDictionary;

            if (info.ContainsKey("files"))
            {
                this.files.AddRange(((BencodingList)info["files"])
                    .Cast<BencodingDictionary>()
                    .Select(z => new TorrentFileInfo(
                        ((BencodingList)z["path"]).Select(x => (string)(BencodingString)x).ToArray(),
                        (long)(BencodingDigit)z["length"])));
            }
            else
            {
                this.files.Add(new TorrentFileInfo(new[] { (string)(BencodingString)info["name"] }, (long)(BencodingDigit)info["length"]));
            }

            return this;
        }

        public byte[] GetInfoByte() => this.innerDictionary["info"].OriginBytes();

        public TorrentFileInfo[] Files => this.files.ToArray();

        public long TotalSize => this.Files.Sum(z => z.FileSize);

        public string GetMagnetLink(string infoHash) => "magnet:?xt=urn:btih:" + infoHash;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="torrentStream"></param>
        /// <exception cref="System.ArgumentException">此文件为非法种子</exception>
        /// <returns></returns>
        public static TorrentInfo From(Stream torrentStream)
        {
            var info = new TorrentInfo();
            return info.Load(torrentStream);
        }
    }
}