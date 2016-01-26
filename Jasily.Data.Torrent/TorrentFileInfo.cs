using System.Linq;

namespace Jasily.Data.Torrent
{
    public class TorrentFileInfo
    {
        public string Path { get; }

        public string FileName { get; }

        public long FileSize { get; }

        public TorrentFileInfo(string[] filePaths, long fileSize)
        {
            this.Path = System.IO.Path.Combine(filePaths);
            this.FileName = filePaths.Last();
            this.FileSize = fileSize;
        }
    }
}