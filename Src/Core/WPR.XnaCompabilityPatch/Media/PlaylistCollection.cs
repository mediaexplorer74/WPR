using System.Collections;
using System.Collections.Generic;

namespace WPR.XnaCompability.Media;

public sealed class PlaylistCollection : IEnumerable<Playlist>, IEnumerable
{
    private readonly List<Playlist> playlists = new();

    internal PlaylistCollection()
    {
    }

    public int Count => playlists.Count;

    public Playlist this[int index] => playlists[index];

    public IEnumerator<Playlist> GetEnumerator() => playlists.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
