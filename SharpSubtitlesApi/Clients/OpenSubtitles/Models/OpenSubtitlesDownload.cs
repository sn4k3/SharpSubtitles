using System;
using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models;

/// <summary>
/// Request a download url for a subtitle. Subtitle file in temporary URL will be always in UTF-8 encoding.<br/>
/// https://opensubtitles.stoplight.io/docs/opensubtitles-api/6be7f6ae2d918-download
/// </summary>
public class OpenSubtitlesDownload
{
    [JsonPropertyName("link")]
    public Uri? Link { get; set; }

    [JsonPropertyName("file_name")]
    public string FileName { get; set; } = string.Empty;

    [JsonPropertyName("requests")]
    public int Requests { get; set; }

    [JsonPropertyName("remaining")]
    public int Remaining { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("reset_time")]
    public string ResetTime { get; set; } = string.Empty;

    [JsonPropertyName("reset_time_utc")]
    public DateTime ResetTimeUtc { get; set; }

    public override string ToString()
    {
        return $"{nameof(Link)}: {Link}, {nameof(FileName)}: {FileName}, {nameof(Requests)}: {Requests}, {nameof(Remaining)}: {Remaining}, {nameof(Message)}: {Message}, {nameof(ResetTime)}: {ResetTime}, {nameof(ResetTimeUtc)}: {ResetTimeUtc}";
    }
}