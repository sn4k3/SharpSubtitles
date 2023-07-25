using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models;

/// <summary>
/// List subtitle formats recognized by the API<br/>
/// https://opensubtitles.stoplight.io/docs/opensubtitles-api/69b286fc7506e-subtitle-formats
/// </summary>
public class OpenSubtitlesSubtitleFormats
{
    [JsonPropertyName("output_formats")]
    public List<string>? OutputFormats { get; set; }
}