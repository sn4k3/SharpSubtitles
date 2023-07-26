using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models;

/// <summary>
/// Destroy a user token to end a session. Bearer token is required for this endpoint.<br/>
/// https://opensubtitles.stoplight.io/docs/opensubtitles-api/9fe4d6d078e50-logout
/// </summary>
public class OpenSubtitlesLogout
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public int Status { get; set; }

    public override string ToString()
    {
        return $"{nameof(Message)}: {Message}, {nameof(Status)}: {Status}";
    }
}