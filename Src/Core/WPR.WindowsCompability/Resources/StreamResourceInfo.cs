using System;
using System.IO;

namespace WPR.WindowsCompability.Resources;

public sealed class StreamResourceInfo
{
    public StreamResourceInfo(Stream stream, string? contentType)
    {
        Stream = stream ?? throw new ArgumentNullException(nameof(stream));
        ContentType = contentType;
    }

    public Stream Stream { get; }

    public string? ContentType { get; }
}
