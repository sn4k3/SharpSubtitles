using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models;

/// <summary>
/// https://opensubtitles.stoplight.io/docs/opensubtitles-api/6be7f6ae2d918-download
/// </summary>
public class OpenSubtitlesPostDownload
{
    [JsonPropertyName("file_id")]
    public int FileId { get; set; }

    public OpenSubtitlesPostDownload()
    {
    }

    public OpenSubtitlesPostDownload(int fileId)
    {
        FileId = fileId;
    }

    public override string ToString()
    {
        return $"{nameof(FileId)}: {FileId}";
    }
}