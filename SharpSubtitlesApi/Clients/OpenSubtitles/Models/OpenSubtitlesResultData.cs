using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models;

public class OpenSubtitlesResultData<T> where T : class
{
	[JsonPropertyName("errors")]
	public string[]? Errors { get; set; }

	[JsonPropertyName("status")]
	public int? Status { get; set; }

	[JsonPropertyName("data")]
    public T? Data { get; set; }

    public override string ToString()
    {
        return $"{nameof(Data)}: {Data}";
    }
}