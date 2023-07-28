using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models;

/// <summary>
/// https://opensubtitles.stoplight.io/docs/opensubtitles-api/73acf79accc0a-login
/// </summary>
public class OpenSubtitlesUser : OpenSubtitlesBaseModel
{
    [JsonPropertyName("allowed_downloads")]
    public int AllowedDownloads { get; set; }

    [JsonPropertyName("allowed_translations")]
    public int AllowedTranslations { get; set; }

    [JsonPropertyName("level")]
    public string Level { get; set; } = string.Empty;

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("ext_installed")]
    public bool ExtInstalled { get; set; }

    [JsonPropertyName("vip")]
    public bool Vip { get; set; }

    public override string ToString()
    {
        return $"{nameof(AllowedDownloads)}: {AllowedDownloads}, {nameof(AllowedTranslations)}: {AllowedTranslations}, {nameof(Level)}: {Level}, {nameof(UserId)}: {UserId}, {nameof(ExtInstalled)}: {ExtInstalled}, {nameof(Vip)}: {Vip}";
    }
}