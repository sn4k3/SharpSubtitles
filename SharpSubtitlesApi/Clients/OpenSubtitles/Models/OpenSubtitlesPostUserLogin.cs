using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models;

/// <summary>
/// Gather informations about the user authenticated by a bearer token.<br/>
/// User information are already sent when user is authenticated, and the remaining downloads is returned with each download, but you can also get these information here.
/// </summary>
public class OpenSubtitlesPostUserLogin
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    public OpenSubtitlesPostUserLogin()
    {
    }

    public OpenSubtitlesPostUserLogin(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public override string ToString()
    {
        return $"{nameof(Username)}: {Username}, {nameof(Password)}: {Password}";
    }
}