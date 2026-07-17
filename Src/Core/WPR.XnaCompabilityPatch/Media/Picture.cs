using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace WPR.XnaCompability.Media;

public sealed class Picture
{
    private readonly byte[] _thumbnail;

    internal Picture(string name, DateTime date, byte[] thumbnail)
    {
        Name = name;
        Date = date;
        _thumbnail = thumbnail;
    }

    public string Name { get; }
    public DateTime Date { get; }
    public Stream GetThumbnail() => new MemoryStream(_thumbnail, writable: false);
}

public sealed class PictureCollection : IEnumerable<Picture>, IEnumerable
{
    private readonly List<Picture> _pictures = [];

    public int Count => _pictures.Count;
    public IEnumerator<Picture> GetEnumerator() => _pictures.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
