namespace WPR.XnaCompability.Media;

public sealed class Playlist
{
    internal Playlist(string name, SongCollection songs)
    {
        Name = name;
        Songs = songs;
    }

    public string Name { get; }

    public SongCollection Songs { get; }
}
