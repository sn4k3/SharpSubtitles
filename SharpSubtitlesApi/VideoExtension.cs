using System;

namespace SharpSubtitlesApi;

public class VideoExtension
{
    /// <summary>
    /// Get a list of video extensions
    /// </summary>
    public static readonly VideoExtension[] Extensions = {
        new("3gp", "3rd Generation Partnership Project"),
        new("avi", "Audio Video Interleave"),
        new("divx", "DivX video format"),
        new("mp4", "MPEG-4 Part 14"),
        new("m4v", "MPEG-4 Video File"),
        new("mkv", "Matroska Video"),
        new("mpeg", "MPEG"),
        new("mpg", "MPEG"),
        new("ogv", "Ogg Video"),
        new("wmv", "Windows Media Video"),
        new("webm", "WebM"),
    };

    public string Extension { get; }

    public string Name { get; }

    public VideoExtension(string extension, string name)
    {
        Extension = extension.ToLower();
        Name = name;
    }

    public void Deconstruct(out string extension, out string name)
    {
        extension = Extension;
        name = Name;
    }

    public override string ToString()
    {
        return $"{Extension} ({Name})";
    }

    protected bool Equals(VideoExtension other)
    {
        return string.Equals(Extension, other.Extension, StringComparison.OrdinalIgnoreCase);
    }

    protected bool Equals(string extension)
    {
        if (string.IsNullOrWhiteSpace(extension)) return false;
        if (extension[0] == '.') extension = extension.Remove(0, 1);
        return string.Equals(Extension, extension, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj is string str) return Equals(str);
        if (obj.GetType() != this.GetType()) return false;
        return Equals((VideoExtension)obj);
    }

    public override int GetHashCode()
    {
        return Extension.GetHashCode();
    }
}