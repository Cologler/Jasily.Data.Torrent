using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Jasily.Data.Torrent.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var stream = File.OpenRead(@"xxx.torrent"))
            {
                var t = TorrentInfo.From(stream);
                var info = t.GetInfoByte();
                var hashBytes = SHA1.Create().ComputeHash(info);
                var infoHash = BitConverter.ToString(hashBytes).Replace("-", "");
            }
        }
    }
}
