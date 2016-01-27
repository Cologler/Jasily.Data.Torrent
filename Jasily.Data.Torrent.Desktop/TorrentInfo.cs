using System;
using System.IO;
using System.Security.Cryptography;

namespace Jasily.Data.Torrent.Desktop
{
    public class TorrentInfo : Jasily.Data.Torrent.TorrentInfo
    {
        public override string GetInfoHash()
        {
            var bytes = this.GetInfoByte();
            var hashBytes = SHA1.Create().ComputeHash(bytes);
            var infoHash = BitConverter.ToString(hashBytes).Replace("-", "");
            return infoHash;
        }

        public override string GetMagnetLink() => this.GetMagnetLink(this.GetInfoHash());

        public new static TorrentInfo From(Stream torrentStream)
        {
            var info = new TorrentInfo();
            info.Load(torrentStream);
            return info;
        }

        public new static TorrentInfo TryLoad(Stream torrentStream)
        {
            try
            {
                return From(torrentStream);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
