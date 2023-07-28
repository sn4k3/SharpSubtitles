using SharpSubtitlesApi.Converters;
using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models;


/// <summary>
/// <para>Extracts as much information as possible from a video filename.</para>
/// <para>It has a very powerful matcher that allows to guess properties from a video using its filename only.This matcher works with both movies and tv shows episodes.</para>
/// https://opensubtitles.stoplight.io/docs/opensubtitles-api/7783e082edcf7-guessit
/// </summary>
public class OpenSubtitlesGuessIt : OpenSubtitlesBaseModel
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("year")]
    public int? Year { get; set; }

	[JsonPropertyName("language")]
	public string? Language { get; set; }

	[JsonPropertyName("subtitle_language")]
	public string? SubtitleLanguage { get; set; }

	[JsonPropertyName("screen_size")]
	public string? ScreenSize { get; set; }

	[JsonPropertyName("streaming_service")]
	public string? StreamingService { get; set; }

	[JsonPropertyName("source")]
	public string? Source { get; set; }

	[JsonPropertyName("other")]
	public string? Other { get; set; }

	[JsonPropertyName("audio_codec")]
	[JsonConverter(typeof(JsonStringToArrayConverter))]
	public string[]? AudioCodec { get; set; }

	[JsonPropertyName("audio_channels")]
	public string? AudioChannels { get; set; }

	[JsonPropertyName("video_codec")]
	public string? VideoCodec { get; set; }

	[JsonPropertyName("release_group")]
	public string? ReleaseGroup { get; set; }

	[JsonPropertyName("Type")]
	public string Type { get; set; } = string.Empty;
	
	public override string ToString()
	{
		return $"{nameof(Title)}: {Title}, {nameof(Year)}: {Year}, {nameof(Language)}: {Language}, {nameof(SubtitleLanguage)}: {SubtitleLanguage}, {nameof(ScreenSize)}: {ScreenSize}, {nameof(StreamingService)}: {StreamingService}, {nameof(Source)}: {Source}, {nameof(Other)}: {Other}, {nameof(AudioCodec)}: {AudioCodec}, {nameof(AudioChannels)}: {AudioChannels}, {nameof(VideoCodec)}: {VideoCodec}, {nameof(ReleaseGroup)}: {ReleaseGroup}, {nameof(Type)}: {Type}";
	}
}