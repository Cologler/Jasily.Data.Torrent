# Jasily.Data.Torrent

## how to use

``` cs
using (var stream = File.OpenRead(@"xxx.torrent"))
{
    var t = TorrentInfo.From(stream);
    var info = t.GetInfoByte();
    var hashBytes = SHA1.Create().ComputeHash(info); // on desktop
    var infoHash = BitConverter.ToString(hashBytes).Replace("-", "");
}
```
