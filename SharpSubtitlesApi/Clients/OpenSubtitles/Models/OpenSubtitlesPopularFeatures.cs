using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models;

/// <summary>
/// Discover popular features on opensubtitles.com, according to last 30 days downloads.
/// https://opensubtitles.stoplight.io/docs/opensubtitles-api/6d285998026d0-popular-features
/// </summary>
public class OpenSubtitlesPopularFeatures
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OpenSubtitlesMovieType Type { get; set; }

    [JsonPropertyName("attributes")]
    public OpenSubtitlesPopularFeaturesAttributes Attributes { get; set; } = new();

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Type)}: {Type}, {nameof(Attributes)}: {Attributes}";
    }

    public class OpenSubtitlesPopularFeaturesAttributes
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("original_title")]
        public string OriginalTitle { get; set; } = string.Empty;

        [JsonPropertyName("imdb_id")]
        public int ImdbId { get; set; }

        [JsonPropertyName("tmdb_id")]
        public int? TmdbId { get; set; }

        [JsonPropertyName("feature_id")]
        public string FeatureId { get; set; } = string.Empty;

        [JsonPropertyName("year")]
        public string Year { get; set; } = string.Empty;

        [JsonPropertyName("title_aka")]
        public string[] TitleAka { get; set; } = Array.Empty<string>();

        [JsonPropertyName("subtitles_counts")]
        public IReadOnlyDictionary<string, int> SubtitlesCounts { get; set; } = new Dictionary<string, int>();

        [JsonPropertyName("url")]
        public Uri? Url { get; set; }

        [JsonPropertyName("img_url")]
        public Uri? ImgUrl { get; set; }

        public override string ToString()
        {
            return $"{nameof(Title)}: {Title}, {nameof(OriginalTitle)}: {OriginalTitle}, {nameof(ImdbId)}: {ImdbId}, {nameof(TmdbId)}: {TmdbId}, {nameof(FeatureId)}: {FeatureId}, {nameof(Year)}: {Year}, {nameof(TitleAka)}: {TitleAka}, {nameof(SubtitlesCounts)}: {SubtitlesCounts}, {nameof(Url)}: {Url}, {nameof(ImgUrl)}: {ImgUrl}";
        }
    }
}