using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models;

/// <summary>
/// Get the languages table containing the codes and names used through the API<br/>
/// https://opensubtitles.stoplight.io/docs/opensubtitles-api/1de776d20e873-languages
/// </summary>
public class OpenSubtitlesLanguage : OpenSubtitlesBaseModel
{
    [JsonPropertyName("language_code")]
    public string? LanguageCode { get; set; }

    [JsonPropertyName("language_name")]
    public string? LanguageName { get; set; }

    public override string ToString()
    {
        return $"{nameof(LanguageCode)}: {LanguageCode}, {nameof(LanguageName)}: {LanguageName}";
    }
}