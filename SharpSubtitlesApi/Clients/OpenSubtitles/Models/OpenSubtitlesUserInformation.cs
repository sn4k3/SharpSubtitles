using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models;

/// <summary>
/// Gather informations about the user authenticated by a bearer token.<br/>
/// User information are already sent when user is authenticated, and the remaining downloads is returned with each download, but you can also get these information here.
/// </summary>
public class OpenSubtitlesUserInformation
{
    [JsonPropertyName("allowed_downloads")]
    public int AllowedDownloads { get; set; }

    [JsonPropertyName("level")]
    public string Level { get; set; } = string.Empty;

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("ext_installed")]
    public bool ExtInstalled { get; set; }

    [JsonPropertyName("vip")]
    public bool Vip { get; set; }

    [JsonPropertyName("downloads_count")]
    public int DownloadsCount { get; set; }

    [JsonPropertyName("remaining_downloads")]
    public int RemainingDownloads { get; set; }

    public override string ToString()
    {
        return $"{nameof(AllowedDownloads)}: {AllowedDownloads}, {nameof(Level)}: {Level}, {nameof(UserId)}: {UserId}, {nameof(ExtInstalled)}: {ExtInstalled}, {nameof(Vip)}: {Vip}, {nameof(DownloadsCount)}: {DownloadsCount}, {nameof(RemainingDownloads)}: {RemainingDownloads}";
    }
}