using System;
using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models;

/// <summary>
/// <para>Create a token to authenticate a user. If response code is 401 Unathorized stop sending further requests with the same credentials, login is "expensive" operation.</para>
/// <para>Request rate limit is 1 request per 1 second.</para>
/// <para>Further API requests must continue on returned base_url host, which can have different cache time for search results and different request rate limits.</para>
/// https://opensubtitles.stoplight.io/docs/opensubtitles-api/73acf79accc0a-login
/// </summary>
public class OpenSubtitlesLogin
{
    [JsonPropertyName("user")]
    public OpenSubtitlesUser User { get; set; } = new();

    [JsonPropertyName("base_url")]
    public Uri? BaseUrl { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public int Status { get; set; }

    public override string ToString()
    {
        return $"{nameof(User)}: {User}, {nameof(BaseUrl)}: {BaseUrl}, {nameof(Token)}: {Token}, {nameof(Status)}: {Status}";
    }
}